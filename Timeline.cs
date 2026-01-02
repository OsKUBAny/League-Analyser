using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace League_Analyser
{
    public class TimelineData
    {
        private MainWindow mainWindow;
        private Info info;
        private Data data;

        public DataType.MatchLao match;
        public DataType.TimelineDto timelineDto;
        public List<DataType.EventsTimeLineDto> eventsList;

        public event Action EventTotalDamageLoaded;

        public List<TotalDamage> eventTotalDamage;

        public const string typeObjectDestroyed = "BUILDING_KILL";
        public const string typeMonsterKilled = "ELITE_MONSTER_KILL";
        public const string typeChampionKilled = "CHAMPION_KILL";

        public class DamageStats
        {
            public int physical { get; set; } = 0;
            public int magic { get; set; } = 0;
            public int trueDamage { get; set; } = 0;
            public int all { get; set; } = 0;
        }

        public class SpellData
        {
            public string championName { get; set; }
            public string spellType { get; set; }
            public string spellImageName { get; set; }
            public BitmapImage spellImage { get; set; }
            public string spellName { get; set; }
            public string spellDescription { get; set; }
        }

        public class TotalDamage
        {
            //PlayerId -1 refers to total damage dealt to victim, rest is for players.
            public int playerId { get; set; }
            public DamageStats damage { get; set; } = new DamageStats();
        }

        public void LoadResourcesInit(DataType.MatchLao match_)
        {
            mainWindow = (MainWindow)App.Current.MainWindow;
            info = mainWindow.info;
            data = mainWindow.data;

            match = match_;
        }

        public async Task<bool> LoadData()
        {
            info.CreateNewPrompt(Info.Messages.process_timeline_downloading);
            timelineDto = await data.GetMatchTimeline(match.preview.matchId);
            info.CreateNewPrompt(Info.Messages.process_terminateProcess);
            if (timelineDto == null)
            {
                info.CreateNewPrompt(Info.Messages.error_timeline_downloadFailed);
                return false;
            }
            return true;
        }

        public LoadResources.ImagePath_t GetImagePathForEvent(string eventType)
        {
            switch (eventType)
            {
                case typeObjectDestroyed: return LoadResources.ImagePath_t.gC_timeline_structures;
                case typeMonsterKilled: return LoadResources.ImagePath_t.gC_timeline_monsters;
                case typeChampionKilled: return LoadResources.ImagePath_t.gC_timeline_kills;
                default: return LoadResources.ImagePath_t.resources;
            }
        }

        public string GetImageNameForEvent(DataType.EventsTimeLineDto eventData, bool returnObjName)
        {
            string resourceImg = null;
            string objName = null;

            switch (eventData.type)
            {
                case typeObjectDestroyed:
                    {
                        if (eventData.buildingType == "TOWER_BUILDING")
                        {
                            resourceImg = "turret_";
                            objName = Messages.timeline_eventName_turretDestroyed;
                        }
                        else
                        {
                            resourceImg = "inhibitor_";
                            objName = Messages.timeline_eventName_inhibitorDestroyed;
                        }
                        // Reversed icons for myTeamId <==> enemy/allay
                        if (eventData.teamId == match.gameInfo.myTeamId) resourceImg += "enemy.png";
                        else resourceImg += "allay.png";

                        break;
                    }
                case typeMonsterKilled:
                    {
                        if (eventData.monsterType == null)
                        {
                            info.CreateNewPrompt(Info.Messages.error_timeline_eventDataEmpty);
                            return null;
                        }

                        switch ((string)eventData.monsterType)
                        {
                            case "DRAGON":
                                {
                                    if (eventData.monsterSubType == null)
                                    {
                                        info.CreateNewPrompt(Info.Messages.error_timeline_eventDataEmpty);
                                        return null;
                                    }
                                    resourceImg = "dragon_";
                                    switch ((string)eventData.monsterSubType)
                                    {
                                        case "AIR_DRAGON": resourceImg += "cloud.png"; objName = Messages.timeline_eventName_dragonCloudKilled; break;
                                        case "EARTH_DRAGON": resourceImg += "mountain.png"; objName = Messages.timeline_eventName_dragonMountainKilled; break;
                                        case "FIRE_DRAGON": resourceImg += "infernal.png"; objName = Messages.timeline_eventName_dragonInfernalKilled; break;
                                        case "WATER_DRAGON": resourceImg += "ocean.png"; objName = Messages.timeline_eventName_dragonOceanKilled; break;
                                        case "HEXTECH_DRAGON": resourceImg += "hextech.png"; objName = Messages.timeline_eventName_dragonHextechKilled; break;
                                        case "CHEMTECH_DRAGON": resourceImg += "chemtech.png"; objName = Messages.timeline_eventName_dragonChemtechKilled; break;
                                        case "ELDER_DRAGON": resourceImg += "elder.png"; objName = Messages.timeline_eventName_dragonElderKilled; break;
                                    }
                                    break;
                                }
                            case "RIFTHERALD": resourceImg = "herald.png"; objName = Messages.timeline_eventName_heraldKilled; break;
                            case "BARON_NASHOR": resourceImg = "baron.png"; objName = Messages.timeline_eventName_baronKilled; break;
                            case "HORDE": resourceImg = "voidgrub.png"; objName = Messages.timeline_eventName_voidgrubKilled; break;
                            case "ATAKHAN": resourceImg = "atakhan.png"; objName = Messages.timeline_eventName_atakhanKilled; break;
                        }
                        break;
                    }
                case typeChampionKilled:
                    {
                        resourceImg = "kill_";
                        try
                        {
                            if (eventData.killerId == 0) throw new Exception();

                            string playerPuuid = timelineDto.info.participants.Find(p => p.participantId == eventData.killerId).puuid;
                            int playerTeamId = match.participants.Find(p => p.accountDto.puuid == playerPuuid).teamId;

                            if (playerTeamId == match.gameInfo.myTeamId)
                            {
                                resourceImg += "allay.png";
                                objName = Messages.timeline_eventName_enemyKill;
                            }
                            else
                            {
                                resourceImg += "enemy.png";
                                objName = Messages.timeline_eventName_allayKill;
                            }
                        }
                        catch (Exception)
                        {
                            if (eventData.killerId == 0 && eventData.victimId != 0)
                            {
                                objName = Messages.timeline_eventName_suicideKill;

                                string playerPuuid = timelineDto.info.participants.Find(p => p.participantId == eventData.victimId).puuid;
                                int playerTeamId = match.participants.Find(p => p.accountDto.puuid == playerPuuid).teamId;

                                if (playerTeamId != match.gameInfo.myTeamId) resourceImg += "allay.png";
                                else resourceImg += "enemy.png";
                            }
                            else
                            {
                                resourceImg = "allay.png";
                                objName = Messages.timeline_eventName_playerKill;
                            }
                        }
                        break;
                    }
                default: return null;
            }
            if (returnObjName) return objName;
            else return resourceImg;
        }
        public bool GetEventsFromTimeline(List<int> playerIdList, bool k, bool d, bool a)
        {
            eventsList = new List<DataType.EventsTimeLineDto>();

            foreach (var frame in timelineDto.info.frames)
            {
                foreach (var eventData in frame.events.Where(p => p.type == typeObjectDestroyed ||
                                                                 p.type == typeMonsterKilled ||
                                                                 p.type == typeChampionKilled))
                {
                    if (eventData.type == typeChampionKilled)
                    {

                        bool contains = (k && eventData.killerId.HasValue && playerIdList.Contains((int)eventData.killerId)) ||
                                        (d && eventData.victimId.HasValue && playerIdList.Contains((int)eventData.victimId)) ||
                                        (a && eventData.assistingParticipantIds != null &&
                                              eventData.assistingParticipantIds.Any(p => playerIdList.Contains(p)));

                        if (contains == true)
                        {
                            eventsList.Add(eventData);
                        }
                    }
                    else eventsList.Add(eventData);
                }
            }
            if (eventsList.Any(p => p.type == typeMonsterKilled) == false) return false;
            else return true;
        }
        public DamageStats CalculateDamage(List<DataType.DamageDealt> eventsList)
        {
            DamageStats damageStats = new DamageStats();

            foreach (DataType.DamageDealt damageEvent in eventsList)
            {
                damageStats.physical += damageEvent.physicalDamage;
                damageStats.magic += damageEvent.magicDamage;
                damageStats.trueDamage += damageEvent.trueDamage;
                damageStats.all = damageStats.all + damageEvent.physicalDamage + damageEvent.magicDamage + damageEvent.trueDamage;
            }
            return damageStats;
        }
        private string GetSpellImageName(int playerId, int spellType)
        {
            SpellData spellData = new SpellData();

            if (spellType <= 3)
            {
                try
                {
                    string puuid = timelineDto.info.participants.Find(p => p.participantId == playerId).puuid;
                    int championId = match.participants.Find(p => p.accountDto.puuid == puuid).championId;
                    var championData = data.championDataDto.data.FirstOrDefault(p => p.Value.key == championId.ToString());

                    if (spellType == -1) return championData.Value.passive.image.full;

                    List<DataType.Spell> spells = championData.Value.spells;
                    return spells[spellType].image.full;

                }
                catch (Exception)
                {
                    info.CreateNewPrompt(Info.Messages.error_timeline_imageNotFound);
                    return null;
                }
            }
            else if (spellType <= 5)
            {
                try
                {
                    string puuid = timelineDto.info.participants.Find(p => p.participantId == playerId).puuid;
                    int summonerId;
                    if (spellType == 4) summonerId = match.participants.Find(p => p.accountDto.puuid == puuid).spellD;
                    else summonerId = match.participants.Find(p => p.accountDto.puuid == puuid).spellF;

                    var summonerData = data.summonerDto.data.FirstOrDefault(p => p.Value.key == summonerId.ToString());

                    return summonerData.Value.image.full;

                }
                catch (Exception)
                {
                    info.CreateNewPrompt(Info.Messages.error_timeline_imageNotFound);
                    return null;
                }
            }
            else return null;
        }

        public SpellData GetSpellData(int playerId, int spellType)
        {
            //spellType:
            // -3: other
            // -2: aa
            // -1: passive
            // 0-3: QWER
            // 4,5: DF

            SpellData spellData = new SpellData();
            LoadResources.ImagePath_t imgPath;

            if (spellType <= -2) imgPath = LoadResources.ImagePath_t.gC_timeline_misc;
            else if (spellType == -1) imgPath = LoadResources.ImagePath_t.DD_passive;
            else imgPath = LoadResources.ImagePath_t.DD_spell;


            switch (spellType)
            {
                case -3: { spellData.spellType = null; spellData.spellImageName = "other.png"; 
                        spellData.spellName = Messages.timeline_attack_other; spellData.spellDescription = null; break; }

                case -2: { spellData.spellType = null; spellData.spellImageName = "autoattack.png"; 
                        spellData.spellName = Messages.timeline_attack_autoattack; spellData.spellDescription = null; break; }

                case -1: { spellData.spellType = "P"; spellData.spellImageName = GetSpellImageName(playerId, spellType); break; }
                case 0: { spellData.spellType = "Q"; spellData.spellImageName = GetSpellImageName(playerId, spellType); break; }
                case 1: { spellData.spellType = "W"; spellData.spellImageName = GetSpellImageName(playerId, spellType); break; }
                case 2: { spellData.spellType = "E"; spellData.spellImageName = GetSpellImageName(playerId, spellType); break; }
                case 3: { spellData.spellType = "R"; spellData.spellImageName = GetSpellImageName(playerId, spellType); break; }
                case 4: { spellData.spellType = "D"; spellData.spellImageName = GetSpellImageName(playerId, spellType); break; }
                case 5: { spellData.spellType = "F"; spellData.spellImageName = GetSpellImageName(playerId, spellType); break; }
                default: break;
            }

            if (playerId != 0)
            {
                try
                {
                    string puuid = timelineDto.info.participants.Find(p => p.participantId == playerId).puuid;
                    int championId = match.participants.Find(p => p.accountDto.puuid == puuid).championId;
                    var championData = data.championDataDto.data.FirstOrDefault(p => p.Value.key == championId.ToString());

                    spellData.championName = championData.Value.name;

                    if (spellType == -1)
                    {
                        spellData.spellName = championData.Value.passive.name;
                        spellData.spellDescription = championData.Value.passive.description;
                    }
                    else if (spellType >= 0 && spellType <= 3)
                    {
                        spellData.spellName = championData.Value.spells[spellType].name;
                        spellData.spellDescription = championData.Value.spells[spellType].description;
                    }
                    else if (spellType >= 4)
                    {
                        int summonerId;
                        if (spellType == 4) summonerId = match.participants.Find(p => p.accountDto.puuid == puuid).spellD;
                        else summonerId = match.participants.Find(p => p.accountDto.puuid == puuid).spellF;

                        var summonerData = data.summonerDto.data.FirstOrDefault(p => p.Value.key == summonerId.ToString());

                        spellData.spellName = summonerData.Value.name;
                        spellData.spellDescription = summonerData.Value.description;
                    }

                }
                catch (Exception) { }
            }
            else
            {
                //spellType:
                //10 - other
                //11 - minion
                //12 - tower
                //13 - monster

                switch (spellType)
                {
                    case 10: { spellData.spellType = null; spellData.spellImageName = "other.png"; 
                            spellData.spellName = Messages.timeline_attack_other; spellData.spellDescription = null; break; }

                    case 11: { spellData.spellType = null; spellData.spellImageName = "minion.png"; 
                            spellData.spellName = Messages.timeline_attack_minion; spellData.spellDescription = null; break; }

                    case 12: { spellData.spellType = null; spellData.spellImageName = "tower.png"; 
                            spellData.spellName = Messages.timeline_attack_turret; spellData.spellDescription = null; break; }

                    case 13: { spellData.spellType = null; spellData.spellImageName = "monster.png"; 
                            spellData.spellName = Messages.timeline_attack_monster; spellData.spellDescription = null; break; }
                }

                imgPath = LoadResources.ImagePath_t.gC_timeline_misc;
            }

            spellData.spellImage = LoadResources.LoadImage(spellData.spellImageName, imgPath, true).image;

            return spellData;
        }

        public void GetPlayerTotalDamages(DataType.EventsTimeLineDto eventData)
        {
            List<TotalDamage> playerDamages = new List<TotalDamage>();

            if (IsNotNullOrZero(eventData.victimId) && eventData.victimDamageDealt != null)
            {
                try
                {
                    playerDamages.Add(new TotalDamage()
                    {
                        playerId = (int)eventData.victimId,
                        damage = CalculateDamage(eventData.victimDamageDealt)
                    });
                }
                catch (Exception)
                {
                    playerDamages.Add(new TotalDamage()
                    {
                        playerId = (int)eventData.victimId
                    });
                }
            }

            if (IsNotNullOrZero(eventData.killerId) && eventData.victimDamageReceived != null)
            {
                try
                {
                    playerDamages.Add(new TotalDamage()
                    {
                        playerId = (int)eventData.killerId,
                        damage = CalculateDamage(eventData.victimDamageReceived.Where(p => p.participantid == (int)eventData.killerId).ToList())
                    });
                }
                catch (Exception)
                {
                    playerDamages.Add(new TotalDamage()
                    {
                        playerId = (int)eventData.killerId
                    });
                }

            }

            if (eventData.assistingParticipantIds != null)
            {
                foreach (int player in eventData.assistingParticipantIds)
                {
                    try
                    {
                        playerDamages.Add(new TotalDamage()
                        {
                            playerId = player,
                            damage = CalculateDamage(eventData.victimDamageReceived.Where(p => p.participantid == player).ToList())
                        });
                    }
                    catch (Exception)
                    {
                        playerDamages.Add(new TotalDamage()
                        {
                            playerId = player
                        });
                    }
                }
            }

            if (eventData.victimDamageReceived != null)
            {
                playerDamages.Add(new TotalDamage()
                {
                    playerId = 0, // '0' refers to other damage sources (minions, towers, monsters, etc.).
                    damage = CalculateDamage(eventData.victimDamageReceived.Where(p => p.participantid == 0).ToList())
                });
            }

            if (eventData.victimDamageReceived != null)  //Total damage
            {
                List<int> playerIds = new List<int>();
                if (IsNotNullOrZero(eventData.killerId)) playerIds.Add((int)eventData.killerId);
                if (eventData.assistingParticipantIds != null) playerIds.AddRange(eventData.assistingParticipantIds);
                playerIds.Add(0);

                playerDamages.Add(new TotalDamage()
                {
                    playerId = -1, // '-1' refers to total damage dealt to victim.
                    damage = CalculateDamage(eventData.victimDamageReceived.Where(p => playerIds.Contains(p.participantid)).ToList())
                });
            }

            eventTotalDamage = playerDamages;
            if (eventTotalDamage != null) EventTotalDamageLoaded?.Invoke();
        }
        public bool IsNotNullOrZero(int? val)
        {
            return val.HasValue && val.Value != 0;
        }

        public void ResetTotalDamageEvent()
        {
            EventTotalDamageLoaded -= EventTotalDamageLoaded;
        }
    }
}
