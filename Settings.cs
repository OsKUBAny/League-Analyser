using Newtonsoft.Json.Linq;
using SharpCompress.Readers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace League_Analyser
{
    public class Settings
    {
        private MainWindow mainWindow;
        private Info info;
        private Data data;
        private LoadResources loadResources;
        private ApiData apiData;

        private string profilesPath = "data/player/";
        private string settingsPath = "data/settings.dll";
        public ObservableCollection<string> profilesList = new ObservableCollection<string>();
        public bool updateDDneeded = false;
        public UpdateData updateData;
        private string gitReleaseUrl;
        private string gitAccesToken;
        private int downloadMatchQuantity = 50;

        public class UpdateData
        {
            public bool isUpdateNeeded { get; set; }
            public string version { get; set; }
            public string name { get; set; }
            public string description { get; set; }
            public string date { get; set; }
            public string url { get; set; }
            public string fileName { get; set; }
        }

        public void SettingsInit()
        {
            gitReleaseUrl = PrivateKeys.GitHubRepoPath;
            gitAccesToken = PrivateKeys.GitHubToken;

            mainWindow = (MainWindow)App.Current.MainWindow;
            info = mainWindow.info;
            data = mainWindow.data;
            loadResources = mainWindow.loadResources;
            apiData = mainWindow.apiData;

            try { profilesList = new ObservableCollection<string>(Directory.GetFiles(profilesPath).Select(Path.GetFileNameWithoutExtension)); }
            catch (Exception ex) { info.CreateNewPrompt(Info.Messages.error_settings_loadProfileListError, ex.Message); }

            mainWindow.button_update.Click += async (sender, e) =>
            {
                if (mainWindow.isProcessOngoing == true) return;

                mainWindow.isProcessOngoing = true;
                await UpdateMatches();
                mainWindow.isProcessOngoing = false;
            };
        }

        public static readonly List<string> serversList = new List<string>
        {
            "EUN1",
            "EUW1",
            "TR1",
            "RU",
            "LA1",
            "LA2",
            "NA1",
            "BR1",
            "JP1",
            "KR",
            "OC1",
            "SG2",
            "TW2",
            "VN2"
        };

        public static readonly Dictionary<string, string> languagesList = new Dictionary<string, string>
        {
            { "pl", "Polski"},
            { "en", "English"}
        };

        public void InitializeSettings()
        {
            SettingsInit();
            View.Screens.Settings settingsScreen = new View.Screens.Settings();
            Grid.SetRow(settingsScreen, 1);
            mainWindow.mainGrid.Children.Add(settingsScreen);
        }

        public async Task<bool> CreateNewProfileReference(string name, string tag, string serverName)
        {
            info.CreateNewPrompt(Info.Messages.process_settings_loading);

            DataType.AccountDto result;
            result = await apiData.ApiGetData(typeof(DataType.AccountDto), serverName, ApiData.EndPoint.getAccountByRiotId, name, tag);
            if (result == null)
            {
                info.CreateNewPrompt(Info.Messages.warning_settings_profileNotAdded);
                return false;
            }

            Data.Player profile = new Data.Player
            {
                account = new DataType.AccountDto
                {
                    gameName = result.gameName,
                    tagLine = result.tagLine
                },
                server = serverName
            };
            Data.PlayerData playerData = new Data.PlayerData
            {
                player = profile
            };

            if (await SaveDataToFile(playerData, true) == true)
            {
                info.CreateNewPrompt(Info.Messages.ok_settings_profileAdded);
                profilesList.Add(profile.account.gameName);

                return true;
            }
            info.CreateNewPrompt(Info.Messages.warning_settings_profileNotAdded);
            return false;
        }

        public async Task<bool> SaveDataToFile(Data.PlayerData playerData, bool generateNew)
        {
            bool result;

            if (generateNew == true)
            {
                playerData.dataStructVersion = data.dataStructVersion;
                playerData.historyGameIds = new List<string>();
                playerData.matches = new List<DataType.MatchLao>();
            }

            string jsonText = await loadResources.Serialize(playerData);
            if (jsonText == null) return false;
            if (playerData.player == null || playerData.player.account == null || string.IsNullOrEmpty(playerData.player.account.gameName))
            {
                info.CreateNewPrompt(Info.Messages.error_settings_noReferenceToProfile);
                return false;
            }
            result = await loadResources.SaveTextToFile(jsonText, string.Format("{0}{1}.json", profilesPath, playerData.player.account.gameName));
            return result;
        }

        public async void LoadProfileInformations(string profileName, View.UserControls.SettingsProfileDetail detailsPanel)
        {
            if (string.IsNullOrEmpty(profileName) == true) return;

            string dataVersion = "(unknown)";
            int matchCount = 0;
            string lastMatchDate = "-";
            bool validDataVersion = false;

            string dataRaw = await loadResources.ReadTextFile(string.Format("{0}{1}.json", profilesPath, profileName));
            try
            {
                JObject jsonObject = JObject.Parse(dataRaw);

                dataVersion = jsonObject["dataStructVersion"].ToString();
                matchCount = jsonObject["historyGameIds"].Count();
                validDataVersion = (dataVersion == data.dataStructVersion);
                if (matchCount > 0 && validDataVersion == true) lastMatchDate = jsonObject["matches"][0]["preview"]["timestamp"].ToString();
            }
            catch (Exception ex)
            {
                info.CreateNewPrompt(Info.Messages.error_settings_loadProfileDetailsError, ex.Message);
            }

            detailsPanel.dataVersion.Text = dataVersion;
            detailsPanel.numberOfMatches.Text = matchCount.ToString();
            detailsPanel.lastGameDate.Text = lastMatchDate;
            if (validDataVersion == false)
            {
                detailsPanel.dataVersion.Foreground = new SolidColorBrush(Color.FromArgb(255, 255, 0, 0));
                detailsPanel.outdatedDataVersionText.Visibility = Visibility.Visible;
                detailsPanel.button_downloadAll.Visibility = Visibility.Collapsed;
                detailsPanel.button_loadProfile.Visibility = Visibility.Collapsed;
            }
        }

        public bool DeleteProfile(string profileName)
        {
            MessageBoxResult result = MessageBox.Show
            (
                "Czy na pewno chcesz usunąć ten profil?", string.Format("Usuwanie profiu {0}", profileName),
                MessageBoxButton.YesNo,
                MessageBoxImage.Exclamation
            );
            if (result == MessageBoxResult.No) return false;

            try
            {
                string filePath = string.Format("{0}{1}.json", profilesPath, profileName);

                if (File.Exists(filePath)) File.Delete(filePath);
                else
                {
                    info.CreateNewPrompt(Info.Messages.error_settings_deleteProfilePathError, filePath);
                    return false;
                }
            }
            catch (Exception ex)
            {
                info.CreateNewPrompt(Info.Messages.error_settings_deleteProfileError, ex.Message);
                return false;
            }

            if (mainWindow.activeProfileName.Text.Contains(profileName))
            {
                data.player = new Data.Player();
                data.historyGameIds = new List<string>();
                data.matches = new List<DataType.MatchLao>();

                mainWindow.activeProfileName.Visibility = Visibility.Hidden;
                mainWindow.activeProfileName.Text = string.Empty;
                mainWindow.button_update.Visibility = Visibility.Hidden;
                mainWindow.menuButtons.button_matchHistory.Visibility = Visibility.Collapsed;
                mainWindow.menuButtons.button_stats.Visibility = Visibility.Collapsed;
            }

            profilesList.Remove(profileName);
            info.CreateNewPrompt(Info.Messages.ok_settings_profileDeleted);
            return true;
        }

        public async Task LoadProfileData(string profileName, bool saveSettingsFile)
        {
            info.CreateNewPrompt(Info.Messages.process_settings_loadProfile);

            string fileRaw = await loadResources.ReadTextFile(string.Format("{0}{1}.json", profilesPath, profileName));
            if (fileRaw == null)
            {
                info.CreateNewPrompt(Info.Messages.error_settings_loadProfileError);
                return;
            }

            Data.PlayerData playerData = await loadResources.Deserialize(fileRaw, typeof(Data.PlayerData));
            if (playerData == null)
            {
                info.CreateNewPrompt(Info.Messages.error_settings_loadProfileError);
                return;
            }

            data.player = playerData.player;
            data.historyGameIds = playerData.historyGameIds;
            data.matches = playerData.matches;

            mainWindow.activeProfileName.Text = string.Format("{0} #{1} [{2}]", data.player.account.gameName, data.player.account.tagLine, data.player.server);
            mainWindow.activeProfileName.Visibility = Visibility.Visible;
            mainWindow.button_update.Visibility = Visibility.Visible;
            mainWindow.menuButtons.button_matchHistory.Visibility = Visibility.Visible;
            mainWindow.menuButtons.button_stats.Visibility = Visibility.Visible;

            if (saveSettingsFile == true) SaveSettingsFile();
            info.CreateNewPrompt(Info.Messages.ok_settings_profileLoaded, data.player.account.gameName);
        }

        public async Task DownloadProfilDataFromZero(string profileName, View.UserControls.SettingsProfileDetail detailsPanel)
        {
            info.CreateNewPrompt(Info.Messages.process_settings_downloadLoadProfileData);

            string fileRaw = await loadResources.ReadTextFile(string.Format("{0}{1}.json", profilesPath, profileName));
            if (fileRaw == null)
            {
                info.CreateNewPrompt(Info.Messages.error_settings_downloadLoadProfileDataError);
                return;
            }

            Data.PlayerData foo_profileReference = await loadResources.Deserialize(fileRaw, typeof(Data.PlayerData));
            Data.Player profileReference = foo_profileReference?.player;
            if (profileReference == null || profileReference.account == null)
            {
                info.CreateNewPrompt(Info.Messages.error_settings_downloadLoadProfileDataError);
                return;
            }

            info.UpdatePrompt(Info.Messages.process_settings_downloadProfileApiReference);
            DataType.AccountDto account = await apiData.ApiGetData(typeof(DataType.AccountDto), profileReference.server, ApiData.EndPoint.getAccountByRiotId,
                profileReference.account.gameName, profileReference.account.tagLine);
            if (account == null)
            {
                info.CreateNewPrompt(Info.Messages.error_settings_downloadProfileApiReferenceError);
                return;
            }
            profileReference.account.puuid = account.puuid;

            info.UpdatePrompt(Info.Messages.process_settings_downloadMatchList);
            List<string> matchList = await apiData.ApiGetData(typeof(List<string>), profileReference.server, ApiData.EndPoint.getListOfMatchIds,
                profileReference.account.puuid, 0, downloadMatchQuantity);
            if (matchList == null)
            {
                info.CreateNewPrompt(Info.Messages.error_settings_downloadProfileApiReferenceError);
                return;
            }

            List<DataType.MatchLao> matchesLao = new List<DataType.MatchLao>();
            bool allMatchesDownloaded = true;
            for (int i = 0; i < matchList.Count; i++)
            {
                info.UpdatePrompt(Info.Messages.process_settings_downloadMatches, i + 1, matchList.Count);
                DataType.MatchLao match = await data.GetMatch(matchList[i], profileReference);

                if (match == null)
                {
                    matchList.RemoveAt(i);
                    if (i >= 0) i -= 1;
                    allMatchesDownloaded = false;
                }
                else matchesLao.Add(match);
            }
            if (matchesLao.Count == 0)
            {
                info.CreateNewPrompt(Info.Messages.warning_settings_downloadMatchesNone);
                return;
            }

            info.UpdatePrompt(Info.Messages.process_settings_downloadSavingProfile);
            Data.PlayerData playerData = new Data.PlayerData
            {
                player = profileReference,
                historyGameIds = matchList,
                matches = matchesLao,
                dataStructVersion = data.dataStructVersion,
            };
            bool result = await SaveDataToFile(playerData, false);
            if (result == false)
            {
                info.CreateNewPrompt(Info.Messages.warning_settings_downloadMatchesFailed);
                return;
            }

            if (allMatchesDownloaded == false) info.CreateNewPrompt(Info.Messages.warning_settings_downloadMatchesNotAll);
            info.CreateNewPrompt(Info.Messages.ok_settings_downloadMatchesFinished);

            LoadProfileInformations(playerData.player.account.gameName, detailsPanel);
        }

        public async Task UpdateMatches()
        {
            if (data.historyGameIds == null || data.player == null) return;
            info.CreateNewPrompt(Info.Messages.process_settings_downloadMatchList);

            Data.Player playerRef = data.player;

            if (playerRef.account.puuid == null || playerRef.account.puuid == "")
            {
                DataType.AccountDto foo_account = await apiData.ApiGetData(typeof(DataType.AccountDto), playerRef.server, ApiData.EndPoint.getAccountByRiotId,
                    playerRef.account.gameName, playerRef.account.tagLine);
                if (foo_account == null)
                {
                    info.CreateNewPrompt(Info.Messages.error_settings_downloadProfileApiReferenceError);
                    return;
                }
                playerRef.account.puuid = foo_account.puuid;
            }

            List<string> oldList = data.historyGameIds;
            List<string> newList = await apiData.ApiGetData(typeof(List<string>), playerRef.server, ApiData.EndPoint.getListOfMatchIds,
                playerRef.account.puuid, 0, downloadMatchQuantity);
            if (newList == null)
            {
                info.CreateNewPrompt(Info.Messages.error_settings_downloadMatchListError);
                return;
            }

            List<string> matchesToDownload;
            if (oldList.Count == 0) matchesToDownload = newList;
            else matchesToDownload = newList.TakeWhile(p => p != oldList[0]).ToList();

            if (matchesToDownload.Count == 0)
            {
                info.CreateNewPrompt(Info.Messages.info_settings_downloadMatchesUpToDate);
                return;
            }

            List<DataType.MatchLao> matchesDownloaded = new List<DataType.MatchLao>();
            bool allMatchesDownloaded = true;
            for (int i = 0; i < matchesToDownload.Count; i++)
            {
                info.UpdatePrompt(Info.Messages.process_settings_downloadMatches, i + 1, matchesToDownload.Count);
                DataType.MatchLao match = await data.GetMatch(matchesToDownload[i], playerRef);
                if (match == null)
                {
                    matchesToDownload.RemoveAt(i);
                    if (i >= 0) i -= 1;
                    allMatchesDownloaded = false;
                }
                else matchesDownloaded.Add(match);
            }
            if (matchesDownloaded.Count == 0)
            {
                info.CreateNewPrompt(Info.Messages.warning_settings_downloadMatchesNone);
                return;
            }
            if (allMatchesDownloaded == false) info.CreateNewPrompt(Info.Messages.warning_settings_downloadMatchesNotAll);

            List<string> updatedMatchIdsList = oldList; updatedMatchIdsList.InsertRange(0, matchesToDownload);
            List<DataType.MatchLao> updatedMatcheslist = data.matches; updatedMatcheslist.InsertRange(0, matchesDownloaded);

            Data.PlayerData newData = new Data.PlayerData
            {
                player = playerRef,
                historyGameIds = updatedMatchIdsList,
                matches = updatedMatcheslist,
                dataStructVersion = data.dataStructVersion
            };

            bool result = await SaveDataToFile(newData, false);
            if (result == false)
            {
                info.CreateNewPrompt(Info.Messages.warning_settings_updateMatchesFailed);
                return;
            }

            data.historyGameIds = updatedMatchIdsList;
            data.matches = updatedMatcheslist;
            new MatchHistory().InitializeMatchHistory();
            info.CreateNewPrompt(Info.Messages.ok_settings_updateMatchesCompleted, matchesToDownload.Count);
        }

        public async void LoadSettingsFile()
        {
            string defaultProfile = Properties.Settings.Default.DefaultProfile;
            if (defaultProfile != "") await LoadProfileData(defaultProfile, false);

            string onlineDDversion = (await apiData.ApiGetData(typeof(List<string>), "DD", ApiData.EndPoint.DDgetVersions))[0];
            string localDDversion = "";

            if (Directory.Exists("data/"))
            {
                try
                {
                    var foo_folderNames = Directory.GetDirectories("data/").Select(Path.GetFileName).Where(p => p.StartsWith("dragontail-"));
                    var foo_folderName = foo_folderNames.FirstOrDefault();
                    if (foo_folderName == null) throw new Exception("Nie odnaleziono referencji do folderu DataDragon");

                    localDDversion = Regex.Match(foo_folderName, @"\d+(\.\d+)*$").ToString();
                    data.gameVersion = localDDversion;
                }
                catch (Exception ex)
                {
                    info.CreateNewPrompt(Info.Messages.error_settings_noDDreference, ex.Message);
                }
            }
            else info.CreateNewPrompt(Info.Messages.error_settings_noDataFolder);

            if (localDDversion == "")
            {
                info.CreateNewPrompt(Info.Messages.warning_settings_DDnotExist);
                updateDDneeded = true;
            }
            else
            {
                loadResources.LoadAllAssets();
                if (localDDversion != onlineDDversion)
                {
                    info.CreateNewPrompt(Info.Messages.info_settings_DDhasUpdate);
                    updateDDneeded = true;
                }
            }

            using (HttpClient httpClient = new HttpClient())
            {
                try
                {
                    httpClient.DefaultRequestHeaders.Add("User-Agent", "CSharp-App");
                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", gitAccesToken);

                    HttpResponseMessage response = await httpClient.GetAsync(gitReleaseUrl);
                    response.EnsureSuccessStatusCode();

                    string jsonResponse = await response.Content.ReadAsStringAsync();
                    JObject releaseData = JObject.Parse(jsonResponse);

                    updateData = new UpdateData
                    {
                        version = data.applicationVersion,
                        isUpdateNeeded = false
                    };

                    updateData.version = releaseData["tag_name"]?.ToString();
                    updateData.name = releaseData["name"]?.ToString();
                    updateData.description = releaseData["body"]?.ToString();

                    string foo_date = releaseData["published_at"]?.ToString();

                    DateTimeOffset utcDateTime = DateTimeOffset.ParseExact
                        (foo_date, "dd.MM.yyyy HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal);
                    updateData.date = utcDateTime.LocalDateTime.ToString("dd.MM.yyyy, HH:mm");

                    updateData.fileName = releaseData["assets"]?[0]?["name"].ToString();
                    updateData.url = releaseData["assets"]?[0]?["browser_download_url"]?.ToString();


                    if (updateData.version != data.applicationVersion)
                    {
                        if (string.IsNullOrEmpty(updateData.url))
                            throw new Exception("Nie udało się pobrać odnośnika do pobrania nowej wersji");

                        info.CreateNewPrompt(Info.Messages.info_settings_appHasUpdate);
                        updateData.isUpdateNeeded = true;
                    }
                    else if (File.Exists("updateProcess.bat")) File.Delete("updateProcess.bat");
                }
                catch (Exception ex)
                {
                    info.CreateNewPrompt(Info.Messages.error_settings_checkUpdatefailed, ex.Message);
                }
            }

            mainWindow.appVersion.Text = string.Format("Wersja aplikacji:        {0}", data.applicationVersion);
            mainWindow.dataVersion.Text = string.Format("Wersja danych:         {0}", data.dataStructVersion);
            mainWindow.patchVersion.Text = string.Format("Patch (DataDragon): {0}", data.gameVersion);

            mainWindow.menuButtons.button_settings.Visibility = Visibility.Visible;
        }

        public void SaveSettingsFile()
        {
            Properties.Settings.Default.DefaultProfile = data.player.account.gameName;
            Properties.Settings.Default.Save();
            info.CreateNewPrompt(Info.Messages.info_settings_settingsSaved);
        }

        public async void SaveSettingsLanguage(string lang, string resourcesLang)
        {
            Properties.Settings.Default.Language = lang;
            if(resourcesLang != null) Properties.Settings.Default.ResourcesLanguage = resourcesLang;
            Properties.Settings.Default.Save();

            if (resourcesLang != null) await loadResources.LoadAllAssets();
            info.CreateNewPrompt(Info.Messages.info_settings_LanguageSettingsSaved);
        }

        public async Task UpdateDataDragon()
        {
            mainWindow.Dispatcher.Invoke(delegate { info.CreateNewPrompt(Info.Messages.process_settings_DdupdateStarting); });

            string onlineDDversion = (await apiData.ApiGetData(typeof(List<string>), "DD", ApiData.EndPoint.DDgetVersions))[0];
            if (onlineDDversion == null)
            {
                mainWindow.Dispatcher.Invoke(delegate { info.CreateNewPrompt(Info.Messages.error_settings_DdupdateFailedToGetVersion); });
                return;
            }
            string DDurl = string.Format("https://ddragon.leagueoflegends.com/cdn/dragontail-{0}.tgz", onlineDDversion);
            string DDfilePath = string.Format("data/dragontail-{0}(temp).tgz", onlineDDversion);

            try
            {
                using (HttpClient client = new HttpClient())
                using (HttpResponseMessage response = await client.GetAsync(DDurl, HttpCompletionOption.ResponseHeadersRead))
                {
                    response.EnsureSuccessStatusCode();

                    long? totalKBytes = response.Content.Headers.ContentLength / 1000;
                    long downloadedKBytes = 0;

                    using (Stream webStream = await response.Content.ReadAsStreamAsync())
                    using (FileStream fileStream = new FileStream
                        (DDfilePath, FileMode.Create, FileAccess.Write, FileShare.None, (128 * 1024 * 1024), true))
                    {
                        byte[] buffer = new byte[128 * 1024 * 1024];
                        int bytesRead;
                        while ((bytesRead = await webStream.ReadAsync(buffer, 0, buffer.Length)) > 0)
                        {
                            await fileStream.WriteAsync(buffer, 0, bytesRead);
                            downloadedKBytes += bytesRead / 1000;

                            if (totalKBytes.HasValue)
                            {
                                double downloadedMB = (double)downloadedKBytes / 1000.0;
                                double totalMB = (double)totalKBytes / 1000.0;
                                int percentValue = (int)((double)((double)downloadedKBytes / (double)totalKBytes) * 100);

                                mainWindow.Dispatcher.Invoke(delegate
                                {
                                    info.UpdatePrompt(Info.Messages.process_settings_DdupdateDownloading,
                                    percentValue, downloadedMB, totalMB);
                                });
                            }
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                mainWindow.Dispatcher.Invoke(delegate { info.CreateNewPrompt(Info.Messages.error_settings_DdupdateFailed, ex.Message); });

                if (File.Exists(DDfilePath))
                    try { File.Delete(DDfilePath); }
                    catch (Exception ex1)
                    {
                        mainWindow.Dispatcher.Invoke(delegate
                        {
                            info.CreateNewPrompt(Info.Messages.error_settings_DdupdateBackupRemoveFailed, ex1.Message);
                        });
                    }
                return;
            }

            mainWindow.Dispatcher.Invoke(delegate { info.UpdatePrompt(Info.Messages.process_settings_DdupdateUnzipingStarted); });

            string tempFolderPath = string.Format("data/dragontail-{0}(temp)", onlineDDversion);
            if (!Directory.Exists(tempFolderPath))
            {
                Directory.CreateDirectory(tempFolderPath);
            }

            using (Stream stream = File.OpenRead(DDfilePath))
            {
                var reader = ReaderFactory.Open(stream);
                SharpCompress.Common.ExtractionOptions sharpOptions = new SharpCompress.Common.ExtractionOptions
                {
                    ExtractFullPath = true,
                    Overwrite = true
                };

                long excractedKB = 0;
                long totalKB = 0;
                while (reader.MoveToNextEntry())
                {
                    if (!reader.Entry.IsDirectory)
                    {
                        totalKB += reader.Entry.Size / 1000;
                    }
                }
                stream.Seek(0, SeekOrigin.Begin);
                reader = ReaderFactory.Open(stream);

                while (reader.MoveToNextEntry())
                {
                    if (!reader.Entry.IsDirectory)
                    {
                        reader.WriteEntryToDirectory(tempFolderPath, sharpOptions);
                        excractedKB += reader.Entry.Size / 1000;

                        double extractedMB = (double)excractedKB / 1000.0;
                        double totalMB = (double)totalKB / 1000.0;
                        int percentValue = (int)((double)((double)excractedKB / (double)totalKB) * 100);
                        mainWindow.Dispatcher.Invoke(delegate
                        {
                            info.UpdatePrompt(Info.Messages.process_settings_DdupdateUnziping,
                                percentValue, extractedMB, totalMB);
                        });
                    }
                }
            }

            mainWindow.Dispatcher.Invoke(delegate { info.UpdatePrompt(Info.Messages.process_settings_DdupdateFinishing); });

            try
            {
                if (File.Exists(DDfilePath)) File.Delete(DDfilePath);

                var directories = Directory.GetDirectories("data/");
                foreach (var dir in directories)
                {
                    string folderName = Path.GetFileName(dir);
                    if (folderName.StartsWith("dragontail-") && !folderName.EndsWith("(temp)")) Directory.Delete(dir, true);
                }

                if (Directory.Exists(tempFolderPath)) Directory.Move(tempFolderPath, tempFolderPath.Replace("(temp)", ""));
            }
            catch (Exception ex)
            {
                Directory.Delete(tempFolderPath, true);
                mainWindow.Dispatcher.Invoke(delegate { info.CreateNewPrompt(Info.Messages.error_settings_DdupdateFinishingError, ex.Message); });
            }

            mainWindow.Dispatcher.Invoke(delegate { info.CreateNewPrompt(Info.Messages.ok_settings_DdupdateFinished, onlineDDversion); });
        }

        public async Task UpdateApplication()
        {
            info.CreateNewPrompt(Info.Messages.process_settings_appUpdateStarting);

            string tempFileName = "updateData(temp).zip";
            string tempFileDir = "update(temp)";
            try
            {
                using (HttpClient client = new HttpClient())
                using (HttpResponseMessage response = await client.GetAsync(updateData.url, HttpCompletionOption.ResponseHeadersRead))
                {
                    client.DefaultRequestHeaders.Add("User-Agent", "LeagueAnalyser");
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", gitAccesToken);
                    response.EnsureSuccessStatusCode();

                    long? totalKBytes = response.Content.Headers.ContentLength / 1000;
                    long downloadedKBytes = 0;

                    using (Stream webStream = await response.Content.ReadAsStreamAsync())
                    using (FileStream fileStream = new FileStream(
                        tempFileName, FileMode.Create, FileAccess.Write, FileShare.None, 8192, true))
                    {
                        byte[] buffer = new byte[8192];
                        int bytesRead;
                        while ((bytesRead = await webStream.ReadAsync(buffer, 0, buffer.Length)) > 0)
                        {
                            await fileStream.WriteAsync(buffer, 0, bytesRead);
                            downloadedKBytes += bytesRead / 1000;

                            if (totalKBytes.HasValue)
                            {
                                int percentValue = (int)((double)((double)downloadedKBytes / (double)totalKBytes) * 100);
                                info.UpdatePrompt(Info.Messages.process_settings_appUpdateDownloading,
                                    percentValue, downloadedKBytes, totalKBytes);
                            }
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                info.CreateNewPrompt(Info.Messages.error_settings_appUpdateDownloadFail, ex.Message);

                if (File.Exists(tempFileName))
                    try { File.Delete(tempFileName); }
                    catch (Exception ex1) { info.CreateNewPrompt(Info.Messages.error_settings_appUpdateBackupFailed, ex1.Message); }
                return;
            }

            info.UpdatePrompt(Info.Messages.process_settings_appUpdateUnziping);
            try
            {
                if (Directory.Exists(tempFileDir)) Directory.Delete(tempFileDir, true);
                Directory.CreateDirectory(tempFileDir);

                ZipFile.ExtractToDirectory(tempFileName, tempFileDir);
                if (File.Exists(tempFileName)) File.Delete(tempFileName);
            }
            catch (Exception ex)
            {
                info.CreateNewPrompt(Info.Messages.error_settings_appUpdateUnzipingFailed, ex.Message);
                if (Directory.Exists(tempFileDir)) Directory.Delete(tempFileDir);
                if (File.Exists(tempFileName))
                    try { File.Delete(tempFileName); }
                    catch (Exception ex1) { info.CreateNewPrompt(Info.Messages.error_settings_appUpdateBackupFailed, ex1.Message); }
                return;
            }

            string batFile = string.Format(
                    @"@echo off
                    set APP_NAME=League Analyser.exe
                    set UPDATE_DIR={0}
                    set TEMP_DIR={0}
                    
                    echo Przygotowywanie do instalacji...
                    timeout /t 5 >nul
                    
                    echo Zamykanie aplikacji...
                    taskkill /F /IM %APP_NAME% >nul 2>&1
                    timeout /t 2 >nul

                    echo Podmienianie plików...
                    xcopy /E /Y ""%UPDATE_DIR%\*"" "".\"" >nul 2>&1

                    echo Usuwanie plików aktualizacji...
                    rmdir /s /q ""%TEMP_DIR%""

                    echo Uruchamianie nowej wersji aplikacji...
                    start """" ""%APP_NAME%""

                    exit",
                    tempFileDir);


            if (File.Exists("updateProcess.bat")) File.Delete("updateProcess.bat");
            bool result = await loadResources.SaveTextToFile(batFile, "updateProcess.bat");
            if (result == false)
            {
                info.CreateNewPrompt(Info.Messages.error_settings_appUpdateInstallerError);
                return;
            }

            info.UpdatePrompt(Info.Messages.process_settings_appUpdateReboot);
            await Task.Delay(5000);
            mainWindow.isProcessOngoing = false;
            Process.Start("updateProcess.bat");
            Environment.Exit(0);
        }
    }
}

