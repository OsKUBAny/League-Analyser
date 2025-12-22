using System;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Input;

namespace League_Analyser.View.UserControls
{
    public partial class TimelineAxis : UserControl
    {
        private MainWindow mainWindow;
        private Info info;
        public TimelineData timeline;
        private bool isErrorOccured = false;
        public Timeline timelineCtrl;

        private long gameLength;
        private long axisTs_begining = 0;
        private long axisTs_end;
        private long axisTs_window = 0;
        private long axisTs_middle;

        private double axisTs_factor = 2; //Warning! Don't know why but it works well with 2 only
        private long axisTs_windowLimit = 10;

        public TimelineAxis()
        {
            InitializeComponent();
            mainWindow = (MainWindow)App.Current.MainWindow;
            info = mainWindow?.info;

            comboList_players.timelineAxisCtrl = this;


            scrollBar.ScrollBarMoved += (sender, value) =>
            {
                axisTs_begining = (long)(value.beginingPoint * gameLength);
                axisTs_middle = (long)(value.middlePoint * gameLength);
                axisTs_end = (long)(value.endPoint * gameLength);

                SetAxisTime();
            };
        }

        public void SetAxisData(long gameLenghtData)
        {
            // In Unix
            gameLength = gameLenghtData;
            axisTs_end = gameLength;
            axisTs_window = axisTs_end - axisTs_begining;
            axisTs_middle = gameLength / 2;

            scrollBar.SetThumbWidth(1, -1);
            SetAxisTime();
            comboList_players.timeline = timeline;
            comboList_players.Player_PropertyChanged(null, null);
        }

        private void SetAxisTime()
        {
            ts_1.Text = String.Format("{0:D2}:{1:D2}", (axisTs_begining / 60) % 60, axisTs_begining % 60);
            ts_2.Text = String.Format("{0:D2}:{1:D2}", ((axisTs_middle - (axisTs_window / 4)) / 60) % 60, (axisTs_middle - (axisTs_window / 4)) % 60);
            ts_3.Text = String.Format("{0:D2}:{1:D2}", (axisTs_middle / 60) % 60, axisTs_middle % 60);
            ts_4.Text = String.Format("{0:D2}:{1:D2}", ((axisTs_middle + (axisTs_window / 4)) / 60) % 60, (axisTs_middle + (axisTs_window / 4)) % 60);
            ts_5.Text = String.Format("{0:D2}:{1:D2}", (axisTs_end / 60) % 60, axisTs_end % 60);
            UpdateEventsOnTimeline();
        }

        private void AxisZoomChanged(object sender, MouseButtonEventArgs e)
        {
            if ((sender as TextBlock).Name == "button_zoomIn")
            {
                axisTs_window = (long)(axisTs_window / axisTs_factor);
                if (axisTs_window < axisTs_windowLimit)
                {
                    axisTs_window = axisTs_windowLimit;
                    scrollBar.SetThumbWidth((double)axisTs_windowLimit / gameLength, -1);
                }
                else
                {
                    axisTs_begining = axisTs_middle - (axisTs_window / 2);
                    axisTs_end = axisTs_end + (axisTs_window / 2);

                    scrollBar.SetThumbWidth((double)axisTs_window / gameLength, -1);
                }
            }
            else if ((sender as TextBlock).Name == "button_zoomOut")
            {
                axisTs_window = (long)(axisTs_window * axisTs_factor);
                if (axisTs_window > gameLength)
                {
                    axisTs_begining = 0;
                    axisTs_end = gameLength;
                    axisTs_window = gameLength;

                    scrollBar.SetThumbWidth(1, -1);
                }
                else
                {
                    scrollBar.SetThumbWidth((double)axisTs_window / gameLength, -1);
                }
            }
            SetAxisTime();
        }

