using Newtonsoft.Json;
using System.Collections.Generic;

namespace League_Analyser
{
    public class DataType
    {
        public class AccountDto
        {
            public string puuid { get; set; }
            public string gameName { get; set; }
            public string tagLine { get; set; }
        }

        public class MatchDto
        {
            public MetadataDto metadata { get; set; }
            public InfoDto info { get; set; }
        }

        public class MetadataDto
        {
            public string dataVersion { get; set; }
            public string matchId { get; set; }
            public List<string> participants { get; set; }
        }

        public class InfoDto
        {
            public string endOfgameResult { get; set; }
            public long gameCreation { get; set; }
            public long gameDuration { get; set; }
            public long gameEndTimestamp { get; set; }
            public long gameId { get; set; }
            public string gameMode { get; set; }
            public string gameName { get; set; }
            public long gameStartTimestamp { get; set; }
            public string gameType { get; set; }
            public string gameVersion { get; set; }
            public int mapId { get; set; }
            public List<ParticipantDto> participants { get; set; }
            public string platformId { get; set; }
            public int queueId { get; set; }
            public List<TeamDto> teams { get; set; }
            public string tournamentCode { get; set; }
        }

        public class ParticipantDto
        {
            public int allInPings { get; set; }
            public int assistMePings { get; set; }
            public int assists { get; set; }
            public int baronKills { get; set; }
            public int bountyLevel { get; set; }
            public int champExperiance { get; set; }
            public int champLevel { get; set; }
            public int championId { get; set; }
            public string championName { get; set; }
            public int commandPings { get; set; }
            public int championTransform { get; set; }
            public int consumablesPurchased { get; set; }
            public ChallangesDto challenges { get; set; }
            public int damageDealtToBuildings { get; set; }
            public int damageDealtToObjectives { get; set; }
            public int damageDealtToTurrets { get; set; }
            public int damageSelfMitigated { get; set; }
            public int deaths { get; set; }
            public int detectorWardsPlaced { get; set; }
            public int doubleKills { get; set; }
            public int dragonKills { get; set; }
            public bool eligibleForProgression { get; set; }
            public int enemyMissingPings { get; set; }
            public int enemyVisionPings { get; set; }
            public bool firstBloodAssist { get; set; }
            public bool firstBloodKill { get; set; }
            public bool firstTowerAssist { get; set; }
            public bool firstTowerKill { get; set; }
            public bool gameEndedInEarlySurrender { get; set; }
            public bool gameEndedInSurrender { get; set; }
            public int holdPings { get; set; }
            public int getBackPings { get; set; }
            public int goldEarned { get; set; }
            public int goldSpent { get; set; }
            public string individualPosition { get; set; }
            public int inhibitorKills { get; set; }
            public int inhibitorTakedowns { get; set; }
            public int inhibitorLost { get; set; }
            public int item0 { get; set; }
            public int item1 { get; set; }
            public int item2 { get; set; }
            public int item3 { get; set; }
            public int item4 { get; set; }
            public int item5 { get; set; }
            public int item6 { get; set; }
            public int itemsPurchased { get; set; }
            public int killingSprees { get; set; }
            public int kills { get; set; }
            public string lane { get; set; }
            public int largestCriticalStrike { get; set; }
            public int largestKillingSpree { get; set; }
            public int largestMultiKill { get; set; }
            public int longestTimeSpentLiving { get; set; }
            public int magicDagameDealt { get; set; }
            public int magicDamageDealtToChampions { get; set; }
            public int magicDamageTaken { get; set; }
            public MissionsDto missions { get; set; }
            public int neutralMinionsKilled { get; set; }
            public int needVisionPings { get; set; }
            public int nexusKills { get; set; }
            public int nexustakedowns { get; set; }
            public int nexusLost { get; set; }
            public int objectivesStolen { get; set; }
            public int objectivesStolenAssists { get; set; }
            public int onMyWayPings { get; set; }
            public int participantId { get; set; }
            public int playerScore0 { get; set; }
            public int playerScore1 { get; set; }
            public int playerScore2 { get; set; }
            public int playerScore3 { get; set; }
            public int playerScore4 { get; set; }
            public int playerScore5 { get; set; }
            public int playerScore6 { get; set; }
            public int playerScore7 { get; set; }
            public int playerScore8 { get; set; }
            public int playerScore9 { get; set; }
            public int playerScore10 { get; set; }
            public int playerScore11 { get; set; }
            public int pentaKills { get; set; }
            public PerksDto perks { get; set; }
            public int physicalDamageDealt { get; set; }
            public int physicalDamageDealtToChampions { get; set; }
            public int physicalDamageTaken { get; set; }
            public int placement { get; set; }
            public int playerAugment1 { get; set; }
            public int playerAugment2 { get; set; }
            public int playerAugment3 { get; set; }
            public int playerAugment4 { get; set; }
            public int playerSubteamId { get; set; }
            public int pushPings { get; set; }
            public int profileIcon { get; set; }
            public string puuid { get; set; }
            public int quadraKills { get; set; }
            public string riotIdGameName { get; set; }
            public string riotIdTagline { get; set; }
            public string role { get; set; }
            public int sightWardsBoughtInGame { get; set; }
            public int spell1Casts { get; set; }
            public int spell2Casts { get; set; }
            public int spell3Casts { get; set; }
            public int spell4Casts { get; set; }
            public int subteamPlacement { get; set; }
            public int summoner1Casts { get; set; }
            public int summoner1Id { get; set; }
            public int summoner2Casts { get; set; }
            public int summoner2Id { get; set; }
            public string summonerId { get; set; }
            public int summonerLevel { get; set; }
            public string summonerName { get; set; }
            public bool teamEarlySurrendered { get; set; }
            public int teamId { get; set; }
            public string teamPosition { get; set; }
            public int timeCCingOthers { get; set; }
            public int timePlayed { get; set; }
            public int totalAllyJungleMinionsKilled { get; set; }
            public int totalDamageDealt { get; set; }
            public int totalDamageDealtToChampions { get; set; }
            public int totalDamageShieldedOnTeammates { get; set; }
            public int totalDamageTaken { get; set; }
            public int totalEnemyJungleMinionsKilled { get; set; }
            public int totalHeal { get; set; }
            public int totalHealsOnTeammates { get; set; }
            public int totalMinionsKilled { get; set; }
            public int totalTimeCCDealt { get; set; }
            public int totalTimeSpentDead { get; set; }
            public int totalUnitsHealed { get; set; }
            public int tripleKills { get; set; }
            public int trueDamageDealt { get; set; }
            public int trueDamageDealtToChampions { get; set; }
            public int trueDamageTaken { get; set; }
            public int turretKills { get; set; }
            public int turretTakedowns { get; set; }
            public int turretsLost { get; set; }
            public int unrealKills { get; set; }
            public int visionScore { get; set; }
            public int visionClearedPings { get; set; }
            public int visionWardsBoughtInGame { get; set; }
            public int wardsKilled { get; set; }
            public int wardsPlaced { get; set; }
            public bool win { get; set; }
        }

