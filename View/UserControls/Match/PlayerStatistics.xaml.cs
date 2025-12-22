using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using displayType_t = League_Analyser.MatchHistory.Statistics_t.DisplayType_t;

namespace League_Analyser.View.UserControls
{
    public partial class PlayerStatistics : UserControl
    {
        private DataType.Participant participant;
        private MatchHistory.Statistics statisticType;

        public bool isError = false;
        private int maxValue;
        private int maxValueAdditional;
        private int barWidth;
        private int value;
        private int valueAdditional;

        private SolidColorBrush defaultColor = new SolidColorBrush(Colors.LightGray);
        private SolidColorBrush claimerColor = new SolidColorBrush(Colors.DarkGreen);
        private SolidColorBrush assistColor = new SolidColorBrush(Colors.SandyBrown);

        private LinearGradientBrush neutralGradient = new LinearGradientBrush
        {
            StartPoint = new Point(0, 0),
            EndPoint = new Point(1, 0),
            GradientStops =
            {
                new GradientStop((Color)ColorConverter.ConvertFromString("#FF0068FF"), 1),
                new GradientStop((Color)ColorConverter.ConvertFromString("#FF5198FF"), 0.5)
            }
        };
        private LinearGradientBrush bestGradient = new LinearGradientBrush
        {
            StartPoint = new Point(0, 0),
            EndPoint = new Point(1, 0),
            GradientStops =
            {
                new GradientStop((Color)ColorConverter.ConvertFromString("#FF0A8004"), 1),
                new GradientStop((Color)ColorConverter.ConvertFromString("#FF0BE200"), 0.5)
            }
        };
        private LinearGradientBrush secondColor = new LinearGradientBrush
        {
            StartPoint = new Point(0, 0),
            EndPoint = new Point(1, 0),
            GradientStops =
            {
                new GradientStop((Color)ColorConverter.ConvertFromString("#FFFF5555"), 1),
                new GradientStop((Color)ColorConverter.ConvertFromString("#FFDC0000"), 0.5)
            }
        };
        private LinearGradientBrush statusClaimerGrad = new LinearGradientBrush
        {
            StartPoint = new Point(0, 0),
            EndPoint = new Point(1, 0),
            GradientStops =
            {
                new GradientStop((Color)ColorConverter.ConvertFromString("#00000000"), 1),
                new GradientStop((Color)ColorConverter.ConvertFromString("#FF00F30B"), 0.5)
            }
        };
        private LinearGradientBrush statusAssistGrad = new LinearGradientBrush
        {
            StartPoint = new Point(0, 0),
            EndPoint = new Point(1, 0),
            GradientStops =
            {
                new GradientStop((Color)ColorConverter.ConvertFromString("#00000000"), 1),
                new GradientStop((Color)ColorConverter.ConvertFromString("#FFFFEC31"), 0.5)
            }
        };

        public PlayerStatistics(DataType.Participant participantData, MatchHistory.Statistics statisticTypeData,
            int maxValueData, int maxValueAdditionalData)
        {
            InitializeComponent();
            participant = participantData;
            statisticType = statisticTypeData;

            maxValue = maxValueData;
            maxValueAdditional = maxValueAdditionalData;
            barWidth = (int)(Width * 0.8);

            statusText.Visibility = Visibility.Hidden;
            statusIndicator.Visibility = Visibility.Hidden;

            try { value = Convert.ToInt32(statisticType.ValueGetter(participant)); }
            catch (Exception) { isError = true; return; }

            if (statisticType.DisplayType == displayType_t.twoValuesAndBar)
            {
                try { valueAdditional = Convert.ToInt32(statisticType.ValueGetterAdditional(participant)); }
                catch (Exception) { isError = true; return; }
            }
            else valueAdditional = 0;


            switch (statisticType.DisplayType)
            {
                case displayType_t.valueAndBar: { RenderValueBar(); return; }
                case displayType_t.twoValuesAndBar: { RenderDoubleBar(); return; }
                case displayType_t.boolOnly: { RenderBoolValue(); return; }
                default: { isError = true; return; }
            }
        }

        private void RenderValueBar()
        {
            valueLeft.Visibility = Visibility.Hidden;
            barRight.Visibility = Visibility.Hidden;

            valueRight.Text = value.ToString("#,##0");

            try
            {
                barLeft.Width = ((double)value / maxValue) * barWidth;
            }
            catch (Exception)
            {
                isError = true;
                barLeft.Width = 0;
                barLeft.Fill = defaultColor;
            }

            if (value == maxValue)
            {
                valueRight.FontWeight = FontWeights.Bold;
                barLeft.Fill = bestGradient;
            }
            else barLeft.Fill = neutralGradient;

        }
        private void RenderDoubleBar()
        {
            try
            {
                double percent = ((double)valueAdditional / value) * 100;
                valueLeft.Text = string.Format("{0:0.00}%", percent);
                valueRight.Text = string.Format("{0:0.00}%", 100 - percent);

                barLeft.Width = ((double)valueAdditional / value) * barWidth;
                barRight.Width = barWidth - barLeft.Width;

                barLeft.Fill = neutralGradient;
                barRight.Fill = secondColor;
            }
            catch (Exception)
            {
                isError = true;
                barLeft.Width = barWidth;
                barLeft.Fill = defaultColor;
                barRight.Visibility = Visibility.Hidden;

                valueRight.Text = value.ToString();
                valueLeft.Text = valueAdditional.ToString();
                return;
            }

            if (valueAdditional == maxValueAdditional)
            {
                valueLeft.FontWeight = FontWeights.Bold;
                valueRight.FontWeight = FontWeights.Bold;
                barLeft.Fill = bestGradient;
            }
            else barLeft.Fill = neutralGradient;
        }
        private void RenderBoolValue()
        {
            barLeft.Visibility = Visibility.Hidden;
            barRight.Visibility = Visibility.Hidden;
            valueLeft.Visibility = Visibility.Hidden;
            valueRight.Visibility = Visibility.Hidden;

            if (value == 2)
            {
                statusText.Visibility = Visibility.Visible;
                statusIndicator.Visibility = Visibility.Visible;

                statusText.Text = "ZDOBYWCA";
                statusText.Foreground = claimerColor;
                statusIndicator.Fill = statusClaimerGrad;
            }
            else if (value == 1)
            {
                statusText.Visibility = Visibility.Visible;
                statusIndicator.Visibility = Visibility.Visible;

                statusText.Text = "ASYSTA";
                statusText.Foreground = assistColor;
                statusIndicator.Fill = statusAssistGrad;
            }
        }
    }
}
