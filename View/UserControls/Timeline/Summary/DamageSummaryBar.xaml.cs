using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace League_Analyser.View.UserControls
{
    public partial class DamageSummaryBar : UserControl
    {
        private TimelineData timeline;
        private int playerId;
        private bool isVictim;

        public DamageSummaryBar(TimelineData timeline_, int playerId_, bool isVictim_)
        {
            InitializeComponent();
            timeline = timeline_;
            playerId = playerId_;
            isVictim = isVictim_;

            timeline.EventTotalDamageLoaded += () => LoadDamageData();
        }
        private void LoadDamageData()
        {
            if (timeline.eventTotalDamage.Any(p => p.playerId == playerId) == false && isVictim == false) return;

            TimelineData.DamageStats damage = new TimelineData.DamageStats();
            if (isVictim == false)
            {

                try { damage = timeline.eventTotalDamage.Find(p => p.playerId == playerId).damage; }
                catch (Exception) { }
            }

            if (isVictim == true)
            {
                List<DataType.DamageDealt> victimDamages = new List<DataType.DamageDealt>();
                damage = new TimelineData.DamageStats();

                foreach (TimelineData.TotalDamage playerDamage in timeline.eventTotalDamage.Where(
                    p => p.playerId != playerId && p.playerId != -1))
                {
                    victimDamages.Add(new DataType.DamageDealt()
                    {
                        physicalDamage = playerDamage.damage.physical,
                        magicDamage = playerDamage.damage.magic,
                        trueDamage = playerDamage.damage.trueDamage
                    });

                }
                damage = timeline.CalculateDamage(victimDamages);
                damageText_total.Foreground = new SolidColorBrush(Colors.DarkOrange);
                damageText_total.FontWeight = FontWeights.Bold;
            }

            damageText_total.Text = damage.all.ToString("#,##0");

            if (damage.all == 0) playerDamageContainer.Visibility = Visibility.Collapsed;
            else
            {
                damageText_physical.Text = damage.physical.ToString("#,##0");
                damageText_magic.Text = damage.magic.ToString("#,##0");
                damageText_true.Text = damage.trueDamage.ToString("#,##0");

                double damagePercent_physical = (double)damage.physical / damage.all;
                double damagePercent_magic = (double)damage.magic / damage.all;
                double damagePercent_true = (double)damage.trueDamage / damage.all;

                damageFill_physical.Width = damageFill_container.Width * damagePercent_physical;
                damageFill_magic.Width = damageFill_container.Width * damagePercent_magic;
                damageFill_true.Width = damageFill_container.Width * damagePercent_true;

                damagePercentText_physical.Text = Math.Round(damagePercent_physical * 100, 0, MidpointRounding.AwayFromZero) + "%";
                damagePercentText_magic.Text = Math.Round(damagePercent_magic * 100, 0, MidpointRounding.AwayFromZero) + "%";
                damagePercentText_true.Text = Math.Round(damagePercent_true * 100, 0, MidpointRounding.AwayFromZero) + "%";
            }

            if (isVictim) fightDamageContainer.Visibility = Visibility.Collapsed;
            else LoadFightDamageData(damage);
        }

        private void LoadFightDamageData(TimelineData.DamageStats damage)
        {
            double damageFightPercent;

            if (damage.all == 0) damageFightPercent = 0;
            else damageFightPercent = (double)damage.all / timeline.eventTotalDamage.Find(p => p.playerId == -1).damage.all;

            damageFightText_total.Text = Math.Round(damageFightPercent * 100, 0, MidpointRounding.AwayFromZero) + "%";
            damageFightFill.Width = fightDamageContainer.Width * damageFightPercent;
        }
    }
}