        public class ChallangesDto
        {
            public int baronBuffGoldAdvantageOverThreshold { get; set; }
            public float controlWardTimeCoverageInRiverOrEnemyHalf { get; set; }
            public float earliestBaron { get; set; }
            public float earliestDragonTakedown { get; set; }
            public float earliestElderDragon { get; set; }
            public int earlyLaningPhaseGoldExpAdvantage { get; set; }
            public float fasterSupportQuestCompletion { get; set; }
            public float fastestLegendary { get; set; }
            public int hadAfkTeammate { get; set; }
            public int highestChampionDamage { get; set; }
            public int highestCrowdControlScore { get; set; }
            public int highestWardKills { get; set; }
            public int junglerKillsEarlyJungle { get; set; }
            public int killsOnLanersEarlyJungleAsJungler { get; set; }
            public int laningPhaseGoldExpAdvantage { get; set; }
            public int legendaryCount { get; set; }
            public float maxCsAdvantageOnLaneOpponent { get; set; }
            public int maxLevelLeadLaneOpponent { get; set; }
            public int mostWardsDestroyedOneSweeper { get; set; }
            public int mythicItemUsed { get; set; }
            public int playedChampSelectPosition { get; set; }
            public int soloTurretsLategame { get; set; }
            public int takedownsFirst25Minutes { get; set; }
            public int teleportTakedowns { get; set; }
            public int thirdInhibitorDestroyedTime { get; set; }
            public int threeWardsOneSweeperCount { get; set; }
            public float visionScoreAdvantageLaneOpponent { get; set; }
            public int InfernalScalePickup { get; set; }
            public int fistBumpParticipation { get; set; }
            public int voidMonsterKill { get; set; }
            public int abilityUses { get; set; }
            public int acesBefore15Minutes { get; set; }
            public float alliedJungleMonsterKills { get; set; }
            public int baronTakedowns { get; set; }
            public int blastConeOppositeOpponentCount { get; set; }
            public float bountyGold { get; set; }
            public int buffsStolen { get; set; }
            public int completeSupportQuestInTime { get; set; }
            public int controlWardsPlaced { get; set; }
            public float damagePerMinute { get; set; }
            public float damageTakenOnTeamPercentage { get; set; }
            public int dancedWithRiftHerald { get; set; }
            public int deathsByEnemyChamps { get; set; }
            public int dodgeSkillShotsSmallWindow { get; set; }
            public int doubleAces { get; set; }
            public int dragonTakedowns { get; set; }
            public List<int> legendaryItemUsed { get; set; }
            public float effectiveHealAndShielding { get; set; }
            public int elderDragonKillsWithOpposingSoul { get; set; }
            public int elderDragonMultikills { get; set; }
            public int enemyChampionImmobilizations { get; set; }
            public float enemyJungleMonsterKills { get; set; }
            public int epicMonsterKillsNearEnemyJungler { get; set; }
            public int epicMonsterKillsWithin30SecondsOfSpawn { get; set; }
            public int epicMonsterSteals { get; set; }
            public int epicMonsterStolenWithoutSmite { get; set; }
            public int firstTurretKilled { get; set; }
            public float firstTurretKilledTime { get; set; }
            public int flawlessAces { get; set; }
            public int fullTeamTakedown { get; set; }
            public float gameLength { get; set; }
            public int getTakedownsInAllLanesEarlyJungleAsLaner { get; set; }
            public float goldPerMinute { get; set; }
            public int hadOpenNexus { get; set; }
            public int immobilizeAndKillWithAlly { get; set; }
            public int initialBuffCount { get; set; }
            public int initialCrabCount { get; set; }
            public float jungleCsBefore10Minutes { get; set; }
            public int junglerTakedownsNearDamagedEpicMonster { get; set; }
            public float kda { get; set; }
            public int killAfterHiddenWithAlly { get; set; }
            public int killedChampTookFullTeamDamageSurvived { get; set; }
            public int killingSprees { get; set; }
            public float killParticipation { get; set; }
            public int killsNearEnemyTurret { get; set; }
            public int killsOnOtherLanesEarlyJungleAsLaner { get; set; }
            public int killsOnRecentlyHealedByAramPack { get; set; }
            public int killsUnderOwnTurret { get; set; }
            public int killsWithHelpFromEpicMonster { get; set; }
            public int knockEnemyIntoTeamAndKill { get; set; }
            public int kTurretsDestroyedBeforePlatesFall { get; set; }
            public int landSkillShotsEarlyGame { get; set; }
            public int laneMinionsFirst10Minutes { get; set; }
            public int lostAnInhibitor { get; set; }
            public int maxKillDeficit { get; set; }
            public int mejaisFullStackInTime { get; set; }
            public float moreEnemyJungleThanOpponent { get; set; }
            public int multiKillOneSpell { get; set; }
            public int multikills { get; set; }
            public int multikillsAfterAggressiveFlash { get; set; }
            public int multiTurretRiftHeraldCount { get; set; }
            public int outerTurretExecutesBefore10Minutes { get; set; }
            public int outnumberedKills { get; set; }
            public int outnumberedNexusKill { get; set; }
            public int perfectDragonSoulsTaken { get; set; }
            public int perfectGame { get; set; }
            public int pickKillWithAlly { get; set; }
            public int poroExplosions { get; set; }
            public int quickCleanse { get; set; }
            public int quickFirstTurret { get; set; }
            public int quickSoloKills { get; set; }
            public int riftHeraldTakedowns { get; set; }
            public int saveAllyFromDeath { get; set; }
            public int scuttleCrabKills { get; set; }
            public float shortestTimeToAceFromFirstTakedown { get; set; }
            public int skillshotsDodged { get; set; }
            public int skillshotsHit { get; set; }
            public int snowballsHit { get; set; }
            public int soloBaronKills { get; set; }
            public int SWARM_DefeatAatrox { get; set; }
            public int SWARM_DefeatBriar { get; set; }
            public int SWARM_DefeatMiniBosses { get; set; }
            public int SWARM_EvolveWeapon { get; set; }
            public int SWARM_Have3Passives { get; set; }
            public int SWARM_KillEnemy { get; set; }
            public float SWARM_PickupGold { get; set; }
            public int SWARM_ReachLevel50 { get; set; }
            public int SWARM_Survive15Min { get; set; }
            public int SWARM_WinWith5EvolvedWeapons { get; set; }
            public int soloKills { get; set; }
            public int stealthWardsPlaced { get; set; }
            public int survivedSingleDigitHpCount { get; set; }
            public int survivedThreeImmobilizesInFight { get; set; }
            public int takedownOnFirstTurret { get; set; }
            public int takedowns { get; set; }
            public int takedownsAfterGainingLevelAdvantage { get; set; }
            public int takedownsBeforeJungleMinionSpawn { get; set; }
            public int takedownsFirstXMinutes { get; set; }
            public int takedownsInAlcove { get; set; }
            public int takedownsInEnemyFountain { get; set; }
            public int teamBaronKills { get; set; }
            public float teamDamagePercentage { get; set; }
            public int teamElderDragonKills { get; set; }
            public int teamRiftHeraldKills { get; set; }
            public int tookLargeDamageSurvived { get; set; }
            public int turretPlatesTaken { get; set; }
            public int turretsTakenWithRiftHerald { get; set; }
            public int turretTakedowns { get; set; }
            public int twentyMinionsIn3SecondsCount { get; set; }
            public int twoWardsOneSweeperCount { get; set; }
            public int unseenRecalls { get; set; }
            public float visionScorePerMinute { get; set; }
            public int wardsGuarded { get; set; }
            public int wardTakedowns { get; set; }
            public int wardTakedownsBefore20M { get; set; }
        }

