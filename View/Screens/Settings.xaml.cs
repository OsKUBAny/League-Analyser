using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace League_Analyser.View.Screens
{
    public partial class Settings : UserControl
    {
        private MainWindow mainWindow = (MainWindow)App.Current.MainWindow;
        private League_Analyser.Settings settings;

        public Settings()
        {
            InitializeComponent();
            settings = mainWindow.settings;

            comboBox_profiles.ItemsSource = settings.profilesList;
            if (settings.profilesList.Count > 0) comboBox_profiles.SelectedIndex = 0;

            settings.profilesList.CollectionChanged += (sender, e) =>
            {
                if ((sender as ObservableCollection<string>).Count > 0)
                    comboBox_profiles.SelectedIndex = comboBox_profiles.Items.Count - 1;
            };

            if (settings.updateDDneeded == true) panel_DDupdate.Visibility = Visibility.Visible;
            if (settings.updateData.isUpdateNeeded == true)
            {
                AppUpdateVersion.Text = string.Format("wersja {0}", settings.updateData.version);
                AppUpdateDate.Text = settings.updateData.date;
                AppUpdateTitle.Text = settings.updateData.name;
                AppUpdateDescription.Text = settings.updateData.description;
                panel_AppUpdate.Visibility = Visibility.Visible;
            }
        }

        private void button_addProfile_Click(object sender, RoutedEventArgs e)
        {
            if (mainWindow.isProcessOngoing == true) return;

            button_addProfile.Visibility = Visibility.Collapsed;

            View.UserControls.SettingsNewProfile control = new UserControls.SettingsNewProfile();
            control.VerticalAlignment = VerticalAlignment.Top;
            control.HorizontalAlignment = HorizontalAlignment.Left;
            control.Margin = new Thickness(203, 0, 0, 0);
            Grid.SetRow(control, 2);

            control.button_Cancel.Click += (sender, e) =>
            {
                gridAddProfile.Children.Remove(control);
                button_addProfile.Visibility = Visibility.Visible;
            };
            control.button_Add.Click += async (sender, e) =>
            {
                string name = control.textBox_name.Text;
                string tag = control.textBox_tag.Text;
                string server = control.comboBox_server.SelectedValue.ToString();

                mainWindow.isProcessOngoing = true;
                if (await settings.CreateNewProfileReference(name, tag, server) == true)
                {
                    gridAddProfile.Children.Remove(control);
                    button_addProfile.Visibility = Visibility.Visible;
                    button_selectProfile_Click(null, null);
                }
                mainWindow.isProcessOngoing = false;
            };
            gridAddProfile.Children.Add(control);
        }

        private void button_selectProfile_Click(object sender, RoutedEventArgs e)
        {
            if (mainWindow.isProcessOngoing == true) return;
            if (comboBox_profiles.SelectedIndex < 0) return;

            View.UserControls.SettingsProfileDetail detailsPanel = new UserControls.SettingsProfileDetail();
            Grid.SetColumn(detailsPanel, 1);

            string profileName = comboBox_profiles.SelectedItem.ToString();
            settings.LoadProfileInformations(profileName, detailsPanel);

            detailsPanel.profileName.Text = profileName;
            RedrawProfileDetailsPanel(detailsPanel);
        }

        private void RedrawProfileDetailsPanel(UserControls.SettingsProfileDetail detailsPanel)
        {
            if (localGrid.Children.OfType<View.UserControls.SettingsProfileDetail>().Any() == true)
            {
                UserControls.SettingsProfileDetail oldControl = localGrid.Children.OfType<UserControls.SettingsProfileDetail>().First();
                oldControl.animation_SlideOut.Completed += (sender, e) =>
                {
                    localGrid.Children.Remove(oldControl);
                    localGrid.Children.Add(detailsPanel);
                };
                oldControl.animation_SlideOut.Begin();
            }
            else localGrid.Children.Add(detailsPanel);
        }

        private async void button_updateDD_Click(object sender, RoutedEventArgs e)
        {
            if (mainWindow.isProcessOngoing == true) return;

            if (localGrid.Children.OfType<View.UserControls.SettingsProfileDetail>().Any() == true)
            {
                UserControls.SettingsProfileDetail control = localGrid.Children.OfType<UserControls.SettingsProfileDetail>().First();
                control.animation_SlideOut.Completed += (sender, e) =>
                {
                    localGrid.Children.Remove(control);
                };
                control.animation_SlideOut.Begin();
            }

            mainWindow.isProcessOngoing = true;
            await Task.Run(() => settings.UpdateDataDragon());
            mainWindow.isProcessOngoing = false;

            settings.updateDDneeded = false;
            panel_DDupdate.Visibility = Visibility.Collapsed;
            settings.LoadSettingsFile();
        }

        private async void button_updateApp_Click(object sender, RoutedEventArgs e)
        {
            if (mainWindow.isProcessOngoing == true) return;

            if (localGrid.Children.OfType<View.UserControls.SettingsProfileDetail>().Any() == true)
            {
                UserControls.SettingsProfileDetail control = localGrid.Children.OfType<UserControls.SettingsProfileDetail>().First();
                control.animation_SlideOut.Completed += (sender, e) =>
                {
                    localGrid.Children.Remove(control);
                };
                control.animation_SlideOut.Begin();
            }

            mainWindow.isProcessOngoing = true;
            await settings.UpdateApplication();
            mainWindow.isProcessOngoing = false;
        }
    }
}
