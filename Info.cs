using League_Analyser.View.UserControls;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace League_Analyser
{
    public class Info
    {
        private MainWindow mainWindow;
        private Queue<InfoPanel> promptsQueue = new Queue<InfoPanel>();

        public void InfoInit()
        {
            mainWindow = (MainWindow)App.Current.MainWindow;
        }

        public enum InfoType
        {
            none = 0,
            info,
            process,
            ok,
            warning,
            error
        };

        public static class Messages
        {
            public static readonly Message error_prompt_listEmpty = new Message
            (
                InfoType.error,
                Prompts.prompt_listEmpty_title,
                Prompts.prompt_listEmpty_msg
            );
            public static readonly Message error_prompt_argsInvalid = new Message
            (
                InfoType.error,
                Prompts.prompt_argsInvalid_title,
                Prompts.prompt_argsInvalid_msg
            );

            public static readonly Message error_api_timeout = new Message
            (
                InfoType.error,
                Prompts.api_timeout_title,
                Prompts.api_timeout_msg
            );
            public static readonly Message warning_api_tooManyRequests = new Message
            (
                InfoType.warning,
                Prompts.api_tooManyRequests_title,
                Prompts.api_tooManyRequests_msg
            );
            public static readonly Message process_api_awaiting = new Message
            (
                InfoType.process,
                Prompts.api_waiting_title,
                Prompts.api_waiting_msg
            );
            public static readonly Message process_api_reconnecting = new Message
            (
                InfoType.process,
                Prompts.api_reconnecting_title,
                Prompts.api_reconnecting_msg
            );
            public static readonly Message error_api_euneOnFire = new Message
            (
                InfoType.error,
                Prompts.api_euneOnFire_title,
                Prompts.api_euneOnFire_msg
            );
            public static readonly Message error_api_apiError = new Message
            (
                InfoType.error,
                Prompts.api_error,
                "{0}"
            );
            public static readonly Message error_api_unexpectedError = new Message
            (
                InfoType.error,
                Prompts.api_unexpectedError,
                "{0}"
            );

            public static readonly Message error_loadResources_deserializeError = new Message
            (
                InfoType.error,
                Prompts.loadResources_deserialyze,
                "{0}"
            );
            public static readonly Message error_loadResources_serializeError = new Message
            (
                InfoType.error,
                Prompts.loadResources_serialyze,
                "{0}"
            );
            public static readonly Message error_loadResources_loadFileError = new Message
            (
                InfoType.error,
                Prompts.loadResources_loadFile,
                "{0}"
            );
            public static readonly Message error_loadResources_saveFileError = new Message
            (
                InfoType.error,
                Prompts.loadResources_saveFile,
                "{0}"
            );
            public static readonly Message error_loadResources_loadImageError = new Message
            (
                InfoType.error,
                Prompts.loadResources_loadImage,
                "{0}"
            );

            public static readonly Message process_settings_loading = new Message
            (
                InfoType.process,
                Prompts.settings_downloading,
                ""
            );
            public static readonly Message warning_settings_profileNotAdded = new Message
            (
                InfoType.warning,
                Prompts.profile_notAdded,
                ""
            );
            public static readonly Message ok_settings_profileAdded = new Message
            (
                InfoType.ok,
                Prompts.profile_added,
                ""
            );
            public static readonly Message error_settings_noReferenceToProfile = new Message
            (
                InfoType.error,
                Prompts.loadResources_saveFile,
                Prompts.profile_noReference
            );
            public static readonly Message error_settings_loadProfileListError = new Message
            (
                InfoType.error,
                Prompts.profile_listError,
                "{0}"
            );
            public static readonly Message error_settings_loadProfileDetailsError = new Message
            (
                InfoType.error,
                Prompts.profile_readError,
                "{0}"
            );
            public static readonly Message error_settings_deleteProfilePathError = new Message
            (
                InfoType.error,
                Prompts.profile_deleteError,
                Prompts.profile_pathNotExist
            );
            public static readonly Message error_settings_deleteProfileError = new Message
            (
                InfoType.error,
                Prompts.profile_deleteError,
                "{0}"
            );
            public static readonly Message ok_settings_profileDeleted = new Message
            (
                InfoType.ok,
                Prompts.profile_deleted,
                ""
            );
            public static readonly Message process_settings_loadProfile = new Message
            (
                InfoType.process,
                Prompts.settings_reading,
                ""
            );
            public static readonly Message error_settings_loadProfileError = new Message
            (
                InfoType.error,
                Prompts.profile_notLoaded_title,
                Prompts.profile_notLoaded_msg
            );
            public static readonly Message ok_settings_profileLoaded = new Message
            (
                InfoType.ok,
                Prompts.profile_loadedSucessfully_title,
                Prompts.profile_loadedSucessfully_msg
            );
            public static readonly Message error_settings_saveSettingsError = new Message
            (
                InfoType.error,
                Prompts.settings_saveError_title,
                Prompts.settings_saveError_msg
            );
            public static readonly Message info_settings_settingsSaved = new Message
            (
                InfoType.info,
                Prompts.settings_saved,
                ""
            );
            public static readonly Message info_settings_LanguageSettingsSaved = new Message
            (
                InfoType.info,
                Prompts.settings_saved,
                Prompts.settings_languageSaved
            );
            public static readonly Message error_settings_loadSettingsError = new Message
            (
                InfoType.error,
                Prompts.settings_loadFailed_title,
                Prompts.settings_loadFailed_msg
            );
            public static readonly Message error_settings_noDDreference = new Message
            (
                InfoType.error,
                Prompts.dd_noReference,
                "{0}"
            );
            public static readonly Message error_settings_noDataFolder = new Message
            (
                InfoType.error,
                Prompts.dd_noReference,
                Prompts.dd_noFolder
            );
            public static readonly Message warning_settings_DDnotExist = new Message
            (
                InfoType.warning,
                Prompts.dd_notExist,
                Prompts.dd_update
            );
            public static readonly Message info_settings_DDhasUpdate = new Message
            (
                InfoType.info,
                Prompts.dd_hasUpdate,
                Prompts.dd_update
            );
            public static readonly Message error_settings_checkUpdatefailed = new Message
            (
                InfoType.error,
                Prompts.update_checkFailed,
                "{0}"
            );
            public static readonly Message info_settings_appHasUpdate = new Message
            (
                InfoType.info,
                Prompts.update_isUpdate_title,
                Prompts.update_isUpdate_msg
            );
            public static readonly Message process_settings_DdupdateStarting = new Message
            (
                InfoType.process,
                Prompts.dd_downloading,
                Prompts.update_init
            );
            public static readonly Message error_settings_DdupdateFailedToGetVersion = new Message
            (
                InfoType.error,
                Prompts.dd_updateError,
                Prompts.dd_noVersionReference
            );
            public static readonly Message process_settings_DdupdateDownloading = new Message
            (
                InfoType.process,
                Prompts.dd_downloading,
                Prompts.update_downloadProgressMb
            );
            public static readonly Message error_settings_DdupdateFailed = new Message
            (
                InfoType.error,
                Prompts.dd_updateError,
                "{0}"
            );
            public static readonly Message error_settings_DdupdateBackupRemoveFailed = new Message
            (
                InfoType.error,
                Prompts.dd_deleteUpdateFilesError,
                "{0}"
            );
            public static readonly Message process_settings_DdupdateUnzipingStarted = new Message
            (
                InfoType.process,
                Prompts.dd_downloading,
                Prompts.update_unzipInit
            );
            public static readonly Message process_settings_DdupdateUnziping = new Message
            (
                InfoType.process,
                Prompts.dd_downloading,
                Prompts.update_unzipMb
            );
            public static readonly Message process_settings_DdupdateFinishing = new Message
            (
                InfoType.process,
                Prompts.dd_downloading,
                Prompts.update_finishing
            );
            public static readonly Message error_settings_DdupdateFinishingError = new Message
            (
                InfoType.error,
                Prompts.dd_updateRevert,
                "{0}"
            );
            public static readonly Message ok_settings_DdupdateFinished = new Message
            (
                InfoType.ok,
                Prompts.dd_updateFinished_title,
                Prompts.dd_updateFinished_msg
            );
            public static readonly Message process_settings_appUpdateStarting = new Message
            (
                InfoType.process,
                Prompts.update_updateProcess,
                Prompts.update_downloading
            );
            public static readonly Message process_settings_appUpdateDownloading = new Message
            (
                InfoType.process,
                Prompts.update_updateProcess,
                Prompts.update_downloadProgressKb
            );
            public static readonly Message error_settings_appUpdateDownloadFail = new Message
            (
                InfoType.error,
                Prompts.update_downloadFailed,
                "{0}"
            );
            public static readonly Message error_settings_appUpdateBackupFailed = new Message
            (
                InfoType.error,
                Prompts.update_deleteUpdateFilesError,
                "{0}"
            );
            public static readonly Message process_settings_appUpdateUnziping = new Message
            (
                InfoType.process,
                Prompts.update_updateProcess,
                Prompts.update_unzipInit
            );
            public static readonly Message error_settings_appUpdateUnzipingFailed = new Message
            (
                InfoType.error,
                Prompts.update_unzipFailed,
                "{0}"
            );
            public static readonly Message error_settings_appUpdateInstallerError = new Message
            (
                InfoType.error,
                Prompts.update_installerError_title,
                Prompts.update_installerError_msg
            );
            public static readonly Message process_settings_appUpdateReboot = new Message
            (
                InfoType.process,
                Prompts.update_updateProcess,
                Prompts.update_rebootInfo
            );
            public static readonly Message process_settings_downloadLoadProfileData = new Message
            (
                InfoType.process,
                Prompts.settings_downloading,
                Prompts.profile_loadingData
            );
            public static readonly Message error_settings_downloadLoadProfileDataError = new Message
            (
                InfoType.error,
                Prompts.load_loadingError,
                Prompts.profile_loadError
            );
            public static readonly Message process_settings_downloadProfileApiReference = new Message
            (
                InfoType.process,
                Prompts.settings_downloading,
                Prompts.profile_load
            );
            public static readonly Message error_settings_downloadProfileApiReferenceError = new Message
            (
                InfoType.error,
                Prompts.load_downloadingError,
                Prompts.profile_downloadError
            );
            public static readonly Message process_settings_downloadMatchList = new Message
            (
                InfoType.process,
                Prompts.settings_downloading,
                Prompts.match_downloadingList
            );
            public static readonly Message process_settings_downloadMatches = new Message
            (
                InfoType.process,
                Prompts.settings_downloading,
                Prompts.match_download
            );
            public static readonly Message error_settings_downloadMatchListError = new Message
            (
                InfoType.error,
                Prompts.load_downloadingError,
                Prompts.match_downloadListFailed
            );
            public static readonly Message warning_settings_downloadMatchesFailed = new Message
            (
                InfoType.warning,
                Prompts.profile_saveError,
                Prompts.match_downloadFromZeroFailed
            );
            public static readonly Message warning_settings_updateMatchesFailed = new Message
            (
                InfoType.warning,
                Prompts.update_saveError_title,
                Prompts.update_saveError_msg
            );
            public static readonly Message warning_settings_downloadMatchesNone = new Message
            (
                InfoType.warning,
                Prompts.load_downloadingError,
                Prompts.match_downloadFailed
            );
            public static readonly Message process_settings_downloadSavingProfile = new Message
            (
                InfoType.process,
                Prompts.settings_downloading,
                Prompts.profile_save
            );
            public static readonly Message warning_settings_downloadMatchesNotAll = new Message
            (
                InfoType.warning,
                Prompts.load_downloadingError,
                Prompts.match_notAllDownloaded
            );
            public static readonly Message ok_settings_downloadMatchesFinished = new Message
            (
                InfoType.ok,
                Prompts.profile_updatedSucessfully_title,
                Prompts.profile_updatedSucessfully_msg
            );
            public static readonly Message info_settings_downloadMatchesUpToDate = new Message
            (
                InfoType.info,
                Prompts.match_dataUpToDate,
                ""
            );
            public static readonly Message ok_settings_updateMatchesCompleted = new Message
            (
                InfoType.ok,
                Prompts.match_updatedSucessfully_title,
                Prompts.match_updatedSucessfully_msg
            );

            public static readonly Message process_matchHistory_loading = new Message
            (
                InfoType.process,
                Prompts.load_loading,
                ""
            );
            public static readonly Message process_terminateProcess = new Message
            (
                InfoType.none,
                "",
                ""
            );
            public static readonly Message warning_matchHistory_sourcesNotLoaded = new Message
            (
                InfoType.warning,
                Prompts.load_loadingError,
                Prompts.match_dataAllNotLoaded
            );
            public static readonly Message warning_matchHistory_playerSourcesNotLoaded = new Message
            (
                InfoType.warning,
                Prompts.load_loadingError,
                Prompts.match_graphicAllNotLoaded
            );
            public static readonly Message warning_matchHistory_passedMatchIsNotFound = new Message
            (
                InfoType.warning,
                Prompts.load_loadingError,
                Prompts.match_notFound
            );
            public static readonly Message warning_matchHistory_playerStatsNotLoaded = new Message
            (
                InfoType.warning,
                Prompts.load_loadingError,
                Prompts.match_statAllNotLoaded
            );
            public static readonly Message process_timeline_downloading = new Message
            (
                InfoType.process,
                Prompts.load_loading,
                ""
            );
            public static readonly Message error_timeline_downloadFailed = new Message
            (
                InfoType.error,
                Prompts.load_loadingError,
                Prompts.match_detailsNotLoaded
            );
            public static readonly Message error_timeline_imageNotFound = new Message
            (
                InfoType.error,
                Prompts.match_graphicError_title,
                Prompts.match_graphicError_msg
            );
            public static readonly Message error_timeline_eventDataEmpty = new Message
            (
                InfoType.error,
                Prompts.load_loadingError,
                Prompts.match_emptyTimeline
            );
            public class Message
            {
                public InfoType type { get; }
                public string title { get; }
                public string message { get; }

                public Message(InfoType Type, string Title, string Message)
                {
                    type = Type;
                    title = Title;
                    message = Message;
                }
            }
        }

        public class PromptParameters
        {
            public string title { get; set; }
            public string message { get; set; }
            public LinearGradientBrush color { get; set; }
            public BitmapImage icon { get; set; }
            public string iconName { get; set; }
            public int time { get; set; }
            public Visibility closeVisibility { get; set; }
        }

        public static LinearGradientBrush GetGradientFromColor(Color color)
        {
            double darkenFactor = 100;
            byte Darken(byte value) => (byte)Math.Max(0, value - darkenFactor);
            return new LinearGradientBrush
            {
                StartPoint = new Point(0.5, 0),
                EndPoint = new Point(0.5, 1),
                GradientStops = new GradientStopCollection
                {
                    new GradientStop(Color.FromArgb(255, color.R, color.G, color.B), 0),
                    new GradientStop(Color.FromArgb(255, Darken(color.R), Darken(color.G), Darken(color.B)), 1)
                }
            };
        }

        public static Dictionary<InfoType, PromptParameters> PromptValues = new Dictionary<InfoType, PromptParameters>
        {
            {
                InfoType.none, new PromptParameters
                {
                    color = GetGradientFromColor(Color.FromArgb(255, 0, 0, 0)),
                    icon = null,
                    time = 3000,
                    closeVisibility = Visibility.Visible
                }
            },
            {
                InfoType.info, new PromptParameters
                {
                    color = GetGradientFromColor(Color.FromArgb(255, 23, 133, 212)),
                    iconName="info_icon_info.png",
                    time = 3000,
                    closeVisibility = Visibility.Visible
                }
            },
            {
                InfoType.process, new PromptParameters
                {
                    color = GetGradientFromColor(Color.FromArgb(255, 23, 133, 212)),
                    iconName="info_icon_process.png",
                    time = int.MaxValue, //Quasi-infinity
                    closeVisibility = Visibility.Collapsed
                }
            },
            {
                InfoType.ok, new PromptParameters
                {
                    color = GetGradientFromColor(Color.FromArgb(255, 9, 237, 17)),
                    iconName="info_icon_ok.png",
                    time = 3000,
                    closeVisibility = Visibility.Visible
                }
            },
            {
                InfoType.warning, new PromptParameters
                {
                    color = GetGradientFromColor(Color.FromArgb(255, 232, 169, 9)),
                    iconName="info_icon_warning.png",
                    time = 3000,
                    closeVisibility = Visibility.Visible
                }
            },
            {
                InfoType.error, new PromptParameters
                {
                    color =  GetGradientFromColor(Color.FromArgb(255, 255, 0, 0)),
                    iconName="info_icon_error.png",
                    time = 5000,
                    closeVisibility = Visibility.Visible
                }
            }
        };

        // Creates new prompt that is added to queue to be later displayed on main window.
        public void CreateNewPrompt(Messages.Message promptData, params object[] args)
        {
            PromptParameters parameters = PromptValues.GetValueOrDefault(promptData.type);
            parameters.title = promptData.title;
            parameters.icon = LoadResources.LoadImage(parameters.iconName, LoadResources.ImagePath_t.resources, true).image;

            try
            {
                if (args == null || args.Length == 0) parameters.message = promptData.message;
                else parameters.message = string.Format(promptData.message, args);
            }
            catch (Exception) { CreateNewPrompt(Messages.error_prompt_argsInvalid, promptData.title); return; }

            if (promptData.type == InfoType.none)
            {
                if (promptsQueue.Count > 0)
                {
                    promptsQueue.Peek().ClosePrompt(null, null);
                    promptsQueue.Clear();
                }
            }
            else
            {
                InfoPanel infoPanel = new InfoPanel(this, parameters);
                Grid.SetRow(infoPanel, 1);
                Grid.SetZIndex(infoPanel, 999);

                promptsQueue.Enqueue(infoPanel);
                ManagePrompt(false);
            }
        }

        // Manage adding/removing prompts from screen. Called when prompt is created and when actual prompt is removed from screen.
        public void ManagePrompt(bool isClosing)
        {
            if (promptsQueue.Count > 0)
            {
                if (isClosing == true)
                {
                    promptsQueue.Dequeue();

                    if (promptsQueue.Count > 0)
                    {
                        InfoPanel panel = promptsQueue.Peek();
                        mainWindow.mainGrid.Children.Add(panel);
                        panel.StartPrompt();
                    }
                }
                else
                {
                    if (promptsQueue.Count == 1)
                    {
                        InfoPanel panel = promptsQueue.Peek();
                        mainWindow.mainGrid.Children.Add(panel);
                        panel.StartPrompt();
                    }
                    else
                    {
                        InfoPanel activePrompt = promptsQueue.Peek();
                        InfoPanel lastPrompt = promptsQueue.ToArray()[promptsQueue.Count - 1];

                        if (lastPrompt.buttonClose.Visibility != Visibility.Collapsed)
                        {
                            var oldList = promptsQueue.ToArray();
                            Queue<InfoPanel> newList = new Queue<InfoPanel>();

                            if (activePrompt.buttonClose.Visibility == Visibility.Collapsed)
                            {
                                newList.Enqueue(activePrompt);
                                activePrompt.ClosePrompt(null, null);
                            }

                            foreach (InfoPanel prompt in oldList)
                            {
                                if (prompt.buttonClose.Visibility != Visibility.Collapsed) newList.Enqueue(prompt);
                            }
                            promptsQueue = newList;
                        }
                    }
                }
            }
        }

        // Update prompt's message content with given parameters.
        public void UpdatePrompt(Messages.Message promptData, params object[] args)
        {
            if (promptsQueue.Count > 0)
            {
                InfoPanel panel = promptsQueue.Peek();
                if (panel.buttonClose.Visibility == Visibility.Collapsed)
                {
                    try
                    {
                        if (args == null || args.Length == 0) panel.textBlock_message.Text = promptData.message;
                        else panel.textBlock_message.Text = string.Format(promptData.message, args);
                    }
                    catch (Exception) { CreateNewPrompt(Messages.error_prompt_argsInvalid, promptData.title); return; }
                }
            }
            else CreateNewPrompt(Messages.error_prompt_listEmpty);
        }
    }
}