        public class MissionsDto
        {
            public int playerScore0 { get; set; }
            public int playerScore1 { get; set; }
            public int playerScore2 { get; set; }
            public int playerScore3 { get; set; }
            public int playerScore4 { get; set; }
            public int playerScore5 { get; set; }
            public int playerScore6 { get; set; }
            public int playerScore7 { get; set; }
            public int playerScore8 { get; set; }
            public int playerScore9 { get; set; }
            public int playerScore10 { get; set; }
            public int playerScore11 { get; set; }
        }

        public class PerksDto
        {
            public PerkStatsDto statPerks { get; set; }
            public List<PerkStyleDto> styles { get; set; }
        }

        public class PerkStatsDto
        {
            public int defense { get; set; }
            public int flex { get; set; }
            public int offense { get; set; }
        }

        public class PerkStyleDto
        {
            public string description { get; set; }
            public List<PerkStyleSelection> selections { get; set; }
            public int style { get; set; }
        }

        public class PerkStyleSelection
        {
            public int perk { get; set; }
            public int var1 { get; set; }
            public int var2 { get; set; }
            public int var3 { get; set; }
        }

        public class TeamDto
        {
            public List<BanDto> bans { get; set; }
            public ObjectivesDto objectives { get; set; }
            public int teamId { get; set; }
            public bool win { get; set; }
        }

