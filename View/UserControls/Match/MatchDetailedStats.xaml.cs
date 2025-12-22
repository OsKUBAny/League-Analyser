using System;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;

namespace League_Analyser.View.UserControls
{
    public partial class MatchDetailedStats : UserControl
    {
        private MainWindow mainWindow;
        private Info info;
        private DataType.MatchLao match;

        private Storyboard animation_slideIn;
        private Storyboard animation_slideOut;


        public MatchDetailedStats(DataType.MatchLao matchData)
        {
            InitializeComponent();

            mainWindow = (MainWindow)App.Current.MainWindow;
            info = mainWindow.info;
            match = matchData;

            animation_slideIn = (Storyboard)this.Resources["SlideIn"];
            animation_slideOut = (Storyboard)this.Resources["SlideOut"];

            comboBox_category.ItemsSource = Enum.GetValues(typeof(MatchHistory.Statistics_t.CategoryType_t))
                .Cast<MatchHistory.Statistics_t.CategoryType_t>().Select(p => new
                {
                    key = p,
                    value = ((DescriptionAttribute)Attribute.GetCustomAttribute(typeof(MatchHistory.Statistics_t.CategoryType_t)
                    .GetField(p.ToString()), typeof(DescriptionAttribute))).Description
                }).ToList();
            comboBox_category.DisplayMemberPath = "value";
            comboBox_category.SelectedValuePath = "key";
            comboBox_category.SelectionChanged += (sender, e) => StatsCategoryChanged();
            comboBox_type.SelectionChanged += (sender, e) => StatsTypeChanged();

            comboBox_category.SelectedIndex = 0;
            StatsCategoryChanged();
        }

        private void StatsCategoryChanged()
        {
            comboBox_type.ItemsSource = MatchHistory.Statistics_t.Items.Where(p => p.Category ==
                    (MatchHistory.Statistics_t.CategoryType_t)comboBox_category.SelectedValue).ToList();
            if (comboBox_type.Items.Count > 0) comboBox_type.SelectedIndex = 0;
        }

        private async void StatsTypeChanged()
        {
            await AnimateCurtain(true);

            allayTeam.Children.Clear();
            enemyTeam.Children.Clear();

            MatchHistory.Statistics statisticType = (MatchHistory.Statistics)comboBox_type.SelectedItem;
            if (statisticType == null) return;

            bool isError = false;
            int errorCount = 0;

            int maxValue = match.participants.Max(p => Convert.ToInt32(statisticType.ValueGetter(p)));
            int maxValueAdditional = 0;
            if (statisticType.DisplayType == MatchHistory.Statistics_t.DisplayType_t.twoValuesAndBar)
                maxValueAdditional = match.participants.Max(p => Convert.ToInt32(statisticType.ValueGetterAdditional(p)));

            if (statisticType.DisplayType == MatchHistory.Statistics_t.DisplayType_t.boolOnly) scaleInfo.Visibility = Visibility.Hidden;
            else scaleInfo.Visibility = Visibility.Visible;

            foreach (DataType.Participant player in match.participants)
            {
                PlayerStatistics control = new PlayerStatistics(player, statisticType, maxValue, maxValueAdditional);
                if (control.isError == true)
                {
                    isError = true;
                    errorCount++;
                }

                if (player.teamId == match.gameInfo.myTeamId) allayTeam.Children.Add(control);
                else enemyTeam.Children.Add(control);
            }

            await AnimateCurtain(false);
            curtain.Visibility = Visibility.Visible;

            if (isError == true) info.CreateNewPrompt(Info.Messages.warning_matchHistory_playerStatsNotLoaded, errorCount);
        }

        public Task AnimateCurtain(bool isSlideIn)
        {
            var taskResult = new TaskCompletionSource<bool>();
            EventHandler onCompleted = null;
            curtain.Width = 0;

            if (isSlideIn)
            {
                onCompleted = (sender, e) =>
                {
                    animation_slideIn.Completed -= onCompleted;
                    curtain.HorizontalAlignment = HorizontalAlignment.Right;
                    taskResult.SetResult(true);
                };

                animation_slideIn.Completed += onCompleted;
                animation_slideIn.Begin();
            }
            else
            {
                onCompleted = (sender, e) =>
                {
                    animation_slideOut.Completed -= onCompleted;
                    curtain.HorizontalAlignment = HorizontalAlignment.Left;
                    taskResult.SetResult(true);
                };

                animation_slideOut.Completed += onCompleted;
                animation_slideOut.Begin();
            }

            return taskResult.Task;
        }
    }
}
