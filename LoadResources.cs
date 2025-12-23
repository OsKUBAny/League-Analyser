using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace League_Analyser
{
    public class LoadResources
    {
        private MainWindow mainWindow;
        private Info info;
        private Data data;

        public void LoadResourcesInit()
        {
            mainWindow = (MainWindow)App.Current.MainWindow;
            info = mainWindow.info;
            data = mainWindow.data;
        }

        public class Resource
        {
            public string ResourceType { get; }
            public string Path { get; }
            public Type DataType { get; }

            public Resource(string resourceType, string path, Type dataType)
            {
                ResourceType = resourceType;
                Path = path;
                DataType = dataType;
            }
        }

        public static class Resources
        {
            public static Resource maps = new Resource
                (
                resourceType: "gameConstants",
                path: "data/gameConstants/maps.json",
                dataType: typeof(List<DataType.MapsDto>)
                );
            public static Resource champions = new Resource
                (
                resourceType: "dragontail",
                path: "data/dragontail-{0}/{0}/data/{1}/championFull.json",
                dataType: typeof(DataType.ChampionDataDto)
                );
            public static Resource items = new Resource
                (
                resourceType: "dragontail",
                path: "data/dragontail-{0}/{0}/data/{1}/item.json",
                dataType: typeof(DataType.ItemClass.Items)
                );
            public static Resource summonerSpells = new Resource
                (
                resourceType: "dragontail",
                path: "data/dragontail-{0}/{0}/data/{1}/summoner.json",
                dataType: typeof(DataType.SummonerSpell)
                );
            public static Resource resourceLanguages = new Resource
                (
                resourceType: "dragontail",
                path: "data/dragontail-{0}/languages.json",
                dataType: typeof(List<string>)
                );
        }

        public class LoadedImage
        {
            public BitmapImage image { get; set; }
            public bool result { get; set; }
        }

        public enum ImagePath_t
        {
            resources = 0,
            DD_champion = 1,
            DD_item,
            DD_spell,
            DD_passive,
            DD_map,
            gC_img_maps = 6,
            gC_timeline_kills = 7,
            gC_timeline_monsters,
            gC_timeline_structures,
            gC_timeline_misc
        }

        // Load all assets from files (all static JSONs)
        public async Task LoadAllAssets()
        {
            var tasks = new List<Task<dynamic>>
            {
                loadObject(Resources.maps),
                loadObject(Resources.champions),
                loadObject(Resources.items),
                loadObject(Resources.summonerSpells),
                loadObject(Resources.resourceLanguages)
            };
            var results = await Task.WhenAll(tasks);

            data.mapsDto = results[0];
            data.championDataDto = results[1];
            data.itemsDto = results[2];
            data.summonerDto = results[3];
            data.resourceLanguages = ConvertLanguagesList(results[4]);
        }

        // Return object parsed as <DataType> from selected resources list.
        private async Task<dynamic> loadObject(Resource resource)
        {
            string dataRaw;
            dynamic obj;

            dataRaw = await ReadTextFile(string.Format(resource.Path, data.gameVersion, Properties.Settings.Default.ResourcesLanguage));
            if (dataRaw == null) return null;
            obj = await Deserialize(dataRaw, resource.DataType);

            return obj;
        }

        // Deserialize Json type string into given type object.
        public async Task<dynamic> Deserialize(string data, Type type)
        {
            dynamic obj;

            obj = await Task.Run(() =>
            {
                try { return JsonConvert.DeserializeObject(data, type, new FloatToIntConverter()); }
                catch (Exception ex) { return ex.Message; }
            });


            if (obj is string)
            {
                info.CreateNewPrompt(Info.Messages.error_loadResources_deserializeError, obj);
                return null;
            }
            else return obj;
        }

        // Serialize object into Json type string.
        public async Task<string> Serialize(dynamic obj)
        {
            string result;
            try { result = await Task.Run(() => JsonConvert.SerializeObject(obj, Formatting.Indented)); }
            catch (Exception ex)
            {
                info.CreateNewPrompt(Info.Messages.error_loadResources_serializeError, ex.Message);
                return null;
            }
            return result;
        }

        // Convert float value to int if parameter's type and returned type are different.
        public class FloatToIntConverter : Newtonsoft.Json.JsonConverter
        {
            public override bool CanConvert(Type objectType)
            {
                return objectType == typeof(int) || objectType == typeof(int?);
            }

            public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
            {
                if (reader.TokenType == JsonToken.Null)
                {
                    return objectType == typeof(int?) ? (int?)null : 0;
                }

                if (reader.TokenType == JsonToken.Float || reader.TokenType == JsonToken.Integer)
                {
                    try { return Convert.ToInt32(reader.Value); }
                    catch (OverflowException) { return Int32.MaxValue; }
                }
                throw new JsonSerializationException($"Nieoczekiwany typ: {reader.TokenType}");
            }

            public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
            {
                writer.WriteValue(value);
            }
        }

        // Read static Json from file.
        public async Task<string> ReadTextFile(string path)
        {
            string result;
            try { result = await Task.Run(() => File.ReadAllText(path)); }
            catch (Exception ex)
            {
                info.CreateNewPrompt(Info.Messages.error_loadResources_loadFileError, ex.Message);
                return null;
            }
            return result;
        }

        public async Task<bool> SaveTextToFile(string text, string path)
        {
            try { await Task.Run(() => File.WriteAllText(path, text)); }
            catch (Exception ex)
            {
                info.CreateNewPrompt(Info.Messages.error_loadResources_saveFileError, ex.Message);
                return false;
            }
            return true;
        }

        // Parse html-like string from item.json to get all stats and passive effects descriptions.
        public static List<string>[] ParseItemDescritpion(string raw)
        {
            var statsList = new List<string>[2];
            statsList[0] = new List<string>();
            statsList[1] = new List<string>();

            var statRegex = new Regex(@"<attention>(\d+%?)</attention>\s*([\p{L}\d\s\-]+)");
            var statMatches = statRegex.Matches(raw);
            foreach (Match statMatch in statMatches)
            {
                if (statMatch.Groups.Count >= 3)
                {
                    statsList[0].Add(statMatch.Groups[2].Value.Trim());
                    statsList[1].Add(statMatch.Groups[1].Value.Trim());
                }
            }

            var passiveRegex = new Regex(@"<passive>(.*?)</passive>\s*([\s\S]*?)(?=(<br>\s*<br>|<active>|</mainText>))");
            var passiveMatches = passiveRegex.Matches(raw);
            foreach (Match passiveMatch in passiveMatches)
            {
                if (passiveMatch.Groups.Count > 1)
                {
                    string name = passiveMatch.Groups[1].Value.Trim();
                    name = Regex.Replace(name, @"<[^>]+>", "");
                    name = name.Replace("&nbsp;", " ");
                    statsList[0].Add(name);

                    string value = passiveMatch.Groups[2].Value.Trim();
                    value = Regex.Replace(value, @"<[^>]+>", "");
                    value = value.Replace("&nbsp;", " ");
                    statsList[1].Add(value);
                }
            }

            var activeRegex = new Regex(@"<active>([^<]+?)</active>\s*(.*?)(?=(<active>|</mainText>))");
            var activeMatches = activeRegex.Matches(raw);
            foreach (Match activeMatch in activeMatches)
            {
                if (activeMatch.Groups.Count > 1)
                {
                    string name = activeMatch.Groups[1].Value.Trim();
                    name = Regex.Replace(name, @"<[^>]+>", "");
                    name = name.Replace("&nbsp;", " ");
                    statsList[0].Add(name);

                    string value = activeMatch.Groups[2].Value.Trim();
                    value = Regex.Replace(value, @"<[^>]+>", "");
                    value = value.Replace("&nbsp;", " ");
                    statsList[1].Add(value);
                }
            }
            return statsList;
        }

        // Load image from resources or external files.
        public static LoadedImage LoadImage(string fileName, ImagePath_t pathType, bool makePrompt)
        {
            MainWindow mainWindow_ = (MainWindow)App.Current.MainWindow;
            Info info_ = mainWindow_.info;
            string DD_version = mainWindow_.data.gameVersion;
            LoadedImage img = new LoadedImage();

            string path = null;

            string pathInternal = "pack://application:,,,/"; //For internal reources
            string pathDD = string.Format("pack://siteoforigin:,,,/data/dragontail-{0}/{0}/img/", DD_version); // Datadragon
            string pathGC = "pack://siteoforigin:,,,/data/gameConstants/"; // GameConstants

            switch (pathType)
            {
                case ImagePath_t.resources: path = pathInternal + "Resources/Generic/"; break;
                case ImagePath_t.DD_champion: path = pathDD + "champion/"; break;
                case ImagePath_t.DD_item: path = pathDD + "item/"; break;
                case ImagePath_t.DD_spell: path = pathDD + "spell/"; break;
                case ImagePath_t.DD_passive: path = pathDD + "passive/"; break;
                case ImagePath_t.DD_map: path = pathDD + "map/"; break;
                case ImagePath_t.gC_img_maps: path = pathGC + "img/maps/"; break;
                case ImagePath_t.gC_timeline_kills: path = pathGC + "timeline/kills/"; break;
                case ImagePath_t.gC_timeline_monsters: path = pathGC + "timeline/monsters/"; break;
                case ImagePath_t.gC_timeline_structures: path = pathGC + "timeline/structures/"; break;
                case ImagePath_t.gC_timeline_misc: path = pathGC + "timeline/misc/"; break;
            }

            try
            {
                img.image = new BitmapImage(new Uri(string.Format("{0}{1}", path, fileName)));
                img.result = true;
            }
            catch (Exception ex)
            {
                img.image = new BitmapImage(new Uri(string.Format("pack://application:,,,/Resources/Generic/empty.png")));
                img.result = false;
                if (makePrompt == true) info_.CreateNewPrompt(Info.Messages.error_loadResources_loadImageError, ex.Message);
            }

            return img;
        }

        // Convert languages list to Dictionary type object
        private Dictionary<string, string> ConvertLanguagesList(List<string> languagesList)
        {
            Dictionary<string, string> languagesDictionary = new Dictionary<string, string>();

            foreach (string language in languagesList)
            {
                try
                {
                    var cultureCode = language.Replace('_', '-'); // Languages file contains codes in format xx_XX but we need xx-XX
                    var foo_culture = new CultureInfo(cultureCode);
                    string languageName = foo_culture.NativeName;

                    languagesDictionary.Add(language, languageName);
                }
                catch (Exception) { }
            }
            return languagesDictionary; ;
        }
    }
}