        public class BanDto
        {
            public int championId { get; set; }
            public int pickTurn { get; set; }
        }

        public class ObjectivesDto
        {
            public ObjectiveDto baron { get; set; }
            public ObjectiveDto champion { get; set; }
            public ObjectiveDto dragon { get; set; }
            public ObjectiveDto horde { get; set; }
            public ObjectiveDto inhibitor { get; set; }
            public ObjectiveDto riftHerald { get; set; }
            public ObjectiveDto tower { get; set; }
        }

        public class ObjectiveDto
        {
            public bool first { get; set; }
            public int kills { get; set; }
        }

        public class TimelineDto
        {
            public MetadataTimeLineDto metadata { get; set; }
            public InfoTimeLineDto info { get; set; }
        }

        public class MetadataTimeLineDto
        {
            public string dataVersion { get; set; }
            public string matchId { get; set; }
            public List<string> participants { get; set; }
        }

        public class InfoTimeLineDto
        {
            public string endOfgameResult { get; set; }
            public long frameInterval { get; set; }
            public long gameId { get; set; }
            public List<ParticipantTimeLineDto> participants { get; set; }
            public List<FramesTimeLineDto> frames { get; set; }
        }

        public class ParticipantTimeLineDto
        {
            public int participantId { get; set; }
            public string puuid { get; set; }
        }

