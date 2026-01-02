using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Xml.Linq;

namespace League_Analyser.View.UserControls
{
    public partial class SpellPopup : UserControl
    {
        private TimelineSummary timelineSummaryCtrl;
        private Storyboard animation_fadeIn;
        private TimelineData.DamageStats damage;
        private TimelineData.SpellData spellData;
        private int playerId;
        public SpellPopup(MouseButtonEventArgs e, TimelineSummary timelineSummary_,
            TimelineData.SpellData spellData_, TimelineData.DamageStats damage_, int playerId_)
        {
            InitializeComponent();
            timelineSummaryCtrl = timelineSummary_;
            damage = damage_;
            spellData = spellData_;
            playerId = playerId_;

            animation_fadeIn = (Storyboard)FindResource("FadeIn");
            this.MouseLeave += (sender, e) => { timelineSummaryCtrl.localGrid.Children.Remove(this); };

            SetPosition(e);
            ManagePopups();
            LoadSpellData();
            animation_fadeIn.Begin();
        }

        private void LoadSpellData()
        {
            image.Source = spellData.spellImage;
            skillName.Text = spellData.spellName;

            string parsedDescription = spellData.spellDescription;
            if (parsedDescription != null)
            {
                parsedDescription = Regex.Replace(spellData.spellDescription, @"<[^>]+>", "");
                parsedDescription = parsedDescription.Replace("&nbsp;", " ");
                parsedDescription = WebUtility.HtmlDecode(parsedDescription);
            }

            if (spellData.spellType == null) skillTypeName.Text = spellData.championName;
            else skillTypeName.Text = string.Format("{0} - {1}", spellData.championName, spellData.spellType);

            skillDescription.Text = parsedDescription;

            TextBlock toolTipDescription = new TextBlock();
            toolTipDescription.Text = parsedDescription;
            toolTipDescription.Foreground = new SolidColorBrush(Colors.White);
            toolTipDescription.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#E5232D37"));
            toolTipDescription.TextWrapping = TextWrapping.WrapWithOverflow;
            toolTipDescription.MaxWidth = 250;
            toolTipDescription.MaxHeight = 1000;
            toolTipDescription.FontSize = 10;
            toolTipDescription.Padding = new Thickness(3);

            skillDescription.ToolTip = toolTipDescription;

            damageTotal.Text = damage.all.ToString("#,##0");
            damagePhysical.Text = damage.physical.ToString("#,##0");
            damageMagic.Text = damage.magic.ToString("#,##0");
            damageTrue.Text = damage.trueDamage.ToString("#,##0");

            bool isPlayerAVictim = timelineSummaryCtrl.eventData.victimId == playerId;

            DamageBar damageBar = new DamageBar(damage, timelineSummaryCtrl.timeline, playerId, isPlayerAVictim);
            damageBar.HorizontalAlignment = HorizontalAlignment.Right;
            damageBar.VerticalAlignment = VerticalAlignment.Bottom;
            localGrid.Children.Add(damageBar);
        }

        private void SetPosition(MouseButtonEventArgs e)
        {
            Point foo_loc = e.GetPosition(timelineSummaryCtrl.localGrid.Children[1]);
            Thickness foo_margin = new Thickness();
            foo_margin.Left = foo_loc.X + 60;

            Grid.SetRow(this, 1);

            if ((this.Height + foo_loc.Y + 60) < timelineSummaryCtrl.localGrid.Children[1].RenderSize.Height)
            {
                foo_margin.Top = foo_loc.Y + 60;
                this.Margin = foo_margin;
            }
            else
            {
                foo_margin.Top = foo_loc.Y - this.Height - 60;
                this.Margin = foo_margin;
            }
        }

        private void ManagePopups()
        {
            List<SpellPopup> popupsList = timelineSummaryCtrl.localGrid.Children.OfType<SpellPopup>().ToList();
            for (int i = popupsList.Count - 1; i >= 0; i--)
            {
                timelineSummaryCtrl.localGrid.Children.Remove(popupsList[i]);
            }

            timelineSummaryCtrl.localGrid.Children.Add(this);
        }
    }
}
