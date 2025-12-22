using System.Windows.Controls;
using System.Windows.Media.Animation;

namespace League_Analyser.View.UserControls
{
    public partial class SettingsProfileDetail : UserControl
    {
        private MainWindow mainWindow = (MainWindow)App.Current.MainWindow;
        private Settings settings;

        public Storyboard animation_SlideIn;
        public Storyboard animation_SlideOut;

        public SettingsProfileDetail()
        {
            InitializeComponent();

            settings = mainWindow.settings;

            animation_SlideIn = (Storyboard)this.Resources["SlideIn"];
            animation_SlideOut = (Storyboard)this.Resources["SlideOut"];
            this.Loaded += (sender, e) => { animation_SlideIn.Begin(); };

            button_delete.Click += (sender, e) =>
            {
                if (mainWindow.isProcessOngoing == true) return;

                if (settings.DeleteProfile(profileName.Text) == true)
                {
                    animation_SlideOut.Completed += (sender, e) =>
                    {
                        if (this.Parent != null) ((Panel)this.Parent).Children.Remove(this);
                    };
                    animation_SlideOut.Begin();
                }
            };

            button_loadProfile.Click += async (sender, e) =>
            {
                if (mainWindow.isProcessOngoing == true) return;

                mainWindow.isProcessOngoing = true;
                await settings.LoadProfileData(profileName.Text, true);
                mainWindow.isProcessOngoing = false;
            };

            button_downloadAll.Click += async (sender, e) =>
            {
                if (mainWindow.isProcessOngoing == true) return;

                mainWindow.isProcessOngoing = true;
                await settings.DownloadProfilDataFromZero(profileName.Text, this);
                mainWindow.isProcessOngoing = false;
            };
        }


    }
}