        public class FramesTimeLineDto
        {
            public List<EventsTimeLineDto> events { get; set; }
            public ParticipantFramesDto participantFrames { get; set; }
            public int timestamp { get; set; }
        }

        public class EventsTimeLineDto
        {
            public long timestamp { get; set; }
            public long realTimestamp { get; set; }
            public string type { get; set; }

#pragma warning disable CS8632 //For possible null values

            public List<int>? assistingParticipantIds { get; set; }
            public int? killerId { get; set; }
            public int? victimId { get; set; }
            public PositionDto? position { get; set; }
            public int? teamId { get; set; }

            //Object destroyed
            public string? buildingType { get; set; }
            public string? towerType { get; set; }
            public string? laneType { get; set; }
            public int? bounty { get; set; }

            //Monster kill
            public string? monsterType { get; set; }
            public string? monsterSubType { get; set; }

            //Champion kill
            public List<DamageDealt>? victimDamageDealt { get; set; }
            public List<DamageDealt>? victimDamageReceived { get; set; }

#pragma warning restore CS8632
        }

        public class DamageDealt
        {
            public string type { get; set; }
            public bool basic { get; set; }
            public string name { get; set; }
            public int participantid { get; set; }
            public string spellName { get; set; }
            public int spellSlot { get; set; }
            public int physicalDamage { get; set; }
            public int magicDamage { get; set; }
            public int trueDamage { get; set; }
        }

        public class ParticipantFramesDto
        {
            [JsonProperty("1")]
            public ParticipantFrameDto player1 { get; set; }
            [JsonProperty("2")]
            public ParticipantFrameDto player2 { get; set; }
            [JsonProperty("3")]
            public ParticipantFrameDto player3 { get; set; }
            [JsonProperty("4")]
            public ParticipantFrameDto player4 { get; set; }
            [JsonProperty("5")]
            public ParticipantFrameDto player5 { get; set; }
            [JsonProperty("6")]
            public ParticipantFrameDto player6 { get; set; }
            [JsonProperty("7")]
            public ParticipantFrameDto player7 { get; set; }
            [JsonProperty("8")]
            public ParticipantFrameDto player8 { get; set; }
            [JsonProperty("9")]
            public ParticipantFrameDto player9 { get; set; }
        }

        public class ParticipantFrameDto
        {
            public ChampionStatsDto championStats { get; set; }
            public int currentGold { get; set; }
            public DamageStatsDto damageStats { get; set; }
            public int goldPerSecond { get; set; }
            public int jungleMinionsKilled { get; set; }
            public int level { get; set; }
            public int minionsKilled { get; set; }
            public int participantId { get; set; }
            public PositionDto position { get; set; }
            public int timeEnemySpentControlled { get; set; }
            public int totalGold { get; set; }
            public int xp { get; set; }
        }

        public class ChampionStatsDto
        {
            public int abilityHaste { get; set; }
            public int abilityPower { get; set; }
            public int armor { get; set; }
            public int armorPen { get; set; }
            public int armorPenPercent { get; set; }
            public int attackDamage { get; set; }
            public int attackSpeed { get; set; }
            public int bonusArmorPenPercent { get; set; }
            public int bonusMagicPenPercent { get; set; }
            public int ccReduction { get; set; }
            public int cooldownReduction { get; set; }
            public int health { get; set; }
            public int healthMax { get; set; }
            public int healthRegen { get; set; }
            public int lifesteal { get; set; }
            public int magicPen { get; set; }
            public int magicPenPercent { get; set; }
            public int magicResist { get; set; }
            public int movementSpeed { get; set; }
            public int omnivamp { get; set; }
            public int physicalVamp { get; set; }
            public int power { get; set; }
            public int powerMax { get; set; }
            public int powerRegen { get; set; }
            public int spellVamp { get; set; }
        }

