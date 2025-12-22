using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Effects;

namespace League_Analyser.View.UserControls
{
    public partial class TimelineKillSummary : UserControl
    {
        private MainWindow mainWindow;
        private TimelineData timeline;
        private TimelineSummary timelineSummaryCtrl;
        private List<DataType.DamageDealt> damageEvents;

        private double damageTypePercentValue = 0.5;

        public TimelineKillSummary(TimelineData timeline_, TimelineSummary timelineSummary_,
            string playerName_, int playerId, List<DataType.DamageDealt> damageEvents_)
        {
            InitializeComponent();
            mainWindow = (MainWindow)App.Current.MainWindow;

            playerName.Text = playerName_;
            damageEvents = damageEvents_;
            timeline = timeline_;
            timelineSummaryCtrl = timelineSummary_;

            if (playerId > 0)
            {
                // Autoattack
                if (damageEvents != null && damageEvents.Any(p => p.basic == true))
                {
                    TimelineData.DamageStats damageStats = timeline.CalculateDamage(damageEvents.Where(p => p.basic == true).ToList());

                    TimelineData.SpellData spellData = timeline.GetSpellData(playerId, -2);

                    spellsContainer.Children.Add(CreateSpellData(spellData, damageStats, playerId));
                }

                // Passive
                if (damageEvents != null && damageEvents.Any(p => p.spellSlot == 63))
                {
                    TimelineData.DamageStats damageStats = timeline.CalculateDamage(damageEvents.Where(p => p.spellSlot == 63).ToList());

                    TimelineData.SpellData spellData = timeline.GetSpellData(playerId, -1);

                    spellsContainer.Children.Add(CreateSpellData(spellData, damageStats, playerId));
                }

                // QWER, DF
                for (int i = 0; i <= 5; i++)
                {
                    if (damageEvents != null && damageEvents.Any(p => p.spellSlot == i))
                    {
                        TimelineData.DamageStats damageStats = timeline.CalculateDamage(damageEvents.Where(p => p.spellSlot == i).ToList());

                        TimelineData.SpellData spellData = timeline.GetSpellData(playerId, i);

                        spellsContainer.Children.Add(CreateSpellData(spellData, damageStats, playerId));
                    }
                }

                // Other
                if (damageEvents != null && damageEvents.Any(
                    p => p.basic == false && ((p.spellSlot != 63 && p.spellSlot > 5) || p.spellSlot < 0)))
                {
                    TimelineData.DamageStats damageStats = timeline.CalculateDamage(damageEvents.Where(
                        p => p.basic == false && ((p.spellSlot != 63 && p.spellSlot > 5) || p.spellSlot < 0)).ToList());

                    TimelineData.SpellData spellData = timeline.GetSpellData(playerId, -3);
                    spellsContainer.Children.Add(CreateSpellData(spellData, damageStats, playerId));
                }
            }
            else if (playerId == 0)
            {
                // Minion
                if (damageEvents != null && damageEvents.Any(p => p.type == "MINION"))
                {
                    TimelineData.DamageStats damageStats = timeline.CalculateDamage(damageEvents.Where(
                        p => p.type == "MINION").ToList());

                    TimelineData.SpellData spellData = timeline.GetSpellData(playerId, 11);
                    spellsContainer.Children.Add(CreateSpellData(spellData, damageStats, playerId));
                }
                // Tower
                if (damageEvents != null && damageEvents.Any(p => p.type == "TOWER"))
                {
                    TimelineData.DamageStats damageStats = timeline.CalculateDamage(damageEvents.Where(
                        p => p.type == "TOWER").ToList());

                    TimelineData.SpellData spellData = timeline.GetSpellData(playerId, 12);
                    spellsContainer.Children.Add(CreateSpellData(spellData, damageStats, playerId));
                }
                // Monster
                if (damageEvents != null && damageEvents.Any(p => p.type == "MONSTER"))
                {
                    TimelineData.DamageStats damageStats = timeline.CalculateDamage(damageEvents.Where(
                        p => p.type == "MONSTER").ToList());

                    TimelineData.SpellData spellData = timeline.GetSpellData(playerId, 13);
                    spellsContainer.Children.Add(CreateSpellData(spellData, damageStats, playerId));
                }
                // Other
                if (damageEvents != null && damageEvents.Any(p => p.type != "MINION" && p.type != "TOWER" && p.type != "MONSTER"))
                {
                    TimelineData.DamageStats damageStats = timeline.CalculateDamage(damageEvents.Where(
                        p => p.type != "MINION" && p.type != "TOWER" && p.type != "MONSTER").ToList());

                    TimelineData.SpellData spellData = timeline.GetSpellData(playerId, 10);
                    spellsContainer.Children.Add(CreateSpellData(spellData, damageStats, playerId));
                }
            }
            DamageSummaryBar summaryBar = new DamageSummaryBar(timeline, playerId, timelineSummaryCtrl.eventData.victimId == playerId);
            localGrid.Children.Add(summaryBar);
        }

        private Grid CreateSpellData(TimelineData.SpellData spellData, TimelineData.DamageStats damage, int playerId)
        {
            Grid container = new Grid();
            container.Margin = new Thickness(3, 0, 0, 0);

            Image image = new Image();
            image.Width = 25;
            image.Height = 25;
            image.VerticalAlignment = VerticalAlignment.Top;
            image.Source = spellData.spellImage;

            TextBlock value = new TextBlock();
            value.FontSize = 8;
            value.HorizontalAlignment = HorizontalAlignment.Center;
            value.VerticalAlignment = VerticalAlignment.Bottom;
            value.Text = damage.all.ToString("#,##0");

            if (((double)damage.physical / damage.all) > damageTypePercentValue) value.Foreground = new SolidColorBrush(Colors.Red);
            else if (((double)damage.magic / damage.all) > damageTypePercentValue) value.Foreground = new SolidColorBrush(Colors.LightBlue);
            else if (((double)damage.trueDamage / damage.all) > damageTypePercentValue) value.Foreground = new SolidColorBrush(Colors.White);
            else value.Foreground = new SolidColorBrush(Colors.LightGray);

            TextBlock spellT = new TextBlock();
            spellT.FontSize = 15;
            spellT.Foreground = new SolidColorBrush(Colors.White);
            spellT.FontWeight = FontWeights.Black;
            spellT.VerticalAlignment = VerticalAlignment.Bottom;
            spellT.HorizontalAlignment = HorizontalAlignment.Right;
            spellT.Margin = new Thickness(0, 0, 2, 7);
            spellT.Text = spellData.spellType;

            TextBlock spellTShadow = new TextBlock();
            spellTShadow.FontSize = 17;
            spellTShadow.Foreground = new SolidColorBrush(Colors.Black);
            spellTShadow.FontWeight = FontWeights.Black;
            spellTShadow.VerticalAlignment = VerticalAlignment.Bottom;
            spellTShadow.HorizontalAlignment = HorizontalAlignment.Right;
            spellTShadow.Margin = new Thickness(0, 0, 3, 8);
            spellTShadow.Text = spellData.spellType;

            DropShadowEffect shadowEffect = new DropShadowEffect
            {
                Color = Colors.Black,
                BlurRadius = 5,
                ShadowDepth = 2,
                Opacity = 1
            };
            spellT.Effect = shadowEffect;

            container.Children.Add(image);
            container.Children.Add(value);
            container.Children.Add(spellTShadow);
            container.Children.Add(spellT);

            container.MouseLeftButtonDown += (sender, e) =>
            { new SpellPopup(e, timelineSummaryCtrl, spellData, damage, playerId); };

            return container;
        }
    }
}