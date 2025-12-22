using System;
using System.Diagnostics;
using System.Net;
using System.Threading.Tasks;

namespace League_Analyser
{
    public class ApiData
    {
        private MainWindow mainWindow;
        private Info info;
        private LoadResources loadResources;

        private static string apiKey;

        public void ApiDataInit()
        {
            apiKey = PrivateKeys.RiotApiKey;

            mainWindow = (MainWindow)App.Current.MainWindow;
            info = mainWindow.info;
            loadResources = mainWindow.loadResources;
        }

        public static class EndPoint
        {
            public const string getAccountByRiotId = "riot/account/v1/accounts/by-riot-id/{0}/{1}?";
            public const string getListOfMatchIds = "lol/match/v5/matches/by-puuid/{0}/ids?start={1}&count={2}&";
            public const string getMatchByMatchId = "lol/match/v5/matches/{0}?";
            public const string getMatchTimelineByMatchId = "lol/match/v5/matches/{0}/timeline?";
            public const string getGameVersion = "lol/match/v5/matches/{0}/timeline?";
            public const string DDgetVersions = "api/versions.json";
        }

        private string ApiGetRegion(string server)
        {
            switch (server)
            {
                case "EUN1": return "europe.api.riotgames.com/";
                case "EUW1": return "europe.api.riotgames.com/";
                case "TR1": return "europe.api.riotgames.com/";
                case "RU": return "europe.api.riotgames.com/";
                case "LA1": return "americas.api.riotgames.com/";
                case "LA2": return "americas.api.riotgames.com/";
                case "NA1": return "americas.api.riotgames.com/";
                case "BR1": return "americas.api.riotgames.com/";
                case "JP1": return "asia.api.riotgames.com/";
                case "KR": return "asia.api.riotgames.com/";
                case "OC1": return "sea.api.riotgames.com/";
                case "SG2": return "sea.api.riotgames.com/";
                case "TW2": return "sea.api.riotgames.com/";
                case "VN2": return "sea.api.riotgames.com/";

                case "DD": return "ddragon.leagueoflegends.com/";
                default: return "europe.api.riotgames.com/";
            }
        }

        // Can be called globally, create API url and asynchronicly donwload and deserialize data.
        public async Task<dynamic> ApiGetData(Type dataType, string server, string callType, params object[] parameters)
        {
            string apiUrl;
            apiUrl = string.Format("https://{0}", ApiGetRegion(server));
            apiUrl += string.Format(callType, parameters);
            if (server != "DD") apiUrl += string.Format("api_key={0}", apiKey);

            dynamic result = await ApiExecute(apiUrl, dataType);
            return result;  // result = null if procces failed at any poit.
        }

        // Called by ApiGetData, handles download and deserialyze process.
        private async Task<dynamic> ApiExecute(string path, Type type)
        {
            WebClient wc = new WebClient();
            Uri uri = new Uri(path);
            string result;
            bool isServerOverloaded = false;
            dynamic obj;
            Stopwatch stopwatchApi = new Stopwatch();
            stopwatchApi.Start();

        DownloadProcess:

            try { result = await wc.DownloadStringTaskAsync(uri); }
            catch (WebException ex)
            {
                if (ex.Response is HttpWebResponse response)
                {
                    // Handling API overload exception
                    if ((int)response.StatusCode == 429)
                    {
                        if (stopwatchApi.Elapsed.TotalSeconds >= 130) //2min + 10sec
                        {
                            info.CreateNewPrompt(Info.Messages.error_api_timeout);
                            return null;
                        }
                        if (isServerOverloaded == false)
                        {
                            info.CreateNewPrompt(Info.Messages.warning_api_tooManyRequests);
                            info.CreateNewPrompt(Info.Messages.process_api_awaiting);
                        }
                        info.UpdatePrompt(Info.Messages.process_api_awaiting);
                        await Task.Delay(10000);
                        info.UpdatePrompt(Info.Messages.process_api_reconnecting);
                        isServerOverloaded = true;
                        goto DownloadProcess;
                    }
                    else if ((int)response.StatusCode >= 500 && (int)response.StatusCode < 600)
                    {
                        info.CreateNewPrompt(Info.Messages.error_api_euneOnFire);
                    }
                    else info.CreateNewPrompt(Info.Messages.error_api_apiError, ex.Message);
                }
                else info.CreateNewPrompt(Info.Messages.error_api_unexpectedError, ex.Message);

                return null;
            }

            obj = await loadResources.Deserialize(result, type);

            return obj;
        }
    }
}
