using System;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace League_Analyser.View.UserControls
{
    public partial class MatchPreview : UserControl
    {
        private DataType.Preview match;
        public bool isError = false;
        public string errorMessage = string.Empty;

        private MainWindow mainWindow;
        private Data data;
        private MatchHistory matchHistory;

        public MatchPreview(DataType.Preview matchPreviev)
        {
            InitializeComponent();
            if (DesignerProperties.GetIsInDesignMode(this)) return;

            mainWindow = (MainWindow)App.Current.MainWindow;
            match = matchPreviev;
            data = mainWindow.data;
            matchHistory = mainWindow.matchHistory;

            LoadMap();
            LoadChampion();
            LoadResult();

            date.Text = match.timestamp;
            kda.Text = string.Format("{0}/{1}/{2}", match.kills, match.deaths, match.assists);

            MouseLeftButtonDown += (sender, e) => { matchHistory.SelectMatch(this, match.matchId); };
        }

        private void LoadChampion()
        {
            try
            {
                var foo_champion = data.championDataDto.data.FirstOrDefault(p => p.Value.key == match.championId.ToString());
                if (foo_champion.Value == null) throw new Exception("Zwrócono pusty obiekt championDataDto");
                champion.Source = GetImageFromFile(LoadResources.ImagePath_t.DD_champion, foo_champion.Value.image.full).ImageSource;
            }
            catch (Exception ex)
            {
                isError = true;
                errorMessage = ex.Message;
                champion.Source = GetImageFromFile((LoadResources.ImagePath_t)(-1), null).ImageSource;
            }
        }

        private void LoadMap()
        {
            if (match == null || match.mapId == 0)
            {
                Background = GetImageFromFile((LoadResources.ImagePath_t)(-1), null);
                return;
            }

            switch (match.mapId)
            {
                case 11:
                    {
                        Background = GetImageFromFile(LoadResources.ImagePath_t.gC_img_maps, "map_SR.png");
                        break;
                    }
                case 12:
                    {
                        Background = GetImageFromFile(LoadResources.ImagePath_t.gC_img_maps, "map_ARAM.png");
                        break;
                    }
                default:
                    {
                        Background = GetImageFromFile((LoadResources.ImagePath_t)(-1), null);
                        break;
                    }
            }

            try { mapName.Text = data.mapsDto.Find(p => p.mapId == match.mapId).mapName; }
            catch (Exception) { mapName.Text = "(nieznana mapa)"; }
            gameType.Text = match.mode;
        }

        private void LoadResult()
        {
            result.Fill = new LinearGradientBrush
            {
                StartPoint = new Point(0, 0),
                EndPoint = new Point(0, 1),

                GradientStops = match.result ? new GradientStopCollection //game won
                {
                    new GradientStop((Color)ColorConverter.ConvertFromString("#FF0B7600"), 1),
                    new GradientStop((Color)ColorConverter.ConvertFromString("#FF17FF00"), 0.5)
                }
                : new GradientStopCollection // game lost
                {
                    new GradientStop((Color)ColorConverter.ConvertFromString("#FF760000"), 1),
                    new GradientStop((Color)ColorConverter.ConvertFromString("#FFFF0000"), 0.5)
                }
            };
        }

        private ImageBrush GetImageFromFile(LoadResources.ImagePath_t pathType, string var)
        {
            LoadResources.LoadedImage loadedImage = LoadResources.LoadImage(var, pathType, false);

            ImageBrush brush = new ImageBrush(loadedImage.image);
            brush.Stretch = Stretch.UniformToFill;

            if ((int)pathType == -1 || string.IsNullOrEmpty(var)) loadedImage.result = true;

            if (loadedImage.result == false)
            {
                isError = true;
                errorMessage = "Nie udało się załadować grafiki";
            }

            return brush;
        }
    }
}
