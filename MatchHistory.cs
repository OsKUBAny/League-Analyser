using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.AccessControl;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace League_Analyser
{
    public class MatchHistory
    {
        private MainWindow mainWindow;
        private Info info;
        private Data data;

        public static class Statistics_t
        {
            public enum DisplayType_t
            {
                boolOnly = 0,
                valueAndBar,
                twoValuesAndBar
            };

            public enum CategoryType_t
            {
                [Display(Name = nameof(Messages.matchHistory_category_general), ResourceType = typeof(Messages))]
                generalStats = 0,

                [Display(Name = nameof(Messages.matchHistory_category_gold), ResourceType = typeof(Messages))]
                gold,

                [Display(Name = nameof(Messages.matchHistory_category_damageDealt), ResourceType = typeof(Messages))]
                damageDealt,

                [Display(Name = nameof(Messages.matchHistory_category_damageRecived), ResourceType = typeof(Messages))]
                damageTaken,

                [Display(Name = nameof(Messages.matchHistory_category_gameTime), ResourceType = typeof(Messages))]
                gameTime,

                [Display(Name = nameof(Messages.matchHistory_category_achievements), ResourceType = typeof(Messages))]
                gameTargets,

                [Display(Name = nameof(Messages.matchHistory_category_playerStats), ResourceType = typeof(Messages))]
                championStats,

                [Display(Name = nameof(Messages.matchHistory_category_skills), ResourceType = typeof(Messages))]
                abilities
            };

            public static readonly List<Statistics> Items = new List<Statistics>()
            {
                //Fist stat is default statistic and it has to be CategoryType_t 0!
                new Statistics(Messages.matchHistory_statistic_general_kills, CategoryType_t.generalStats, p => p.kills, DisplayType_t.valueAndBar),
                new Statistics(Messages.matchHistory_statistic_general_assists, CategoryType_t.generalStats, p => p.assists, DisplayType_t.valueAndBar),
                new Statistics(Messages.matchHistory_statistic_general_deaths, CategoryType_t.generalStats, p => p.deaths, DisplayType_t.valueAndBar),
                new Statistics(Messages.matchHistory_statistic_general_minions, CategoryType_t.generalStats, p => p.minions, DisplayType_t.valueAndBar),

                new Statistics(Messages.matchHistory_statistic_gold_gained, CategoryType_t.gold, p => p.goldEarned, DisplayType_t.valueAndBar),
                new Statistics(Messages.matchHistory_statistic_gold_spend, CategoryType_t.gold, p => p.goldSpent, DisplayType_t.valueAndBar),
                new Statistics(Messages.matchHistory_statistic_gold_ratio, CategoryType_t.gold, p => p.goldEarned, p=>p.goldSpent, DisplayType_t.twoValuesAndBar),

                new Statistics(Messages.matchHistory_statistic_damage_physical, CategoryType_t.damageDealt, p => p.damageDealt.physical, DisplayType_t.valueAndBar),
                new Statistics(Messages.matchHistory_statistic_damage_magic, CategoryType_t.damageDealt, p => p.damageDealt.magic, DisplayType_t.valueAndBar),
                new Statistics(Messages.matchHistory_statistic_damage_true, CategoryType_t.damageDealt, p => p.damageDealt.trueDamage, DisplayType_t.valueAndBar),
                new Statistics(Messages.matchHistory_statistic_damage_total, CategoryType_t.damageDealt, p => p.damageDealt.total, DisplayType_t.valueAndBar),
                new Statistics(Messages.matchHistory_statistic_damage_objectives, CategoryType_t.damageDealt, p => p.damageDealtToBuildings, DisplayType_t.valueAndBar),
                new Statistics(Messages.matchHistory_statistic_damage_physicalPercent, CategoryType_t.damageDealt,p => p.damageDealt.total, p => p.damageDealt.physical, DisplayType_t.twoValuesAndBar),
                new Statistics(Messages.matchHistory_statistic_damage_magicPercent, CategoryType_t.damageDealt,p => p.damageDealt.total, p => p.damageDealt.magic, DisplayType_t.twoValuesAndBar),
                new Statistics(Messages.matchHistory_statistic_damage_truePercent, CategoryType_t.damageDealt,p => p.damageDealt.total, p => p.damageDealt.trueDamage, DisplayType_t.twoValuesAndBar),
                new Statistics(Messages.matchHistory_statistic_damage_teamPercent, CategoryType_t.damageDealt, p => p.totalTeamDamage, p => p.damageDealt.total, DisplayType_t.twoValuesAndBar),

                new Statistics(Messages.matchHistory_statistic_damage_physical, CategoryType_t.damageTaken, p => p.damageTaken.physical, DisplayType_t.valueAndBar),
                new Statistics(Messages.matchHistory_statistic_damage_magic, CategoryType_t.damageTaken, p => p.damageTaken.magic, DisplayType_t.valueAndBar),
                new Statistics(Messages.matchHistory_statistic_damage_true, CategoryType_t.damageTaken, p => p.damageTaken.trueDamage, DisplayType_t.valueAndBar),
                new Statistics(Messages.matchHistory_statistic_damage_total, CategoryType_t.damageTaken, p => p.damageTaken.total, DisplayType_t.valueAndBar),

                new Statistics(Messages.matchHistory_statistic_time_alive, CategoryType_t.gameTime, p => p.timePlayed, p => p.timeSpentAlive, DisplayType_t.twoValuesAndBar),
                new Statistics(Messages.matchHistory_statistic_time_aliveMax, CategoryType_t.gameTime, p => p.timeSpentAliveMax, DisplayType_t.valueAndBar),
                new Statistics(Messages.matchHistory_statistic_time_cc, CategoryType_t.gameTime, p => p.timeCCdealt, DisplayType_t.valueAndBar),

                new Statistics(Messages.matchHistory_statistic_achivements_double, CategoryType_t.gameTargets, p => p.doubleKills, DisplayType_t.valueAndBar),
                new Statistics(Messages.matchHistory_statistic_achivements_tripple, CategoryType_t.gameTargets, p => p.tripleKills, DisplayType_t.valueAndBar),
                new Statistics(Messages.matchHistory_statistic_achivements_quadra, CategoryType_t.gameTargets, p => p.quadraKills, DisplayType_t.valueAndBar),
                new Statistics(Messages.matchHistory_statistic_achivements_penta, CategoryType_t.gameTargets, p => p.pentaKills, DisplayType_t.valueAndBar),
                new Statistics(Messages.matchHistory_statistic_achivements_seriesMax, CategoryType_t.gameTargets, p => p.largestKillingSpree, DisplayType_t.valueAndBar),
                new Statistics(Messages.matchHistory_statistic_achivements_turretParticipant, CategoryType_t.gameTargets, p => p.turretsTakedowns, DisplayType_t.valueAndBar),
                new Statistics(Messages.matchHistory_statistic_achivements_inhibitorParticipant, CategoryType_t.gameTargets, p => p.inhibitorsTakedowns, DisplayType_t.valueAndBar),
                new Statistics(Messages.matchHistory_statistic_achivements_firstBlood, CategoryType_t.gameTargets, p => p.firstBlood, DisplayType_t.boolOnly),
                new Statistics(Messages.matchHistory_statistic_achivements_firstTurret, CategoryType_t.gameTargets, p => p.firstTurret, DisplayType_t.boolOnly),
                new Statistics(Messages.matchHistory_statistic_achivements_steals, CategoryType_t.gameTargets, p => p.objectivesStolen, DisplayType_t.valueAndBar),

                new Statistics(Messages.matchHistory_statistic_player_critMax, CategoryType_t.championStats, p => p.largestCrit, DisplayType_t.valueAndBar),
                new Statistics(Messages.matchHistory_statistic_player_healthRestored, CategoryType_t.championStats, p => p.totalHeal, DisplayType_t.valueAndBar),
                new Statistics(Messages.matchHistory_statistic_player_damageMitigated, CategoryType_t.championStats, p => p.totalDamageMitigated, DisplayType_t.valueAndBar),
                new Statistics(Messages.matchHistory_statistic_player_teammatesHealed, CategoryType_t.championStats, p => p.totalHealsOnTeammates, DisplayType_t.valueAndBar),
                new Statistics(Messages.matchHistory_statistic_player_damageShielded, CategoryType_t.championStats, p => p.totalDamageShieldedOnTeammates, DisplayType_t.valueAndBar),
                new Statistics(Messages.matchHistory_statistic_player_visionScore, CategoryType_t.championStats, p => p.visionScore, DisplayType_t.valueAndBar),
                new Statistics(Messages.matchHistory_statistic_player_wardsPlaced, CategoryType_t.championStats, p => p.wardsPlaced, DisplayType_t.valueAndBar),

                new Statistics(Messages.matchHistory_statistic_skills_q, CategoryType_t.abilities, p => p.spellCastQ, DisplayType_t.valueAndBar),
                new Statistics(Messages.matchHistory_statistic_skills_w, CategoryType_t.abilities, p => p.spellCastW, DisplayType_t.valueAndBar),
                new Statistics(Messages.matchHistory_statistic_skills_e, CategoryType_t.abilities, p => p.spellCastE, DisplayType_t.valueAndBar),
                new Statistics(Messages.matchHistory_statistic_skills_r, CategoryType_t.abilities, p => p.spellCastR, DisplayType_t.valueAndBar),
                new Statistics(Messages.matchHistory_statistic_skills_qwerTotal, CategoryType_t.abilities, p => p.spellCastTotal, DisplayType_t.valueAndBar),
                new Statistics(Messages.matchHistory_statistic_skills_qPercent, CategoryType_t.abilities, p => p.spellCastTotal, p => p.spellCastQ, DisplayType_t.twoValuesAndBar),
                new Statistics(Messages.matchHistory_statistic_skills_wPercent, CategoryType_t.abilities, p => p.spellCastTotal, p => p.spellCastW, DisplayType_t.twoValuesAndBar),
                new Statistics(Messages.matchHistory_statistic_skills_ePercent, CategoryType_t.abilities, p => p.spellCastTotal, p => p.spellCastE, DisplayType_t.twoValuesAndBar),
                new Statistics(Messages.matchHistory_statistic_skills_rPercent, CategoryType_t.abilities, p => p.spellCastTotal, p => p.spellCastR, DisplayType_t.twoValuesAndBar),
                new Statistics(Messages.matchHistory_statistic_skills_d, CategoryType_t.abilities, p => p.spellCastD, DisplayType_t.valueAndBar),
                new Statistics(Messages.matchHistory_statistic_skills_f, CategoryType_t.abilities, p => p.spellCastF, DisplayType_t.valueAndBar),
                new Statistics(Messages.matchHistory_statistic_skills_dfRatio, CategoryType_t.abilities, (p => p.spellCastD + p.spellCastF), p => p.spellCastD, DisplayType_t.twoValuesAndBar), // Warning: sick variable, handled manually
                new Statistics(Messages.matchHistory_statistic_skills_fForFlash, CategoryType_t.abilities, p => p.hasFlashOnF, DisplayType_t.boolOnly)
            };
        }

        public class Statistics
        {
            public string DisplayName { get; set; }
            public Statistics_t.CategoryType_t Category { get; set; }
            public Func<DataType.Participant, object> ValueGetter { get; set; }
            public Func<DataType.Participant, object> ValueGetterAdditional { get; set; }
            public Statistics_t.DisplayType_t DisplayType { get; set; }

            public Statistics(string name, Statistics_t.CategoryType_t category, Func<DataType.Participant, object> valueGetter, Statistics_t.DisplayType_t displayType)
            {
                DisplayName = name;
                Category = category;
                ValueGetter = valueGetter;
                ValueGetterAdditional = null;
                DisplayType = displayType;
            }
            public Statistics(string name, Statistics_t.CategoryType_t category, Func<DataType.Participant, object> valueGetter, Func<DataType.Participant, object> valueGetterAdditional, Statistics_t.DisplayType_t displayType)
            {
                DisplayName = name;
                Category = category;
                ValueGetter = valueGetter;
                ValueGetterAdditional = valueGetterAdditional;
                DisplayType = displayType;
            }

            public override string ToString()
            {
                return DisplayName;
            }
        }

        public void MatchHistoryInit()
        {
            mainWindow = (MainWindow)App.Current.MainWindow;
            info = mainWindow.info;
            data = mainWindow.data;
        }

        public async void InitializeMatchHistory()
        {
            MatchHistoryInit();
            info.CreateNewPrompt(Info.Messages.process_matchHistory_loading);
            await Task.Delay(500);

            View.Screens.MatchHistory matchHistoryScreen = new View.Screens.MatchHistory();
            Grid.SetRow(matchHistoryScreen, 1);

            for (int i = mainWindow.mainGrid.Children.Count - 1; i >= 0; i--)
            {
                var control = mainWindow.mainGrid.Children[i];
                if (control.GetType() == typeof(View.Screens.MatchHistory)) mainWindow.mainGrid.Children.Remove(control);
            }

            mainWindow.mainGrid.Children.Add(matchHistoryScreen);

            bool isError = false;
            int errorCount = 0;

            foreach (DataType.MatchLao match in data.matches)
            {
                View.UserControls.MatchPreview control = new View.UserControls.MatchPreview(match.preview);
                matchHistoryScreen.matchList.Children.Add(control);
                if (control.isError == true)
                {
                    isError = true;
                    errorCount++;
                }
            }
            if (isError == true) info.CreateNewPrompt(Info.Messages.warning_matchHistory_sourcesNotLoaded, errorCount);
            else info.CreateNewPrompt(Info.Messages.process_terminateProcess);
        }

        public async void SelectMatch(View.UserControls.MatchPreview newSelectedControl, string matchId)
        {
            var matchHistoryScreen = mainWindow.mainGrid.Children.OfType<View.Screens.MatchHistory>().FirstOrDefault
                (p => p.Name == "matchHistory");

            foreach (View.UserControls.MatchPreview control in matchHistoryScreen.matchList.Children)
            {
                control.selected.Visibility = Visibility.Hidden;
            }
            newSelectedControl.selected.Visibility = Visibility.Visible;

            var oldStatScreen = matchHistoryScreen.gridStatistics.Children.OfType<View.UserControls.MatchStatistics>().FirstOrDefault
                (p => p.Name == "matchStatistics");

            if (oldStatScreen != null) await oldStatScreen.ClosePanel();

            View.UserControls.MatchStatistics matchStatistics = new View.UserControls.MatchStatistics(matchId);
            matchHistoryScreen.gridStatistics.Children.Add(matchStatistics);
        }
    }
}
