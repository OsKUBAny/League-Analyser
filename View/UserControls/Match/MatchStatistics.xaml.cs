using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace League_Analyser.View.UserControls
{
    public partial class MatchStatistics : UserControl
    {
        private MainWindow mainWindow;
        private Info info;
        private Data data;

        private Storyboard animation_slideIn;
        private Storyboard animation_slideOut;
        private DataType.MatchLao match;
        private MatchDetailedStats matchDetailedStats;
        private Timeline timelineControl;

        private bool modeTimeline = false;

        public MatchStatistics(string gameId)
        {
            InitializeComponent();

            mainWindow = (MainWindow)App.Current.MainWindow;
            info = mainWindow.info;
            data = mainWindow.data;

            animation_slideIn = (Storyboard)this.Resources["SlideIn"];
            animation_slideOut = (Storyboard)this.Resources["SlideOut"];

            match = data.matches.Find(p => p.preview.matchId == gameId);
            if (match == null)
            {
                info.CreateNewPrompt(Info.Messages.warning_matchHistory_passedMatchIsNotFound);
                return;
            }

            timelineControl = new Timeline(match);
            timelineControl.VerticalAlignment = VerticalAlignment.Top;
            timelineControl.HorizontalAlignment = HorizontalAlignment.Right;
            timelineControl.Margin = new Thickness(15, 100, 10, 0);
            Panel.SetZIndex(timelineControl, 999);
            timelineControl.Visibility = Visibility.Collapsed;
            localGrid.Children.Add(timelineControl);

            string foo_mapName;
            try { foo_mapName = data.mapsDto.Find(p => p.mapId == match.preview.mapId).mapName; }
            catch (Exception) { foo_mapName = "(nieznana mapa)"; }
            mapName.Text = string.Format("{0} - {1}", foo_mapName, match.preview.mode);

            date.Text = match.preview.timestamp;
            gameDuration.Text = string.Format("Czas trwania: {0} min", match.gameInfo.gameDuration);

            if (match.preview.result == true)
            {
                result.Text = "ZWYCIĘSTWO";
                result.Foreground = new SolidColorBrush(Color.FromRgb(0, 255, 0));
            }
            else
            {
                result.Text = "PORAŻKA";
                result.Foreground = new SolidColorBrush(Color.FromRgb(255, 0, 0));
            }

            if (match.gameInfo.gameEndedInSurrender == true) surrender.Visibility = Visibility.Visible;
            else surrender.Visibility = Visibility.Collapsed;

            View.UserControls.MatchSummary matchSummary = new View.UserControls.MatchSummary(match);
            matchSummary.VerticalAlignment = VerticalAlignment.Top;
            matchSummary.Margin = new Thickness(0, 30, 0, 0);
            localGrid.Children.Add(matchSummary);

            if (match.gameInfo.myTeamId == 100)
            {
                allayTeamGradient.GradientStops[1].Color = Color.FromArgb(0, 255, 0, 0);
                allayTeamGradient.GradientStops[2].Color = Color.FromRgb(255, 0, 0);

                enemyTeamGradient.GradientStops[1].Color = Color.FromArgb(0, 0, 0, 255);
                enemyTeamGradient.GradientStops[2].Color = Color.FromRgb(0, 0, 255);
            }
            else
            {
                allayTeamGradient.GradientStops[1].Color = Color.FromArgb(0, 0, 0, 255);
                allayTeamGradient.GradientStops[2].Color = Color.FromRgb(0, 0, 255);

                enemyTeamGradient.GradientStops[1].Color = Color.FromArgb(0, 255, 0, 0);
                enemyTeamGradient.GradientStops[2].Color = Color.FromRgb(255, 0, 0);
            }

            bool isError = false;
            int errorCount = 0;
            foreach (DataType.Participant player in match.participants)
            {
                View.UserControls.MatchPlayer control = new View.UserControls.MatchPlayer(player);
                if (control.isError == true)
                {
                    isError = true;
                    errorCount++;
                }

                if (player.teamId == match.gameInfo.myTeamId) allayTeam.Children.Add(control);
                else enemyTeam.Children.Add(control);
            }
            if (isError == true) info.CreateNewPrompt(Info.Messages.warning_matchHistory_playerSourcesNotLoaded, errorCount);

            matchDetailedStats = new MatchDetailedStats(match);
            localGrid.Children.Add(matchDetailedStats);
            animation_slideIn.Begin();
        }

        public Task ClosePanel()
        {
            var taskResult = new TaskCompletionSource<bool>();
            animation_slideOut.Completed += (sender, e) =>
            {
                taskResult.SetResult(true);

                if (this.Parent is Panel parentPanel)
                {
                    parentPanel.Children.Remove(this);
                }
            };

            animation_slideOut.Begin();
            return taskResult.Task;
        }

        public async void SwitchTimelineMode(object sender, EventArgs e)
        {
            if (modeTimeline == false)
            {
                downloadTimeline.Fill = new SolidColorBrush(Colors.LawnGreen);
                downloadTimelineText.Foreground = new SolidColorBrush(Colors.Black);
                downloadTimelineIcon.Source = LoadResources.LoadImage("info_icon_ok.png", LoadResources.ImagePath_t.resources, false).image;
                modeTimeline = true;

                await timelineControl.ShowPanel();
                matchDetailedStats.Visibility = Visibility.Collapsed;
            }
            else
            {
                downloadTimeline.Fill = new SolidColorBrush(Colors.MidnightBlue);
                downloadTimelineText.Foreground = new SolidColorBrush(Colors.White);
                downloadTimelineIcon.Source = LoadResources.LoadImage("info_icon_info.png", LoadResources.ImagePath_t.resources, false).image;
                modeTimeline = false;

                matchDetailedStats.Visibility = Visibility.Visible;
                await timelineControl.ClosePanel();
            }
        }
    }
}