        public class DamageStatsDto
        {
            public int magicDamageDone { get; set; }
            public int magicDamageDoneToChampions { get; set; }
            public int magicDamageTaken { get; set; }
            public int physicalDamageDone { get; set; }
            public int physicalDamageDoneToChampions { get; set; }
            public int physicalDamageTaken { get; set; }
            public int totalDamageDone { get; set; }
            public int totalDamageDoneToChampions { get; set; }
            public int totalDamageTaken { get; set; }
            public int trueDamageDone { get; set; }
            public int trueDamageDoneToChampions { get; set; }
            public int trueDamageTaken { get; set; }
        }

        public class PositionDto
        {
            public int x { get; set; }
            public int y { get; set; }
        }

        public class MapsDto
        {
            public int mapId { get; set; }
            public string mapName { get; set; }
            public string notes { get; set; }
        }

        public class ChampionDataDto
        {
            public string type { get; set; }
            public string format { get; set; }
            public string version { get; set; }
            public Dictionary<string, Champion> data { get; set; }
        }

        public class Champion
        {
            public string version { get; set; }
            public string id { get; set; }
            public string key { get; set; }
            public string name { get; set; }
            public string title { get; set; }
            public string blurb { get; set; }
            public Info info { get; set; }
            public Image image { get; set; }
            public List<string> tags { get; set; }
            public string partype { get; set; }
            public Stats stats { get; set; }
            public List<Spell> spells { get; set; }
            public Passive passive { get; set; }
        }

        public class ItemClass
        {
            public class Items
            {
                public string type { get; set; }
                public string version { get; set; }
                public Dictionary<string, Item> data { get; set; }
            }

            public class Rune
            {
                public bool isrune { get; set; }
                public int tier { get; set; }
                public string type { get; set; }
            }

            public class Gold
            {
                public int @base { get; set; }
                public int total { get; set; }
                public int sell { get; set; }
                public bool purchasable { get; set; }
            }

            public class Stats
            {
                public int FlatHPPoolMod { get; set; }
                public int rFlatHPModPerLevel { get; set; }
                public int FlatMPPoolMod { get; set; }
                public int rFlatMPModPerLevel { get; set; }
                public int PercentHPPoolMod { get; set; }
                public int PercentMPPoolMod { get; set; }
                public int FlatHPRegenMod { get; set; }
                public int rFlatHPRegenModPerLevel { get; set; }
                public int PercentHPRegenMod { get; set; }
                public int FlatMPRegenMod { get; set; }
                public int rFlatMPRegenModPerLevel { get; set; }
                public int PercentMPRegenMod { get; set; }
                public int FlatArmorMod { get; set; }
                public int rFlatArmorModPerLevel { get; set; }
                public int PercentArmorMod { get; set; }
                public int rFlatArmorPenetrationMod { get; set; }
                public int rFlatArmorPenetrationModPerLevel { get; set; }
                public int rPercentArmorPenetrationMod { get; set; }
                public int rPercentArmorPenetrationModPerLevel { get; set; }
                public int FlatPhysicalDamageMod { get; set; }
                public int rFlatPhysicalDamageModPerLevel { get; set; }
                public int PercentPhysicalDamageMod { get; set; }
                public int FlatMagicDamageMod { get; set; }
                public int rFlatMagicDamageModPerLevel { get; set; }
                public int PercentMagicDamageMod { get; set; }
                public int FlatMovementSpeedMod { get; set; }
                public int rFlatMovementSpeedModPerLevel { get; set; }
                public int PercentMovementSpeedMod { get; set; }
                public int rPercentMovementSpeedModPerLevel { get; set; }
                public int FlatAttackSpeedMod { get; set; }
                public int PercentAttackSpeedMod { get; set; }
                public int rPercentAttackSpeedModPerLevel { get; set; }
                public int rFlatDodgeMod { get; set; }
                public int rFlatDodgeModPerLevel { get; set; }
                public int PercentDodgeMod { get; set; }
                public int FlatCritChanceMod { get; set; }
                public int rFlatCritChanceModPerLevel { get; set; }
                public int PercentCritChanceMod { get; set; }
                public int FlatCritDamageMod { get; set; }
                public int rFlatCritDamageModPerLevel { get; set; }
                public int PercentCritDamageMod { get; set; }
                public int FlatBlockMod { get; set; }
                public int PercentBlockMod { get; set; }
                public int FlatSpellBlockMod { get; set; }
                public int rFlatSpellBlockModPerLevel { get; set; }
                public int PercentSpellBlockMod { get; set; }
                public int FlatEXPBonus { get; set; }
                public int PercentEXPBonus { get; set; }
                public int rPercentCooldownMod { get; set; }
                public int rPercentCooldownModPerLevel { get; set; }
                public int rFlatTimeDeadMod { get; set; }
                public int rFlatTimeDeadModPerLevel { get; set; }
                public int rPercentTimeDeadMod { get; set; }
                public int rPercentTimeDeadModPerLevel { get; set; }
                public int rFlatGoldPer10Mod { get; set; }
                public int rFlatMagicPenetrationMod { get; set; }
                public int rFlatMagicPenetrationModPerLevel { get; set; }
                public int rPercentMagicPenetrationMod { get; set; }
                public int rPercentMagicPenetrationModPerLevel { get; set; }
                public int FlatEnergyRegenMod { get; set; }
                public int rFlatEnergyRegenModPerLevel { get; set; }
                public int FlatEnergyPoolMod { get; set; }
                public int rFlatEnergyModPerLevel { get; set; }
                public int PercentLifeStealMod { get; set; }
                public int PercentSpellVampMod { get; set; }
            }

