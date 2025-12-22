using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace League_Analyser
{
    public class Data
    {
        private MainWindow mainWindow;
        private ApiData apiData;

        public void DataInit()
        {
            mainWindow = (MainWindow)App.Current.MainWindow;
            apiData = mainWindow.apiData;
        }

        public string applicationVersion = "1.6";
        public string dataStructVersion = "1.0";
        public string gameVersion = "0.0";
        public string savedPlayerDataPath = "data/player/{0}.json";

        public Player player = new Player();
        public List<string> historyGameIds = new List<string>();
        public List<DataType.MatchLao> matches = new List<DataType.MatchLao>();

        public List<DataType.MapsDto> mapsDto;
        public DataType.ChampionDataDto championDataDto;
        public DataType.ItemClass.Items itemsDto;
        public DataType.SummonerSpell summonerDto;

        public class Player
        {
            public DataType.AccountDto account { get; set; }
            public string server { get; set; }
        }

        public class PlayerData
        {
            public string dataStructVersion { get; set; }
            public Player player { get; set; }
            public List<string> historyGameIds { get; set; }
            public List<DataType.MatchLao> matches { get; set; }
        }

        public async Task<DataType.MatchLao> GetMatch(string id, Player playerRef)
        {
            DataType.MatchDto matchInfo;

            matchInfo = await apiData.ApiGetData(typeof(DataType.MatchDto), playerRef.server, ApiData.EndPoint.getMatchByMatchId, id);
            if (matchInfo == null) return null;

            DataType.Preview preview = GetMatchPreview(id, matchInfo, playerRef.account.gameName);
            if (preview == null) return null;

            DataType.GameInfo gameInfo = GetMatchGameInfo(matchInfo, playerRef.account.puuid);
            if (gameInfo == null) return null;

            List<DataType.Participant> participants = GetMatchParticipants(matchInfo, gameInfo.myTeamId);
            if (participants == null) return null;

            int totalAllayGold = 0;
            int totalEnemyGold = 0;

            foreach (DataType.Participant participant in participants)
            {
                if (participant.teamId == gameInfo.myTeamId) totalAllayGold += participant.goldEarned;
                else totalEnemyGold += participant.goldEarned;
            }

            gameInfo.allayTeamGold = totalAllayGold;
            gameInfo.enemyTeamGold = totalEnemyGold;

            DataType.MatchLao match = new DataType.MatchLao();
            match.preview = preview;
            match.gameInfo = gameInfo;
            match.participants = participants;
            return match;
        }

        public dynamic GetMatchPreview(string id, DataType.MatchDto matchInfo, string gameName)
        {
            DataType.Preview preview = new DataType.Preview();

            DataType.ParticipantDto myself = matchInfo.info.participants.Find(p => p.riotIdGameName == gameName);
            if (myself == null) return null;

            preview.matchId = id;
            preview.timestampUnix = matchInfo.info.gameCreation;
            preview.timestamp = DateTimeOffset.FromUnixTimeMilliseconds(preview.timestampUnix).DateTime.ToLocalTime().ToString("dd.MM.yyyy, HH:mm");
            preview.mapId = matchInfo.info.mapId;
            preview.mode = matchInfo.info.gameMode;
            preview.championId = myself.championId;
            preview.kills = myself.kills;
            preview.deaths = myself.deaths;
            preview.assists = myself.assists;
            preview.result = myself.win;

            return preview;
        }

        public dynamic GetMatchGameInfo(DataType.MatchDto matchInfo, string puuid)
        {
            DataType.GameInfo gameInfo = new DataType.GameInfo();

            gameInfo.gameDurationUnix = matchInfo.info.gameDuration;
            gameInfo.gameDuration = string.Format("{0}:{1:D2}", (matchInfo.info.gameDuration / 60) % 60, matchInfo.info.gameDuration % 60);
            gameInfo.gameVersion = matchInfo.info.gameVersion;
            try
            {
                var foo_participant = matchInfo.info.participants.Find(p => p.puuid == puuid);
                if (foo_participant == null) throw new Exception();
                gameInfo.myTeamId = foo_participant.teamId;

                gameInfo.allayTeamKills = matchInfo.info.teams.FirstOrDefault(p => p.teamId == gameInfo.myTeamId).objectives.champion.kills;
                gameInfo.enemyTeamKills = matchInfo.info.teams.FirstOrDefault(p => p.teamId != gameInfo.myTeamId).objectives.champion.kills;

                gameInfo.allayTeamTurrets = matchInfo.info.teams.FirstOrDefault(p => p.teamId == gameInfo.myTeamId).objectives.tower.kills;
                gameInfo.enemyTeamTurrets = matchInfo.info.teams.FirstOrDefault(p => p.teamId != gameInfo.myTeamId).objectives.tower.kills;

                gameInfo.allayTeamDragons = matchInfo.info.teams.FirstOrDefault(p => p.teamId == gameInfo.myTeamId).objectives.dragon.kills;
                gameInfo.enemyTeamDragons = matchInfo.info.teams.FirstOrDefault(p => p.teamId != gameInfo.myTeamId).objectives.dragon.kills;

                gameInfo.allayTeamHeralds = matchInfo.info.teams.FirstOrDefault(p => p.teamId == gameInfo.myTeamId).objectives.riftHerald.kills;
                gameInfo.enemyTeamHeralds = matchInfo.info.teams.FirstOrDefault(p => p.teamId != gameInfo.myTeamId).objectives.riftHerald.kills;

                gameInfo.allayTeamBarons = matchInfo.info.teams.FirstOrDefault(p => p.teamId == gameInfo.myTeamId).objectives.baron.kills;
                gameInfo.enemyTeamBarons = matchInfo.info.teams.FirstOrDefault(p => p.teamId != gameInfo.myTeamId).objectives.baron.kills;

                gameInfo.gameEndedInSurrender = matchInfo.info.participants.FirstOrDefault(p => p.win == false).gameEndedInSurrender;
            }
            catch (Exception) { return null; }
            return gameInfo;
        }

        public async Task<dynamic> GetMatchTimeline(string matchId)
        {
            DataType.TimelineDto timelineDto = await apiData.ApiGetData(typeof(DataType.TimelineDto), player.server, ApiData.EndPoint.getMatchTimelineByMatchId, matchId);
            if (timelineDto == null) return null;

            return timelineDto;
        }

        public dynamic GetMatchParticipants(DataType.MatchDto matchInfo, int myTeamId)
        {
            List<DataType.Participant> participants = new List<DataType.Participant>();

            int totalDamageAllay = 0;
            int totalDamageEnemies = 0;
            foreach (var participant in matchInfo.info.participants)
            {
                if (participant.teamId == myTeamId) totalDamageAllay += participant.totalDamageDealtToChampions;
                else totalDamageEnemies += participant.totalDamageDealtToChampions;
            }

            for (int i = 0; i < matchInfo.info.participants.Count; i++)
            {
                DataType.Participant participantLao = new DataType.Participant();
                DataType.ParticipantDto participantDto = matchInfo.info.participants[i];

                participantLao.teamId = participantDto.teamId;
                participantLao.accountDto = new DataType.AccountDto()
                {
                    puuid = participantDto.puuid,
                    gameName = participantDto.riotIdGameName,
                    tagLine = participantDto.riotIdTagline
                };

                participantLao.championId = participantDto.championId;
                participantLao.champLevel = participantDto.champLevel;
                participantLao.kills = participantDto.kills;
                participantLao.deaths = participantDto.deaths;
                participantLao.assists = participantDto.assists;
                participantLao.minions = participantDto.totalMinionsKilled;
                participantLao.goldEarned = participantDto.goldEarned;
                participantLao.goldSpent = participantDto.goldSpent;
                participantLao.damageDealt = new DataType.Damage
                {
                    physical = participantDto.physicalDamageDealtToChampions,
                    magic = participantDto.magicDamageDealtToChampions,
                    trueDamage = participantDto.trueDamageDealtToChampions,
                    total = participantDto.totalDamageDealtToChampions
                };

                participantLao.damageDealtToBuildings = participantDto.damageDealtToBuildings;
                if (participantDto.teamId == myTeamId) participantLao.totalTeamDamage = totalDamageAllay;
                else participantLao.totalTeamDamage = totalDamageEnemies;

                participantLao.damageTaken = new DataType.Damage
                {
                    physical = participantDto.physicalDamageTaken,
                    magic = participantDto.magicDamageTaken,
                    trueDamage = participantDto.trueDamageTaken,
                    total = participantDto.totalDamageTaken
                };

                participantLao.timePlayed = participantDto.timePlayed;
                participantLao.timeSpentAlive = participantDto.timePlayed - participantDto.totalTimeSpentDead;
                participantLao.timeSpentAliveMax = participantDto.longestTimeSpentLiving;
                participantLao.timeCCdealt = participantDto.timeCCingOthers;

                participantLao.doubleKills = participantDto.doubleKills;
                participantLao.tripleKills = participantDto.tripleKills;
                participantLao.quadraKills = participantDto.quadraKills;
                participantLao.pentaKills = participantDto.pentaKills;
                participantLao.turretsTakedowns = participantDto.turretTakedowns;
                participantLao.inhibitorsTakedowns = participantDto.inhibitorTakedowns;
                participantLao.objectivesStolen = participantDto.objectivesStolen;

                if (participantDto.largestKillingSpree == 0 && participantDto.kills > 0) participantLao.largestKillingSpree = 1;
                else participantLao.largestKillingSpree = participantDto.largestKillingSpree;

                if (participantDto.firstBloodKill == true) participantLao.firstBlood = 2;
                else if (participantDto.firstBloodAssist == true) participantLao.firstBlood = 1;
                else participantLao.firstBlood = 0;

                if (participantDto.firstTowerKill == true) participantLao.firstTurret = 2;
                else if (participantDto.firstTowerAssist == true) participantLao.firstTurret = 1;
                else participantLao.firstTurret = 0;

                participantLao.largestCrit = participantDto.largestCriticalStrike;
                participantLao.totalHeal = participantDto.totalHeal;
                participantLao.totalDamageMitigated = participantDto.damageSelfMitigated;
                participantLao.totalHealsOnTeammates = participantDto.totalHealsOnTeammates;
                participantLao.totalDamageShieldedOnTeammates = participantDto.totalDamageShieldedOnTeammates;
                participantLao.visionScore = participantDto.visionScore;
                participantLao.wardsPlaced = participantDto.wardsPlaced;
                participantLao.spellCastQ = participantDto.spell1Casts;
                participantLao.spellCastW = participantDto.spell2Casts;
                participantLao.spellCastE = participantDto.spell3Casts;
                participantLao.spellCastR = participantDto.spell4Casts;
                participantLao.spellCastTotal = participantDto.spell1Casts + participantDto.spell2Casts + participantDto.spell3Casts + participantDto.spell4Casts;

                DataType.Spell spellD = summonerDto.data.FirstOrDefault(p => p.Value.key == participantDto.summoner1Id.ToString()).Value;
                participantLao.spellD = participantDto.summoner1Id;
                participantLao.spellCastD = participantDto.summoner1Casts;
                DataType.Spell spellF = summonerDto.data.FirstOrDefault(p => p.Value.key == participantDto.summoner2Id.ToString()).Value;
                participantLao.spellF = participantDto.summoner2Id;
                participantLao.spellCastF = participantDto.summoner2Casts;

                if (spellF.name == "Flash") participantLao.hasFlashOnF = 1;
                else participantLao.hasFlashOnF = 0;

                List<int> itemsLao = new List<int>();
                itemsLao.Insert(0, participantDto.item0);
                itemsLao.Insert(1, participantDto.item1);
                itemsLao.Insert(2, participantDto.item2);
                itemsLao.Insert(3, participantDto.item3);
                itemsLao.Insert(4, participantDto.item4);
                itemsLao.Insert(5, participantDto.item5);
                itemsLao.Insert(6, participantDto.item6);
                participantLao.items = itemsLao;

                participants.Add(participantLao);
            }

            if (participants.Count != 10) return null;
            else return participants;
        }

        public DataType.ItemDto GetPlayerItem(int itemId)
        {
            DataType.ItemDto item = new DataType.ItemDto();
            string id = itemId.ToString();

            var foo_item = itemsDto.data.FirstOrDefault(p => p.Key == id);
            if (foo_item.Value == null) return null;

            item.id = itemId;
            item.name = foo_item.Value.name;
            item.imageName = foo_item.Value.image.full;
            return item;
        }
    }
}
