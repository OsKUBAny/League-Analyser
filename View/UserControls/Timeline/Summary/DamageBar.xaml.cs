using System;
using System.Windows;
using System.Windows.Controls;

namespace League_Analyser.View.UserControls
{
    public partial class DamageBar : UserControl
    {
        private TimelineData.DamageStats damage;
        private TimelineData timeline;
        private int playerId;
        private bool isVictim;
        public DamageBar(TimelineData.DamageStats damage_, TimelineData timeline_, int playerId_, bool isVictim_)
        {
            InitializeComponent();
            damage = damage_;
            timeline = timeline_;
            playerId = playerId_;
            isVictim = isVictim_;

            CalculateDamage();
        }

        private void CalculateDamage()
        {
            if (damage.all == 0)
            {
                localGrid.Visibility = Visibility.Collapsed;
                return;
            }

            double damagePercent_physical = (double)damage.physical / damage.all;
            double damagePercent_magic = (double)damage.magic / damage.all;
            double damagePercent_true = (double)damage.trueDamage / damage.all;

            damageFill_physical.Width = Width * damagePercent_physical;
            damageFill_magic.Width = Width * damagePercent_magic;
            damageFill_true.Width = Width * damagePercent_true;

            damagePercentText_physical.Text = Math.Round(damagePercent_physical * 100, 0, MidpointRounding.AwayFromZero) + "%";
            damagePercentText_magic.Text = Math.Round(damagePercent_magic * 100, 0, MidpointRounding.AwayFromZero) + "%";
            damagePercentText_true.Text = Math.Round(damagePercent_true * 100, 0, MidpointRounding.AwayFromZero) + "%";

            TimelineData.DamageStats totalDamagePlayer = timeline.eventTotalDamage.Find(p => p.playerId == playerId).damage;
            TimelineData.DamageStats totalDamageFight = timeline.eventTotalDamage.Find(p => p.playerId == -1).damage;

            double damagePercent_skill = (double)damage.all / totalDamageFight.all;
            double damagePercent_player = (double)totalDamagePlayer.all / totalDamageFight.all;

            if (isVictim == true)
            {
                damagePercent_skill = (double)damage.all / totalDamagePlayer.all;
                damagePercentText_player.Visibility = Visibility.Collapsed;
                damageTotalPlayer.Visibility = Visibility.Collapsed;
            }

            damagePercentText_skill.Text = string.Format("obrażeń: {0}%",
                Math.Round(damagePercent_skill * 100, 0, MidpointRounding.AwayFromZero));
            damageTotalSkill.Width = Width * damagePercent_skill;

            damagePercentText_player.Text = string.Format("gracza: {0}%",
                Math.Round(damagePercent_player * 100, 0, MidpointRounding.AwayFromZero));
            damageTotalPlayer.Width = Width * damagePercent_player;
        }
    }
}
