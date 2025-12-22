using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;

namespace League_Analyser.View.UserControls
{

    public partial class Timeline : UserControl
    {
        private TimelineData timeline;

        private DataType.MatchLao match;

        private bool isTimelineInitialized = false;
        private Storyboard animation_slideIn;
        private Storyboard animation_slideOut;

        public Timeline(DataType.MatchLao matchData)
        {
            InitializeComponent();
            match = matchData;

            animation_slideIn = (Storyboard)this.Resources["SlideIn"];
            animation_slideOut = (Storyboard)this.Resources["SlideOut"];
            imageProcess.Visibility = Visibility.Visible;
            axis.Visibility = Visibility.Collapsed;

            timeline = new TimelineData();
            timeline.LoadResourcesInit(match);
        }

        private async void LoadData()
        {
            bool result = await timeline.LoadData();
            if (result == true) imageProcess.Visibility = Visibility.Collapsed;
            else await ClosePanel();

            axis.timelineCtrl = this;
            axis.timeline = timeline;
            axis.SetAxisData(match.gameInfo.gameDurationUnix);

            axis.Visibility = Visibility.Visible;

            foreach (string playerPuuid in timeline.timelineDto.metadata.participants)
            {
                string name_ = timeline.match.participants.Find(p => p.accountDto.puuid == playerPuuid).accountDto.gameName;
                int id_ = timeline.timelineDto.metadata.participants.FindIndex(p => p == playerPuuid) + 1;

                axis.comboList_players.playerList.Add(new ComboListBox.Player
                {
                    name = name_,
                    playerId = id_,
                    isSelected = true
                });
            }
            axis.comboList_players.Margin = new Thickness
                (
                axis.comboList_players.Margin.Left,
                (axis.comboList_players.Height - 20) * -1,
                axis.comboList_players.Margin.Right,
                axis.comboList_players.Margin.Bottom
                );

            axis.comboList_players.Player_PropertyChanged(null, null);
        }
        public Task ShowPanel()
        {
            this.Visibility = Visibility.Visible;
            var taskResult = new TaskCompletionSource<bool>();
            EventHandler handler = null;

            handler = (sender, e) =>
            {
                animation_slideIn.Completed -= handler;
                taskResult.SetResult(true);
                if (!isTimelineInitialized)
                {
                    isTimelineInitialized = true;
                    LoadData();
                }
            };

            animation_slideIn.Completed += handler;
            animation_slideIn.Begin(this, true);
            return taskResult.Task;
        }
        public Task ClosePanel()
        {
            var taskResult = new TaskCompletionSource<bool>();
            EventHandler handler = null;

            handler = (sender, e) =>
            {
                animation_slideOut.Completed -= handler;
                this.Visibility = Visibility.Collapsed;
                taskResult.SetResult(true);
            };

            animation_slideOut.Completed += handler;
            animation_slideOut.Begin(this, true);
            return taskResult.Task;
        }
    }
}
