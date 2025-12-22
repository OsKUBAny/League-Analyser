using System;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace League_Analyser.View.UserControls
{
    public partial class MenuButtons : UserControl
    {
        private MainWindow mainWindow;
        private string currentMode;

        private LinearGradientBrush activeColorGradient = new LinearGradientBrush
        {
            StartPoint = new Point(0.5, 0),
            EndPoint = new Point(0.5, 1),
            GradientStops = new GradientStopCollection
            {
                new GradientStop((Color)ColorConverter.ConvertFromString("#FFEAC5AC"), 0),
                new GradientStop((Color)ColorConverter.ConvertFromString("#FFFF6800"), 1)
            }
        };

        public MenuButtons()
        {
            InitializeComponent();
            if (DesignerProperties.GetIsInDesignMode(this)) return;

            foreach (Button control in menuStackPanel.Children.OfType<Button>())
            {
                control.Visibility = Visibility.Collapsed;
            }

            mainWindow = (MainWindow)App.Current.MainWindow;
            button_settings.Tag = new Action(new Settings().InitializeSettings);
            button_matchHistory.Tag = new Action(new MatchHistory().InitializeMatchHistory);
            //TODO: Add remaining initializators

            button_stats.Click -= Button_Click;
        }

        // Handles button click to set current mode.
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Button usedButton = sender as Button;
            string modeName = usedButton.Name;
            if (modeName == currentMode) return;
            if (mainWindow.isProcessOngoing == true) return;

            currentMode = modeName;

            foreach (var button in menuStackPanel.Children.OfType<Button>())
            {
                button.ClearValue(Control.BackgroundProperty);
            }

            for (int i = mainWindow.mainGrid.Children.Count - 1; i >= 0; i--)
            {
                UIElement control = mainWindow.mainGrid.Children[i];

                //Namespace "Settings" is dummy just for purpouse of getting Screens's namespace.
                if (control.GetType().Namespace == typeof(View.Screens.Settings).Namespace)
                {
                    mainWindow.mainGrid.Children.Remove(control);
                }
            }

            usedButton.Background = activeColorGradient;
            if (usedButton.Tag is Action action) action.Invoke();
        }
    }
}
