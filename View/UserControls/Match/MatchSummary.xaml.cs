using System.Windows;
using System.Windows.Controls;

namespace League_Analyser.View.UserControls
{
    public partial class MatchSummary : UserControl
    {
        public MatchSummary() : this(null) { }
        public MatchSummary(DataType.MatchLao data)
        {
            InitializeComponent();
            if (data == null) return;
            DataType.GameInfo game = data.gameInfo;

            AllayKills.Text = game.allayTeamKills.ToString();
            EnemyKills.Text = game.enemyTeamKills.ToString();

            AllayTowers.Text = game.allayTeamTurrets.ToString();
            EnemyTowers.Text = game.enemyTeamTurrets.ToString();

            AllayGold.Text = ((double)((double)game.allayTeamGold / 1000)).ToString("F1") + "k";
            EnemyGold.Text = ((double)((double)game.enemyTeamGold / 1000)).ToString("F1") + "k";

            AllayDragons.Text = game.allayTeamDragons.ToString();
            EnemyDragons.Text = game.enemyTeamDragons.ToString();

            AllayHeralds.Text = game.allayTeamHeralds.ToString();
            EnemyHeralds.Text = game.enemyTeamHeralds.ToString();

            AllayBarons.Text = game.allayTeamBarons.ToString();
            EnemyBarons.Text = game.enemyTeamBarons.ToString();

            if (data.preview.mapId != 11) monsters.Visibility = Visibility.Hidden;

        }
    }
}