            public class Item
            {
                public string id { get; set; }
                public string name { get; set; }
                public Image image { get; set; }
                public Rune rune { get; set; }
                public Gold gold { get; set; }
                public string group { get; set; }
                public string description { get; set; }
                public string colloq { get; set; }
                public string plaintext { get; set; }
                public bool consumed { get; set; }
                public int stacks { get; set; }
                public int depth { get; set; }
                public bool consumeOnFull { get; set; }
                public List<object> from { get; set; }
                public List<object> into { get; set; }
                public int specialRecipe { get; set; }
                public bool inStore { get; set; }
                public bool hideFromAll { get; set; }
                public string requiredChampion { get; set; }
                public string requiredAlly { get; set; }
                public Stats stats { get; set; }
                public List<string> tags { get; set; }
                public Dictionary<int, bool> maps { get; set; }
            }
        }

        public class Info
        {
            public int attack { get; set; }
            public int defense { get; set; }
            public int magic { get; set; }
            public int difficulty { get; set; }
        }

        public class Image
        {
            public string full { get; set; }
            public string sprite { get; set; }
            public string group { get; set; }
            public int x { get; set; }
            public int y { get; set; }
            public int w { get; set; }
            public int h { get; set; }
        }

        public class Stats
        {
            public float hp { get; set; }
            public float hpperlevel { get; set; }
            public float mp { get; set; }
            public float mpperlevel { get; set; }
            public float movespeed { get; set; }
            public float armor { get; set; }
            public float armorperlevel { get; set; }
            public float spellblock { get; set; }
            public float spellblockperlevel { get; set; }
            public float attackrange { get; set; }
            public float hpregen { get; set; }
            public float hpregenperlevel { get; set; }
            public float mpregen { get; set; }
            public float mpregenperlevel { get; set; }
            public float crit { get; set; }
            public float critperlevel { get; set; }
            public float attackdamage { get; set; }
            public float attackdamageperlevel { get; set; }
            public float attackspeedperlevel { get; set; }
            public float attackspeed { get; set; }
        }

        public class SummonerSpell
        {
            public string type { get; set; }
            public string version { get; set; }
            public Dictionary<string, Spell> data { get; set; }
        }

