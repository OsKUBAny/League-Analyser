using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace League_Analyser.View.UserControls
{
    public partial class TimelineSummary : UserControl
    {
        private MainWindow mainWindow;
        private Info info;
        private Data data;
        public Timeline timelineCtrl;
        public TimelineData timeline;

        private Storyboard animation_slideIn;
        private Storyboard animation_slideOut;

        public DataType.EventsTimeLineDto eventData;

        public TimelineSummary(DataType.EventsTimeLineDto eventData_, TimelineData timeline_, Timeline timelineCtrl_)
        {
            InitializeComponent();
            eventData = eventData_;
            timelineCtrl = timelineCtrl_;
            timeline = timeline_;

            mainWindow = (MainWindow)App.Current.MainWindow;
            info = mainWindow.info;
            data = mainWindow.data;

            animation_slideIn = (Storyboard)this.Resources["SlideIn"];
            animation_slideOut = (Storyboard)this.Resources["SlideOut"];

            animation_slideIn.Begin();

            timeline.ResetTotalDamageEvent();

            try
            {
                if (eventData.killerId == 0) throw new Exception();

                string playerPuuid = timeline.timelineDto.info.participants.Find(p => p.participantId == eventData.killerId).puuid;
                int playerTeamId = timeline.match.participants.Find(p => p.accountDto.puuid == playerPuuid).teamId;

                if (playerTeamId == timeline.match.gameInfo.myTeamId) eventTeam.Text = Messages.timeline_byAllayTeam;
                else eventTeam.Text = Messages.timeline_byEnemyTeam;
            }
            catch (Exception)
            {
                if (eventData.killerId == 0 && timeline.IsNotNullOrZero(eventData.victimId))
                {
                    string playerPuuid = timeline.timelineDto.info.participants.Find(p => p.participantId == eventData.victimId).puuid;
                    int playerTeamId = timeline.match.participants.Find(p => p.accountDto.puuid == playerPuuid).teamId;

                    if (playerTeamId != timeline.match.gameInfo.myTeamId) eventTeam.Text = Messages.timeline_inEnemyTeam;
                    else eventTeam.Text = Messages.timeline_inAllayTeam;
                }
                else eventTeam.Text = "";
            }

            eventMap.Source = LoadResources.LoadImage(string.Format("map{0}.png", timeline.match.preview.mapId),
                LoadResources.ImagePath_t.DD_map, true).image;

            eventTimestamp.Text = String.Format("{0:D2}:{1:D2}", (eventData.timestamp / 1000 / 60) % 60, (eventData.timestamp / 1000) % 60);

            if (timeline.IsNotNullOrZero(eventData.victimId)) participantsList.Children.Add(CreateChampionData((int)eventData.victimId, 0));
            if (timeline.IsNotNullOrZero(eventData.killerId)) participantsList.Children.Add(CreateChampionData((int)eventData.killerId, 1));
            if (eventData.assistingParticipantIds != null)
            {
                foreach (int player in eventData.assistingParticipantIds)
                {
                    participantsList.Children.Add(CreateChampionData((int)player, 2));
                }
            }
            if (eventData.victimDamageReceived != null && eventData.victimDamageReceived.Any(p => p.participantid == 0))
            {
                participantsList.Children.Add(CreateChampionData(0, 3));
            }

            timeline.GetPlayerTotalDamages(eventData);
            SetDataForObjectiveType();
        }

        private void SetDataForObjectiveType()
        {
            string resourcesName = timeline.GetImageNameForEvent(eventData, false);
            eventType.Text = timeline.GetImageNameForEvent(eventData, true);
            LoadResources.ImagePath_t resourcesType = timeline.GetImagePathForEvent(eventData.type);

            if (eventType.Text == null) eventType.Text = Messages.timeline_noEventTypeData;

            eventIcon.Source = LoadResources.LoadImage(resourcesName, resourcesType, true).image;

            AddPointOnMap(eventData.position);
        }

        private StackPanel CreateChampionData(int playerId, int type)
        {
            //type:
            //0 - victim
            //1 - killer
            //2 - assist
            //3 - other (Kills)

            StackPanel container = new StackPanel();
            container.Orientation = Orientation.Horizontal;
            container.HorizontalAlignment = HorizontalAlignment.Left;

            Image player = new Image();
            player.Width = 50;
            player.Height = 50;

            Border border = new Border();
            border.BorderThickness = new Thickness(2.5);
            border.Margin = new Thickness(0, 0, 0, 5);

            TextBlock playerName = new TextBlock();
            playerName.Foreground = new SolidColorBrush(Colors.LightGray);
            playerName.FontSize = 15;
            playerName.HorizontalAlignment = HorizontalAlignment.Left;
            playerName.VerticalAlignment = VerticalAlignment.Top;
            playerName.Margin = new Thickness(5, 0, 0, 0);

            border.Child = player;

            container.Children.Add(border);

            switch (type)
            {
                case 0: border.BorderBrush = new SolidColorBrush(Colors.Red); border.Margin = new Thickness(0, 0, 0, 30); break;
                case 1: border.BorderBrush = new SolidColorBrush(Colors.LawnGreen); break;
                case 2: border.BorderBrush = new SolidColorBrush(Colors.Yellow); break;
                case 3: border.BorderBrush = new SolidColorBrush(Colors.Yellow); break;
            }

            if (type == 3) // Other sources (minions, towers, monsters etc.
            {
                player.Source = LoadResources.LoadImage("other.png", LoadResources.ImagePath_t.gC_timeline_misc, true).image;
                playerName.Text = Messages.timeline_other;
            }
            else
            {
                string championPath = null;
                string playerPuuid;

                try
                {
                    playerPuuid = timeline.timelineDto.info.participants.Find(p => p.participantId == playerId).puuid;
                    int championId = timeline.match.participants.Find(p => p.accountDto.puuid == playerPuuid).championId;
                    var championData = data.championDataDto.data.FirstOrDefault(p => p.Value.key == championId.ToString());
                    championPath = championData.Value.image.full;

                    playerName.Text = timeline.match.participants.Find(p => p.accountDto.puuid == playerPuuid).accountDto.gameName;
                }
                catch (Exception)
                {
                    info.CreateNewPrompt(Info.Messages.error_timeline_imageNotFound);
                    playerName.Text = Messages.timeline_unknownPlayer;
                }

                player.Source = LoadResources.LoadImage(championPath, LoadResources.ImagePath_t.DD_champion, true).image;
            }


            if (eventData.type == "CHAMPION_KILL") container.Children.Add(CreateChampionKillData(playerName.Text, playerId, type == 0));
            else container.Children.Add(playerName);

            return container;
        }

        private TimelineKillSummary CreateChampionKillData(string playerName, int playerId, bool isVictim)
        {
            List<DataType.DamageDealt> damages = new List<DataType.DamageDealt>();

            if (isVictim == true) damages = eventData.victimDamageDealt;
            else
            {
                foreach (DataType.DamageDealt damageEvent in eventData.victimDamageReceived.Where(p => p.participantid == playerId))
                {
                    damages.Add(damageEvent);
                }
            }

            TimelineKillSummary containter = new TimelineKillSummary(timeline, this, playerName, playerId, damages);
            return containter;
        }

        private void AddPointOnMap(DataType.PositionDto eventPosition)
        {
            double mapSize = eventMap.Width;
            double mapMin_x;
            double mapMin_y;
            double mapMax_x;
            double mapMax_y;

            if (timeline.match.preview.mapId == 11)//SR
            {
                mapMin_x = -120; mapMin_y = -120;
                mapMax_x = 14870; mapMax_y = 14980;
            }
            else if (timeline.match.preview.mapId == 12)//HA
            {
                mapMin_x = -28; mapMin_y = -19;
                mapMax_x = 12849; mapMax_y = 12858;
            }
            else return;

            double point_x = (((double)eventPosition.x + mapMin_x) / (mapMax_x - mapMin_x)) * mapSize;
            double point_y = mapSize - (((double)eventPosition.y + mapMin_y) / (mapMax_y - mapMin_y)) * mapSize;

            Ellipse point = new Ellipse();
            point.Width = 15; point.Height = 15;
            point.Fill = new SolidColorBrush(Colors.Red);
            point.Stroke = new SolidColorBrush(Colors.White);
            point.StrokeThickness = 2;

            point_x -= point.Width / 2;
            point_y -= point.Height / 2;

            Canvas.SetLeft(point, point_x);
            Canvas.SetTop(point, point_y);
            canvasMap.Children.Add(point);
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
    }
}