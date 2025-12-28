using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace League_Analyser.View.UserControls
{
    public partial class ItemPopup : UserControl
    {
        private MainWindow mainWindow;
        private Storyboard animation_fadeIn;

        public ItemPopup(MouseButtonEventArgs e, DataType.ItemClass.Item item, DataType.Spell spell)
        {
            InitializeComponent();
            if (DesignerProperties.GetIsInDesignMode(this)) return;

            mainWindow = (MainWindow)App.Current.MainWindow;
            animation_fadeIn = (Storyboard)FindResource("FadeIn");

            this.MouseLeave += (sender, e) => { mainWindow.mainGrid.Children.Remove(this); };

            if (item != null) SetItemData(item);
            else if (spell != null) SetSpellData(spell);

            SetPosition(e);
            ManagePopups();
            animation_fadeIn.Begin();
        }

        private void SetPosition(MouseButtonEventArgs e)
        {
            Point foo_loc = e.GetPosition(mainWindow.mainGrid.Children[1]);
            Thickness foo_margin = new Thickness();
            foo_margin.Left = foo_loc.X;

            Grid.SetRow(this, 1);

            if ((this.Height + foo_loc.Y + 25) < mainWindow.mainGrid.Children[1].RenderSize.Height)
            {
                foo_margin.Top = foo_loc.Y + 25;
                this.Margin = foo_margin;
            }
            else
            {
                foo_margin.Top = foo_loc.Y - this.Height - 25;
                this.Margin = foo_margin;
            }
        }

        private void ManagePopups()
        {
            List<View.UserControls.ItemPopup> popupsList = mainWindow.mainGrid.Children.OfType<View.UserControls.ItemPopup>().ToList();
            for (int i = popupsList.Count - 1; i >= 0; i--)
            {
                mainWindow.mainGrid.Children.Remove(popupsList[i]);
            }

            mainWindow.mainGrid.Children.Add(this);
        }

        private void SetItemData(DataType.ItemClass.Item item)
        {
            ImageBrush brush;

            brush = new ImageBrush(LoadResources.LoadImage(item.image.full, LoadResources.ImagePath_t.DD_item, false).image);
            brush.Stretch = Stretch.UniformToFill;
            image.Source = brush.ImageSource;

            name.Text = item.name;
            description.Text = item.plaintext;
            gold.Text = item.gold.total.ToString("#,0");

            List<string>[] statsList = LoadResources.ParseItemDescritpion(item.description);
            for (int i = 0; i < statsList[0].Count; i++)
            {
                TextBlock controlText = new TextBlock
                {
                    HorizontalAlignment = HorizontalAlignment.Left,
                    VerticalAlignment = VerticalAlignment.Top,
                    FontSize = 10,
                    Width = 245,
                    TextWrapping = TextWrapping.WrapWithOverflow,
                };

                Run statTitle = new Run(statsList[0][i]) { Foreground = Brushes.LightBlue, FontWeight = FontWeights.Bold };
                Run foo_pause = new Run("  ");
                Run statDescription = new Run(statsList[1][i]) { Foreground = Brushes.White, FontWeight = FontWeights.Normal };
                controlText.Inlines.Add(statTitle);
                controlText.Inlines.Add(foo_pause);
                controlText.Inlines.Add(statDescription);

                stats.Children.Add(controlText);
                stats.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
                this.Height = 5 + stats.DesiredSize.Height;
            }
        }

        private void SetSpellData(DataType.Spell spell)
        {
            ImageBrush brush;
            brush = new ImageBrush(LoadResources.LoadImage(spell.image.full, LoadResources.ImagePath_t.DD_spell, false).image);

            brush.Stretch = Stretch.UniformToFill;
            image.Source = brush.ImageSource;

            name.Text = spell.name;
            description.Text = spell.description;

            // 'Seconds' text is related to resources language
            System.Globalization.CultureInfo resLang;
            try { resLang = new System.Globalization.CultureInfo(Properties.Settings.Default.ResourcesLanguage); }
            catch (Exception) {  resLang = new System.Globalization.CultureInfo("en-US"); }

            gold.Text = string.Format("{0} {1}", spell.cooldownBurn, Messages.ResourceManager.GetString("match_popupSeconds", resLang));
            goldIcon.Visibility = Visibility.Collapsed;
        }
    }
}