        private void Scroll_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (Keyboard.Modifiers.HasFlag(ModifierKeys.Control))
            {
                long oldAxisTsBegining = axisTs_begining;
                double mousePosition = Mouse.GetPosition((Grid)sender).X;
                mousePosition -= eventsContainer.Margin.Left;
                mousePosition = mousePosition / eventsContainer.Width;

                if (mousePosition < 0) mousePosition = 0;
                else if (mousePosition > 1) mousePosition = 1;



                var foo_sender = new TextBlock();
                if (e.Delta > 0)
                {
                    axisTs_window = (long)(axisTs_window / axisTs_factor);
                    if (axisTs_window < axisTs_windowLimit)
                    {
                        axisTs_window = axisTs_windowLimit;
                        scrollBar.SetThumbWidth((double)axisTs_windowLimit / gameLength, -1);
                    }
                    else
                    {
                        axisTs_begining = (long)(mousePosition * axisTs_window) + oldAxisTsBegining;
                        axisTs_end = axisTs_begining + axisTs_window;
                        axisTs_middle = axisTs_begining + axisTs_window / 2;

                        scrollBar.SetThumbWidth((double)axisTs_window / gameLength, (double)((double)axisTs_middle / gameLength));
                    }
                }
                else
                {
                    axisTs_window = (long)(axisTs_window * axisTs_factor);
                    if (axisTs_window > gameLength)
                    {
                        axisTs_begining = 0;
                        axisTs_end = gameLength;
                        axisTs_window = gameLength;

                        scrollBar.SetThumbWidth(1, -1);
                    }
                    else
                    {
                        axisTs_begining = (long)(axisTs_begining - (mousePosition * axisTs_window / axisTs_factor));
                        axisTs_end = axisTs_begining + axisTs_window;
                        axisTs_middle = axisTs_begining + axisTs_window / 2;

                        scrollBar.SetThumbWidth((double)axisTs_window / gameLength, (double)((double)axisTs_middle / gameLength));
                    }
                }
                SetAxisTime();
            }
            else
            {
                scrollBar.thumbPositionOld = scrollBar.Thumb.Margin.Left;
                scrollBar.MoveScrollThumb(e.Delta / 20);
            }
        }

        public void UpdateEventsOnTimeline()
        {
            if (timeline.eventsList == null) return;

            eventsContainer_buildings.Children.Clear();
            eventsContainer_monsters.Children.Clear();
            eventsContainer_kills.Children.Clear();

            foreach (var element in timeline.eventsList)
            {
                AddEventsOnTimeline(element);
            }
        }

        private void AddEventsOnTimeline(DataType.EventsTimeLineDto eventData)
        {
            string resourcesName = timeline.GetImageNameForEvent(eventData, false); // Can be null, processed correctly later
            LoadResources.ImagePath_t resourcesType = timeline.GetImagePathForEvent(eventData.type);

            Canvas container = GetContainerForEvent(eventData.type);

            Image eventT = new Image();

            eventT.Tag = eventData;
            eventT.Width = 20;
            eventT.Height = 20;
            eventT.MouseLeftButtonDown += ShowEventDetailsAsync;

            double position = ((double)(eventData.timestamp / 1000));
            if (position < axisTs_begining || position > axisTs_end) return;
            double pos_percent = ((double)position - axisTs_begining) / (axisTs_end - axisTs_begining);
            pos_percent = pos_percent * container.Width - (eventT.Width / 2);

            Canvas.SetLeft(eventT, pos_percent);

            LoadResources.LoadedImage loadedImage = LoadResources.LoadImage(resourcesName, resourcesType, false);
            eventT.Source = loadedImage.image;

            if (loadedImage.result == false && isErrorOccured == false)
            {
                isErrorOccured = true;
                info.CreateNewPrompt(Info.Messages.error_timeline_imageNotFound);
            }

            container.Children.Add(eventT);
        }

        private Canvas GetContainerForEvent(string type)
        {
            switch (type)
            {
                case TimelineData.typeObjectDestroyed: return eventsContainer_buildings;
                case TimelineData.typeMonsterKilled: return eventsContainer_monsters;
                case TimelineData.typeChampionKilled: return eventsContainer_kills;

                default: return eventsContainer_buildings;
            }
        }

        private async void ShowEventDetailsAsync(object s, EventArgs e)
        {
            var controlsList = timelineCtrl.localGrid.Children.OfType<TimelineSummary>().ToList();
            foreach (var control in controlsList)
            {
                await control.ClosePanel();
            }

            DataType.EventsTimeLineDto data = (DataType.EventsTimeLineDto)(s as Image).Tag;
            if (data == null)
            {
                info.CreateNewPrompt(Info.Messages.error_timeline_eventDataEmpty);
                return;
            }
            TimelineSummary summary = new TimelineSummary(data, timeline, timelineCtrl);
            timelineCtrl.localGrid.Children.Add(summary);
        }
    }
}