        public class Spell
        {
            public string id { get; set; }
            public string name { get; set; }
            public string description { get; set; }
            public string tooltip { get; set; }
            public int maxrank { get; set; }
            public List<int> cooldown { get; set; }
            public string cooldownBurn { get; set; }
            public List<int> cost { get; set; }
            public string costBurn { get; set; }
            public string key { get; set; }
            public int summonerLevel { get; set; }
            public List<string> modes { get; set; }
            public string costType { get; set; }
            public string maxammo { get; set; }
            public List<int> range { get; set; }
            public string rangeBurn { get; set; }
            public Image image { get; set; }
            public string resource { get; set; }
            public int casts { get; set; }
        }

        public class Passive
        {
            public string name { get; set; }
            public string description { get; set; }
            public Image image { get; set; }
        }

        public class MatchLao
        {
            public Preview preview { get; set; }
            public GameInfo gameInfo { get; set; }
            public List<Participant> participants { get; set; }
        }

        public class Preview
        {
            public string matchId { get; set; }
            public long timestampUnix { get; set; }
            public string timestamp { get; set; }
            public int mapId { get; set; }
            public string mode { get; set; }
            public int championId { get; set; }
            public int kills { get; set; }
            public int deaths { get; set; }
            public int assists { get; set; }
            public bool result { get; set; }
        }

        public class GameInfo
        {
            public long gameDurationUnix { get; set; }
            public string gameDuration { get; set; }
            public string gameVersion { get; set; }
            public int myTeamId { get; set; }
            public int allayTeamKills { get; set; }
            public int allayTeamTurrets { get; set; }
            public int allayTeamDragons { get; set; }
            public int allayTeamHeralds { get; set; }
            public int allayTeamBarons { get; set; }
            public int allayTeamGold { get; set; }
            public int enemyTeamKills { get; set; }
            public int enemyTeamTurrets { get; set; }
            public int enemyTeamDragons { get; set; }
            public int enemyTeamHeralds { get; set; }
            public int enemyTeamBarons { get; set; }
            public int enemyTeamGold { get; set; }
            public bool gameEndedInSurrender { get; set; }
        }

        public class Participant
        {
            public int teamId { get; set; }
            public AccountDto accountDto { get; set; }
            public int championId { get; set; }
            public int champLevel { get; set; }
            public int kills { get; set; }
            public int deaths { get; set; }
            public int assists { get; set; }
            public int minions { get; set; }
            public List<int> items { get; set; }
            public int goldEarned { get; set; }
            public int goldSpent { get; set; }
            public Damage damageDealt { get; set; }
            public Damage damageTaken { get; set; }
            public int totalTeamDamage { get; set; }
            public int damageDealtToBuildings { get; set; }
            public int timePlayed { get; set; }
            public int timeSpentAlive { get; set; }
            public int timeCCdealt { get; set; }
            public int timeSpentAliveMax { get; set; }
            public int doubleKills { get; set; }
            public int tripleKills { get; set; }
            public int quadraKills { get; set; }
            public int pentaKills { get; set; }
            public int largestKillingSpree { get; set; }
            public int turretsTakedowns { get; set; }
            public int inhibitorsTakedowns { get; set; }
            public int firstBlood { get; set; }     // 0 if none, 1 if assisted, 2 if killed
            public int firstTurret { get; set; }    // 0 if none, 1 if assisted, 2 if killed
            public int objectivesStolen { get; set; }
            public int largestCrit { get; set; }
            public int totalHeal { get; set; }
            public int totalDamageMitigated { get; set; }
            public int totalHealsOnTeammates { get; set; }
            public int totalDamageShieldedOnTeammates { get; set; }
            public int visionScore { get; set; }
            public int wardsPlaced { get; set; }
            public int spellCastQ { get; set; }
            public int spellCastW { get; set; }
            public int spellCastE { get; set; }
            public int spellCastR { get; set; }
            public int spellCastTotal { get; set; }
            public int spellD { get; set; }
            public int spellF { get; set; }
            public int spellCastD { get; set; }
            public int spellCastF { get; set; }
            public int hasFlashOnF { get; set; }    // 0 if no, 1 if yes

        }
        public class Damage
        {
            public int physical { get; set; }
            public int magic { get; set; }
            public int trueDamage { get; set; }
            public int total { get; set; }
        }

        public class ItemDto
        {
            public int id { get; set; }
            public string name { get; set; }
            public string imageName { get; set; }
        }
    }
}

