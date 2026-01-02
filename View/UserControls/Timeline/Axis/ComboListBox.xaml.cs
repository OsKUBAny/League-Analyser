using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace League_Analyser.View.UserControls
{
    public partial class ComboListBox : UserControl
    {
        public class Player : INotifyPropertyChanged
        {
            private bool _isSelected;
            public event PropertyChangedEventHandler PropertyChanged;

            public string name { get; set; }
            public int playerId { get; set; }
            public bool isSelected
            {
                get => _isSelected;
                set
                {
                    if (_isSelected != value)
                    {
                        _isSelected = value;
                        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(isSelected)));
                    }
                }
            }
        }
        // class Player works also with event types (kill = id 1, death = id 2, assist = id 3)
        // didn't want to create new class if this one works too :)

        public TimelineAxis timelineAxisCtrl;
        public TimelineData timeline;
        public ObservableCollection<Player> playerList { get; } = new ObservableCollection<Player>();
        public ObservableCollection<Player> eventList { get; } = new ObservableCollection<Player>();
        private bool isPlayerListOpen = false;
        private bool isEventListOpen = false;

        public ComboListBox()
        {
            InitializeComponent();
            playerListContainer.Visibility = Visibility.Collapsed;
            eventListContainer.Visibility = Visibility.Collapsed;
            DataContext = this;

            playerList.CollectionChanged += (s, e) =>
            {
                foreach (var player in playerList)
                {
                    player.PropertyChanged += Player_PropertyChanged;
                }
            };

            eventList.CollectionChanged += (s, e) =>
            {
                foreach (var event_ in eventList)
                {
                    event_.PropertyChanged += Player_PropertyChanged;
                }
            };

            eventList.Add(new Player
            {
                name = "K - " + Messages.timeline_axis_kills,
                playerId = 1,
                isSelected = true
            });
            eventList.Add(new Player
            {
                name = "D - " + Messages.timeline_axis_deaths,
                playerId = 2,
                isSelected = true
            });
            eventList.Add(new Player
            {
                name = "A - " + Messages.timeline_axis_assists,
                playerId = 3,
                isSelected = true
            });
        }

        private void playerListButton_clicked(object sender, MouseButtonEventArgs e)
        {
            isPlayerListOpen = !isPlayerListOpen;

            if (isPlayerListOpen == true) playerListContainer.Visibility = Visibility.Visible;
            else playerListContainer.Visibility = Visibility.Collapsed;
        }

        private void eventListButton_clicked(object sender, MouseButtonEventArgs e)
        {
            isEventListOpen = !isEventListOpen;

            if (isEventListOpen == true) eventListContainer.Visibility = Visibility.Visible;
            else eventListContainer.Visibility = Visibility.Collapsed;
        }

        public void Player_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            int selectedPlayerCount = playerList.Count(p => p.isSelected);

            if (selectedPlayerCount == 1) playerListButton.Text = playerList.First(p => p.isSelected == true).name;
            else playerListButton.Text = string.Format(Messages.timeline_axis_selectedPlayers, selectedPlayerCount, playerList.Count);

            List<int> playerIdList = new List<int>();
            foreach (Player player in playerList)
            {
                if (player.isSelected) playerIdList.Add(player.playerId);
            }

            bool isK = eventList.First(p => p.playerId == 1).isSelected;
            bool isD = eventList.First(p => p.playerId == 2).isSelected;
            bool isA = eventList.First(p => p.playerId == 3).isSelected;

            string eventListInfo = null;

            if (isK)
            {
                eventListInfo = "K";
                if (isD) eventListInfo += "/D";
                if (isA) eventListInfo += "/A";
            }
            else if (isD)
            {
                eventListInfo = "D";
                if (isA) eventListInfo += "/A";
            }
            else if (isA) eventListInfo = "A";
            else eventListInfo = Messages.timeline_axis_none;

            eventListButton.Text = eventListInfo;

            if (timeline == null) return;
            if (timeline.GetEventsFromTimeline(playerIdList, isK, isD, isA) == false)
                timelineAxisCtrl.eventsContainer_monsters.Visibility = Visibility.Collapsed;

            timelineAxisCtrl.UpdateEventsOnTimeline();
        }
    }
}
