using System.Collections.Generic;

namespace HKViz {
    internal static class EnemyInfos {

        public static readonly Dictionary<string, EnemyInfo> enemies = new() {
            ["Dummy"] = new EnemyInfo {
                name = "Dummy",
                shortCode = "1",
                neededForJournal = 0
            },
            ["Crawler"] = new EnemyInfo {
                name = "Crawler",
                shortCode = "2",
                neededForJournal = 0
            },
            ["Buzzer"] = new EnemyInfo {
                name = "Buzzer",
                shortCode = "3",
                neededForJournal = 45
            },
            ["Bouncer"] = new EnemyInfo {
                name = "Bouncer",
                shortCode = "4",
                neededForJournal = 25
            },
            ["Climber"] = new EnemyInfo {
                name = "Climber",
                shortCode = "5",
                neededForJournal = 30
            },
            ["Hopper"] = new EnemyInfo {
                name = "Hopper",
                shortCode = "6",
                neededForJournal = 25
            },
            ["Worm"] = new EnemyInfo {
                name = "Worm",
                shortCode = "7",
                neededForJournal = 10
            },
            ["Spitter"] = new EnemyInfo {
                name = "Spitter",
                shortCode = "8",
                neededForJournal = 20
            },
            ["Hatcher"] = new EnemyInfo {
                name = "Hatcher",
                shortCode = "9",
                neededForJournal = 15
            },
            ["Hatchling"] = new EnemyInfo {
                name = "Hatchling",
                shortCode = "A",
                neededForJournal = 30
            },
            ["ZombieRunner"] = new EnemyInfo {
                name = "ZombieRunner",
                shortCode = "B",
                neededForJournal = 35
            },
            ["ZombieHornhead"] = new EnemyInfo {
                name = "ZombieHornhead",
                shortCode = "C",
                neededForJournal = 35
            },
            ["ZombieLeaper"] = new EnemyInfo {
                name = "ZombieLeaper",
                shortCode = "D",
                neededForJournal = 35
            },
            ["ZombieBarger"] = new EnemyInfo {
                name = "ZombieBarger",
                shortCode = "E",
                neededForJournal = 35
            },
            ["ZombieShield"] = new EnemyInfo {
                name = "ZombieShield",
                shortCode = "F",
                neededForJournal = 10
            },
            ["ZombieGuard"] = new EnemyInfo {
                name = "ZombieGuard",
                shortCode = "G",
                neededForJournal = 6
            },
            ["BigBuzzer"] = new EnemyInfo {
                name = "BigBuzzer",
                shortCode = "H",
                neededForJournal = 2
            },
            ["BigFly"] = new EnemyInfo {
                name = "BigFly",
                shortCode = "I",
                neededForJournal = 3
            },
            ["Mawlek"] = new EnemyInfo {
                name = "Mawlek",
                shortCode = "J",
                neededForJournal = 1
            },
            ["FalseKnight"] = new EnemyInfo {
                name = "FalseKnight",
                shortCode = "K",
                neededForJournal = 1
            },
            ["Roller"] = new EnemyInfo {
                name = "Roller",
                shortCode = "L",
                neededForJournal = 20
            },
            ["Blocker"] = new EnemyInfo {
                name = "Blocker",
                shortCode = "M",
                neededForJournal = 1
            },
            ["PrayerSlug"] = new EnemyInfo {
                name = "PrayerSlug",
                shortCode = "N",
                neededForJournal = 2
            },
            ["MenderBug"] = new EnemyInfo {
                name = "MenderBug",
                shortCode = "O",
                neededForJournal = 1
            },
            ["MossmanRunner"] = new EnemyInfo {
                name = "MossmanRunner",
                shortCode = "P",
                neededForJournal = 25
            },
            ["MossmanShaker"] = new EnemyInfo {
                name = "MossmanShaker",
                shortCode = "Q",
                neededForJournal = 25
            },
            ["Mosquito"] = new EnemyInfo {
                name = "Mosquito",
                shortCode = "R",
                neededForJournal = 25
            },
            ["BlobFlyer"] = new EnemyInfo {
                name = "BlobFlyer",
                shortCode = "S",
                neededForJournal = 20
            },
            ["FungifiedZombie"] = new EnemyInfo {
                name = "FungifiedZombie",
                shortCode = "T",
                neededForJournal = 10
            },
            ["PlantShooter"] = new EnemyInfo {
                name = "PlantShooter",
                shortCode = "U",
                neededForJournal = 15
            },
            ["MossCharger"] = new EnemyInfo {
                name = "MossCharger",
                shortCode = "V",
                neededForJournal = 15
            },
            ["MegaMossCharger"] = new EnemyInfo {
                name = "MegaMossCharger",
                shortCode = "W",
                neededForJournal = 1
            },
            ["SnapperTrap"] = new EnemyInfo {
                name = "SnapperTrap",
                shortCode = "X",
                neededForJournal = 15
            },
            ["MossKnight"] = new EnemyInfo {
                name = "MossKnight",
                shortCode = "Y",
                neededForJournal = 8
            },
            ["GrassHopper"] = new EnemyInfo {
                name = "GrassHopper",
                shortCode = "Z",
                neededForJournal = 15
            },
            ["AcidFlyer"] = new EnemyInfo {
                name = "AcidFlyer",
                shortCode = "10",
                neededForJournal = 8
            },
            ["AcidWalker"] = new EnemyInfo {
                name = "AcidWalker",
                shortCode = "11",
                neededForJournal = 8
            },
            ["MossFlyer"] = new EnemyInfo {
                name = "MossFlyer",
                shortCode = "12",
                neededForJournal = 25
            },
            ["MossKnightFat"] = new EnemyInfo {
                name = "MossKnightFat",
                shortCode = "13",
                neededForJournal = 10
            },
            ["MossWalker"] = new EnemyInfo {
                name = "MossWalker",
                shortCode = "14",
                neededForJournal = 30
            },
            ["InfectedKnight"] = new EnemyInfo {
                name = "InfectedKnight",
                shortCode = "15",
                neededForJournal = 1
            },
            ["LazyFlyer"] = new EnemyInfo {
                name = "LazyFlyer",
                shortCode = "16",
                neededForJournal = 1
            },
            ["ZapBug"] = new EnemyInfo {
                name = "ZapBug",
                shortCode = "17",
                neededForJournal = 1
            },
            ["Jellyfish"] = new EnemyInfo {
                name = "Jellyfish",
                shortCode = "18",
                neededForJournal = 12
            },
            ["JellyCrawler"] = new EnemyInfo {
                name = "JellyCrawler",
                shortCode = "19",
                neededForJournal = 20
            },
            ["MegaJellyfish"] = new EnemyInfo {
                name = "MegaJellyfish",
                shortCode = "1A",
                neededForJournal = 1
            },
            ["FungoonBaby"] = new EnemyInfo {
                name = "FungoonBaby",
                shortCode = "1B",
                neededForJournal = 30
            },
            ["MushroomTurret"] = new EnemyInfo {
                name = "MushroomTurret",
                shortCode = "1C",
                neededForJournal = 20
            },
            ["Mantis"] = new EnemyInfo {
                name = "Mantis",
                shortCode = "1D",
                neededForJournal = 25
            },
            ["MushroomRoller"] = new EnemyInfo {
                name = "MushroomRoller",
                shortCode = "1E",
                neededForJournal = 20
            },
            ["MushroomBrawler"] = new EnemyInfo {
                name = "MushroomBrawler",
                shortCode = "1F",
                neededForJournal = 8
            },
            ["MushroomBaby"] = new EnemyInfo {
                name = "MushroomBaby",
                shortCode = "1G",
                neededForJournal = 20
            },
            ["MantisFlyerChild"] = new EnemyInfo {
                name = "MantisFlyerChild",
                shortCode = "1H",
                neededForJournal = 25
            },
            ["FungusFlyer"] = new EnemyInfo {
                name = "FungusFlyer",
                shortCode = "1I",
                neededForJournal = 20
            },
            ["FungCrawler"] = new EnemyInfo {
                name = "FungCrawler",
                shortCode = "1J",
                neededForJournal = 15
            },
            ["MantisLord"] = new EnemyInfo {
                name = "MantisLord",
                shortCode = "1K",
                neededForJournal = 1
            },
            ["BlackKnight"] = new EnemyInfo {
                name = "BlackKnight",
                shortCode = "1L",
                neededForJournal = 10
            },
            ["ElectricMage"] = new EnemyInfo {
                name = "ElectricMage",
                shortCode = "1M",
                neededForJournal = 6
            },
            ["Mage"] = new EnemyInfo {
                name = "Mage",
                shortCode = "1N",
                neededForJournal = 20
            },
            ["MageKnight"] = new EnemyInfo {
                name = "MageKnight",
                shortCode = "1O",
                neededForJournal = 2
            },
            ["RoyalDandy"] = new EnemyInfo {
                name = "RoyalDandy",
                shortCode = "1P",
                neededForJournal = 25
            },
            ["RoyalCoward"] = new EnemyInfo {
                name = "RoyalCoward",
                shortCode = "1Q",
                neededForJournal = 25
            },
            ["RoyalPlumper"] = new EnemyInfo {
                name = "RoyalPlumper",
                shortCode = "1R",
                neededForJournal = 25
            },
            ["FlyingSentrySword"] = new EnemyInfo {
                name = "FlyingSentrySword",
                shortCode = "1S",
                neededForJournal = 30
            },
            ["FlyingSentryJavelin"] = new EnemyInfo {
                name = "FlyingSentryJavelin",
                shortCode = "1T",
                neededForJournal = 25
            },
            ["Sentry"] = new EnemyInfo {
                name = "Sentry",
                shortCode = "1U",
                neededForJournal = 25
            },
            ["SentryFat"] = new EnemyInfo {
                name = "SentryFat",
                shortCode = "1V",
                neededForJournal = 20
            },
            ["MageBlob"] = new EnemyInfo {
                name = "MageBlob",
                shortCode = "1W",
                neededForJournal = 25
            },
            ["GreatShieldZombie"] = new EnemyInfo {
                name = "GreatShieldZombie",
                shortCode = "1X",
                neededForJournal = 10
            },
            ["JarCollector"] = new EnemyInfo {
                name = "JarCollector",
                shortCode = "1Y",
                neededForJournal = 1
            },
            ["MageBalloon"] = new EnemyInfo {
                name = "MageBalloon",
                shortCode = "1Z",
                neededForJournal = 15
            },
            ["MageLord"] = new EnemyInfo {
                name = "MageLord",
                shortCode = "20",
                neededForJournal = 1
            },
            ["GorgeousHusk"] = new EnemyInfo {
                name = "GorgeousHusk",
                shortCode = "21",
                neededForJournal = 1
            },
            ["FlipHopper"] = new EnemyInfo {
                name = "FlipHopper",
                shortCode = "22",
                neededForJournal = 20
            },
            ["Flukeman"] = new EnemyInfo {
                name = "Flukeman",
                shortCode = "23",
                neededForJournal = 20
            },
            ["Inflater"] = new EnemyInfo {
                name = "Inflater",
                shortCode = "24",
                neededForJournal = 20
            },
            ["Flukefly"] = new EnemyInfo {
                name = "Flukefly",
                shortCode = "25",
                neededForJournal = 15
            },
            ["FlukeMother"] = new EnemyInfo {
                name = "FlukeMother",
                shortCode = "26",
                neededForJournal = 1
            },
            ["DungDefender"] = new EnemyInfo {
                name = "DungDefender",
                shortCode = "27",
                neededForJournal = 1
            },
            ["CrystalCrawler"] = new EnemyInfo {
                name = "CrystalCrawler",
                shortCode = "28",
                neededForJournal = 15
            },
            ["CrystalFlyer"] = new EnemyInfo {
                name = "CrystalFlyer",
                shortCode = "29",
                neededForJournal = 20
            },
            ["LaserBug"] = new EnemyInfo {
                name = "LaserBug",
                shortCode = "2A",
                neededForJournal = 15
            },
            ["BeamMiner"] = new EnemyInfo {
                name = "BeamMiner",
                shortCode = "2B",
                neededForJournal = 15
            },
            ["ZombieMiner"] = new EnemyInfo {
                name = "ZombieMiner",
                shortCode = "2C",
                neededForJournal = 20
            },
            ["MegaBeamMiner"] = new EnemyInfo {
                name = "MegaBeamMiner",
                shortCode = "2D",
                neededForJournal = 2
            },
            ["MinesCrawler"] = new EnemyInfo {
                name = "MinesCrawler",
                shortCode = "2E",
                neededForJournal = 15
            },
            ["AngryBuzzer"] = new EnemyInfo {
                name = "AngryBuzzer",
                shortCode = "2F",
                neededForJournal = 15
            },
            ["BurstingBouncer"] = new EnemyInfo {
                name = "BurstingBouncer",
                shortCode = "2G",
                neededForJournal = 15
            },
            ["BurstingZombie"] = new EnemyInfo {
                name = "BurstingZombie",
                shortCode = "2H",
                neededForJournal = 15
            },
            ["SpittingZombie"] = new EnemyInfo {
                name = "SpittingZombie",
                shortCode = "2I",
                neededForJournal = 15
            },
            ["BabyCentipede"] = new EnemyInfo {
                name = "BabyCentipede",
                shortCode = "2J",
                neededForJournal = 35
            },
            ["BigCentipede"] = new EnemyInfo {
                name = "BigCentipede",
                shortCode = "2K",
                neededForJournal = 10
            },
            ["CentipedeHatcher"] = new EnemyInfo {
                name = "CentipedeHatcher",
                shortCode = "2L",
                neededForJournal = 15
            },
            ["LesserMawlek"] = new EnemyInfo {
                name = "LesserMawlek",
                shortCode = "2M",
                neededForJournal = 10
            },
            ["SlashSpider"] = new EnemyInfo {
                name = "SlashSpider",
                shortCode = "2N",
                neededForJournal = 15
            },
            ["SpiderCorpse"] = new EnemyInfo {
                name = "SpiderCorpse",
                shortCode = "2O",
                neededForJournal = 15
            },
            ["ShootSpider"] = new EnemyInfo {
                name = "ShootSpider",
                shortCode = "2P",
                neededForJournal = 20
            },
            ["MiniSpider"] = new EnemyInfo {
                name = "MiniSpider",
                shortCode = "2Q",
                neededForJournal = 25
            },
            ["SpiderFlyer"] = new EnemyInfo {
                name = "SpiderFlyer",
                shortCode = "2R",
                neededForJournal = 20
            },
            ["MimicSpider"] = new EnemyInfo {
                name = "MimicSpider",
                shortCode = "2S",
                neededForJournal = 1
            },
            ["BeeHatchling"] = new EnemyInfo {
                name = "BeeHatchling",
                shortCode = "2T",
                neededForJournal = 30
            },
            ["BeeStinger"] = new EnemyInfo {
                name = "BeeStinger",
                shortCode = "2U",
                neededForJournal = 15
            },
            ["BigBee"] = new EnemyInfo {
                name = "BigBee",
                shortCode = "2V",
                neededForJournal = 12
            },
            ["HiveKnight"] = new EnemyInfo {
                name = "HiveKnight",
                shortCode = "2W",
                neededForJournal = 1
            },
            ["BlowFly"] = new EnemyInfo {
                name = "BlowFly",
                shortCode = "2X",
                neededForJournal = 20
            },
            ["CeilingDropper"] = new EnemyInfo {
                name = "CeilingDropper",
                shortCode = "2Y",
                neededForJournal = 15
            },
            ["GiantHopper"] = new EnemyInfo {
                name = "GiantHopper",
                shortCode = "2Z",
                neededForJournal = 10
            },
            ["GrubMimic"] = new EnemyInfo {
                name = "GrubMimic",
                shortCode = "30",
                neededForJournal = 5
            },
            ["MawlekTurret"] = new EnemyInfo {
                name = "MawlekTurret",
                shortCode = "31",
                neededForJournal = 10
            },
            ["OrangeScuttler"] = new EnemyInfo {
                name = "OrangeScuttler",
                shortCode = "32",
                neededForJournal = 20
            },
            ["HealthScuttler"] = new EnemyInfo {
                name = "HealthScuttler",
                shortCode = "33",
                neededForJournal = 10
            },
            ["Pigeon"] = new EnemyInfo {
                name = "Pigeon",
                shortCode = "34",
                neededForJournal = 15
            },
            ["ZombieHive"] = new EnemyInfo {
                name = "ZombieHive",
                shortCode = "35",
                neededForJournal = 10
            },
            ["DreamGuard"] = new EnemyInfo {
                name = "DreamGuard",
                shortCode = "36",
                neededForJournal = 20
            },
            ["Hornet"] = new EnemyInfo {
                name = "Hornet",
                shortCode = "37",
                neededForJournal = 2
            },
            ["AbyssCrawler"] = new EnemyInfo {
                name = "AbyssCrawler",
                shortCode = "38",
                neededForJournal = 20
            },
            ["SuperSpitter"] = new EnemyInfo {
                name = "SuperSpitter",
                shortCode = "39",
                neededForJournal = 25
            },
            ["Sibling"] = new EnemyInfo {
                name = "Sibling",
                shortCode = "3A",
                neededForJournal = 25
            },
            ["PalaceFly"] = new EnemyInfo {
                name = "PalaceFly",
                shortCode = "3B",
                neededForJournal = 10
            },
            ["EggSac"] = new EnemyInfo {
                name = "EggSac",
                shortCode = "3C",
                neededForJournal = 5
            },
            ["Mummy"] = new EnemyInfo {
                name = "Mummy",
                shortCode = "3D",
                neededForJournal = 10
            },
            ["OrangeBalloon"] = new EnemyInfo {
                name = "OrangeBalloon",
                shortCode = "3E",
                neededForJournal = 10
            },
            ["AbyssTendril"] = new EnemyInfo {
                name = "AbyssTendril",
                shortCode = "3F",
                neededForJournal = 10
            },
            ["HeavyMantis"] = new EnemyInfo {
                name = "HeavyMantis",
                shortCode = "3G",
                neededForJournal = 15
            },
            ["TraitorLord"] = new EnemyInfo {
                name = "TraitorLord",
                shortCode = "3H",
                neededForJournal = 1
            },
            ["MantisHeavyFlyer"] = new EnemyInfo {
                name = "MantisHeavyFlyer",
                shortCode = "3I",
                neededForJournal = 16
            },
            ["GardenZombie"] = new EnemyInfo {
                name = "GardenZombie",
                shortCode = "3J",
                neededForJournal = 20
            },
            ["RoyalGuard"] = new EnemyInfo {
                name = "RoyalGuard",
                shortCode = "3K",
                neededForJournal = 2
            },
            ["WhiteRoyal"] = new EnemyInfo {
                name = "WhiteRoyal",
                shortCode = "3L",
                neededForJournal = 10
            },
            ["Oblobble"] = new EnemyInfo {
                name = "Oblobble",
                shortCode = "3M",
                neededForJournal = 3
            },
            ["Zote"] = new EnemyInfo {
                name = "Zote",
                shortCode = "3N",
                neededForJournal = 1
            },
            ["Blobble"] = new EnemyInfo {
                name = "Blobble",
                shortCode = "3O",
                neededForJournal = 15
            },
            ["ColMosquito"] = new EnemyInfo {
                name = "ColMosquito",
                shortCode = "3P",
                neededForJournal = 15
            },
            ["ColRoller"] = new EnemyInfo {
                name = "ColRoller",
                shortCode = "3Q",
                neededForJournal = 20
            },
            ["ColFlyingSentry"] = new EnemyInfo {
                name = "ColFlyingSentry",
                shortCode = "3R",
                neededForJournal = 25
            },
            ["ColMiner"] = new EnemyInfo {
                name = "ColMiner",
                shortCode = "3S",
                neededForJournal = 25
            },
            ["ColShield"] = new EnemyInfo {
                name = "ColShield",
                shortCode = "3T",
                neededForJournal = 25
            },
            ["ColWorm"] = new EnemyInfo {
                name = "ColWorm",
                shortCode = "3U",
                neededForJournal = 20
            },
            ["ColHopper"] = new EnemyInfo {
                name = "ColHopper",
                shortCode = "3V",
                neededForJournal = 15
            },
            ["LobsterLancer"] = new EnemyInfo {
                name = "LobsterLancer",
                shortCode = "3W",
                neededForJournal = 1
            },
            ["GhostAladar"] = new EnemyInfo {
                name = "GhostAladar",
                shortCode = "3X",
                neededForJournal = 1
            },
            ["GhostXero"] = new EnemyInfo {
                name = "GhostXero",
                shortCode = "3Y",
                neededForJournal = 1
            },
            ["GhostHu"] = new EnemyInfo {
                name = "GhostHu",
                shortCode = "3Z",
                neededForJournal = 1
            },
            ["GhostMarmu"] = new EnemyInfo {
                name = "GhostMarmu",
                shortCode = "40",
                neededForJournal = 1
            },
            ["GhostNoEyes"] = new EnemyInfo {
                name = "GhostNoEyes",
                shortCode = "41",
                neededForJournal = 1
            },
            ["GhostMarkoth"] = new EnemyInfo {
                name = "GhostMarkoth",
                shortCode = "42",
                neededForJournal = 1
            },
            ["GhostGalien"] = new EnemyInfo {
                name = "GhostGalien",
                shortCode = "43",
                neededForJournal = 1
            },
            ["WhiteDefender"] = new EnemyInfo {
                name = "WhiteDefender",
                shortCode = "44",
                neededForJournal = 1
            },
            ["GreyPrince"] = new EnemyInfo {
                name = "GreyPrince",
                shortCode = "45",
                neededForJournal = 1
            },
            ["ZotelingBalloon"] = new EnemyInfo {
                name = "ZotelingBalloon",
                shortCode = "46",
                neededForJournal = 1
            },
            ["ZotelingHopper"] = new EnemyInfo {
                name = "ZotelingHopper",
                shortCode = "47",
                neededForJournal = 1
            },
            ["ZotelingBuzzer"] = new EnemyInfo {
                name = "ZotelingBuzzer",
                shortCode = "48",
                neededForJournal = 1
            },
            ["HollowKnight"] = new EnemyInfo {
                name = "HollowKnight",
                shortCode = "49",
                neededForJournal = 1
            },
            ["FinalBoss"] = new EnemyInfo {
                name = "FinalBoss",
                shortCode = "4A",
                neededForJournal = 1
            },
            ["HunterMark"] = new EnemyInfo {
                name = "HunterMark",
                shortCode = "4B",
                neededForJournal = 1
            },
            ["FlameBearerSmall"] = new EnemyInfo {
                name = "FlameBearerSmall",
                shortCode = "4C",
                neededForJournal = 3
            },
            ["FlameBearerMed"] = new EnemyInfo {
                name = "FlameBearerMed",
                shortCode = "4D",
                neededForJournal = 4
            },
            ["FlameBearerLarge"] = new EnemyInfo {
                name = "FlameBearerLarge",
                shortCode = "4E",
                neededForJournal = 5
            },
            ["Grimm"] = new EnemyInfo {
                name = "Grimm",
                shortCode = "4F",
                neededForJournal = 1
            },
            ["NightmareGrimm"] = new EnemyInfo {
                name = "NightmareGrimm",
                shortCode = "4G",
                neededForJournal = 1
            },
            ["BindingSeal"] = new EnemyInfo {
                name = "BindingSeal",
                shortCode = "4H",
                neededForJournal = 1
            },
            ["FatFluke"] = new EnemyInfo {
                name = "FatFluke",
                shortCode = "4I",
                neededForJournal = 8
            },
            ["PaleLurker"] = new EnemyInfo {
                name = "PaleLurker",
                shortCode = "4J",
                neededForJournal = 1
            },
            ["NailBros"] = new EnemyInfo {
                name = "NailBros",
                shortCode = "4K",
                neededForJournal = 1
            },
            ["Paintmaster"] = new EnemyInfo {
                name = "Paintmaster",
                shortCode = "4L",
                neededForJournal = 1
            },
            ["Nailsage"] = new EnemyInfo {
                name = "Nailsage",
                shortCode = "4M",
                neededForJournal = 1
            },
            ["HollowKnightPrime"] = new EnemyInfo {
                name = "HollowKnightPrime",
                shortCode = "4N",
                neededForJournal = 1
            },
            ["GodseekerMask"] = new EnemyInfo {
                name = "GodseekerMask",
                shortCode = "4O",
                neededForJournal = 1
            },
            ["VoidIdol_1"] = new EnemyInfo {
                name = "VoidIdol_1",
                shortCode = "4P",
                neededForJournal = 1
            },
            ["VoidIdol_2"] = new EnemyInfo {
                name = "VoidIdol_2",
                shortCode = "4Q",
                neededForJournal = 1
            },
            ["VoidIdol_3"] = new EnemyInfo {
                name = "VoidIdol_3",
                shortCode = "4R",
                neededForJournal = 1
            },

        };
    }
}