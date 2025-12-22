using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
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
                [Description("Statystyki ogólne")]
                generalStats = 0,

                [Description("Złoto")]
                gold,

                [Description("Zadane obrażenie")]
                damageDealt,

                [Description("Otrzymane obrażenia")]
                damageTaken,

                [Description("Czas gry")]
                gameTime,

                [Description("Cele i osiągnięcia")]
                gameTargets,

                [Description("Statystyki postaci")]
                championStats,

                [Description("Użyte umiejętności")]
                abilities
            };

            public static readonly List<Statistics> Items = new List<Statistics>()
            {
                //Fist stat is default statistic and it has to be CategoryType_t 0!
                new Statistics("Zabójstwa", CategoryType_t.generalStats, p => p.kills, DisplayType_t.valueAndBar),
                new Statistics("Asysty", CategoryType_t.generalStats, p => p.assists, DisplayType_t.valueAndBar),
                new Statistics("Zgony", CategoryType_t.generalStats, p => p.deaths, DisplayType_t.valueAndBar),
                new Statistics("Zabite miniony", CategoryType_t.generalStats, p => p.minions, DisplayType_t.valueAndBar),

                new Statistics("Zdobyte złoto", CategoryType_t.gold, p => p.goldEarned, DisplayType_t.valueAndBar),
                new Statistics("Wydane złoto", CategoryType_t.gold, p => p.goldSpent, DisplayType_t.valueAndBar),
                new Statistics("Stosunek złota (wydane/zdobyte)", CategoryType_t.gold, p => p.goldEarned, p=>p.goldSpent, DisplayType_t.twoValuesAndBar),

                new Statistics("Fizyczne", CategoryType_t.damageDealt, p => p.damageDealt.physical, DisplayType_t.valueAndBar),
                new Statistics("Magiczne", CategoryType_t.damageDealt, p => p.damageDealt.magic, DisplayType_t.valueAndBar),
                new Statistics("True damage", CategoryType_t.damageDealt, p => p.damageDealt.trueDamage, DisplayType_t.valueAndBar),
                new Statistics("Całkowite", CategoryType_t.damageDealt, p => p.damageDealt.total, DisplayType_t.valueAndBar),
                new Statistics("Obiektom", CategoryType_t.damageDealt, p => p.damageDealtToBuildings, DisplayType_t.valueAndBar),
                new Statistics("% fizycznych", CategoryType_t.damageDealt,p => p.damageDealt.total, p => p.damageDealt.physical, DisplayType_t.twoValuesAndBar),
                new Statistics("% magicznych", CategoryType_t.damageDealt,p => p.damageDealt.total, p => p.damageDealt.magic, DisplayType_t.twoValuesAndBar),
                new Statistics("% true damage", CategoryType_t.damageDealt,p => p.damageDealt.total, p => p.damageDealt.trueDamage, DisplayType_t.twoValuesAndBar),
                new Statistics("% całej drużyny", CategoryType_t.damageDealt, p => p.totalTeamDamage, p => p.damageDealt.total, DisplayType_t.twoValuesAndBar),

                new Statistics("Fizyczne", CategoryType_t.damageTaken, p => p.damageTaken.physical, DisplayType_t.valueAndBar),
                new Statistics("Magiczne", CategoryType_t.damageTaken, p => p.damageTaken.magic, DisplayType_t.valueAndBar),
                new Statistics("True damage", CategoryType_t.damageTaken, p => p.damageTaken.trueDamage, DisplayType_t.valueAndBar),
                new Statistics("Całkowite", CategoryType_t.damageTaken, p => p.damageTaken.total, DisplayType_t.valueAndBar),

                new Statistics("% czasu będąc żywym", CategoryType_t.gameTime, p => p.timePlayed, p => p.timeSpentAlive, DisplayType_t.twoValuesAndBar),
                new Statistics("Najdłuższy czas bycia żywym", CategoryType_t.gameTime, p => p.timeSpentAliveMax, DisplayType_t.valueAndBar),
                new Statistics("Czas zadanych efektów CC", CategoryType_t.gameTime, p => p.timeCCdealt, DisplayType_t.valueAndBar),

                new Statistics("Double kills", CategoryType_t.gameTargets, p => p.doubleKills, DisplayType_t.valueAndBar),
                new Statistics("Tripple kills", CategoryType_t.gameTargets, p => p.tripleKills, DisplayType_t.valueAndBar),
                new Statistics("Quadra kills", CategoryType_t.gameTargets, p => p.quadraKills, DisplayType_t.valueAndBar),
                new Statistics("Penta kills", CategoryType_t.gameTargets, p => p.pentaKills, DisplayType_t.valueAndBar),
                new Statistics("Największa seria zabójstw", CategoryType_t.gameTargets, p => p.largestKillingSpree, DisplayType_t.valueAndBar),
                new Statistics("Udział w zniszczeniu wież", CategoryType_t.gameTargets, p => p.turretsTakedowns, DisplayType_t.valueAndBar),
                new Statistics("Udział w zniszczeniu inhibiorów", CategoryType_t.gameTargets, p => p.inhibitorsTakedowns, DisplayType_t.valueAndBar),
                new Statistics("Pierwsza krew", CategoryType_t.gameTargets, p => p.firstBlood, DisplayType_t.boolOnly),
                new Statistics("Pierwsza wieża", CategoryType_t.gameTargets, p => p.firstTurret, DisplayType_t.boolOnly),
                new Statistics("Ukradzione cele", CategoryType_t.gameTargets, p => p.objectivesStolen, DisplayType_t.valueAndBar),

                new Statistics("Największe trafienie krytyczne", CategoryType_t.championStats, p => p.largestCrit, DisplayType_t.valueAndBar),
                new Statistics("Przywrócone zdrowie", CategoryType_t.championStats, p => p.totalHeal, DisplayType_t.valueAndBar),
                new Statistics("Obrażenia zmniejszone przez pancerz/odporność", CategoryType_t.championStats, p => p.totalDamageMitigated, DisplayType_t.valueAndBar),
                new Statistics("Uleczenie sojuszników", CategoryType_t.championStats, p => p.totalHealsOnTeammates, DisplayType_t.valueAndBar),
                new Statistics("Obrażenia zablokowane przez rzucone tarcze", CategoryType_t.championStats, p => p.totalDamageShieldedOnTeammates, DisplayType_t.valueAndBar),
                new Statistics("Punkty wizji", CategoryType_t.championStats, p => p.visionScore, DisplayType_t.valueAndBar),
                new Statistics("Postawione totemy", CategoryType_t.championStats, p => p.wardsPlaced, DisplayType_t.valueAndBar),

                new Statistics("Użycia Q", CategoryType_t.abilities, p => p.spellCastQ, DisplayType_t.valueAndBar),
                new Statistics("Użycia W", CategoryType_t.abilities, p => p.spellCastW, DisplayType_t.valueAndBar),
                new Statistics("Użycia E", CategoryType_t.abilities, p => p.spellCastE, DisplayType_t.valueAndBar),
                new Statistics("Użycia R", CategoryType_t.abilities, p => p.spellCastR, DisplayType_t.valueAndBar),
                new Statistics("Łącznie [Q, W, E, R]", CategoryType_t.abilities, p => p.spellCastTotal, DisplayType_t.valueAndBar),
                new Statistics("% użycia Q", CategoryType_t.abilities, p => p.spellCastTotal, p => p.spellCastQ, DisplayType_t.twoValuesAndBar),
                new Statistics("% użycia W", CategoryType_t.abilities, p => p.spellCastTotal, p => p.spellCastW, DisplayType_t.twoValuesAndBar),
                new Statistics("% użycia E", CategoryType_t.abilities, p => p.spellCastTotal, p => p.spellCastE, DisplayType_t.twoValuesAndBar),
                new Statistics("% użycia R", CategoryType_t.abilities, p => p.spellCastTotal, p => p.spellCastR, DisplayType_t.twoValuesAndBar),
                new Statistics("Użycia D", CategoryType_t.abilities, p => p.spellCastD, DisplayType_t.valueAndBar),
                new Statistics("Użycia F", CategoryType_t.abilities, p => p.spellCastF, DisplayType_t.valueAndBar),
                new Statistics("% użycia D / % użycia F", CategoryType_t.abilities, p => p.spellCastD, p => p.spellCastF, DisplayType_t.twoValuesAndBar), // Warning: sick variable, handled manually
                new Statistics("\"F for Flash\" czyli gracz jest ułomny", CategoryType_t.abilities, p => p.hasFlashOnF, DisplayType_t.boolOnly)
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
