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
            public static readonly Message error_updatePrompt_listEmpty = new Message
            (
                InfoType.error,
                "Wystąpił błąd poczas próby akualizacji powiadomienia",
                "Lista powiadomień jest pusta"
            );
            public static readonly Message error_prompt_argsInvalid = new Message
            (
                InfoType.error,
                "Wystąpił błąd poczas tworzenia powiadomienia",
                "Przekazane argumenty nie pasują do formatu komunikatu \"{0}\""
            );

            public static readonly Message error_api_timeout = new Message
            (
                InfoType.error,
                "Wystąpił błąd połączenia z API",
                "Czas połączenia przekroczył żądany limit"
            );
            public static readonly Message warning_api_tooManyRequests = new Message
            (
                InfoType.warning,
                "Nastąpiło przeciążenie serwera z powodu zbyt dużej liczby zapytań",
                "Program oczekuje na ponowną możliwość połączenia"
            );
            public static readonly Message process_api_awaiting = new Message
            (
                InfoType.process,
                "Trwa pobieranie danych...",
                "Oczekiwanie na ponowne połączenie"
            );
            public static readonly Message process_api_reconnecting = new Message
            (
                InfoType.process,
                "Trwa pobieranie danych...",
                "Próba ponownego połączenia"
            );
            public static readonly Message error_api_euneOnFire = new Message
            (
                InfoType.error,
                "Wystąpił błąd połączenia z API",
                "Trwa wspólne posiedzenie przy ognisku w serwerowni, kiełbaski zapewnione przez Riot Games"
            );
            public static readonly Message error_api_apiError = new Message
            (
                InfoType.error,
                "Wystąpił błąd połączenia z API",
                "{0}"
            );
            public static readonly Message error_api_unexpectedError = new Message
            (
                InfoType.error,
                "Wystąpił nieoczekiwany błąd przy próbie łączenia z serwerem",
                "{0}"
            );

            public static readonly Message error_loadResources_deserializeError = new Message
            (
                InfoType.error,
                "Wystąpił błąd deserializacji danych",
                "{0}"
            );
            public static readonly Message error_loadResources_serializeError = new Message
            (
                InfoType.error,
                "Wystąpił błąd podczas serializacji danych",
                "{0}"
            );
            public static readonly Message error_loadResources_loadFileError = new Message
            (
                InfoType.error,
                "Nie udało się załadować danych obiektu",
                "{0}"
            );
            public static readonly Message error_loadResources_saveFileError = new Message
            (
                InfoType.error,
                "Nie udało się zapisać danych do pliku",
                "{0}"
            );
            public static readonly Message error_loadResources_loadImageError = new Message
            (
                InfoType.error,
                "Nie udało się odczytać grafiki",
                "{0}"
            );

            public static readonly Message process_settings_loading = new Message
            (
                InfoType.process,
                "Trwa pobieranie danych...",
                ""
            );
            public static readonly Message warning_settings_profileNotAdded = new Message
            (
                InfoType.warning,
                "Nie udało się dodać nowego profilu",
                ""
            );
            public static readonly Message ok_settings_profileAdded = new Message
            (
                InfoType.ok,
                "Pomyślnie dodano nowy profil",
                ""
            );
            public static readonly Message error_settings_noReferenceToProfile = new Message
            (
                InfoType.error,
                "Nie udało się zapisać danych do pliku",
                "Brak danych odnośnie nazwy użytkownika"
            );
            public static readonly Message error_settings_loadProfileListError = new Message
            (
                InfoType.error,
                "Wystąpił błąd podczas pobierania listy profili",
                "{0}"
            );
            public static readonly Message error_settings_loadProfileDetailsError = new Message
            (
                InfoType.error,
                "Wystąpił błąd odczytu pliku profilu",
                "{0}"
            );
            public static readonly Message error_settings_deleteProfilePathError = new Message
            (
                InfoType.error,
                "Nie udało się usunąć profilu",
                "Ścieżka {0} nie istnieje"
            );
            public static readonly Message error_settings_deleteProfileError = new Message
            (
                InfoType.error,
                "Nie udało się usunąć profilu",
                "{0}"
            );
            public static readonly Message ok_settings_profileDeleted = new Message
            (
                InfoType.ok,
                "Pomyślnie usunięto profil",
                ""
            );
            public static readonly Message process_settings_loadProfile = new Message
            (
                InfoType.process,
                "Trwa odczytywanie danych...",
                ""
            );
            public static readonly Message error_settings_loadProfileError = new Message
            (
                InfoType.error,
                "Wystąpił błąd podczas odczytywannia danych",
                "Nie udało się odczytać danych gracza"
            );
            public static readonly Message ok_settings_profileLoaded = new Message
            (
                InfoType.ok,
                "Pomyślnie załadowano profil gracza",
                "Witaj, {0}!"
            );
            public static readonly Message error_settings_saveSettingsError = new Message
            (
                InfoType.error,
                "Wystąpił błąd podczas próby zapisania ustawiń",
                "Nie udało się zapisać pliku"
            );
            public static readonly Message info_settings_settingsSaved = new Message
            (
                InfoType.info,
                "Zapisano ustawienia",
                ""
            );
            public static readonly Message error_settings_loadSettingsError = new Message
            (
                InfoType.error,
                "Wystąpił błąd podczas próby załadowania ustawień",
                "Nie udało się odczytać pliku"
            );
            public static readonly Message error_settings_noDDreference = new Message
            (
                InfoType.error,
                "Brak odniesienia do wersji plików DataDragon",
                "{0}"
            );
            public static readonly Message error_settings_noDataFolder = new Message
            (
                InfoType.error,
                "Brak odniesienia do wersji plików DataDragon",
                "Nie odnaleziono folderu \"data\""
            );
            public static readonly Message warning_settings_DDnotExist = new Message
            (
                InfoType.warning,
                "Nie znaleziono pliku DataDragon",
                "Przejdź do ustawień i wykonaj aktualizację danych zasobów"
            );
            public static readonly Message info_settings_DDhasUpdate = new Message
            (
                InfoType.info,
                "Dostępna aktualizacja danych DataDragon",
                "Przejdź do ustawień i wykonaj aktualizację danych zasobów"
            );
            public static readonly Message error_settings_checkUpdatefailed = new Message
            (
                InfoType.error,
                "Nie udało się sprawdzić dostępności nowej wersji aplikacji",
                "{0}"
            );
            public static readonly Message info_settings_appHasUpdate = new Message
            (
                InfoType.info,
                "Dostępna jest nowa wersja aplikacji",
                "Przejdź do ustawień aby dokonać aktualizacji"
            );
            public static readonly Message process_settings_DdupdateStarting = new Message
            (
                InfoType.process,
                "Trwa pobieranie pliku DataDragon...",
                "Inicjalizacja pobierania..."
            );
            public static readonly Message error_settings_DdupdateFailedToGetVersion = new Message
            (
                InfoType.error,
                "Wystąpił błąd podczas pobierania aktualizacji danych źródłowych",
                "Nie udało się pobrać wartości najnowszej wersji"
            );
            public static readonly Message process_settings_DdupdateDownloading = new Message
            (
                InfoType.process,
                "Trwa pobieranie pliku DataDragon...",
                "Pobrano {0}% ({1:0.0} z {2:0.0} MB)"
            );
            public static readonly Message error_settings_DdupdateFailed = new Message
            (
                InfoType.error,
                "Wystąpił błąd podczas pobierania aktualizacji danych źródłowych",
                "{0}"
            );
            public static readonly Message error_settings_DdupdateBackupRemoveFailed = new Message
            (
                InfoType.error,
                "Nie udało się usunąć pobranych danych szczątkowych pliku DataDragon",
                "{0}"
            );
            public static readonly Message process_settings_DdupdateUnzipingStarted = new Message
            (
                InfoType.process,
                "Trwa pobieranie pliku DataDragon...",
                "Rozpakowywanie pliku (obliczanie rozmiaru)..."
            );
            public static readonly Message process_settings_DdupdateUnziping = new Message
            (
                InfoType.process,
                "Trwa pobieranie pliku DataDragon...",
                "Rozpakowano {0}% ({1:0.0} z {2:0.0} MB)"
            );
            public static readonly Message process_settings_DdupdateFinishing = new Message
            (
                InfoType.process,
                "Trwa pobieranie pliku DataDragon...",
                "Kończenie aktualizacji..."
            );
            public static readonly Message error_settings_DdupdateFinishingError = new Message
            (
                InfoType.error,
                "Wystąpił błąd podczas próby aktualizacji danych źródłowych. Wycofano zmiany",
                "{0}"
            );
            public static readonly Message ok_settings_DdupdateFinished = new Message
            (
                InfoType.ok,
                "Pomyślnie ukończono aktualizację danych źródłowych",
                "Aktualna wersja to {0}"
            );
            public static readonly Message process_settings_appUpdateStarting = new Message
            (
                InfoType.process,
                "Trwa aktualizacja aplikacji...",
                "Pobieranie plików aktualizacji..."
            );
            public static readonly Message process_settings_appUpdateDownloading = new Message
            (
                InfoType.process,
                "Trwa aktualizacja aplikacji...",
                "Pobrano {0}% ({1:0.0} z {2:0.0} kB)"
            );
            public static readonly Message error_settings_appUpdateDownloadFail = new Message
            (
                InfoType.error,
                "Wystąpił błąd podczas pobierania aktualizacji aplikacji",
                "{0}"
            );
            public static readonly Message error_settings_appUpdateBackupFailed = new Message
            (
                InfoType.error,
                "Nie udało się usunąć pobraych danych szczątkowych pliku aktualizacji",
                "{0}"
            );
            public static readonly Message process_settings_appUpdateUnziping = new Message
            (
                InfoType.process,
                "Trwa aktualizacja aplikacji...",
                "Rozpakowywanie pliku..."
            );
            public static readonly Message error_settings_appUpdateUnzipingFailed = new Message
            (
                InfoType.error,
                "Wystąpił błąd podczas próby rozpakowywania pliku aktualizacji",
                "{0}"
            );
            public static readonly Message error_settings_appUpdateInstallerError = new Message
            (
                InfoType.error,
                "Wystąpił błąd podczas próby aktualizacji aplikacji",
                "Nie udało się utworzyć pliku instalacyjnego"
            );
            public static readonly Message process_settings_appUpdateReboot = new Message
            (
                InfoType.process,
                "Trwa aktualizacja aplikacji...",
                "Za chwilę nastąpi zamknięcie aplikacji i uruchomienie instalatora"
            );
            public static readonly Message process_settings_downloadLoadProfileData = new Message
            (
                InfoType.process,
                "Trwa pobieranie danych...",
                "Ładowanie danych z profilu gracza..."
            );
            public static readonly Message error_settings_downloadLoadProfileDataError = new Message
            (
                InfoType.error,
                "Wystąpił błąd podczas pobierania danych",
                "Nie udało się odczytać danych gracza"
            );
            public static readonly Message process_settings_downloadProfileApiReference = new Message
            (
                InfoType.process,
                "Trwa pobieranie danych...",
                "Pobieranie danych gracza..."
            );
            public static readonly Message error_settings_downloadProfileApiReferenceError = new Message
            (
                InfoType.error,
                "Wystąpił błąd podczas pobierania danych",
                "Nie udało się pobrać danych gracza"
            );
            public static readonly Message process_settings_downloadMatchList = new Message
            (
                InfoType.process,
                "Trwa pobieranie danych...",
                "Pobieranie listy rozgrywek..."
            );
            public static readonly Message process_settings_downloadMatches = new Message
            (
                InfoType.process,
                "Trwa pobieranie danych...",
                "Pobieranie meczu {0} z {1}..."
            );
            public static readonly Message error_settings_downloadMatchListError = new Message
            (
                InfoType.error,
                "Wystąpił błąd podczas pobierania danych",
                "Nie udało się pobrać listy rozgrywek"
            );
            public static readonly Message warning_settings_downloadMatchesFailed = new Message
            (
                InfoType.warning,
                "Wystąpił błąd podczas zapisywania profilu",
                "Proces pobierania danych od zera nie zakończył się pomyślnie"
            );
            public static readonly Message warning_settings_updateMatchesFailed = new Message
            (
                InfoType.warning,
                "Wystąpił błąd podczas zapisu danych",
                "Nie ukończono procesu aktualizacji"
            );
            public static readonly Message warning_settings_downloadMatchesNone = new Message
            (
                InfoType.warning,
                "Wystąpił błąd podczas pobierania danych",
                "Nie udało się pobrać żadnej rozgrywki"
            );
            public static readonly Message process_settings_downloadSavingProfile = new Message
            (
                InfoType.process,
                "Trwa pobieranie danych...",
                "Zapisywanie profilu..."
            );
            public static readonly Message warning_settings_downloadMatchesNotAll = new Message
            (
                InfoType.warning,
                "Wystąpił błąd pobierania",
                "Nie udało się pobrać wszystkich meczy z serwera"
            );
            public static readonly Message ok_settings_downloadMatchesFinished = new Message
            (
                InfoType.ok,
                "Pomyślnie zaktualizowano profil gracza",
                "Można teraz załadować profil"
            );
            public static readonly Message info_settings_downloadMatchesUpToDate = new Message
            (
                InfoType.info,
                "Dane są aktualne",
                ""
            );
            public static readonly Message ok_settings_updateMatchesCompleted = new Message
            (
                InfoType.ok,
                "Pomyslnie zaktualizowano listę gier",
                "Pobrano {0} nowych rozgrywek"
            );

            public static readonly Message process_matchHistory_loading = new Message
            (
                InfoType.process,
                "Trwa ładowanie...",
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
                "Wystąpił błąd podczas ładowania danych",
                "Dla {0} rozgrywek nie udało się w pełni załadować danych"
            );
            public static readonly Message warning_matchHistory_playerSourcesNotLoaded = new Message
            (
                InfoType.warning,
                "Wystąpił błąd podczas ładowania danych",
                "Dla {0} graczy nie udało się w pełni załadować grafiki"
            );
            public static readonly Message warning_matchHistory_passedMatchIsNotFound = new Message
            (
                InfoType.warning,
                "Wystąpił błąd podczas ładowania rozgrywki",
                "Nie znaleziono danych dla tego meczu"
            );
            public static readonly Message warning_matchHistory_playerStatsNotLoaded = new Message
            (
                InfoType.warning,
                "Wystąpił błąd podczas ładowania danych",
                "Dla {0} graczy nie udało się załadować statystyk"
            );
            public static readonly Message process_timeline_downloading = new Message
            (
                InfoType.process,
                "Trwa pobieranie danych...",
                ""
            );
            public static readonly Message error_timeline_downloadFailed = new Message
            (
                InfoType.error,
                "Wystąpił błąd podczas pobierania danych Timeline",
                "Nie udało się pobrać szczegółów rozgrywki dla tej gry"
            );
            public static readonly Message error_timeline_imageNotFound = new Message
            (
                InfoType.error,
                "Wystąpił błąd podczas ładowania grafiki",
                "Nie udało się odnaleźć pliku"
            );
            public static readonly Message error_timeline_eventDataEmpty = new Message
            (
                InfoType.error,
                "Wystąpił błąd podczas ładowania danych",
                "Zwrócono pusty obiekt danych dotyczących wydarzenia"
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
            else CreateNewPrompt(Messages.error_updatePrompt_listEmpty);
        }
    }
}
