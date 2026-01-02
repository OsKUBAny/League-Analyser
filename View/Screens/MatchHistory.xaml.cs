using System.Windows.Controls;

namespace League_Analyser.View.Screens
{
    public partial class MatchHistory : UserControl
    {
        public MatchHistory()
        {
            InitializeComponent();
        }

        private void UpdateVisibleMatchesValue(object sender, ScrollChangedEventArgs e)
        {
            double panelHeight = 92; //Control's height + margin
            double scrollViewerOffset = e.VerticalOffset;
            int visibleMatchesOffset = 7;

            int firstSeen = (int)((scrollViewerOffset + panelHeight / 2) / panelHeight) + 1;
            if (matchList.Children.Count == 0) firstSeen = 0;
            int lastSeen = firstSeen + visibleMatchesOffset;
            if (lastSeen > matchList.Children.Count) lastSeen = matchList.Children.Count;

            visibleMatchesCount.Text = string.Format("({0} - {1}) {2} {3}", 
                firstSeen, lastSeen, Messages.matchHistory_scroll_rangeText ,matchList.Children.Count);
        }
    }
}
