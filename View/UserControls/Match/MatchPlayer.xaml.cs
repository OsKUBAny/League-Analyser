using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace League_Analyser.View.UserControls
{
    public partial class MatchPlayer : UserControl
    {
        private MainWindow mainWindow;
        private Data data;
        public bool isError = false;
        public string errorMessage = string.Empty;

        public MatchPlayer(DataType.Participant player)
        {
            InitializeComponent();
            if (DesignerProperties.GetIsInDesignMode(this)) return;

            mainWindow = (MainWindow)App.Current.MainWindow;
            data = mainWindow.data;

            LoadChampion(player.championId);
            playerName.Text = player.accountDto.gameName;
            championLevel.Text = player.champLevel.ToString();
            kda.Text = string.Format("{0}/{1}/{2}", player.kills, player.deaths, player.assists);
            gold.Text = player.goldEarned.ToString("#,0");
            minions.Text = player.minions.ToString();

            LoadItems(player.items);
            LoadSummonerSpells(player.spellD, player.spellF);
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

        private void LoadChampion(int championId)
        {
            try
            {
                var foo_champion = data.championDataDto.data.FirstOrDefault(p => p.Value.key == championId.ToString());
                if (foo_champion.Value == null) throw new Exception("Zwrócono pusty obiekt championDataDto");
                championImg.Source = GetImageFromFile(LoadResources.ImagePath_t.DD_champion, foo_champion.Value.image.full).ImageSource;
                championName.Text = foo_champion.Value.name;
            }
            catch (Exception ex)
            {
                isError = true;
                errorMessage = ex.Message;
                championImg.Source = GetImageFromFile((LoadResources.ImagePath_t)(-1), null).ImageSource;
                championName.Text = "(nieznana postać)";
            }
        }

        private void LoadItems(List<int> items)
        {
            foreach (int itemId in items)
            {
                if (itemId == 0)
                {
                    Rectangle controlEmpty = new Rectangle
                    {
                        Width = 23,
                        Height = 23,
                        Fill = Brushes.Black,
                        HorizontalAlignment = HorizontalAlignment.Left,
                        Margin = new Thickness(0, 0, 2, 0)
                    };
                    itemsStackPanel.Children.Add(controlEmpty);
                }
                else
                {
                    var item = data.itemsDto.data.FirstOrDefault(p => p.Key == itemId.ToString());

                    Image controlImage = new Image
                    {
                        Source = GetImageFromFile(LoadResources.ImagePath_t.DD_item, item.Value.image.full).ImageSource,
                        Width = 23,
                        Height = 23,
                        HorizontalAlignment = HorizontalAlignment.Left,
                        Margin = new Thickness(0, 0, 2, 0),
                        Tag = item.Key
                    };
                    controlImage.MouseLeftButtonDown += (sender, e) =>
                    { new ItemPopup(e, item.Value, null); };

                    itemsStackPanel.Children.Add(controlImage);
                }

            }
        }

        private void LoadSummonerSpells(int spellD_id, int spellF_id)
        {
            var foo_spellD = data.summonerDto.data.FirstOrDefault(p => p.Value.key == spellD_id.ToString());
            if (foo_spellD.Value == null)
            {
                isError = true;
                spellD.Source = GetImageFromFile((LoadResources.ImagePath_t)(-1), null).ImageSource;
            }
            else
            {
                spellD.Source = GetImageFromFile(LoadResources.ImagePath_t.DD_spell, foo_spellD.Value.image.full).ImageSource;
                spellD.Tag = spellD_id.ToString();

                spellD.MouseLeftButtonDown += (sender, e) =>
                { new ItemPopup(e, null, foo_spellD.Value); };
            }

            var foo_spellF = data.summonerDto.data.FirstOrDefault(p => p.Value.key == spellF_id.ToString());
            if (foo_spellF.Value == null)
            {
                isError = true;
                spellF.Source = GetImageFromFile((LoadResources.ImagePath_t)(-1), null).ImageSource;
            }
            else
            {
                spellF.Source = GetImageFromFile(LoadResources.ImagePath_t.DD_spell, foo_spellF.Value.image.full).ImageSource;
                spellF.Tag = spellF_id.ToString();

                spellF.MouseLeftButtonDown += (sender, e) =>
                { new ItemPopup(e, null, foo_spellF.Value); };
            }
        }
    }
}
