using System.Globalization;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows;

namespace League_Analyser
{
    public partial class MainWindow : Window
    {
        public Info info = new Info();
        public Data data = new Data();
        public LoadResources loadResources = new LoadResources();
        public ApiData apiData = new ApiData();
        public Settings settings = new Settings();
        public MatchHistory matchHistory = new MatchHistory();

        public bool isProcessOngoing = false;
        public string language;

        public MainWindow()
        {
            language = Properties.Settings.Default.Language;

            Thread.CurrentThread.CurrentUICulture = new CultureInfo(language);


            InitializeComponent();

            this.Closing += MainWindow_Closing;
            if (!Directory.Exists("data/player")) Directory.CreateDirectory("data/player");

            info.InfoInit();
            apiData.ApiDataInit();
            loadResources.LoadResourcesInit();
            data.DataInit();
            settings.SettingsInit();
            matchHistory.MatchHistoryInit();

            settings.LoadSettingsFile();
        }

        // Handle attepmt to close application during some process ongoing.
        private void MainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (isProcessOngoing == true)
            {
                var result = MessageBox.Show
                (
                    "Obecnie trwa proces pobierania danych, zamknięcie aplikacji może spowodować uszkodzenie plików." +
                    "\nCzy na pewno chcesz teraz zamknąć aplikację?",
                      "Potwierdzenie zamknięcia",
                      MessageBoxButton.YesNo,
                      MessageBoxImage.Warning
                );

                if (result == MessageBoxResult.No)
                {
                    e.Cancel = true;
                }
            }
        }
    }
}