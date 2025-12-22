using System.Windows.Controls;

namespace League_Analyser.View.UserControls
{
    public partial class SettingsNewProfile : UserControl
    {
        public SettingsNewProfile()
        {
            InitializeComponent();
            comboBox_server.ItemsSource = Settings.serversList;
            comboBox_server.SelectedIndex = 0;
        }
    }
}
