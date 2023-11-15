using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnalyticsRecorder {
    internal class PlayerDataFields {

        public static readonly Dictionary<string, PlayerDataField> fields = new() {
            ["version"] = new PlayerDataField {
                name = "version",
                type = "String",
                shortCode = "1"
            },
            ["awardAllAchievements"] = new PlayerDataField {
                name = "awardAllAchievements",
                type = "Boolean",
                shortCode = "2"
            },
            ["profileID"] = new PlayerDataField {
                name = "profileID",
                type = "Int32",
                shortCode = "3"
            },
            ["playTime"] = new PlayerDataField {
                name = "playTime",
                type = "Single",
                shortCode = "4"
            },
            ["completionPercent"] = new PlayerDataField {
                name = "completionPercent",
                type = "Single",
                shortCode = "5"
            },
            ["openingCreditsPlayed"] = new PlayerDataField {
                name = "openingCreditsPlayed",
                type = "Boolean",
                shortCode = "6"
            },
            ["permadeathMode"] = new PlayerDataField {
                name = "permadeathMode",
                type = "Int32",
                shortCode = "7"
            },
            ["health"] = new PlayerDataField {
                name = "health",
                type = "Int32",
                shortCode = "8"
            },
            ["maxHealth"] = new PlayerDataField {
                name = "maxHealth",
                type = "Int32",
                shortCode = "9"
            },
            ["maxHealthBase"] = new PlayerDataField {
                name = "maxHealthBase",
                type = "Int32",
                shortCode = "A"
            },
            ["healthBlue"] = new PlayerDataField {
                name = "healthBlue",
                type = "Int32",
                shortCode = "B"
            },
            ["joniHealthBlue"] = new PlayerDataField {
                name = "joniHealthBlue",
                type = "Int32",
                shortCode = "C"
            },
            ["damagedBlue"] = new PlayerDataField {
                name = "damagedBlue",
                type = "Boolean",
                shortCode = "D"
            },
            ["heartPieces"] = new PlayerDataField {
                name = "heartPieces",
                type = "Int32",
                shortCode = "E"
            },
            ["heartPieceCollected"] = new PlayerDataField {
                name = "heartPieceCollected",
                type = "Boolean",
                shortCode = "F"
            },
            ["maxHealthCap"] = new PlayerDataField {
                name = "maxHealthCap",
                type = "Int32",
                shortCode = "G"
            },
            ["heartPieceMax"] = new PlayerDataField {
                name = "heartPieceMax",
                type = "Boolean",
                shortCode = "H"
            },
            ["prevHealth"] = new PlayerDataField {
                name = "prevHealth",
                type = "Int32",
                shortCode = "I"
            },
            ["blockerHits"] = new PlayerDataField {
                name = "blockerHits",
                type = "Int32",
                shortCode = "J"
            },
            ["firstGeo"] = new PlayerDataField {
                name = "firstGeo",
                type = "Boolean",
                shortCode = "K"
            },
            ["geo"] = new PlayerDataField {
                name = "geo",
                type = "Int32",
                shortCode = "L"
            },
            ["maxMP"] = new PlayerDataField {
                name = "maxMP",
                type = "Int32",
                shortCode = "M"
            },
            ["MPCharge"] = new PlayerDataField {
                name = "MPCharge",
                type = "Int32",
                shortCode = "N"
            },
            ["MPReserve"] = new PlayerDataField {
                name = "MPReserve",
                type = "Int32",
                shortCode = "O"
            },
            ["MPReserveMax"] = new PlayerDataField {
                name = "MPReserveMax",
                type = "Int32",
                shortCode = "P"
            },
            ["soulLimited"] = new PlayerDataField {
                name = "soulLimited",
                type = "Boolean",
                shortCode = "Q"
            },
            ["vesselFragments"] = new PlayerDataField {
                name = "vesselFragments",
                type = "Int32",
                shortCode = "R"
            },
            ["vesselFragmentCollected"] = new PlayerDataField {
                name = "vesselFragmentCollected",
                type = "Boolean",
                shortCode = "S"
            },
            ["MPReserveCap"] = new PlayerDataField {
                name = "MPReserveCap",
                type = "Int32",
                shortCode = "T"
            },
            ["vesselFragmentMax"] = new PlayerDataField {
                name = "vesselFragmentMax",
                type = "Boolean",
                shortCode = "U"
            },
            ["focusMP_amount"] = new PlayerDataField {
                name = "focusMP_amount",
                type = "Int32",
                shortCode = "V"
            },
            ["atBench"] = new PlayerDataField {
                name = "atBench",
                type = "Boolean",
                shortCode = "W"
            },
            ["respawnScene"] = new PlayerDataField {
                name = "respawnScene",
                type = "String",
                shortCode = "X"
            },
            ["mapZone"] = new PlayerDataField {
                name = "mapZone",
                type = "MapZone",
                shortCode = "Y"
            },
            ["respawnMarkerName"] = new PlayerDataField {
                name = "respawnMarkerName",
                type = "String",
                shortCode = "Z"
            },
            ["respawnType"] = new PlayerDataField {
                name = "respawnType",
                type = "Int32",
                shortCode = "10"
            },
            ["respawnFacingRight"] = new PlayerDataField {
                name = "respawnFacingRight",
                type = "Boolean",
                shortCode = "11"
            },
            ["hazardRespawnLocation"] = new PlayerDataField {
                name = "hazardRespawnLocation",
                type = "Vector3",
                shortCode = "12"
            },
            ["hazardRespawnFacingRight"] = new PlayerDataField {
                name = "hazardRespawnFacingRight",
                type = "Boolean",
                shortCode = "13"
            },
            ["shadeScene"] = new PlayerDataField {
                name = "shadeScene",
                type = "String",
                shortCode = "14"
            },
            ["shadeMapZone"] = new PlayerDataField {
                name = "shadeMapZone",
                type = "String",
                shortCode = "15"
            },
            ["shadePositionX"] = new PlayerDataField {
                name = "shadePositionX",
                type = "Single",
                shortCode = "16"
            },
            ["shadePositionY"] = new PlayerDataField {
                name = "shadePositionY",
                type = "Single",
                shortCode = "17"
            },
            ["shadeHealth"] = new PlayerDataField {
                name = "shadeHealth",
                type = "Int32",
                shortCode = "18"
            },
            ["shadeMP"] = new PlayerDataField {
                name = "shadeMP",
                type = "Int32",
                shortCode = "19"
            },
            ["shadeFireballLevel"] = new PlayerDataField {
                name = "shadeFireballLevel",
                type = "Int32",
                shortCode = "1A"
            },
            ["shadeQuakeLevel"] = new PlayerDataField {
                name = "shadeQuakeLevel",
                type = "Int32",
                shortCode = "1B"
            },
            ["shadeScreamLevel"] = new PlayerDataField {
                name = "shadeScreamLevel",
                type = "Int32",
                shortCode = "1C"
            },
            ["shadeSpecialType"] = new PlayerDataField {
                name = "shadeSpecialType",
                type = "Int32",
                shortCode = "1D"
            },
            ["shadeMapPos"] = new PlayerDataField {
                name = "shadeMapPos",
                type = "Vector3",
                shortCode = "1E"
            },
            ["dreamgateMapPos"] = new PlayerDataField {
                name = "dreamgateMapPos",
                type = "Vector3",
                shortCode = "1F"
            },
            ["geoPool"] = new PlayerDataField {
                name = "geoPool",
                type = "Int32",
                shortCode = "1G"
            },
            ["nailDamage"] = new PlayerDataField {
                name = "nailDamage",
                type = "Int32",
                shortCode = "1H"
            },
            ["nailRange"] = new PlayerDataField {
                name = "nailRange",
                type = "Int32",
                shortCode = "1I"
            },
            ["beamDamage"] = new PlayerDataField {
                name = "beamDamage",
                type = "Int32",
                shortCode = "1J"
            },
            ["canDash"] = new PlayerDataField {
                name = "canDash",
                type = "Boolean",
                shortCode = "1K"
            },
            ["canBackDash"] = new PlayerDataField {
                name = "canBackDash",
                type = "Boolean",
                shortCode = "1L"
            },
            ["canWallJump"] = new PlayerDataField {
                name = "canWallJump",
                type = "Boolean",
                shortCode = "1M"
            },
            ["canSuperDash"] = new PlayerDataField {
                name = "canSuperDash",
                type = "Boolean",
                shortCode = "1N"
            },
            ["canShadowDash"] = new PlayerDataField {
                name = "canShadowDash",
                type = "Boolean",
                shortCode = "1O"
            },
            ["hasSpell"] = new PlayerDataField {
                name = "hasSpell",
                type = "Boolean",
                shortCode = "1P"
            },
            ["fireballLevel"] = new PlayerDataField {
                name = "fireballLevel",
                type = "Int32",
                shortCode = "1Q"
            },
            ["quakeLevel"] = new PlayerDataField {
                name = "quakeLevel",
                type = "Int32",
                shortCode = "1R"
            },
            ["screamLevel"] = new PlayerDataField {
                name = "screamLevel",
                type = "Int32",
                shortCode = "1S"
            },
            ["hasNailArt"] = new PlayerDataField {
                name = "hasNailArt",
                type = "Boolean",
                shortCode = "1T"
            },
            ["hasCyclone"] = new PlayerDataField {
                name = "hasCyclone",
                type = "Boolean",
                shortCode = "1U"
            },
            ["hasDashSlash"] = new PlayerDataField {
                name = "hasDashSlash",
                type = "Boolean",
                shortCode = "1V"
            },
            ["hasUpwardSlash"] = new PlayerDataField {
                name = "hasUpwardSlash",
                type = "Boolean",
                shortCode = "1W"
            },
            ["hasAllNailArts"] = new PlayerDataField {
                name = "hasAllNailArts",
                type = "Boolean",
                shortCode = "1X"
            },
            ["hasDreamNail"] = new PlayerDataField {
                name = "hasDreamNail",
                type = "Boolean",
                shortCode = "1Y"
            },
            ["hasDreamGate"] = new PlayerDataField {
                name = "hasDreamGate",
                type = "Boolean",
                shortCode = "1Z"
            },
            ["dreamNailUpgraded"] = new PlayerDataField {
                name = "dreamNailUpgraded",
                type = "Boolean",
                shortCode = "20"
            },
            ["dreamOrbs"] = new PlayerDataField {
                name = "dreamOrbs",
                type = "Int32",
                shortCode = "21"
            },
            ["dreamOrbsSpent"] = new PlayerDataField {
                name = "dreamOrbsSpent",
                type = "Int32",
                shortCode = "22"
            },
            ["dreamGateScene"] = new PlayerDataField {
                name = "dreamGateScene",
                type = "String",
                shortCode = "23"
            },
            ["dreamGateX"] = new PlayerDataField {
                name = "dreamGateX",
                type = "Single",
                shortCode = "24"
            },
            ["dreamGateY"] = new PlayerDataField {
                name = "dreamGateY",
                type = "Single",
                shortCode = "25"
            },
            ["hasDash"] = new PlayerDataField {
                name = "hasDash",
                type = "Boolean",
                shortCode = "26"
            },
            ["hasWalljump"] = new PlayerDataField {
                name = "hasWalljump",
                type = "Boolean",
                shortCode = "27"
            },
            ["hasSuperDash"] = new PlayerDataField {
                name = "hasSuperDash",
                type = "Boolean",
                shortCode = "28"
            },
            ["hasShadowDash"] = new PlayerDataField {
                name = "hasShadowDash",
                type = "Boolean",
                shortCode = "29"
            },
            ["hasAcidArmour"] = new PlayerDataField {
                name = "hasAcidArmour",
                type = "Boolean",
                shortCode = "2A"
            },
            ["hasDoubleJump"] = new PlayerDataField {
                name = "hasDoubleJump",
                type = "Boolean",
                shortCode = "2B"
            },
            ["hasLantern"] = new PlayerDataField {
                name = "hasLantern",
                type = "Boolean",
                shortCode = "2C"
            },
            ["hasTramPass"] = new PlayerDataField {
                name = "hasTramPass",
                type = "Boolean",
                shortCode = "2D"
            },
            ["hasQuill"] = new PlayerDataField {
                name = "hasQuill",
                type = "Boolean",
                shortCode = "2E"
            },
            ["hasCityKey"] = new PlayerDataField {
                name = "hasCityKey",
                type = "Boolean",
                shortCode = "2F"
            },
            ["hasSlykey"] = new PlayerDataField {
                name = "hasSlykey",
                type = "Boolean",
                shortCode = "2G"
            },
            ["gaveSlykey"] = new PlayerDataField {
                name = "gaveSlykey",
                type = "Boolean",
                shortCode = "2H"
            },
            ["hasWhiteKey"] = new PlayerDataField {
                name = "hasWhiteKey",
                type = "Boolean",
                shortCode = "2I"
            },
            ["usedWhiteKey"] = new PlayerDataField {
                name = "usedWhiteKey",
                type = "Boolean",
                shortCode = "2J"
            },
            ["hasMenderKey"] = new PlayerDataField {
                name = "hasMenderKey",
                type = "Boolean",
                shortCode = "2K"
            },
            ["hasWaterwaysKey"] = new PlayerDataField {
                name = "hasWaterwaysKey",
                type = "Boolean",
                shortCode = "2L"
            },
            ["hasSpaKey"] = new PlayerDataField {
                name = "hasSpaKey",
                type = "Boolean",
                shortCode = "2M"
            },
            ["hasLoveKey"] = new PlayerDataField {
                name = "hasLoveKey",
                type = "Boolean",
                shortCode = "2N"
            },
            ["hasKingsBrand"] = new PlayerDataField {
                name = "hasKingsBrand",
                type = "Boolean",
                shortCode = "2O"
            },
            ["hasXunFlower"] = new PlayerDataField {
                name = "hasXunFlower",
                type = "Boolean",
                shortCode = "2P"
            },
            ["ghostCoins"] = new PlayerDataField {
                name = "ghostCoins",
                type = "Int32",
                shortCode = "2Q"
            },
            ["ore"] = new PlayerDataField {
                name = "ore",
                type = "Int32",
                shortCode = "2R"
            },
            ["foundGhostCoin"] = new PlayerDataField {
                name = "foundGhostCoin",
                type = "Boolean",
                shortCode = "2S"
            },
            ["trinket1"] = new PlayerDataField {
                name = "trinket1",
                type = "Int32",
                shortCode = "2T"
            },
            ["foundTrinket1"] = new PlayerDataField {
                name = "foundTrinket1",
                type = "Boolean",
                shortCode = "2U"
            },
            ["trinket2"] = new PlayerDataField {
                name = "trinket2",
                type = "Int32",
                shortCode = "2V"
            },
            ["foundTrinket2"] = new PlayerDataField {
                name = "foundTrinket2",
                type = "Boolean",
                shortCode = "2W"
            },
            ["trinket3"] = new PlayerDataField {
                name = "trinket3",
                type = "Int32",
                shortCode = "2X"
            },
            ["foundTrinket3"] = new PlayerDataField {
                name = "foundTrinket3",
                type = "Boolean",
                shortCode = "2Y"
            },
            ["trinket4"] = new PlayerDataField {
                name = "trinket4",
                type = "Int32",
                shortCode = "2Z"
            },
            ["foundTrinket4"] = new PlayerDataField {
                name = "foundTrinket4",
                type = "Boolean",
                shortCode = "30"
            },
            ["noTrinket1"] = new PlayerDataField {
                name = "noTrinket1",
                type = "Boolean",
                shortCode = "31"
            },
            ["noTrinket2"] = new PlayerDataField {
                name = "noTrinket2",
                type = "Boolean",
                shortCode = "32"
            },
            ["noTrinket3"] = new PlayerDataField {
                name = "noTrinket3",
                type = "Boolean",
                shortCode = "33"
            },
            ["noTrinket4"] = new PlayerDataField {
                name = "noTrinket4",
                type = "Boolean",
                shortCode = "34"
            },
            ["soldTrinket1"] = new PlayerDataField {
                name = "soldTrinket1",
                type = "Int32",
                shortCode = "35"
            },
            ["soldTrinket2"] = new PlayerDataField {
                name = "soldTrinket2",
                type = "Int32",
                shortCode = "36"
            },
            ["soldTrinket3"] = new PlayerDataField {
                name = "soldTrinket3",
                type = "Int32",
                shortCode = "37"
            },
            ["soldTrinket4"] = new PlayerDataField {
                name = "soldTrinket4",
                type = "Int32",
                shortCode = "38"
            },
            ["simpleKeys"] = new PlayerDataField {
                name = "simpleKeys",
                type = "Int32",
                shortCode = "39"
            },
            ["rancidEggs"] = new PlayerDataField {
                name = "rancidEggs",
                type = "Int32",
                shortCode = "3A"
            },
            ["notchShroomOgres"] = new PlayerDataField {
                name = "notchShroomOgres",
                type = "Boolean",
                shortCode = "3B"
            },
            ["notchFogCanyon"] = new PlayerDataField {
                name = "notchFogCanyon",
                type = "Boolean",
                shortCode = "3C"
            },
            ["gotLurkerKey"] = new PlayerDataField {
                name = "gotLurkerKey",
                type = "Boolean",
                shortCode = "3D"
            },
            ["gMap_doorX"] = new PlayerDataField {
                name = "gMap_doorX",
                type = "Single",
                shortCode = "3E"
            },
            ["gMap_doorY"] = new PlayerDataField {
                name = "gMap_doorY",
                type = "Single",
                shortCode = "3F"
            },
            ["gMap_doorScene"] = new PlayerDataField {
                name = "gMap_doorScene",
                type = "String",
                shortCode = "3G"
            },
            ["gMap_doorMapZone"] = new PlayerDataField {
                name = "gMap_doorMapZone",
                type = "String",
                shortCode = "3H"
            },
            ["gMap_doorOriginOffsetX"] = new PlayerDataField {
                name = "gMap_doorOriginOffsetX",
                type = "Single",
                shortCode = "3I"
            },
            ["gMap_doorOriginOffsetY"] = new PlayerDataField {
                name = "gMap_doorOriginOffsetY",
                type = "Single",
                shortCode = "3J"
            },
            ["gMap_doorSceneWidth"] = new PlayerDataField {
                name = "gMap_doorSceneWidth",
                type = "Single",
                shortCode = "3K"
            },
            ["gMap_doorSceneHeight"] = new PlayerDataField {
                name = "gMap_doorSceneHeight",
                type = "Single",
                shortCode = "3L"
            },
            ["guardiansDefeated"] = new PlayerDataField {
                name = "guardiansDefeated",
                type = "Int32",
                shortCode = "3M"
            },
            ["lurienDefeated"] = new PlayerDataField {
                name = "lurienDefeated",
                type = "Boolean",
                shortCode = "3N"
            },
            ["hegemolDefeated"] = new PlayerDataField {
                name = "hegemolDefeated",
                type = "Boolean",
                shortCode = "3O"
            },
            ["monomonDefeated"] = new PlayerDataField {
                name = "monomonDefeated",
                type = "Boolean",
                shortCode = "3P"
            },
            ["maskBrokenLurien"] = new PlayerDataField {
                name = "maskBrokenLurien",
                type = "Boolean",
                shortCode = "3Q"
            },
            ["maskBrokenHegemol"] = new PlayerDataField {
                name = "maskBrokenHegemol",
                type = "Boolean",
                shortCode = "3R"
            },
            ["maskBrokenMonomon"] = new PlayerDataField {
                name = "maskBrokenMonomon",
                type = "Boolean",
                shortCode = "3S"
            },
            ["maskToBreak"] = new PlayerDataField {
                name = "maskToBreak",
                type = "Int32",
                shortCode = "3T"
            },
            ["elderbug"] = new PlayerDataField {
                name = "elderbug",
                type = "Int32",
                shortCode = "3U"
            },
            ["metElderbug"] = new PlayerDataField {
                name = "metElderbug",
                type = "Boolean",
                shortCode = "3V"
            },
            ["elderbugReintro"] = new PlayerDataField {
                name = "elderbugReintro",
                type = "Boolean",
                shortCode = "3W"
            },
            ["elderbugHistory"] = new PlayerDataField {
                name = "elderbugHistory",
                type = "Int32",
                shortCode = "3X"
            },
            ["elderbugHistory1"] = new PlayerDataField {
                name = "elderbugHistory1",
                type = "Boolean",
                shortCode = "3Y"
            },
            ["elderbugHistory2"] = new PlayerDataField {
                name = "elderbugHistory2",
                type = "Boolean",
                shortCode = "3Z"
            },
            ["elderbugHistory3"] = new PlayerDataField {
                name = "elderbugHistory3",
                type = "Boolean",
                shortCode = "40"
            },
            ["elderbugSpeechSly"] = new PlayerDataField {
                name = "elderbugSpeechSly",
                type = "Boolean",
                shortCode = "41"
            },
            ["elderbugSpeechStation"] = new PlayerDataField {
                name = "elderbugSpeechStation",
                type = "Boolean",
                shortCode = "42"
            },
            ["elderbugSpeechEggTemple"] = new PlayerDataField {
                name = "elderbugSpeechEggTemple",
                type = "Boolean",
                shortCode = "43"
            },
            ["elderbugSpeechMapShop"] = new PlayerDataField {
                name = "elderbugSpeechMapShop",
                type = "Boolean",
                shortCode = "44"
            },
            ["elderbugSpeechBretta"] = new PlayerDataField {
                name = "elderbugSpeechBretta",
                type = "Boolean",
                shortCode = "45"
            },
            ["elderbugSpeechJiji"] = new PlayerDataField {
                name = "elderbugSpeechJiji",
                type = "Boolean",
                shortCode = "46"
            },
            ["elderbugSpeechMinesLift"] = new PlayerDataField {
                name = "elderbugSpeechMinesLift",
                type = "Boolean",
                shortCode = "47"
            },
            ["elderbugSpeechKingsPass"] = new PlayerDataField {
                name = "elderbugSpeechKingsPass",
                type = "Boolean",
                shortCode = "48"
            },
            ["elderbugSpeechInfectedCrossroads"] = new PlayerDataField {
                name = "elderbugSpeechInfectedCrossroads",
                type = "Boolean",
                shortCode = "49"
            },
            ["elderbugSpeechFinalBossDoor"] = new PlayerDataField {
                name = "elderbugSpeechFinalBossDoor",
                type = "Boolean",
                shortCode = "4A"
            },
            ["elderbugRequestedFlower"] = new PlayerDataField {
                name = "elderbugRequestedFlower",
                type = "Boolean",
                shortCode = "4B"
            },
            ["elderbugGaveFlower"] = new PlayerDataField {
                name = "elderbugGaveFlower",
                type = "Boolean",
                shortCode = "4C"
            },
            ["elderbugFirstCall"] = new PlayerDataField {
                name = "elderbugFirstCall",
                type = "Boolean",
                shortCode = "4D"
            },
            ["metQuirrel"] = new PlayerDataField {
                name = "metQuirrel",
                type = "Boolean",
                shortCode = "4E"
            },
            ["quirrelEggTemple"] = new PlayerDataField {
                name = "quirrelEggTemple",
                type = "Int32",
                shortCode = "4F"
            },
            ["quirrelSlugShrine"] = new PlayerDataField {
                name = "quirrelSlugShrine",
                type = "Int32",
                shortCode = "4G"
            },
            ["quirrelRuins"] = new PlayerDataField {
                name = "quirrelRuins",
                type = "Int32",
                shortCode = "4H"
            },
            ["quirrelMines"] = new PlayerDataField {
                name = "quirrelMines",
                type = "Int32",
                shortCode = "4I"
            },
            ["quirrelLeftStation"] = new PlayerDataField {
                name = "quirrelLeftStation",
                type = "Boolean",
                shortCode = "4J"
            },
            ["quirrelLeftEggTemple"] = new PlayerDataField {
                name = "quirrelLeftEggTemple",
                type = "Boolean",
                shortCode = "4K"
            },
            ["quirrelCityEncountered"] = new PlayerDataField {
                name = "quirrelCityEncountered",
                type = "Boolean",
                shortCode = "4L"
            },
            ["quirrelCityLeft"] = new PlayerDataField {
                name = "quirrelCityLeft",
                type = "Boolean",
                shortCode = "4M"
            },
            ["quirrelMinesEncountered"] = new PlayerDataField {
                name = "quirrelMinesEncountered",
                type = "Boolean",
                shortCode = "4N"
            },
            ["quirrelMinesLeft"] = new PlayerDataField {
                name = "quirrelMinesLeft",
                type = "Boolean",
                shortCode = "4O"
            },
            ["quirrelMantisEncountered"] = new PlayerDataField {
                name = "quirrelMantisEncountered",
                type = "Boolean",
                shortCode = "4P"
            },
            ["enteredMantisLordArea"] = new PlayerDataField {
                name = "enteredMantisLordArea",
                type = "Boolean",
                shortCode = "4Q"
            },
            ["visitedDeepnestSpa"] = new PlayerDataField {
                name = "visitedDeepnestSpa",
                type = "Boolean",
                shortCode = "4R"
            },
            ["quirrelSpaReady"] = new PlayerDataField {
                name = "quirrelSpaReady",
                type = "Boolean",
                shortCode = "4S"
            },
            ["quirrelSpaEncountered"] = new PlayerDataField {
                name = "quirrelSpaEncountered",
                type = "Boolean",
                shortCode = "4T"
            },
            ["quirrelArchiveEncountered"] = new PlayerDataField {
                name = "quirrelArchiveEncountered",
                type = "Boolean",
                shortCode = "4U"
            },
            ["quirrelEpilogueCompleted"] = new PlayerDataField {
                name = "quirrelEpilogueCompleted",
                type = "Boolean",
                shortCode = "4V"
            },
            ["metRelicDealer"] = new PlayerDataField {
                name = "metRelicDealer",
                type = "Boolean",
                shortCode = "4W"
            },
            ["metRelicDealerShop"] = new PlayerDataField {
                name = "metRelicDealerShop",
                type = "Boolean",
                shortCode = "4X"
            },
            ["marmOutside"] = new PlayerDataField {
                name = "marmOutside",
                type = "Boolean",
                shortCode = "4Y"
            },
            ["marmOutsideConvo"] = new PlayerDataField {
                name = "marmOutsideConvo",
                type = "Boolean",
                shortCode = "4Z"
            },
            ["marmConvo1"] = new PlayerDataField {
                name = "marmConvo1",
                type = "Boolean",
                shortCode = "50"
            },
            ["marmConvo2"] = new PlayerDataField {
                name = "marmConvo2",
                type = "Boolean",
                shortCode = "51"
            },
            ["marmConvo3"] = new PlayerDataField {
                name = "marmConvo3",
                type = "Boolean",
                shortCode = "52"
            },
            ["marmConvoNailsmith"] = new PlayerDataField {
                name = "marmConvoNailsmith",
                type = "Boolean",
                shortCode = "53"
            },
            ["cornifer"] = new PlayerDataField {
                name = "cornifer",
                type = "Int32",
                shortCode = "54"
            },
            ["metCornifer"] = new PlayerDataField {
                name = "metCornifer",
                type = "Boolean",
                shortCode = "55"
            },
            ["corniferIntroduced"] = new PlayerDataField {
                name = "corniferIntroduced",
                type = "Boolean",
                shortCode = "56"
            },
            ["corniferAtHome"] = new PlayerDataField {
                name = "corniferAtHome",
                type = "Boolean",
                shortCode = "57"
            },
            ["corn_crossroadsEncountered"] = new PlayerDataField {
                name = "corn_crossroadsEncountered",
                type = "Boolean",
                shortCode = "58"
            },
            ["corn_crossroadsLeft"] = new PlayerDataField {
                name = "corn_crossroadsLeft",
                type = "Boolean",
                shortCode = "59"
            },
            ["corn_greenpathEncountered"] = new PlayerDataField {
                name = "corn_greenpathEncountered",
                type = "Boolean",
                shortCode = "5A"
            },
            ["corn_greenpathLeft"] = new PlayerDataField {
                name = "corn_greenpathLeft",
                type = "Boolean",
                shortCode = "5B"
            },
            ["corn_fogCanyonEncountered"] = new PlayerDataField {
                name = "corn_fogCanyonEncountered",
                type = "Boolean",
                shortCode = "5C"
            },
            ["corn_fogCanyonLeft"] = new PlayerDataField {
                name = "corn_fogCanyonLeft",
                type = "Boolean",
                shortCode = "5D"
            },
            ["corn_fungalWastesEncountered"] = new PlayerDataField {
                name = "corn_fungalWastesEncountered",
                type = "Boolean",
                shortCode = "5E"
            },
            ["corn_fungalWastesLeft"] = new PlayerDataField {
                name = "corn_fungalWastesLeft",
                type = "Boolean",
                shortCode = "5F"
            },
            ["corn_cityEncountered"] = new PlayerDataField {
                name = "corn_cityEncountered",
                type = "Boolean",
                shortCode = "5G"
            },
            ["corn_cityLeft"] = new PlayerDataField {
                name = "corn_cityLeft",
                type = "Boolean",
                shortCode = "5H"
            },
            ["corn_waterwaysEncountered"] = new PlayerDataField {
                name = "corn_waterwaysEncountered",
                type = "Boolean",
                shortCode = "5I"
            },
            ["corn_waterwaysLeft"] = new PlayerDataField {
                name = "corn_waterwaysLeft",
                type = "Boolean",
                shortCode = "5J"
            },
            ["corn_minesEncountered"] = new PlayerDataField {
                name = "corn_minesEncountered",
                type = "Boolean",
                shortCode = "5K"
            },
            ["corn_minesLeft"] = new PlayerDataField {
                name = "corn_minesLeft",
                type = "Boolean",
                shortCode = "5L"
            },
            ["corn_cliffsEncountered"] = new PlayerDataField {
                name = "corn_cliffsEncountered",
                type = "Boolean",
                shortCode = "5M"
            },
            ["corn_cliffsLeft"] = new PlayerDataField {
                name = "corn_cliffsLeft",
                type = "Boolean",
                shortCode = "5N"
            },
            ["corn_deepnestEncountered"] = new PlayerDataField {
                name = "corn_deepnestEncountered",
                type = "Boolean",
                shortCode = "5O"
            },
            ["corn_deepnestLeft"] = new PlayerDataField {
                name = "corn_deepnestLeft",
                type = "Boolean",
                shortCode = "5P"
            },
            ["corn_deepnestMet1"] = new PlayerDataField {
                name = "corn_deepnestMet1",
                type = "Boolean",
                shortCode = "5Q"
            },
            ["corn_deepnestMet2"] = new PlayerDataField {
                name = "corn_deepnestMet2",
                type = "Boolean",
                shortCode = "5R"
            },
            ["corn_outskirtsEncountered"] = new PlayerDataField {
                name = "corn_outskirtsEncountered",
                type = "Boolean",
                shortCode = "5S"
            },
            ["corn_outskirtsLeft"] = new PlayerDataField {
                name = "corn_outskirtsLeft",
                type = "Boolean",
                shortCode = "5T"
            },
            ["corn_royalGardensEncountered"] = new PlayerDataField {
                name = "corn_royalGardensEncountered",
                type = "Boolean",
                shortCode = "5U"
            },
            ["corn_royalGardensLeft"] = new PlayerDataField {
                name = "corn_royalGardensLeft",
                type = "Boolean",
                shortCode = "5V"
            },
            ["corn_abyssEncountered"] = new PlayerDataField {
                name = "corn_abyssEncountered",
                type = "Boolean",
                shortCode = "5W"
            },
            ["corn_abyssLeft"] = new PlayerDataField {
                name = "corn_abyssLeft",
                type = "Boolean",
                shortCode = "5X"
            },
            ["metIselda"] = new PlayerDataField {
                name = "metIselda",
                type = "Boolean",
                shortCode = "5Y"
            },
            ["iseldaCorniferHomeConvo"] = new PlayerDataField {
                name = "iseldaCorniferHomeConvo",
                type = "Boolean",
                shortCode = "5Z"
            },
            ["iseldaConvo1"] = new PlayerDataField {
                name = "iseldaConvo1",
                type = "Boolean",
                shortCode = "60"
            },
            ["brettaRescued"] = new PlayerDataField {
                name = "brettaRescued",
                type = "Boolean",
                shortCode = "61"
            },
            ["brettaPosition"] = new PlayerDataField {
                name = "brettaPosition",
                type = "Int32",
                shortCode = "62"
            },
            ["brettaState"] = new PlayerDataField {
                name = "brettaState",
                type = "Int32",
                shortCode = "63"
            },
            ["brettaSeenBench"] = new PlayerDataField {
                name = "brettaSeenBench",
                type = "Boolean",
                shortCode = "64"
            },
            ["brettaSeenBed"] = new PlayerDataField {
                name = "brettaSeenBed",
                type = "Boolean",
                shortCode = "65"
            },
            ["brettaSeenBenchDiary"] = new PlayerDataField {
                name = "brettaSeenBenchDiary",
                type = "Boolean",
                shortCode = "66"
            },
            ["brettaSeenBedDiary"] = new PlayerDataField {
                name = "brettaSeenBedDiary",
                type = "Boolean",
                shortCode = "67"
            },
            ["brettaLeftTown"] = new PlayerDataField {
                name = "brettaLeftTown",
                type = "Boolean",
                shortCode = "68"
            },
            ["slyRescued"] = new PlayerDataField {
                name = "slyRescued",
                type = "Boolean",
                shortCode = "69"
            },
            ["slyBeta"] = new PlayerDataField {
                name = "slyBeta",
                type = "Boolean",
                shortCode = "6A"
            },
            ["metSlyShop"] = new PlayerDataField {
                name = "metSlyShop",
                type = "Boolean",
                shortCode = "6B"
            },
            ["gotSlyCharm"] = new PlayerDataField {
                name = "gotSlyCharm",
                type = "Boolean",
                shortCode = "6C"
            },
            ["slyShellFrag1"] = new PlayerDataField {
                name = "slyShellFrag1",
                type = "Boolean",
                shortCode = "6D"
            },
            ["slyShellFrag2"] = new PlayerDataField {
                name = "slyShellFrag2",
                type = "Boolean",
                shortCode = "6E"
            },
            ["slyShellFrag3"] = new PlayerDataField {
                name = "slyShellFrag3",
                type = "Boolean",
                shortCode = "6F"
            },
            ["slyShellFrag4"] = new PlayerDataField {
                name = "slyShellFrag4",
                type = "Boolean",
                shortCode = "6G"
            },
            ["slyVesselFrag1"] = new PlayerDataField {
                name = "slyVesselFrag1",
                type = "Boolean",
                shortCode = "6H"
            },
            ["slyVesselFrag2"] = new PlayerDataField {
                name = "slyVesselFrag2",
                type = "Boolean",
                shortCode = "6I"
            },
            ["slyVesselFrag3"] = new PlayerDataField {
                name = "slyVesselFrag3",
                type = "Boolean",
                shortCode = "6J"
            },
            ["slyVesselFrag4"] = new PlayerDataField {
                name = "slyVesselFrag4",
                type = "Boolean",
                shortCode = "6K"
            },
            ["slyNotch1"] = new PlayerDataField {
                name = "slyNotch1",
                type = "Boolean",
                shortCode = "6L"
            },
            ["slyNotch2"] = new PlayerDataField {
                name = "slyNotch2",
                type = "Boolean",
                shortCode = "6M"
            },
            ["slySimpleKey"] = new PlayerDataField {
                name = "slySimpleKey",
                type = "Boolean",
                shortCode = "6N"
            },
            ["slyRancidEgg"] = new PlayerDataField {
                name = "slyRancidEgg",
                type = "Boolean",
                shortCode = "6O"
            },
            ["slyConvoNailArt"] = new PlayerDataField {
                name = "slyConvoNailArt",
                type = "Boolean",
                shortCode = "6P"
            },
            ["slyConvoMapper"] = new PlayerDataField {
                name = "slyConvoMapper",
                type = "Boolean",
                shortCode = "6Q"
            },
            ["slyConvoNailHoned"] = new PlayerDataField {
                name = "slyConvoNailHoned",
                type = "Boolean",
                shortCode = "6R"
            },
            ["jijiDoorUnlocked"] = new PlayerDataField {
                name = "jijiDoorUnlocked",
                type = "Boolean",
                shortCode = "6S"
            },
            ["jijiMet"] = new PlayerDataField {
                name = "jijiMet",
                type = "Boolean",
                shortCode = "6T"
            },
            ["jijiShadeOffered"] = new PlayerDataField {
                name = "jijiShadeOffered",
                type = "Boolean",
                shortCode = "6U"
            },
            ["jijiShadeCharmConvo"] = new PlayerDataField {
                name = "jijiShadeCharmConvo",
                type = "Boolean",
                shortCode = "6V"
            },
            ["metJinn"] = new PlayerDataField {
                name = "metJinn",
                type = "Boolean",
                shortCode = "6W"
            },
            ["jinnConvo1"] = new PlayerDataField {
                name = "jinnConvo1",
                type = "Boolean",
                shortCode = "6X"
            },
            ["jinnConvo2"] = new PlayerDataField {
                name = "jinnConvo2",
                type = "Boolean",
                shortCode = "6Y"
            },
            ["jinnConvo3"] = new PlayerDataField {
                name = "jinnConvo3",
                type = "Boolean",
                shortCode = "6Z"
            },
            ["jinnConvoKingBrand"] = new PlayerDataField {
                name = "jinnConvoKingBrand",
                type = "Boolean",
                shortCode = "70"
            },
            ["jinnConvoShadeCharm"] = new PlayerDataField {
                name = "jinnConvoShadeCharm",
                type = "Boolean",
                shortCode = "71"
            },
            ["jinnEggsSold"] = new PlayerDataField {
                name = "jinnEggsSold",
                type = "Int32",
                shortCode = "72"
            },
            ["zote"] = new PlayerDataField {
                name = "zote",
                type = "Int32",
                shortCode = "73"
            },
            ["zoteRescuedBuzzer"] = new PlayerDataField {
                name = "zoteRescuedBuzzer",
                type = "Boolean",
                shortCode = "74"
            },
            ["zoteDead"] = new PlayerDataField {
                name = "zoteDead",
                type = "Boolean",
                shortCode = "75"
            },
            ["zoteDeathPos"] = new PlayerDataField {
                name = "zoteDeathPos",
                type = "Int32",
                shortCode = "76"
            },
            ["zoteSpokenCity"] = new PlayerDataField {
                name = "zoteSpokenCity",
                type = "Boolean",
                shortCode = "77"
            },
            ["zoteLeftCity"] = new PlayerDataField {
                name = "zoteLeftCity",
                type = "Boolean",
                shortCode = "78"
            },
            ["zoteTrappedDeepnest"] = new PlayerDataField {
                name = "zoteTrappedDeepnest",
                type = "Boolean",
                shortCode = "79"
            },
            ["zoteRescuedDeepnest"] = new PlayerDataField {
                name = "zoteRescuedDeepnest",
                type = "Boolean",
                shortCode = "7A"
            },
            ["zoteDefeated"] = new PlayerDataField {
                name = "zoteDefeated",
                type = "Boolean",
                shortCode = "7B"
            },
            ["zoteSpokenColosseum"] = new PlayerDataField {
                name = "zoteSpokenColosseum",
                type = "Boolean",
                shortCode = "7C"
            },
            ["zotePrecept"] = new PlayerDataField {
                name = "zotePrecept",
                type = "Int32",
                shortCode = "7D"
            },
            ["zoteTownConvo"] = new PlayerDataField {
                name = "zoteTownConvo",
                type = "Int32",
                shortCode = "7E"
            },
            ["shaman"] = new PlayerDataField {
                name = "shaman",
                type = "Int32",
                shortCode = "7F"
            },
            ["shamanScreamConvo"] = new PlayerDataField {
                name = "shamanScreamConvo",
                type = "Boolean",
                shortCode = "7G"
            },
            ["shamanQuakeConvo"] = new PlayerDataField {
                name = "shamanQuakeConvo",
                type = "Boolean",
                shortCode = "7H"
            },
            ["shamanFireball2Convo"] = new PlayerDataField {
                name = "shamanFireball2Convo",
                type = "Boolean",
                shortCode = "7I"
            },
            ["shamanScream2Convo"] = new PlayerDataField {
                name = "shamanScream2Convo",
                type = "Boolean",
                shortCode = "7J"
            },
            ["shamanQuake2Convo"] = new PlayerDataField {
                name = "shamanQuake2Convo",
                type = "Boolean",
                shortCode = "7K"
            },
            ["metMiner"] = new PlayerDataField {
                name = "metMiner",
                type = "Boolean",
                shortCode = "7L"
            },
            ["miner"] = new PlayerDataField {
                name = "miner",
                type = "Int32",
                shortCode = "7M"
            },
            ["minerEarly"] = new PlayerDataField {
                name = "minerEarly",
                type = "Int32",
                shortCode = "7N"
            },
            ["hornetGreenpath"] = new PlayerDataField {
                name = "hornetGreenpath",
                type = "Int32",
                shortCode = "7O"
            },
            ["hornetFung"] = new PlayerDataField {
                name = "hornetFung",
                type = "Int32",
                shortCode = "7P"
            },
            ["hornet_f19"] = new PlayerDataField {
                name = "hornet_f19",
                type = "Boolean",
                shortCode = "7Q"
            },
            ["hornetFountainEncounter"] = new PlayerDataField {
                name = "hornetFountainEncounter",
                type = "Boolean",
                shortCode = "7R"
            },
            ["hornetCityBridge_ready"] = new PlayerDataField {
                name = "hornetCityBridge_ready",
                type = "Boolean",
                shortCode = "7S"
            },
            ["hornetCityBridge_completed"] = new PlayerDataField {
                name = "hornetCityBridge_completed",
                type = "Boolean",
                shortCode = "7T"
            },
            ["hornetAbyssEncounter"] = new PlayerDataField {
                name = "hornetAbyssEncounter",
                type = "Boolean",
                shortCode = "7U"
            },
            ["hornetDenEncounter"] = new PlayerDataField {
                name = "hornetDenEncounter",
                type = "Boolean",
                shortCode = "7V"
            },
            ["metMoth"] = new PlayerDataField {
                name = "metMoth",
                type = "Boolean",
                shortCode = "7W"
            },
            ["ignoredMoth"] = new PlayerDataField {
                name = "ignoredMoth",
                type = "Boolean",
                shortCode = "7X"
            },
            ["gladeDoorOpened"] = new PlayerDataField {
                name = "gladeDoorOpened",
                type = "Boolean",
                shortCode = "7Y"
            },
            ["mothDeparted"] = new PlayerDataField {
                name = "mothDeparted",
                type = "Boolean",
                shortCode = "7Z"
            },
            ["completedRGDreamPlant"] = new PlayerDataField {
                name = "completedRGDreamPlant",
                type = "Boolean",
                shortCode = "80"
            },
            ["dreamReward1"] = new PlayerDataField {
                name = "dreamReward1",
                type = "Boolean",
                shortCode = "81"
            },
            ["dreamReward2"] = new PlayerDataField {
                name = "dreamReward2",
                type = "Boolean",
                shortCode = "82"
            },
            ["dreamReward3"] = new PlayerDataField {
                name = "dreamReward3",
                type = "Boolean",
                shortCode = "83"
            },
            ["dreamReward4"] = new PlayerDataField {
                name = "dreamReward4",
                type = "Boolean",
                shortCode = "84"
            },
            ["dreamReward5"] = new PlayerDataField {
                name = "dreamReward5",
                type = "Boolean",
                shortCode = "85"
            },
            ["dreamReward5b"] = new PlayerDataField {
                name = "dreamReward5b",
                type = "Boolean",
                shortCode = "86"
            },
            ["dreamReward6"] = new PlayerDataField {
                name = "dreamReward6",
                type = "Boolean",
                shortCode = "87"
            },
            ["dreamReward7"] = new PlayerDataField {
                name = "dreamReward7",
                type = "Boolean",
                shortCode = "88"
            },
            ["dreamReward8"] = new PlayerDataField {
                name = "dreamReward8",
                type = "Boolean",
                shortCode = "89"
            },
            ["dreamReward9"] = new PlayerDataField {
                name = "dreamReward9",
                type = "Boolean",
                shortCode = "8A"
            },
            ["dreamMothConvo1"] = new PlayerDataField {
                name = "dreamMothConvo1",
                type = "Boolean",
                shortCode = "8B"
            },
            ["bankerAccountPurchased"] = new PlayerDataField {
                name = "bankerAccountPurchased",
                type = "Boolean",
                shortCode = "8C"
            },
            ["metBanker"] = new PlayerDataField {
                name = "metBanker",
                type = "Boolean",
                shortCode = "8D"
            },
            ["bankerBalance"] = new PlayerDataField {
                name = "bankerBalance",
                type = "Int32",
                shortCode = "8E"
            },
            ["bankerDeclined"] = new PlayerDataField {
                name = "bankerDeclined",
                type = "Boolean",
                shortCode = "8F"
            },
            ["bankerTheftCheck"] = new PlayerDataField {
                name = "bankerTheftCheck",
                type = "Boolean",
                shortCode = "8G"
            },
            ["bankerTheft"] = new PlayerDataField {
                name = "bankerTheft",
                type = "Int32",
                shortCode = "8H"
            },
            ["bankerSpaMet"] = new PlayerDataField {
                name = "bankerSpaMet",
                type = "Boolean",
                shortCode = "8I"
            },
            ["metGiraffe"] = new PlayerDataField {
                name = "metGiraffe",
                type = "Boolean",
                shortCode = "8J"
            },
            ["metCharmSlug"] = new PlayerDataField {
                name = "metCharmSlug",
                type = "Boolean",
                shortCode = "8K"
            },
            ["salubraNotch1"] = new PlayerDataField {
                name = "salubraNotch1",
                type = "Boolean",
                shortCode = "8L"
            },
            ["salubraNotch2"] = new PlayerDataField {
                name = "salubraNotch2",
                type = "Boolean",
                shortCode = "8M"
            },
            ["salubraNotch3"] = new PlayerDataField {
                name = "salubraNotch3",
                type = "Boolean",
                shortCode = "8N"
            },
            ["salubraNotch4"] = new PlayerDataField {
                name = "salubraNotch4",
                type = "Boolean",
                shortCode = "8O"
            },
            ["salubraBlessing"] = new PlayerDataField {
                name = "salubraBlessing",
                type = "Boolean",
                shortCode = "8P"
            },
            ["salubraConvoCombo"] = new PlayerDataField {
                name = "salubraConvoCombo",
                type = "Boolean",
                shortCode = "8Q"
            },
            ["salubraConvoOvercharm"] = new PlayerDataField {
                name = "salubraConvoOvercharm",
                type = "Boolean",
                shortCode = "8R"
            },
            ["salubraConvoTruth"] = new PlayerDataField {
                name = "salubraConvoTruth",
                type = "Boolean",
                shortCode = "8S"
            },
            ["cultistTransformed"] = new PlayerDataField {
                name = "cultistTransformed",
                type = "Boolean",
                shortCode = "8T"
            },
            ["metNailsmith"] = new PlayerDataField {
                name = "metNailsmith",
                type = "Boolean",
                shortCode = "8U"
            },
            ["nailSmithUpgrades"] = new PlayerDataField {
                name = "nailSmithUpgrades",
                type = "Int32",
                shortCode = "8V"
            },
            ["honedNail"] = new PlayerDataField {
                name = "honedNail",
                type = "Boolean",
                shortCode = "8W"
            },
            ["nailsmithCliff"] = new PlayerDataField {
                name = "nailsmithCliff",
                type = "Boolean",
                shortCode = "8X"
            },
            ["nailsmithKilled"] = new PlayerDataField {
                name = "nailsmithKilled",
                type = "Boolean",
                shortCode = "8Y"
            },
            ["nailsmithSpared"] = new PlayerDataField {
                name = "nailsmithSpared",
                type = "Boolean",
                shortCode = "8Z"
            },
            ["nailsmithKillSpeech"] = new PlayerDataField {
                name = "nailsmithKillSpeech",
                type = "Boolean",
                shortCode = "90"
            },
            ["nailsmithSheo"] = new PlayerDataField {
                name = "nailsmithSheo",
                type = "Boolean",
                shortCode = "91"
            },
            ["nailsmithConvoArt"] = new PlayerDataField {
                name = "nailsmithConvoArt",
                type = "Boolean",
                shortCode = "92"
            },
            ["metNailmasterMato"] = new PlayerDataField {
                name = "metNailmasterMato",
                type = "Boolean",
                shortCode = "93"
            },
            ["metNailmasterSheo"] = new PlayerDataField {
                name = "metNailmasterSheo",
                type = "Boolean",
                shortCode = "94"
            },
            ["metNailmasterOro"] = new PlayerDataField {
                name = "metNailmasterOro",
                type = "Boolean",
                shortCode = "95"
            },
            ["matoConvoSheo"] = new PlayerDataField {
                name = "matoConvoSheo",
                type = "Boolean",
                shortCode = "96"
            },
            ["matoConvoOro"] = new PlayerDataField {
                name = "matoConvoOro",
                type = "Boolean",
                shortCode = "97"
            },
            ["matoConvoSly"] = new PlayerDataField {
                name = "matoConvoSly",
                type = "Boolean",
                shortCode = "98"
            },
            ["sheoConvoMato"] = new PlayerDataField {
                name = "sheoConvoMato",
                type = "Boolean",
                shortCode = "99"
            },
            ["sheoConvoOro"] = new PlayerDataField {
                name = "sheoConvoOro",
                type = "Boolean",
                shortCode = "9A"
            },
            ["sheoConvoSly"] = new PlayerDataField {
                name = "sheoConvoSly",
                type = "Boolean",
                shortCode = "9B"
            },
            ["sheoConvoNailsmith"] = new PlayerDataField {
                name = "sheoConvoNailsmith",
                type = "Boolean",
                shortCode = "9C"
            },
            ["oroConvoSheo"] = new PlayerDataField {
                name = "oroConvoSheo",
                type = "Boolean",
                shortCode = "9D"
            },
            ["oroConvoMato"] = new PlayerDataField {
                name = "oroConvoMato",
                type = "Boolean",
                shortCode = "9E"
            },
            ["oroConvoSly"] = new PlayerDataField {
                name = "oroConvoSly",
                type = "Boolean",
                shortCode = "9F"
            },
            ["hunterRoared"] = new PlayerDataField {
                name = "hunterRoared",
                type = "Boolean",
                shortCode = "9G"
            },
            ["metHunter"] = new PlayerDataField {
                name = "metHunter",
                type = "Boolean",
                shortCode = "9H"
            },
            ["hunterRewardOffered"] = new PlayerDataField {
                name = "hunterRewardOffered",
                type = "Boolean",
                shortCode = "9I"
            },
            ["huntersMarkOffered"] = new PlayerDataField {
                name = "huntersMarkOffered",
                type = "Boolean",
                shortCode = "9J"
            },
            ["hasHuntersMark"] = new PlayerDataField {
                name = "hasHuntersMark",
                type = "Boolean",
                shortCode = "9K"
            },
            ["metLegEater"] = new PlayerDataField {
                name = "metLegEater",
                type = "Boolean",
                shortCode = "9L"
            },
            ["paidLegEater"] = new PlayerDataField {
                name = "paidLegEater",
                type = "Boolean",
                shortCode = "9M"
            },
            ["refusedLegEater"] = new PlayerDataField {
                name = "refusedLegEater",
                type = "Boolean",
                shortCode = "9N"
            },
            ["legEaterConvo1"] = new PlayerDataField {
                name = "legEaterConvo1",
                type = "Boolean",
                shortCode = "9O"
            },
            ["legEaterConvo2"] = new PlayerDataField {
                name = "legEaterConvo2",
                type = "Boolean",
                shortCode = "9P"
            },
            ["legEaterConvo3"] = new PlayerDataField {
                name = "legEaterConvo3",
                type = "Boolean",
                shortCode = "9Q"
            },
            ["legEaterBrokenConvo"] = new PlayerDataField {
                name = "legEaterBrokenConvo",
                type = "Boolean",
                shortCode = "9R"
            },
            ["legEaterDungConvo"] = new PlayerDataField {
                name = "legEaterDungConvo",
                type = "Boolean",
                shortCode = "9S"
            },
            ["legEaterInfectedCrossroadConvo"] = new PlayerDataField {
                name = "legEaterInfectedCrossroadConvo",
                type = "Boolean",
                shortCode = "9T"
            },
            ["legEaterBoughtConvo"] = new PlayerDataField {
                name = "legEaterBoughtConvo",
                type = "Boolean",
                shortCode = "9U"
            },
            ["legEaterGoldConvo"] = new PlayerDataField {
                name = "legEaterGoldConvo",
                type = "Boolean",
                shortCode = "9V"
            },
            ["legEaterLeft"] = new PlayerDataField {
                name = "legEaterLeft",
                type = "Boolean",
                shortCode = "9W"
            },
            ["tukMet"] = new PlayerDataField {
                name = "tukMet",
                type = "Boolean",
                shortCode = "9X"
            },
            ["tukEggPrice"] = new PlayerDataField {
                name = "tukEggPrice",
                type = "Int32",
                shortCode = "9Y"
            },
            ["tukDungEgg"] = new PlayerDataField {
                name = "tukDungEgg",
                type = "Boolean",
                shortCode = "9Z"
            },
            ["metEmilitia"] = new PlayerDataField {
                name = "metEmilitia",
                type = "Boolean",
                shortCode = "A0"
            },
            ["emilitiaKingsBrandConvo"] = new PlayerDataField {
                name = "emilitiaKingsBrandConvo",
                type = "Boolean",
                shortCode = "A1"
            },
            ["metCloth"] = new PlayerDataField {
                name = "metCloth",
                type = "Boolean",
                shortCode = "A2"
            },
            ["clothEnteredTramRoom"] = new PlayerDataField {
                name = "clothEnteredTramRoom",
                type = "Boolean",
                shortCode = "A3"
            },
            ["savedCloth"] = new PlayerDataField {
                name = "savedCloth",
                type = "Boolean",
                shortCode = "A4"
            },
            ["clothEncounteredQueensGarden"] = new PlayerDataField {
                name = "clothEncounteredQueensGarden",
                type = "Boolean",
                shortCode = "A5"
            },
            ["clothKilled"] = new PlayerDataField {
                name = "clothKilled",
                type = "Boolean",
                shortCode = "A6"
            },
            ["clothInTown"] = new PlayerDataField {
                name = "clothInTown",
                type = "Boolean",
                shortCode = "A7"
            },
            ["clothLeftTown"] = new PlayerDataField {
                name = "clothLeftTown",
                type = "Boolean",
                shortCode = "A8"
            },
            ["clothGhostSpoken"] = new PlayerDataField {
                name = "clothGhostSpoken",
                type = "Boolean",
                shortCode = "A9"
            },
            ["bigCatHitTail"] = new PlayerDataField {
                name = "bigCatHitTail",
                type = "Boolean",
                shortCode = "AA"
            },
            ["bigCatHitTailConvo"] = new PlayerDataField {
                name = "bigCatHitTailConvo",
                type = "Boolean",
                shortCode = "AB"
            },
            ["bigCatMeet"] = new PlayerDataField {
                name = "bigCatMeet",
                type = "Boolean",
                shortCode = "AC"
            },
            ["bigCatTalk1"] = new PlayerDataField {
                name = "bigCatTalk1",
                type = "Boolean",
                shortCode = "AD"
            },
            ["bigCatTalk2"] = new PlayerDataField {
                name = "bigCatTalk2",
                type = "Boolean",
                shortCode = "AE"
            },
            ["bigCatTalk3"] = new PlayerDataField {
                name = "bigCatTalk3",
                type = "Boolean",
                shortCode = "AF"
            },
            ["bigCatKingsBrandConvo"] = new PlayerDataField {
                name = "bigCatKingsBrandConvo",
                type = "Boolean",
                shortCode = "AG"
            },
            ["bigCatShadeConvo"] = new PlayerDataField {
                name = "bigCatShadeConvo",
                type = "Boolean",
                shortCode = "AH"
            },
            ["tisoEncounteredTown"] = new PlayerDataField {
                name = "tisoEncounteredTown",
                type = "Boolean",
                shortCode = "AI"
            },
            ["tisoEncounteredBench"] = new PlayerDataField {
                name = "tisoEncounteredBench",
                type = "Boolean",
                shortCode = "AJ"
            },
            ["tisoEncounteredLake"] = new PlayerDataField {
                name = "tisoEncounteredLake",
                type = "Boolean",
                shortCode = "AK"
            },
            ["tisoEncounteredColosseum"] = new PlayerDataField {
                name = "tisoEncounteredColosseum",
                type = "Boolean",
                shortCode = "AL"
            },
            ["tisoDead"] = new PlayerDataField {
                name = "tisoDead",
                type = "Boolean",
                shortCode = "AM"
            },
            ["tisoShieldConvo"] = new PlayerDataField {
                name = "tisoShieldConvo",
                type = "Boolean",
                shortCode = "AN"
            },
            ["mossCultist"] = new PlayerDataField {
                name = "mossCultist",
                type = "Int32",
                shortCode = "AO"
            },
            ["maskmakerMet"] = new PlayerDataField {
                name = "maskmakerMet",
                type = "Boolean",
                shortCode = "AP"
            },
            ["maskmakerConvo1"] = new PlayerDataField {
                name = "maskmakerConvo1",
                type = "Boolean",
                shortCode = "AQ"
            },
            ["maskmakerConvo2"] = new PlayerDataField {
                name = "maskmakerConvo2",
                type = "Boolean",
                shortCode = "AR"
            },
            ["maskmakerUnmasked1"] = new PlayerDataField {
                name = "maskmakerUnmasked1",
                type = "Boolean",
                shortCode = "AS"
            },
            ["maskmakerUnmasked2"] = new PlayerDataField {
                name = "maskmakerUnmasked2",
                type = "Boolean",
                shortCode = "AT"
            },
            ["maskmakerShadowDash"] = new PlayerDataField {
                name = "maskmakerShadowDash",
                type = "Boolean",
                shortCode = "AU"
            },
            ["maskmakerKingsBrand"] = new PlayerDataField {
                name = "maskmakerKingsBrand",
                type = "Boolean",
                shortCode = "AV"
            },
            ["dungDefenderConvo1"] = new PlayerDataField {
                name = "dungDefenderConvo1",
                type = "Boolean",
                shortCode = "AW"
            },
            ["dungDefenderConvo2"] = new PlayerDataField {
                name = "dungDefenderConvo2",
                type = "Boolean",
                shortCode = "AX"
            },
            ["dungDefenderConvo3"] = new PlayerDataField {
                name = "dungDefenderConvo3",
                type = "Boolean",
                shortCode = "AY"
            },
            ["dungDefenderCharmConvo"] = new PlayerDataField {
                name = "dungDefenderCharmConvo",
                type = "Boolean",
                shortCode = "AZ"
            },
            ["dungDefenderIsmaConvo"] = new PlayerDataField {
                name = "dungDefenderIsmaConvo",
                type = "Boolean",
                shortCode = "B0"
            },
            ["dungDefenderAwoken"] = new PlayerDataField {
                name = "dungDefenderAwoken",
                type = "Boolean",
                shortCode = "B1"
            },
            ["dungDefenderLeft"] = new PlayerDataField {
                name = "dungDefenderLeft",
                type = "Boolean",
                shortCode = "B2"
            },
            ["dungDefenderAwakeConvo"] = new PlayerDataField {
                name = "dungDefenderAwakeConvo",
                type = "Boolean",
                shortCode = "B3"
            },
            ["midwifeMet"] = new PlayerDataField {
                name = "midwifeMet",
                type = "Boolean",
                shortCode = "B4"
            },
            ["midwifeConvo1"] = new PlayerDataField {
                name = "midwifeConvo1",
                type = "Boolean",
                shortCode = "B5"
            },
            ["midwifeConvo2"] = new PlayerDataField {
                name = "midwifeConvo2",
                type = "Boolean",
                shortCode = "B6"
            },
            ["metQueen"] = new PlayerDataField {
                name = "metQueen",
                type = "Boolean",
                shortCode = "B7"
            },
            ["queenTalk1"] = new PlayerDataField {
                name = "queenTalk1",
                type = "Boolean",
                shortCode = "B8"
            },
            ["queenTalk2"] = new PlayerDataField {
                name = "queenTalk2",
                type = "Boolean",
                shortCode = "B9"
            },
            ["queenDung1"] = new PlayerDataField {
                name = "queenDung1",
                type = "Boolean",
                shortCode = "BA"
            },
            ["queenDung2"] = new PlayerDataField {
                name = "queenDung2",
                type = "Boolean",
                shortCode = "BB"
            },
            ["queenHornet"] = new PlayerDataField {
                name = "queenHornet",
                type = "Boolean",
                shortCode = "BC"
            },
            ["queenTalkExtra"] = new PlayerDataField {
                name = "queenTalkExtra",
                type = "Boolean",
                shortCode = "BD"
            },
            ["gotQueenFragment"] = new PlayerDataField {
                name = "gotQueenFragment",
                type = "Boolean",
                shortCode = "BE"
            },
            ["queenConvo_grimm1"] = new PlayerDataField {
                name = "queenConvo_grimm1",
                type = "Boolean",
                shortCode = "BF"
            },
            ["queenConvo_grimm2"] = new PlayerDataField {
                name = "queenConvo_grimm2",
                type = "Boolean",
                shortCode = "BG"
            },
            ["gotKingFragment"] = new PlayerDataField {
                name = "gotKingFragment",
                type = "Boolean",
                shortCode = "BH"
            },
            ["metXun"] = new PlayerDataField {
                name = "metXun",
                type = "Boolean",
                shortCode = "BI"
            },
            ["xunFailedConvo1"] = new PlayerDataField {
                name = "xunFailedConvo1",
                type = "Boolean",
                shortCode = "BJ"
            },
            ["xunFailedConvo2"] = new PlayerDataField {
                name = "xunFailedConvo2",
                type = "Boolean",
                shortCode = "BK"
            },
            ["xunFlowerBroken"] = new PlayerDataField {
                name = "xunFlowerBroken",
                type = "Boolean",
                shortCode = "BL"
            },
            ["xunFlowerBrokeTimes"] = new PlayerDataField {
                name = "xunFlowerBrokeTimes",
                type = "Int32",
                shortCode = "BM"
            },
            ["xunFlowerGiven"] = new PlayerDataField {
                name = "xunFlowerGiven",
                type = "Boolean",
                shortCode = "BN"
            },
            ["xunRewardGiven"] = new PlayerDataField {
                name = "xunRewardGiven",
                type = "Boolean",
                shortCode = "BO"
            },
            ["menderState"] = new PlayerDataField {
                name = "menderState",
                type = "Int32",
                shortCode = "BP"
            },
            ["menderSignBroken"] = new PlayerDataField {
                name = "menderSignBroken",
                type = "Boolean",
                shortCode = "BQ"
            },
            ["allBelieverTabletsDestroyed"] = new PlayerDataField {
                name = "allBelieverTabletsDestroyed",
                type = "Boolean",
                shortCode = "BR"
            },
            ["mrMushroomState"] = new PlayerDataField {
                name = "mrMushroomState",
                type = "Int32",
                shortCode = "BS"
            },
            ["openedMapperShop"] = new PlayerDataField {
                name = "openedMapperShop",
                type = "Boolean",
                shortCode = "BT"
            },
            ["openedSlyShop"] = new PlayerDataField {
                name = "openedSlyShop",
                type = "Boolean",
                shortCode = "BU"
            },
            ["metStag"] = new PlayerDataField {
                name = "metStag",
                type = "Boolean",
                shortCode = "BV"
            },
            ["travelling"] = new PlayerDataField {
                name = "travelling",
                type = "Boolean",
                shortCode = "BW"
            },
            ["stagPosition"] = new PlayerDataField {
                name = "stagPosition",
                type = "Int32",
                shortCode = "BX"
            },
            ["stationsOpened"] = new PlayerDataField {
                name = "stationsOpened",
                type = "Int32",
                shortCode = "BY"
            },
            ["stagConvoTram"] = new PlayerDataField {
                name = "stagConvoTram",
                type = "Boolean",
                shortCode = "BZ"
            },
            ["stagConvoTiso"] = new PlayerDataField {
                name = "stagConvoTiso",
                type = "Boolean",
                shortCode = "C0"
            },
            ["stagRemember1"] = new PlayerDataField {
                name = "stagRemember1",
                type = "Boolean",
                shortCode = "C1"
            },
            ["stagRemember2"] = new PlayerDataField {
                name = "stagRemember2",
                type = "Boolean",
                shortCode = "C2"
            },
            ["stagRemember3"] = new PlayerDataField {
                name = "stagRemember3",
                type = "Boolean",
                shortCode = "C3"
            },
            ["stagEggInspected"] = new PlayerDataField {
                name = "stagEggInspected",
                type = "Boolean",
                shortCode = "C4"
            },
            ["stagHopeConvo"] = new PlayerDataField {
                name = "stagHopeConvo",
                type = "Boolean",
                shortCode = "C5"
            },
            ["nextScene"] = new PlayerDataField {
                name = "nextScene",
                type = "String",
                shortCode = "C6"
            },
            ["littleFoolMet"] = new PlayerDataField {
                name = "littleFoolMet",
                type = "Boolean",
                shortCode = "C7"
            },
            ["ranAway"] = new PlayerDataField {
                name = "ranAway",
                type = "Boolean",
                shortCode = "C8"
            },
            ["seenColosseumTitle"] = new PlayerDataField {
                name = "seenColosseumTitle",
                type = "Boolean",
                shortCode = "C9"
            },
            ["colosseumBronzeOpened"] = new PlayerDataField {
                name = "colosseumBronzeOpened",
                type = "Boolean",
                shortCode = "CA"
            },
            ["colosseumBronzeCompleted"] = new PlayerDataField {
                name = "colosseumBronzeCompleted",
                type = "Boolean",
                shortCode = "CB"
            },
            ["colosseumSilverOpened"] = new PlayerDataField {
                name = "colosseumSilverOpened",
                type = "Boolean",
                shortCode = "CC"
            },
            ["colosseumSilverCompleted"] = new PlayerDataField {
                name = "colosseumSilverCompleted",
                type = "Boolean",
                shortCode = "CD"
            },
            ["colosseumGoldOpened"] = new PlayerDataField {
                name = "colosseumGoldOpened",
                type = "Boolean",
                shortCode = "CE"
            },
            ["colosseumGoldCompleted"] = new PlayerDataField {
                name = "colosseumGoldCompleted",
                type = "Boolean",
                shortCode = "CF"
            },
            ["openedTown"] = new PlayerDataField {
                name = "openedTown",
                type = "Boolean",
                shortCode = "CG"
            },
            ["openedTownBuilding"] = new PlayerDataField {
                name = "openedTownBuilding",
                type = "Boolean",
                shortCode = "CH"
            },
            ["openedCrossroads"] = new PlayerDataField {
                name = "openedCrossroads",
                type = "Boolean",
                shortCode = "CI"
            },
            ["openedGreenpath"] = new PlayerDataField {
                name = "openedGreenpath",
                type = "Boolean",
                shortCode = "CJ"
            },
            ["openedRuins1"] = new PlayerDataField {
                name = "openedRuins1",
                type = "Boolean",
                shortCode = "CK"
            },
            ["openedRuins2"] = new PlayerDataField {
                name = "openedRuins2",
                type = "Boolean",
                shortCode = "CL"
            },
            ["openedFungalWastes"] = new PlayerDataField {
                name = "openedFungalWastes",
                type = "Boolean",
                shortCode = "CM"
            },
            ["openedRoyalGardens"] = new PlayerDataField {
                name = "openedRoyalGardens",
                type = "Boolean",
                shortCode = "CN"
            },
            ["openedRestingGrounds"] = new PlayerDataField {
                name = "openedRestingGrounds",
                type = "Boolean",
                shortCode = "CO"
            },
            ["openedDeepnest"] = new PlayerDataField {
                name = "openedDeepnest",
                type = "Boolean",
                shortCode = "CP"
            },
            ["openedStagNest"] = new PlayerDataField {
                name = "openedStagNest",
                type = "Boolean",
                shortCode = "CQ"
            },
            ["openedHiddenStation"] = new PlayerDataField {
                name = "openedHiddenStation",
                type = "Boolean",
                shortCode = "CR"
            },
            ["dreamReturnScene"] = new PlayerDataField {
                name = "dreamReturnScene",
                type = "String",
                shortCode = "CS"
            },
            ["charmSlots"] = new PlayerDataField {
                name = "charmSlots",
                type = "Int32",
                shortCode = "CT"
            },
            ["charmSlotsFilled"] = new PlayerDataField {
                name = "charmSlotsFilled",
                type = "Int32",
                shortCode = "CU"
            },
            ["hasCharm"] = new PlayerDataField {
                name = "hasCharm",
                type = "Boolean",
                shortCode = "CV"
            },
            ["equippedCharms"] = new PlayerDataField {
                name = "equippedCharms",
                type = "List`1",
                shortCode = "CW"
            },
            ["charmBenchMsg"] = new PlayerDataField {
                name = "charmBenchMsg",
                type = "Boolean",
                shortCode = "CX"
            },
            ["charmsOwned"] = new PlayerDataField {
                name = "charmsOwned",
                type = "Int32",
                shortCode = "CY"
            },
            ["canOvercharm"] = new PlayerDataField {
                name = "canOvercharm",
                type = "Boolean",
                shortCode = "CZ"
            },
            ["overcharmed"] = new PlayerDataField {
                name = "overcharmed",
                type = "Boolean",
                shortCode = "D0"
            },
            ["gotCharm_1"] = new PlayerDataField {
                name = "gotCharm_1",
                type = "Boolean",
                shortCode = "D1"
            },
            ["equippedCharm_1"] = new PlayerDataField {
                name = "equippedCharm_1",
                type = "Boolean",
                shortCode = "D2"
            },
            ["charmCost_1"] = new PlayerDataField {
                name = "charmCost_1",
                type = "Int32",
                shortCode = "D3"
            },
            ["newCharm_1"] = new PlayerDataField {
                name = "newCharm_1",
                type = "Boolean",
                shortCode = "D4"
            },
            ["gotCharm_2"] = new PlayerDataField {
                name = "gotCharm_2",
                type = "Boolean",
                shortCode = "D5"
            },
            ["equippedCharm_2"] = new PlayerDataField {
                name = "equippedCharm_2",
                type = "Boolean",
                shortCode = "D6"
            },
            ["charmCost_2"] = new PlayerDataField {
                name = "charmCost_2",
                type = "Int32",
                shortCode = "D7"
            },
            ["newCharm_2"] = new PlayerDataField {
                name = "newCharm_2",
                type = "Boolean",
                shortCode = "D8"
            },
            ["gotCharm_3"] = new PlayerDataField {
                name = "gotCharm_3",
                type = "Boolean",
                shortCode = "D9"
            },
            ["equippedCharm_3"] = new PlayerDataField {
                name = "equippedCharm_3",
                type = "Boolean",
                shortCode = "DA"
            },
            ["charmCost_3"] = new PlayerDataField {
                name = "charmCost_3",
                type = "Int32",
                shortCode = "DB"
            },
            ["newCharm_3"] = new PlayerDataField {
                name = "newCharm_3",
                type = "Boolean",
                shortCode = "DC"
            },
            ["gotCharm_4"] = new PlayerDataField {
                name = "gotCharm_4",
                type = "Boolean",
                shortCode = "DD"
            },
            ["equippedCharm_4"] = new PlayerDataField {
                name = "equippedCharm_4",
                type = "Boolean",
                shortCode = "DE"
            },
            ["charmCost_4"] = new PlayerDataField {
                name = "charmCost_4",
                type = "Int32",
                shortCode = "DF"
            },
            ["newCharm_4"] = new PlayerDataField {
                name = "newCharm_4",
                type = "Boolean",
                shortCode = "DG"
            },
            ["gotCharm_5"] = new PlayerDataField {
                name = "gotCharm_5",
                type = "Boolean",
                shortCode = "DH"
            },
            ["equippedCharm_5"] = new PlayerDataField {
                name = "equippedCharm_5",
                type = "Boolean",
                shortCode = "DI"
            },
            ["charmCost_5"] = new PlayerDataField {
                name = "charmCost_5",
                type = "Int32",
                shortCode = "DJ"
            },
            ["newCharm_5"] = new PlayerDataField {
                name = "newCharm_5",
                type = "Boolean",
                shortCode = "DK"
            },
            ["gotCharm_6"] = new PlayerDataField {
                name = "gotCharm_6",
                type = "Boolean",
                shortCode = "DL"
            },
            ["equippedCharm_6"] = new PlayerDataField {
                name = "equippedCharm_6",
                type = "Boolean",
                shortCode = "DM"
            },
            ["charmCost_6"] = new PlayerDataField {
                name = "charmCost_6",
                type = "Int32",
                shortCode = "DN"
            },
            ["newCharm_6"] = new PlayerDataField {
                name = "newCharm_6",
                type = "Boolean",
                shortCode = "DO"
            },
            ["gotCharm_7"] = new PlayerDataField {
                name = "gotCharm_7",
                type = "Boolean",
                shortCode = "DP"
            },
            ["equippedCharm_7"] = new PlayerDataField {
                name = "equippedCharm_7",
                type = "Boolean",
                shortCode = "DQ"
            },
            ["charmCost_7"] = new PlayerDataField {
                name = "charmCost_7",
                type = "Int32",
                shortCode = "DR"
            },
            ["newCharm_7"] = new PlayerDataField {
                name = "newCharm_7",
                type = "Boolean",
                shortCode = "DS"
            },
            ["gotCharm_8"] = new PlayerDataField {
                name = "gotCharm_8",
                type = "Boolean",
                shortCode = "DT"
            },
            ["equippedCharm_8"] = new PlayerDataField {
                name = "equippedCharm_8",
                type = "Boolean",
                shortCode = "DU"
            },
            ["charmCost_8"] = new PlayerDataField {
                name = "charmCost_8",
                type = "Int32",
                shortCode = "DV"
            },
            ["newCharm_8"] = new PlayerDataField {
                name = "newCharm_8",
                type = "Boolean",
                shortCode = "DW"
            },
            ["gotCharm_9"] = new PlayerDataField {
                name = "gotCharm_9",
                type = "Boolean",
                shortCode = "DX"
            },
            ["equippedCharm_9"] = new PlayerDataField {
                name = "equippedCharm_9",
                type = "Boolean",
                shortCode = "DY"
            },
            ["charmCost_9"] = new PlayerDataField {
                name = "charmCost_9",
                type = "Int32",
                shortCode = "DZ"
            },
            ["newCharm_9"] = new PlayerDataField {
                name = "newCharm_9",
                type = "Boolean",
                shortCode = "E0"
            },
            ["gotCharm_10"] = new PlayerDataField {
                name = "gotCharm_10",
                type = "Boolean",
                shortCode = "E1"
            },
            ["equippedCharm_10"] = new PlayerDataField {
                name = "equippedCharm_10",
                type = "Boolean",
                shortCode = "E2"
            },
            ["charmCost_10"] = new PlayerDataField {
                name = "charmCost_10",
                type = "Int32",
                shortCode = "E3"
            },
            ["newCharm_10"] = new PlayerDataField {
                name = "newCharm_10",
                type = "Boolean",
                shortCode = "E4"
            },
            ["gotCharm_11"] = new PlayerDataField {
                name = "gotCharm_11",
                type = "Boolean",
                shortCode = "E5"
            },
            ["equippedCharm_11"] = new PlayerDataField {
                name = "equippedCharm_11",
                type = "Boolean",
                shortCode = "E6"
            },
            ["charmCost_11"] = new PlayerDataField {
                name = "charmCost_11",
                type = "Int32",
                shortCode = "E7"
            },
            ["newCharm_11"] = new PlayerDataField {
                name = "newCharm_11",
                type = "Boolean",
                shortCode = "E8"
            },
            ["gotCharm_12"] = new PlayerDataField {
                name = "gotCharm_12",
                type = "Boolean",
                shortCode = "E9"
            },
            ["equippedCharm_12"] = new PlayerDataField {
                name = "equippedCharm_12",
                type = "Boolean",
                shortCode = "EA"
            },
            ["charmCost_12"] = new PlayerDataField {
                name = "charmCost_12",
                type = "Int32",
                shortCode = "EB"
            },
            ["newCharm_12"] = new PlayerDataField {
                name = "newCharm_12",
                type = "Boolean",
                shortCode = "EC"
            },
            ["gotCharm_13"] = new PlayerDataField {
                name = "gotCharm_13",
                type = "Boolean",
                shortCode = "ED"
            },
            ["equippedCharm_13"] = new PlayerDataField {
                name = "equippedCharm_13",
                type = "Boolean",
                shortCode = "EE"
            },
            ["charmCost_13"] = new PlayerDataField {
                name = "charmCost_13",
                type = "Int32",
                shortCode = "EF"
            },
            ["newCharm_13"] = new PlayerDataField {
                name = "newCharm_13",
                type = "Boolean",
                shortCode = "EG"
            },
            ["gotCharm_14"] = new PlayerDataField {
                name = "gotCharm_14",
                type = "Boolean",
                shortCode = "EH"
            },
            ["equippedCharm_14"] = new PlayerDataField {
                name = "equippedCharm_14",
                type = "Boolean",
                shortCode = "EI"
            },
            ["charmCost_14"] = new PlayerDataField {
                name = "charmCost_14",
                type = "Int32",
                shortCode = "EJ"
            },
            ["newCharm_14"] = new PlayerDataField {
                name = "newCharm_14",
                type = "Boolean",
                shortCode = "EK"
            },
            ["gotCharm_15"] = new PlayerDataField {
                name = "gotCharm_15",
                type = "Boolean",
                shortCode = "EL"
            },
            ["equippedCharm_15"] = new PlayerDataField {
                name = "equippedCharm_15",
                type = "Boolean",
                shortCode = "EM"
            },
            ["charmCost_15"] = new PlayerDataField {
                name = "charmCost_15",
                type = "Int32",
                shortCode = "EN"
            },
            ["newCharm_15"] = new PlayerDataField {
                name = "newCharm_15",
                type = "Boolean",
                shortCode = "EO"
            },
            ["gotCharm_16"] = new PlayerDataField {
                name = "gotCharm_16",
                type = "Boolean",
                shortCode = "EP"
            },
            ["equippedCharm_16"] = new PlayerDataField {
                name = "equippedCharm_16",
                type = "Boolean",
                shortCode = "EQ"
            },
            ["charmCost_16"] = new PlayerDataField {
                name = "charmCost_16",
                type = "Int32",
                shortCode = "ER"
            },
            ["newCharm_16"] = new PlayerDataField {
                name = "newCharm_16",
                type = "Boolean",
                shortCode = "ES"
            },
            ["gotCharm_17"] = new PlayerDataField {
                name = "gotCharm_17",
                type = "Boolean",
                shortCode = "ET"
            },
            ["equippedCharm_17"] = new PlayerDataField {
                name = "equippedCharm_17",
                type = "Boolean",
                shortCode = "EU"
            },
            ["charmCost_17"] = new PlayerDataField {
                name = "charmCost_17",
                type = "Int32",
                shortCode = "EV"
            },
            ["newCharm_17"] = new PlayerDataField {
                name = "newCharm_17",
                type = "Boolean",
                shortCode = "EW"
            },
            ["gotCharm_18"] = new PlayerDataField {
                name = "gotCharm_18",
                type = "Boolean",
                shortCode = "EX"
            },
            ["equippedCharm_18"] = new PlayerDataField {
                name = "equippedCharm_18",
                type = "Boolean",
                shortCode = "EY"
            },
            ["charmCost_18"] = new PlayerDataField {
                name = "charmCost_18",
                type = "Int32",
                shortCode = "EZ"
            },
            ["newCharm_18"] = new PlayerDataField {
                name = "newCharm_18",
                type = "Boolean",
                shortCode = "F0"
            },
            ["gotCharm_19"] = new PlayerDataField {
                name = "gotCharm_19",
                type = "Boolean",
                shortCode = "F1"
            },
            ["equippedCharm_19"] = new PlayerDataField {
                name = "equippedCharm_19",
                type = "Boolean",
                shortCode = "F2"
            },
            ["charmCost_19"] = new PlayerDataField {
                name = "charmCost_19",
                type = "Int32",
                shortCode = "F3"
            },
            ["newCharm_19"] = new PlayerDataField {
                name = "newCharm_19",
                type = "Boolean",
                shortCode = "F4"
            },
            ["gotCharm_20"] = new PlayerDataField {
                name = "gotCharm_20",
                type = "Boolean",
                shortCode = "F5"
            },
            ["equippedCharm_20"] = new PlayerDataField {
                name = "equippedCharm_20",
                type = "Boolean",
                shortCode = "F6"
            },
            ["charmCost_20"] = new PlayerDataField {
                name = "charmCost_20",
                type = "Int32",
                shortCode = "F7"
            },
            ["newCharm_20"] = new PlayerDataField {
                name = "newCharm_20",
                type = "Boolean",
                shortCode = "F8"
            },
            ["gotCharm_21"] = new PlayerDataField {
                name = "gotCharm_21",
                type = "Boolean",
                shortCode = "F9"
            },
            ["equippedCharm_21"] = new PlayerDataField {
                name = "equippedCharm_21",
                type = "Boolean",
                shortCode = "FA"
            },
            ["charmCost_21"] = new PlayerDataField {
                name = "charmCost_21",
                type = "Int32",
                shortCode = "FB"
            },
            ["newCharm_21"] = new PlayerDataField {
                name = "newCharm_21",
                type = "Boolean",
                shortCode = "FC"
            },
            ["gotCharm_22"] = new PlayerDataField {
                name = "gotCharm_22",
                type = "Boolean",
                shortCode = "FD"
            },
            ["equippedCharm_22"] = new PlayerDataField {
                name = "equippedCharm_22",
                type = "Boolean",
                shortCode = "FE"
            },
            ["charmCost_22"] = new PlayerDataField {
                name = "charmCost_22",
                type = "Int32",
                shortCode = "FF"
            },
            ["newCharm_22"] = new PlayerDataField {
                name = "newCharm_22",
                type = "Boolean",
                shortCode = "FG"
            },
            ["gotCharm_23"] = new PlayerDataField {
                name = "gotCharm_23",
                type = "Boolean",
                shortCode = "FH"
            },
            ["equippedCharm_23"] = new PlayerDataField {
                name = "equippedCharm_23",
                type = "Boolean",
                shortCode = "FI"
            },
            ["brokenCharm_23"] = new PlayerDataField {
                name = "brokenCharm_23",
                type = "Boolean",
                shortCode = "FJ"
            },
            ["charmCost_23"] = new PlayerDataField {
                name = "charmCost_23",
                type = "Int32",
                shortCode = "FK"
            },
            ["newCharm_23"] = new PlayerDataField {
                name = "newCharm_23",
                type = "Boolean",
                shortCode = "FL"
            },
            ["gotCharm_24"] = new PlayerDataField {
                name = "gotCharm_24",
                type = "Boolean",
                shortCode = "FM"
            },
            ["equippedCharm_24"] = new PlayerDataField {
                name = "equippedCharm_24",
                type = "Boolean",
                shortCode = "FN"
            },
            ["brokenCharm_24"] = new PlayerDataField {
                name = "brokenCharm_24",
                type = "Boolean",
                shortCode = "FO"
            },
            ["charmCost_24"] = new PlayerDataField {
                name = "charmCost_24",
                type = "Int32",
                shortCode = "FP"
            },
            ["newCharm_24"] = new PlayerDataField {
                name = "newCharm_24",
                type = "Boolean",
                shortCode = "FQ"
            },
            ["gotCharm_25"] = new PlayerDataField {
                name = "gotCharm_25",
                type = "Boolean",
                shortCode = "FR"
            },
            ["equippedCharm_25"] = new PlayerDataField {
                name = "equippedCharm_25",
                type = "Boolean",
                shortCode = "FS"
            },
            ["brokenCharm_25"] = new PlayerDataField {
                name = "brokenCharm_25",
                type = "Boolean",
                shortCode = "FT"
            },
            ["charmCost_25"] = new PlayerDataField {
                name = "charmCost_25",
                type = "Int32",
                shortCode = "FU"
            },
            ["newCharm_25"] = new PlayerDataField {
                name = "newCharm_25",
                type = "Boolean",
                shortCode = "FV"
            },
            ["gotCharm_26"] = new PlayerDataField {
                name = "gotCharm_26",
                type = "Boolean",
                shortCode = "FW"
            },
            ["equippedCharm_26"] = new PlayerDataField {
                name = "equippedCharm_26",
                type = "Boolean",
                shortCode = "FX"
            },
            ["charmCost_26"] = new PlayerDataField {
                name = "charmCost_26",
                type = "Int32",
                shortCode = "FY"
            },
            ["newCharm_26"] = new PlayerDataField {
                name = "newCharm_26",
                type = "Boolean",
                shortCode = "FZ"
            },
            ["gotCharm_27"] = new PlayerDataField {
                name = "gotCharm_27",
                type = "Boolean",
                shortCode = "G0"
            },
            ["equippedCharm_27"] = new PlayerDataField {
                name = "equippedCharm_27",
                type = "Boolean",
                shortCode = "G1"
            },
            ["charmCost_27"] = new PlayerDataField {
                name = "charmCost_27",
                type = "Int32",
                shortCode = "G2"
            },
            ["newCharm_27"] = new PlayerDataField {
                name = "newCharm_27",
                type = "Boolean",
                shortCode = "G3"
            },
            ["gotCharm_28"] = new PlayerDataField {
                name = "gotCharm_28",
                type = "Boolean",
                shortCode = "G4"
            },
            ["equippedCharm_28"] = new PlayerDataField {
                name = "equippedCharm_28",
                type = "Boolean",
                shortCode = "G5"
            },
            ["charmCost_28"] = new PlayerDataField {
                name = "charmCost_28",
                type = "Int32",
                shortCode = "G6"
            },
            ["newCharm_28"] = new PlayerDataField {
                name = "newCharm_28",
                type = "Boolean",
                shortCode = "G7"
            },
            ["gotCharm_29"] = new PlayerDataField {
                name = "gotCharm_29",
                type = "Boolean",
                shortCode = "G8"
            },
            ["equippedCharm_29"] = new PlayerDataField {
                name = "equippedCharm_29",
                type = "Boolean",
                shortCode = "G9"
            },
            ["charmCost_29"] = new PlayerDataField {
                name = "charmCost_29",
                type = "Int32",
                shortCode = "GA"
            },
            ["newCharm_29"] = new PlayerDataField {
                name = "newCharm_29",
                type = "Boolean",
                shortCode = "GB"
            },
            ["gotCharm_30"] = new PlayerDataField {
                name = "gotCharm_30",
                type = "Boolean",
                shortCode = "GC"
            },
            ["equippedCharm_30"] = new PlayerDataField {
                name = "equippedCharm_30",
                type = "Boolean",
                shortCode = "GD"
            },
            ["charmCost_30"] = new PlayerDataField {
                name = "charmCost_30",
                type = "Int32",
                shortCode = "GE"
            },
            ["newCharm_30"] = new PlayerDataField {
                name = "newCharm_30",
                type = "Boolean",
                shortCode = "GF"
            },
            ["gotCharm_31"] = new PlayerDataField {
                name = "gotCharm_31",
                type = "Boolean",
                shortCode = "GG"
            },
            ["equippedCharm_31"] = new PlayerDataField {
                name = "equippedCharm_31",
                type = "Boolean",
                shortCode = "GH"
            },
            ["charmCost_31"] = new PlayerDataField {
                name = "charmCost_31",
                type = "Int32",
                shortCode = "GI"
            },
            ["newCharm_31"] = new PlayerDataField {
                name = "newCharm_31",
                type = "Boolean",
                shortCode = "GJ"
            },
            ["gotCharm_32"] = new PlayerDataField {
                name = "gotCharm_32",
                type = "Boolean",
                shortCode = "GK"
            },
            ["equippedCharm_32"] = new PlayerDataField {
                name = "equippedCharm_32",
                type = "Boolean",
                shortCode = "GL"
            },
            ["charmCost_32"] = new PlayerDataField {
                name = "charmCost_32",
                type = "Int32",
                shortCode = "GM"
            },
            ["newCharm_32"] = new PlayerDataField {
                name = "newCharm_32",
                type = "Boolean",
                shortCode = "GN"
            },
            ["gotCharm_33"] = new PlayerDataField {
                name = "gotCharm_33",
                type = "Boolean",
                shortCode = "GO"
            },
            ["equippedCharm_33"] = new PlayerDataField {
                name = "equippedCharm_33",
                type = "Boolean",
                shortCode = "GP"
            },
            ["charmCost_33"] = new PlayerDataField {
                name = "charmCost_33",
                type = "Int32",
                shortCode = "GQ"
            },
            ["newCharm_33"] = new PlayerDataField {
                name = "newCharm_33",
                type = "Boolean",
                shortCode = "GR"
            },
            ["gotCharm_34"] = new PlayerDataField {
                name = "gotCharm_34",
                type = "Boolean",
                shortCode = "GS"
            },
            ["equippedCharm_34"] = new PlayerDataField {
                name = "equippedCharm_34",
                type = "Boolean",
                shortCode = "GT"
            },
            ["charmCost_34"] = new PlayerDataField {
                name = "charmCost_34",
                type = "Int32",
                shortCode = "GU"
            },
            ["newCharm_34"] = new PlayerDataField {
                name = "newCharm_34",
                type = "Boolean",
                shortCode = "GV"
            },
            ["gotCharm_35"] = new PlayerDataField {
                name = "gotCharm_35",
                type = "Boolean",
                shortCode = "GW"
            },
            ["equippedCharm_35"] = new PlayerDataField {
                name = "equippedCharm_35",
                type = "Boolean",
                shortCode = "GX"
            },
            ["charmCost_35"] = new PlayerDataField {
                name = "charmCost_35",
                type = "Int32",
                shortCode = "GY"
            },
            ["newCharm_35"] = new PlayerDataField {
                name = "newCharm_35",
                type = "Boolean",
                shortCode = "GZ"
            },
            ["gotCharm_36"] = new PlayerDataField {
                name = "gotCharm_36",
                type = "Boolean",
                shortCode = "H0"
            },
            ["equippedCharm_36"] = new PlayerDataField {
                name = "equippedCharm_36",
                type = "Boolean",
                shortCode = "H1"
            },
            ["charmCost_36"] = new PlayerDataField {
                name = "charmCost_36",
                type = "Int32",
                shortCode = "H2"
            },
            ["newCharm_36"] = new PlayerDataField {
                name = "newCharm_36",
                type = "Boolean",
                shortCode = "H3"
            },
            ["gotCharm_37"] = new PlayerDataField {
                name = "gotCharm_37",
                type = "Boolean",
                shortCode = "H4"
            },
            ["equippedCharm_37"] = new PlayerDataField {
                name = "equippedCharm_37",
                type = "Boolean",
                shortCode = "H5"
            },
            ["charmCost_37"] = new PlayerDataField {
                name = "charmCost_37",
                type = "Int32",
                shortCode = "H6"
            },
            ["newCharm_37"] = new PlayerDataField {
                name = "newCharm_37",
                type = "Boolean",
                shortCode = "H7"
            },
            ["gotCharm_38"] = new PlayerDataField {
                name = "gotCharm_38",
                type = "Boolean",
                shortCode = "H8"
            },
            ["equippedCharm_38"] = new PlayerDataField {
                name = "equippedCharm_38",
                type = "Boolean",
                shortCode = "H9"
            },
            ["charmCost_38"] = new PlayerDataField {
                name = "charmCost_38",
                type = "Int32",
                shortCode = "HA"
            },
            ["newCharm_38"] = new PlayerDataField {
                name = "newCharm_38",
                type = "Boolean",
                shortCode = "HB"
            },
            ["gotCharm_39"] = new PlayerDataField {
                name = "gotCharm_39",
                type = "Boolean",
                shortCode = "HC"
            },
            ["equippedCharm_39"] = new PlayerDataField {
                name = "equippedCharm_39",
                type = "Boolean",
                shortCode = "HD"
            },
            ["charmCost_39"] = new PlayerDataField {
                name = "charmCost_39",
                type = "Int32",
                shortCode = "HE"
            },
            ["newCharm_39"] = new PlayerDataField {
                name = "newCharm_39",
                type = "Boolean",
                shortCode = "HF"
            },
            ["gotCharm_40"] = new PlayerDataField {
                name = "gotCharm_40",
                type = "Boolean",
                shortCode = "HG"
            },
            ["equippedCharm_40"] = new PlayerDataField {
                name = "equippedCharm_40",
                type = "Boolean",
                shortCode = "HH"
            },
            ["charmCost_40"] = new PlayerDataField {
                name = "charmCost_40",
                type = "Int32",
                shortCode = "HI"
            },
            ["newCharm_40"] = new PlayerDataField {
                name = "newCharm_40",
                type = "Boolean",
                shortCode = "HJ"
            },
            ["fragileHealth_unbreakable"] = new PlayerDataField {
                name = "fragileHealth_unbreakable",
                type = "Boolean",
                shortCode = "HK"
            },
            ["fragileGreed_unbreakable"] = new PlayerDataField {
                name = "fragileGreed_unbreakable",
                type = "Boolean",
                shortCode = "HL"
            },
            ["fragileStrength_unbreakable"] = new PlayerDataField {
                name = "fragileStrength_unbreakable",
                type = "Boolean",
                shortCode = "HM"
            },
            ["royalCharmState"] = new PlayerDataField {
                name = "royalCharmState",
                type = "Int32",
                shortCode = "HN"
            },
            ["hasJournal"] = new PlayerDataField {
                name = "hasJournal",
                type = "Boolean",
                shortCode = "HO"
            },
            ["lastJournalItem"] = new PlayerDataField {
                name = "lastJournalItem",
                type = "Int32",
                shortCode = "HP"
            },
            ["killedDummy"] = new PlayerDataField {
                name = "killedDummy",
                type = "Boolean",
                shortCode = "HQ"
            },
            ["killsDummy"] = new PlayerDataField {
                name = "killsDummy",
                type = "Int32",
                shortCode = "HR"
            },
            ["newDataDummy"] = new PlayerDataField {
                name = "newDataDummy",
                type = "Boolean",
                shortCode = "HS"
            },
            ["seenJournalMsg"] = new PlayerDataField {
                name = "seenJournalMsg",
                type = "Boolean",
                shortCode = "HT"
            },
            ["seenHunterMsg"] = new PlayerDataField {
                name = "seenHunterMsg",
                type = "Boolean",
                shortCode = "HU"
            },
            ["fillJournal"] = new PlayerDataField {
                name = "fillJournal",
                type = "Boolean",
                shortCode = "HV"
            },
            ["journalEntriesCompleted"] = new PlayerDataField {
                name = "journalEntriesCompleted",
                type = "Int32",
                shortCode = "HW"
            },
            ["journalNotesCompleted"] = new PlayerDataField {
                name = "journalNotesCompleted",
                type = "Int32",
                shortCode = "HX"
            },
            ["journalEntriesTotal"] = new PlayerDataField {
                name = "journalEntriesTotal",
                type = "Int32",
                shortCode = "HY"
            },
            ["killedCrawler"] = new PlayerDataField {
                name = "killedCrawler",
                type = "Boolean",
                shortCode = "HZ"
            },
            ["killsCrawler"] = new PlayerDataField {
                name = "killsCrawler",
                type = "Int32",
                shortCode = "I0"
            },
            ["newDataCrawler"] = new PlayerDataField {
                name = "newDataCrawler",
                type = "Boolean",
                shortCode = "I1"
            },
            ["killedBuzzer"] = new PlayerDataField {
                name = "killedBuzzer",
                type = "Boolean",
                shortCode = "I2"
            },
            ["killsBuzzer"] = new PlayerDataField {
                name = "killsBuzzer",
                type = "Int32",
                shortCode = "I3"
            },
            ["newDataBuzzer"] = new PlayerDataField {
                name = "newDataBuzzer",
                type = "Boolean",
                shortCode = "I4"
            },
            ["killedBouncer"] = new PlayerDataField {
                name = "killedBouncer",
                type = "Boolean",
                shortCode = "I5"
            },
            ["killsBouncer"] = new PlayerDataField {
                name = "killsBouncer",
                type = "Int32",
                shortCode = "I6"
            },
            ["newDataBouncer"] = new PlayerDataField {
                name = "newDataBouncer",
                type = "Boolean",
                shortCode = "I7"
            },
            ["killedClimber"] = new PlayerDataField {
                name = "killedClimber",
                type = "Boolean",
                shortCode = "I8"
            },
            ["killsClimber"] = new PlayerDataField {
                name = "killsClimber",
                type = "Int32",
                shortCode = "I9"
            },
            ["newDataClimber"] = new PlayerDataField {
                name = "newDataClimber",
                type = "Boolean",
                shortCode = "IA"
            },
            ["killedHopper"] = new PlayerDataField {
                name = "killedHopper",
                type = "Boolean",
                shortCode = "IB"
            },
            ["killsHopper"] = new PlayerDataField {
                name = "killsHopper",
                type = "Int32",
                shortCode = "IC"
            },
            ["newDataHopper"] = new PlayerDataField {
                name = "newDataHopper",
                type = "Boolean",
                shortCode = "ID"
            },
            ["killedWorm"] = new PlayerDataField {
                name = "killedWorm",
                type = "Boolean",
                shortCode = "IE"
            },
            ["killsWorm"] = new PlayerDataField {
                name = "killsWorm",
                type = "Int32",
                shortCode = "IF"
            },
            ["newDataWorm"] = new PlayerDataField {
                name = "newDataWorm",
                type = "Boolean",
                shortCode = "IG"
            },
            ["killedSpitter"] = new PlayerDataField {
                name = "killedSpitter",
                type = "Boolean",
                shortCode = "IH"
            },
            ["killsSpitter"] = new PlayerDataField {
                name = "killsSpitter",
                type = "Int32",
                shortCode = "II"
            },
            ["newDataSpitter"] = new PlayerDataField {
                name = "newDataSpitter",
                type = "Boolean",
                shortCode = "IJ"
            },
            ["killedHatcher"] = new PlayerDataField {
                name = "killedHatcher",
                type = "Boolean",
                shortCode = "IK"
            },
            ["killsHatcher"] = new PlayerDataField {
                name = "killsHatcher",
                type = "Int32",
                shortCode = "IL"
            },
            ["newDataHatcher"] = new PlayerDataField {
                name = "newDataHatcher",
                type = "Boolean",
                shortCode = "IM"
            },
            ["killedHatchling"] = new PlayerDataField {
                name = "killedHatchling",
                type = "Boolean",
                shortCode = "IN"
            },
            ["killsHatchling"] = new PlayerDataField {
                name = "killsHatchling",
                type = "Int32",
                shortCode = "IO"
            },
            ["newDataHatchling"] = new PlayerDataField {
                name = "newDataHatchling",
                type = "Boolean",
                shortCode = "IP"
            },
            ["killedZombieRunner"] = new PlayerDataField {
                name = "killedZombieRunner",
                type = "Boolean",
                shortCode = "IQ"
            },
            ["killsZombieRunner"] = new PlayerDataField {
                name = "killsZombieRunner",
                type = "Int32",
                shortCode = "IR"
            },
            ["newDataZombieRunner"] = new PlayerDataField {
                name = "newDataZombieRunner",
                type = "Boolean",
                shortCode = "IS"
            },
            ["killedZombieHornhead"] = new PlayerDataField {
                name = "killedZombieHornhead",
                type = "Boolean",
                shortCode = "IT"
            },
            ["killsZombieHornhead"] = new PlayerDataField {
                name = "killsZombieHornhead",
                type = "Int32",
                shortCode = "IU"
            },
            ["newDataZombieHornhead"] = new PlayerDataField {
                name = "newDataZombieHornhead",
                type = "Boolean",
                shortCode = "IV"
            },
            ["killedZombieLeaper"] = new PlayerDataField {
                name = "killedZombieLeaper",
                type = "Boolean",
                shortCode = "IW"
            },
            ["killsZombieLeaper"] = new PlayerDataField {
                name = "killsZombieLeaper",
                type = "Int32",
                shortCode = "IX"
            },
            ["newDataZombieLeaper"] = new PlayerDataField {
                name = "newDataZombieLeaper",
                type = "Boolean",
                shortCode = "IY"
            },
            ["killedZombieBarger"] = new PlayerDataField {
                name = "killedZombieBarger",
                type = "Boolean",
                shortCode = "IZ"
            },
            ["killsZombieBarger"] = new PlayerDataField {
                name = "killsZombieBarger",
                type = "Int32",
                shortCode = "J0"
            },
            ["newDataZombieBarger"] = new PlayerDataField {
                name = "newDataZombieBarger",
                type = "Boolean",
                shortCode = "J1"
            },
            ["killedZombieShield"] = new PlayerDataField {
                name = "killedZombieShield",
                type = "Boolean",
                shortCode = "J2"
            },
            ["killsZombieShield"] = new PlayerDataField {
                name = "killsZombieShield",
                type = "Int32",
                shortCode = "J3"
            },
            ["newDataZombieShield"] = new PlayerDataField {
                name = "newDataZombieShield",
                type = "Boolean",
                shortCode = "J4"
            },
            ["killedZombieGuard"] = new PlayerDataField {
                name = "killedZombieGuard",
                type = "Boolean",
                shortCode = "J5"
            },
            ["killsZombieGuard"] = new PlayerDataField {
                name = "killsZombieGuard",
                type = "Int32",
                shortCode = "J6"
            },
            ["newDataZombieGuard"] = new PlayerDataField {
                name = "newDataZombieGuard",
                type = "Boolean",
                shortCode = "J7"
            },
            ["killedBigBuzzer"] = new PlayerDataField {
                name = "killedBigBuzzer",
                type = "Boolean",
                shortCode = "J8"
            },
            ["killsBigBuzzer"] = new PlayerDataField {
                name = "killsBigBuzzer",
                type = "Int32",
                shortCode = "J9"
            },
            ["newDataBigBuzzer"] = new PlayerDataField {
                name = "newDataBigBuzzer",
                type = "Boolean",
                shortCode = "JA"
            },
            ["killedBigFly"] = new PlayerDataField {
                name = "killedBigFly",
                type = "Boolean",
                shortCode = "JB"
            },
            ["killsBigFly"] = new PlayerDataField {
                name = "killsBigFly",
                type = "Int32",
                shortCode = "JC"
            },
            ["newDataBigFly"] = new PlayerDataField {
                name = "newDataBigFly",
                type = "Boolean",
                shortCode = "JD"
            },
            ["killedMawlek"] = new PlayerDataField {
                name = "killedMawlek",
                type = "Boolean",
                shortCode = "JE"
            },
            ["killsMawlek"] = new PlayerDataField {
                name = "killsMawlek",
                type = "Int32",
                shortCode = "JF"
            },
            ["newDataMawlek"] = new PlayerDataField {
                name = "newDataMawlek",
                type = "Boolean",
                shortCode = "JG"
            },
            ["killedFalseKnight"] = new PlayerDataField {
                name = "killedFalseKnight",
                type = "Boolean",
                shortCode = "JH"
            },
            ["killsFalseKnight"] = new PlayerDataField {
                name = "killsFalseKnight",
                type = "Int32",
                shortCode = "JI"
            },
            ["newDataFalseKnight"] = new PlayerDataField {
                name = "newDataFalseKnight",
                type = "Boolean",
                shortCode = "JJ"
            },
            ["killedRoller"] = new PlayerDataField {
                name = "killedRoller",
                type = "Boolean",
                shortCode = "JK"
            },
            ["killsRoller"] = new PlayerDataField {
                name = "killsRoller",
                type = "Int32",
                shortCode = "JL"
            },
            ["newDataRoller"] = new PlayerDataField {
                name = "newDataRoller",
                type = "Boolean",
                shortCode = "JM"
            },
            ["killedBlocker"] = new PlayerDataField {
                name = "killedBlocker",
                type = "Boolean",
                shortCode = "JN"
            },
            ["killsBlocker"] = new PlayerDataField {
                name = "killsBlocker",
                type = "Int32",
                shortCode = "JO"
            },
            ["newDataBlocker"] = new PlayerDataField {
                name = "newDataBlocker",
                type = "Boolean",
                shortCode = "JP"
            },
            ["killedPrayerSlug"] = new PlayerDataField {
                name = "killedPrayerSlug",
                type = "Boolean",
                shortCode = "JQ"
            },
            ["killsPrayerSlug"] = new PlayerDataField {
                name = "killsPrayerSlug",
                type = "Int32",
                shortCode = "JR"
            },
            ["newDataPrayerSlug"] = new PlayerDataField {
                name = "newDataPrayerSlug",
                type = "Boolean",
                shortCode = "JS"
            },
            ["killedMenderBug"] = new PlayerDataField {
                name = "killedMenderBug",
                type = "Boolean",
                shortCode = "JT"
            },
            ["killsMenderBug"] = new PlayerDataField {
                name = "killsMenderBug",
                type = "Int32",
                shortCode = "JU"
            },
            ["newDataMenderBug"] = new PlayerDataField {
                name = "newDataMenderBug",
                type = "Boolean",
                shortCode = "JV"
            },
            ["killedMossmanRunner"] = new PlayerDataField {
                name = "killedMossmanRunner",
                type = "Boolean",
                shortCode = "JW"
            },
            ["killsMossmanRunner"] = new PlayerDataField {
                name = "killsMossmanRunner",
                type = "Int32",
                shortCode = "JX"
            },
            ["newDataMossmanRunner"] = new PlayerDataField {
                name = "newDataMossmanRunner",
                type = "Boolean",
                shortCode = "JY"
            },
            ["killedMossmanShaker"] = new PlayerDataField {
                name = "killedMossmanShaker",
                type = "Boolean",
                shortCode = "JZ"
            },
            ["killsMossmanShaker"] = new PlayerDataField {
                name = "killsMossmanShaker",
                type = "Int32",
                shortCode = "K0"
            },
            ["newDataMossmanShaker"] = new PlayerDataField {
                name = "newDataMossmanShaker",
                type = "Boolean",
                shortCode = "K1"
            },
            ["killedMosquito"] = new PlayerDataField {
                name = "killedMosquito",
                type = "Boolean",
                shortCode = "K2"
            },
            ["killsMosquito"] = new PlayerDataField {
                name = "killsMosquito",
                type = "Int32",
                shortCode = "K3"
            },
            ["newDataMosquito"] = new PlayerDataField {
                name = "newDataMosquito",
                type = "Boolean",
                shortCode = "K4"
            },
            ["killedBlobFlyer"] = new PlayerDataField {
                name = "killedBlobFlyer",
                type = "Boolean",
                shortCode = "K5"
            },
            ["killsBlobFlyer"] = new PlayerDataField {
                name = "killsBlobFlyer",
                type = "Int32",
                shortCode = "K6"
            },
            ["newDataBlobFlyer"] = new PlayerDataField {
                name = "newDataBlobFlyer",
                type = "Boolean",
                shortCode = "K7"
            },
            ["killedFungifiedZombie"] = new PlayerDataField {
                name = "killedFungifiedZombie",
                type = "Boolean",
                shortCode = "K8"
            },
            ["killsFungifiedZombie"] = new PlayerDataField {
                name = "killsFungifiedZombie",
                type = "Int32",
                shortCode = "K9"
            },
            ["newDataFungifiedZombie"] = new PlayerDataField {
                name = "newDataFungifiedZombie",
                type = "Boolean",
                shortCode = "KA"
            },
            ["killedPlantShooter"] = new PlayerDataField {
                name = "killedPlantShooter",
                type = "Boolean",
                shortCode = "KB"
            },
            ["killsPlantShooter"] = new PlayerDataField {
                name = "killsPlantShooter",
                type = "Int32",
                shortCode = "KC"
            },
            ["newDataPlantShooter"] = new PlayerDataField {
                name = "newDataPlantShooter",
                type = "Boolean",
                shortCode = "KD"
            },
            ["killedMossCharger"] = new PlayerDataField {
                name = "killedMossCharger",
                type = "Boolean",
                shortCode = "KE"
            },
            ["killsMossCharger"] = new PlayerDataField {
                name = "killsMossCharger",
                type = "Int32",
                shortCode = "KF"
            },
            ["newDataMossCharger"] = new PlayerDataField {
                name = "newDataMossCharger",
                type = "Boolean",
                shortCode = "KG"
            },
            ["killedMegaMossCharger"] = new PlayerDataField {
                name = "killedMegaMossCharger",
                type = "Boolean",
                shortCode = "KH"
            },
            ["killsMegaMossCharger"] = new PlayerDataField {
                name = "killsMegaMossCharger",
                type = "Int32",
                shortCode = "KI"
            },
            ["newDataMegaMossCharger"] = new PlayerDataField {
                name = "newDataMegaMossCharger",
                type = "Boolean",
                shortCode = "KJ"
            },
            ["killedSnapperTrap"] = new PlayerDataField {
                name = "killedSnapperTrap",
                type = "Boolean",
                shortCode = "KK"
            },
            ["killsSnapperTrap"] = new PlayerDataField {
                name = "killsSnapperTrap",
                type = "Int32",
                shortCode = "KL"
            },
            ["newDataSnapperTrap"] = new PlayerDataField {
                name = "newDataSnapperTrap",
                type = "Boolean",
                shortCode = "KM"
            },
            ["killedMossKnight"] = new PlayerDataField {
                name = "killedMossKnight",
                type = "Boolean",
                shortCode = "KN"
            },
            ["killsMossKnight"] = new PlayerDataField {
                name = "killsMossKnight",
                type = "Int32",
                shortCode = "KO"
            },
            ["newDataMossKnight"] = new PlayerDataField {
                name = "newDataMossKnight",
                type = "Boolean",
                shortCode = "KP"
            },
            ["killedGrassHopper"] = new PlayerDataField {
                name = "killedGrassHopper",
                type = "Boolean",
                shortCode = "KQ"
            },
            ["killsGrassHopper"] = new PlayerDataField {
                name = "killsGrassHopper",
                type = "Int32",
                shortCode = "KR"
            },
            ["newDataGrassHopper"] = new PlayerDataField {
                name = "newDataGrassHopper",
                type = "Boolean",
                shortCode = "KS"
            },
            ["killedAcidFlyer"] = new PlayerDataField {
                name = "killedAcidFlyer",
                type = "Boolean",
                shortCode = "KT"
            },
            ["killsAcidFlyer"] = new PlayerDataField {
                name = "killsAcidFlyer",
                type = "Int32",
                shortCode = "KU"
            },
            ["newDataAcidFlyer"] = new PlayerDataField {
                name = "newDataAcidFlyer",
                type = "Boolean",
                shortCode = "KV"
            },
            ["killedAcidWalker"] = new PlayerDataField {
                name = "killedAcidWalker",
                type = "Boolean",
                shortCode = "KW"
            },
            ["killsAcidWalker"] = new PlayerDataField {
                name = "killsAcidWalker",
                type = "Int32",
                shortCode = "KX"
            },
            ["newDataAcidWalker"] = new PlayerDataField {
                name = "newDataAcidWalker",
                type = "Boolean",
                shortCode = "KY"
            },
            ["killedMossFlyer"] = new PlayerDataField {
                name = "killedMossFlyer",
                type = "Boolean",
                shortCode = "KZ"
            },
            ["killsMossFlyer"] = new PlayerDataField {
                name = "killsMossFlyer",
                type = "Int32",
                shortCode = "L0"
            },
            ["newDataMossFlyer"] = new PlayerDataField {
                name = "newDataMossFlyer",
                type = "Boolean",
                shortCode = "L1"
            },
            ["killedMossKnightFat"] = new PlayerDataField {
                name = "killedMossKnightFat",
                type = "Boolean",
                shortCode = "L2"
            },
            ["killsMossKnightFat"] = new PlayerDataField {
                name = "killsMossKnightFat",
                type = "Int32",
                shortCode = "L3"
            },
            ["newDataMossKnightFat"] = new PlayerDataField {
                name = "newDataMossKnightFat",
                type = "Boolean",
                shortCode = "L4"
            },
            ["killedMossWalker"] = new PlayerDataField {
                name = "killedMossWalker",
                type = "Boolean",
                shortCode = "L5"
            },
            ["killsMossWalker"] = new PlayerDataField {
                name = "killsMossWalker",
                type = "Int32",
                shortCode = "L6"
            },
            ["newDataMossWalker"] = new PlayerDataField {
                name = "newDataMossWalker",
                type = "Boolean",
                shortCode = "L7"
            },
            ["killedInfectedKnight"] = new PlayerDataField {
                name = "killedInfectedKnight",
                type = "Boolean",
                shortCode = "L8"
            },
            ["killsInfectedKnight"] = new PlayerDataField {
                name = "killsInfectedKnight",
                type = "Int32",
                shortCode = "L9"
            },
            ["newDataInfectedKnight"] = new PlayerDataField {
                name = "newDataInfectedKnight",
                type = "Boolean",
                shortCode = "LA"
            },
            ["killedLazyFlyer"] = new PlayerDataField {
                name = "killedLazyFlyer",
                type = "Boolean",
                shortCode = "LB"
            },
            ["killsLazyFlyer"] = new PlayerDataField {
                name = "killsLazyFlyer",
                type = "Int32",
                shortCode = "LC"
            },
            ["newDataLazyFlyer"] = new PlayerDataField {
                name = "newDataLazyFlyer",
                type = "Boolean",
                shortCode = "LD"
            },
            ["killedZapBug"] = new PlayerDataField {
                name = "killedZapBug",
                type = "Boolean",
                shortCode = "LE"
            },
            ["killsZapBug"] = new PlayerDataField {
                name = "killsZapBug",
                type = "Int32",
                shortCode = "LF"
            },
            ["newDataZapBug"] = new PlayerDataField {
                name = "newDataZapBug",
                type = "Boolean",
                shortCode = "LG"
            },
            ["killedJellyfish"] = new PlayerDataField {
                name = "killedJellyfish",
                type = "Boolean",
                shortCode = "LH"
            },
            ["killsJellyfish"] = new PlayerDataField {
                name = "killsJellyfish",
                type = "Int32",
                shortCode = "LI"
            },
            ["newDataJellyfish"] = new PlayerDataField {
                name = "newDataJellyfish",
                type = "Boolean",
                shortCode = "LJ"
            },
            ["killedJellyCrawler"] = new PlayerDataField {
                name = "killedJellyCrawler",
                type = "Boolean",
                shortCode = "LK"
            },
            ["killsJellyCrawler"] = new PlayerDataField {
                name = "killsJellyCrawler",
                type = "Int32",
                shortCode = "LL"
            },
            ["newDataJellyCrawler"] = new PlayerDataField {
                name = "newDataJellyCrawler",
                type = "Boolean",
                shortCode = "LM"
            },
            ["killedMegaJellyfish"] = new PlayerDataField {
                name = "killedMegaJellyfish",
                type = "Boolean",
                shortCode = "LN"
            },
            ["killsMegaJellyfish"] = new PlayerDataField {
                name = "killsMegaJellyfish",
                type = "Int32",
                shortCode = "LO"
            },
            ["newDataMegaJellyfish"] = new PlayerDataField {
                name = "newDataMegaJellyfish",
                type = "Boolean",
                shortCode = "LP"
            },
            ["killedFungoonBaby"] = new PlayerDataField {
                name = "killedFungoonBaby",
                type = "Boolean",
                shortCode = "LQ"
            },
            ["killsFungoonBaby"] = new PlayerDataField {
                name = "killsFungoonBaby",
                type = "Int32",
                shortCode = "LR"
            },
            ["newDataFungoonBaby"] = new PlayerDataField {
                name = "newDataFungoonBaby",
                type = "Boolean",
                shortCode = "LS"
            },
            ["killedMushroomTurret"] = new PlayerDataField {
                name = "killedMushroomTurret",
                type = "Boolean",
                shortCode = "LT"
            },
            ["killsMushroomTurret"] = new PlayerDataField {
                name = "killsMushroomTurret",
                type = "Int32",
                shortCode = "LU"
            },
            ["newDataMushroomTurret"] = new PlayerDataField {
                name = "newDataMushroomTurret",
                type = "Boolean",
                shortCode = "LV"
            },
            ["killedMantis"] = new PlayerDataField {
                name = "killedMantis",
                type = "Boolean",
                shortCode = "LW"
            },
            ["killsMantis"] = new PlayerDataField {
                name = "killsMantis",
                type = "Int32",
                shortCode = "LX"
            },
            ["newDataMantis"] = new PlayerDataField {
                name = "newDataMantis",
                type = "Boolean",
                shortCode = "LY"
            },
            ["killedMushroomRoller"] = new PlayerDataField {
                name = "killedMushroomRoller",
                type = "Boolean",
                shortCode = "LZ"
            },
            ["killsMushroomRoller"] = new PlayerDataField {
                name = "killsMushroomRoller",
                type = "Int32",
                shortCode = "M0"
            },
            ["newDataMushroomRoller"] = new PlayerDataField {
                name = "newDataMushroomRoller",
                type = "Boolean",
                shortCode = "M1"
            },
            ["killedMushroomBrawler"] = new PlayerDataField {
                name = "killedMushroomBrawler",
                type = "Boolean",
                shortCode = "M2"
            },
            ["killsMushroomBrawler"] = new PlayerDataField {
                name = "killsMushroomBrawler",
                type = "Int32",
                shortCode = "M3"
            },
            ["newDataMushroomBrawler"] = new PlayerDataField {
                name = "newDataMushroomBrawler",
                type = "Boolean",
                shortCode = "M4"
            },
            ["killedMushroomBaby"] = new PlayerDataField {
                name = "killedMushroomBaby",
                type = "Boolean",
                shortCode = "M5"
            },
            ["killsMushroomBaby"] = new PlayerDataField {
                name = "killsMushroomBaby",
                type = "Int32",
                shortCode = "M6"
            },
            ["newDataMushroomBaby"] = new PlayerDataField {
                name = "newDataMushroomBaby",
                type = "Boolean",
                shortCode = "M7"
            },
            ["killedMantisFlyerChild"] = new PlayerDataField {
                name = "killedMantisFlyerChild",
                type = "Boolean",
                shortCode = "M8"
            },
            ["killsMantisFlyerChild"] = new PlayerDataField {
                name = "killsMantisFlyerChild",
                type = "Int32",
                shortCode = "M9"
            },
            ["newDataMantisFlyerChild"] = new PlayerDataField {
                name = "newDataMantisFlyerChild",
                type = "Boolean",
                shortCode = "MA"
            },
            ["killedFungusFlyer"] = new PlayerDataField {
                name = "killedFungusFlyer",
                type = "Boolean",
                shortCode = "MB"
            },
            ["killsFungusFlyer"] = new PlayerDataField {
                name = "killsFungusFlyer",
                type = "Int32",
                shortCode = "MC"
            },
            ["newDataFungusFlyer"] = new PlayerDataField {
                name = "newDataFungusFlyer",
                type = "Boolean",
                shortCode = "MD"
            },
            ["killedFungCrawler"] = new PlayerDataField {
                name = "killedFungCrawler",
                type = "Boolean",
                shortCode = "ME"
            },
            ["killsFungCrawler"] = new PlayerDataField {
                name = "killsFungCrawler",
                type = "Int32",
                shortCode = "MF"
            },
            ["newDataFungCrawler"] = new PlayerDataField {
                name = "newDataFungCrawler",
                type = "Boolean",
                shortCode = "MG"
            },
            ["killedMantisLord"] = new PlayerDataField {
                name = "killedMantisLord",
                type = "Boolean",
                shortCode = "MH"
            },
            ["killsMantisLord"] = new PlayerDataField {
                name = "killsMantisLord",
                type = "Int32",
                shortCode = "MI"
            },
            ["newDataMantisLord"] = new PlayerDataField {
                name = "newDataMantisLord",
                type = "Boolean",
                shortCode = "MJ"
            },
            ["killedBlackKnight"] = new PlayerDataField {
                name = "killedBlackKnight",
                type = "Boolean",
                shortCode = "MK"
            },
            ["killsBlackKnight"] = new PlayerDataField {
                name = "killsBlackKnight",
                type = "Int32",
                shortCode = "ML"
            },
            ["newDataBlackKnight"] = new PlayerDataField {
                name = "newDataBlackKnight",
                type = "Boolean",
                shortCode = "MM"
            },
            ["killedElectricMage"] = new PlayerDataField {
                name = "killedElectricMage",
                type = "Boolean",
                shortCode = "MN"
            },
            ["killsElectricMage"] = new PlayerDataField {
                name = "killsElectricMage",
                type = "Int32",
                shortCode = "MO"
            },
            ["newDataElectricMage"] = new PlayerDataField {
                name = "newDataElectricMage",
                type = "Boolean",
                shortCode = "MP"
            },
            ["killedMage"] = new PlayerDataField {
                name = "killedMage",
                type = "Boolean",
                shortCode = "MQ"
            },
            ["killsMage"] = new PlayerDataField {
                name = "killsMage",
                type = "Int32",
                shortCode = "MR"
            },
            ["newDataMage"] = new PlayerDataField {
                name = "newDataMage",
                type = "Boolean",
                shortCode = "MS"
            },
            ["killedMageKnight"] = new PlayerDataField {
                name = "killedMageKnight",
                type = "Boolean",
                shortCode = "MT"
            },
            ["killsMageKnight"] = new PlayerDataField {
                name = "killsMageKnight",
                type = "Int32",
                shortCode = "MU"
            },
            ["newDataMageKnight"] = new PlayerDataField {
                name = "newDataMageKnight",
                type = "Boolean",
                shortCode = "MV"
            },
            ["killedRoyalDandy"] = new PlayerDataField {
                name = "killedRoyalDandy",
                type = "Boolean",
                shortCode = "MW"
            },
            ["killsRoyalDandy"] = new PlayerDataField {
                name = "killsRoyalDandy",
                type = "Int32",
                shortCode = "MX"
            },
            ["newDataRoyalDandy"] = new PlayerDataField {
                name = "newDataRoyalDandy",
                type = "Boolean",
                shortCode = "MY"
            },
            ["killedRoyalCoward"] = new PlayerDataField {
                name = "killedRoyalCoward",
                type = "Boolean",
                shortCode = "MZ"
            },
            ["killsRoyalCoward"] = new PlayerDataField {
                name = "killsRoyalCoward",
                type = "Int32",
                shortCode = "N0"
            },
            ["newDataRoyalCoward"] = new PlayerDataField {
                name = "newDataRoyalCoward",
                type = "Boolean",
                shortCode = "N1"
            },
            ["killedRoyalPlumper"] = new PlayerDataField {
                name = "killedRoyalPlumper",
                type = "Boolean",
                shortCode = "N2"
            },
            ["killsRoyalPlumper"] = new PlayerDataField {
                name = "killsRoyalPlumper",
                type = "Int32",
                shortCode = "N3"
            },
            ["newDataRoyalPlumper"] = new PlayerDataField {
                name = "newDataRoyalPlumper",
                type = "Boolean",
                shortCode = "N4"
            },
            ["killedFlyingSentrySword"] = new PlayerDataField {
                name = "killedFlyingSentrySword",
                type = "Boolean",
                shortCode = "N5"
            },
            ["killsFlyingSentrySword"] = new PlayerDataField {
                name = "killsFlyingSentrySword",
                type = "Int32",
                shortCode = "N6"
            },
            ["newDataFlyingSentrySword"] = new PlayerDataField {
                name = "newDataFlyingSentrySword",
                type = "Boolean",
                shortCode = "N7"
            },
            ["killedFlyingSentryJavelin"] = new PlayerDataField {
                name = "killedFlyingSentryJavelin",
                type = "Boolean",
                shortCode = "N8"
            },
            ["killsFlyingSentryJavelin"] = new PlayerDataField {
                name = "killsFlyingSentryJavelin",
                type = "Int32",
                shortCode = "N9"
            },
            ["newDataFlyingSentryJavelin"] = new PlayerDataField {
                name = "newDataFlyingSentryJavelin",
                type = "Boolean",
                shortCode = "NA"
            },
            ["killedSentry"] = new PlayerDataField {
                name = "killedSentry",
                type = "Boolean",
                shortCode = "NB"
            },
            ["killsSentry"] = new PlayerDataField {
                name = "killsSentry",
                type = "Int32",
                shortCode = "NC"
            },
            ["newDataSentry"] = new PlayerDataField {
                name = "newDataSentry",
                type = "Boolean",
                shortCode = "ND"
            },
            ["killedSentryFat"] = new PlayerDataField {
                name = "killedSentryFat",
                type = "Boolean",
                shortCode = "NE"
            },
            ["killsSentryFat"] = new PlayerDataField {
                name = "killsSentryFat",
                type = "Int32",
                shortCode = "NF"
            },
            ["newDataSentryFat"] = new PlayerDataField {
                name = "newDataSentryFat",
                type = "Boolean",
                shortCode = "NG"
            },
            ["killedMageBlob"] = new PlayerDataField {
                name = "killedMageBlob",
                type = "Boolean",
                shortCode = "NH"
            },
            ["killsMageBlob"] = new PlayerDataField {
                name = "killsMageBlob",
                type = "Int32",
                shortCode = "NI"
            },
            ["newDataMageBlob"] = new PlayerDataField {
                name = "newDataMageBlob",
                type = "Boolean",
                shortCode = "NJ"
            },
            ["killedGreatShieldZombie"] = new PlayerDataField {
                name = "killedGreatShieldZombie",
                type = "Boolean",
                shortCode = "NK"
            },
            ["killsGreatShieldZombie"] = new PlayerDataField {
                name = "killsGreatShieldZombie",
                type = "Int32",
                shortCode = "NL"
            },
            ["newDataGreatShieldZombie"] = new PlayerDataField {
                name = "newDataGreatShieldZombie",
                type = "Boolean",
                shortCode = "NM"
            },
            ["killedJarCollector"] = new PlayerDataField {
                name = "killedJarCollector",
                type = "Boolean",
                shortCode = "NN"
            },
            ["killsJarCollector"] = new PlayerDataField {
                name = "killsJarCollector",
                type = "Int32",
                shortCode = "NO"
            },
            ["newDataJarCollector"] = new PlayerDataField {
                name = "newDataJarCollector",
                type = "Boolean",
                shortCode = "NP"
            },
            ["killedMageBalloon"] = new PlayerDataField {
                name = "killedMageBalloon",
                type = "Boolean",
                shortCode = "NQ"
            },
            ["killsMageBalloon"] = new PlayerDataField {
                name = "killsMageBalloon",
                type = "Int32",
                shortCode = "NR"
            },
            ["newDataMageBalloon"] = new PlayerDataField {
                name = "newDataMageBalloon",
                type = "Boolean",
                shortCode = "NS"
            },
            ["killedMageLord"] = new PlayerDataField {
                name = "killedMageLord",
                type = "Boolean",
                shortCode = "NT"
            },
            ["killsMageLord"] = new PlayerDataField {
                name = "killsMageLord",
                type = "Int32",
                shortCode = "NU"
            },
            ["newDataMageLord"] = new PlayerDataField {
                name = "newDataMageLord",
                type = "Boolean",
                shortCode = "NV"
            },
            ["killedGorgeousHusk"] = new PlayerDataField {
                name = "killedGorgeousHusk",
                type = "Boolean",
                shortCode = "NW"
            },
            ["killsGorgeousHusk"] = new PlayerDataField {
                name = "killsGorgeousHusk",
                type = "Int32",
                shortCode = "NX"
            },
            ["newDataGorgeousHusk"] = new PlayerDataField {
                name = "newDataGorgeousHusk",
                type = "Boolean",
                shortCode = "NY"
            },
            ["killedFlipHopper"] = new PlayerDataField {
                name = "killedFlipHopper",
                type = "Boolean",
                shortCode = "NZ"
            },
            ["killsFlipHopper"] = new PlayerDataField {
                name = "killsFlipHopper",
                type = "Int32",
                shortCode = "O0"
            },
            ["newDataFlipHopper"] = new PlayerDataField {
                name = "newDataFlipHopper",
                type = "Boolean",
                shortCode = "O1"
            },
            ["killedFlukeman"] = new PlayerDataField {
                name = "killedFlukeman",
                type = "Boolean",
                shortCode = "O2"
            },
            ["killsFlukeman"] = new PlayerDataField {
                name = "killsFlukeman",
                type = "Int32",
                shortCode = "O3"
            },
            ["newDataFlukeman"] = new PlayerDataField {
                name = "newDataFlukeman",
                type = "Boolean",
                shortCode = "O4"
            },
            ["killedInflater"] = new PlayerDataField {
                name = "killedInflater",
                type = "Boolean",
                shortCode = "O5"
            },
            ["killsInflater"] = new PlayerDataField {
                name = "killsInflater",
                type = "Int32",
                shortCode = "O6"
            },
            ["newDataInflater"] = new PlayerDataField {
                name = "newDataInflater",
                type = "Boolean",
                shortCode = "O7"
            },
            ["killedFlukefly"] = new PlayerDataField {
                name = "killedFlukefly",
                type = "Boolean",
                shortCode = "O8"
            },
            ["killsFlukefly"] = new PlayerDataField {
                name = "killsFlukefly",
                type = "Int32",
                shortCode = "O9"
            },
            ["newDataFlukefly"] = new PlayerDataField {
                name = "newDataFlukefly",
                type = "Boolean",
                shortCode = "OA"
            },
            ["killedFlukeMother"] = new PlayerDataField {
                name = "killedFlukeMother",
                type = "Boolean",
                shortCode = "OB"
            },
            ["killsFlukeMother"] = new PlayerDataField {
                name = "killsFlukeMother",
                type = "Int32",
                shortCode = "OC"
            },
            ["newDataFlukeMother"] = new PlayerDataField {
                name = "newDataFlukeMother",
                type = "Boolean",
                shortCode = "OD"
            },
            ["killedDungDefender"] = new PlayerDataField {
                name = "killedDungDefender",
                type = "Boolean",
                shortCode = "OE"
            },
            ["killsDungDefender"] = new PlayerDataField {
                name = "killsDungDefender",
                type = "Int32",
                shortCode = "OF"
            },
            ["newDataDungDefender"] = new PlayerDataField {
                name = "newDataDungDefender",
                type = "Boolean",
                shortCode = "OG"
            },
            ["killedCrystalCrawler"] = new PlayerDataField {
                name = "killedCrystalCrawler",
                type = "Boolean",
                shortCode = "OH"
            },
            ["killsCrystalCrawler"] = new PlayerDataField {
                name = "killsCrystalCrawler",
                type = "Int32",
                shortCode = "OI"
            },
            ["newDataCrystalCrawler"] = new PlayerDataField {
                name = "newDataCrystalCrawler",
                type = "Boolean",
                shortCode = "OJ"
            },
            ["killedCrystalFlyer"] = new PlayerDataField {
                name = "killedCrystalFlyer",
                type = "Boolean",
                shortCode = "OK"
            },
            ["killsCrystalFlyer"] = new PlayerDataField {
                name = "killsCrystalFlyer",
                type = "Int32",
                shortCode = "OL"
            },
            ["newDataCrystalFlyer"] = new PlayerDataField {
                name = "newDataCrystalFlyer",
                type = "Boolean",
                shortCode = "OM"
            },
            ["killedLaserBug"] = new PlayerDataField {
                name = "killedLaserBug",
                type = "Boolean",
                shortCode = "ON"
            },
            ["killsLaserBug"] = new PlayerDataField {
                name = "killsLaserBug",
                type = "Int32",
                shortCode = "OO"
            },
            ["newDataLaserBug"] = new PlayerDataField {
                name = "newDataLaserBug",
                type = "Boolean",
                shortCode = "OP"
            },
            ["killedBeamMiner"] = new PlayerDataField {
                name = "killedBeamMiner",
                type = "Boolean",
                shortCode = "OQ"
            },
            ["killsBeamMiner"] = new PlayerDataField {
                name = "killsBeamMiner",
                type = "Int32",
                shortCode = "OR"
            },
            ["newDataBeamMiner"] = new PlayerDataField {
                name = "newDataBeamMiner",
                type = "Boolean",
                shortCode = "OS"
            },
            ["killedZombieMiner"] = new PlayerDataField {
                name = "killedZombieMiner",
                type = "Boolean",
                shortCode = "OT"
            },
            ["killsZombieMiner"] = new PlayerDataField {
                name = "killsZombieMiner",
                type = "Int32",
                shortCode = "OU"
            },
            ["newDataZombieMiner"] = new PlayerDataField {
                name = "newDataZombieMiner",
                type = "Boolean",
                shortCode = "OV"
            },
            ["killedMegaBeamMiner"] = new PlayerDataField {
                name = "killedMegaBeamMiner",
                type = "Boolean",
                shortCode = "OW"
            },
            ["killsMegaBeamMiner"] = new PlayerDataField {
                name = "killsMegaBeamMiner",
                type = "Int32",
                shortCode = "OX"
            },
            ["newDataMegaBeamMiner"] = new PlayerDataField {
                name = "newDataMegaBeamMiner",
                type = "Boolean",
                shortCode = "OY"
            },
            ["killedMinesCrawler"] = new PlayerDataField {
                name = "killedMinesCrawler",
                type = "Boolean",
                shortCode = "OZ"
            },
            ["killsMinesCrawler"] = new PlayerDataField {
                name = "killsMinesCrawler",
                type = "Int32",
                shortCode = "P0"
            },
            ["newDataMinesCrawler"] = new PlayerDataField {
                name = "newDataMinesCrawler",
                type = "Boolean",
                shortCode = "P1"
            },
            ["killedAngryBuzzer"] = new PlayerDataField {
                name = "killedAngryBuzzer",
                type = "Boolean",
                shortCode = "P2"
            },
            ["killsAngryBuzzer"] = new PlayerDataField {
                name = "killsAngryBuzzer",
                type = "Int32",
                shortCode = "P3"
            },
            ["newDataAngryBuzzer"] = new PlayerDataField {
                name = "newDataAngryBuzzer",
                type = "Boolean",
                shortCode = "P4"
            },
            ["killedBurstingBouncer"] = new PlayerDataField {
                name = "killedBurstingBouncer",
                type = "Boolean",
                shortCode = "P5"
            },
            ["killsBurstingBouncer"] = new PlayerDataField {
                name = "killsBurstingBouncer",
                type = "Int32",
                shortCode = "P6"
            },
            ["newDataBurstingBouncer"] = new PlayerDataField {
                name = "newDataBurstingBouncer",
                type = "Boolean",
                shortCode = "P7"
            },
            ["killedBurstingZombie"] = new PlayerDataField {
                name = "killedBurstingZombie",
                type = "Boolean",
                shortCode = "P8"
            },
            ["killsBurstingZombie"] = new PlayerDataField {
                name = "killsBurstingZombie",
                type = "Int32",
                shortCode = "P9"
            },
            ["newDataBurstingZombie"] = new PlayerDataField {
                name = "newDataBurstingZombie",
                type = "Boolean",
                shortCode = "PA"
            },
            ["killedSpittingZombie"] = new PlayerDataField {
                name = "killedSpittingZombie",
                type = "Boolean",
                shortCode = "PB"
            },
            ["killsSpittingZombie"] = new PlayerDataField {
                name = "killsSpittingZombie",
                type = "Int32",
                shortCode = "PC"
            },
            ["newDataSpittingZombie"] = new PlayerDataField {
                name = "newDataSpittingZombie",
                type = "Boolean",
                shortCode = "PD"
            },
            ["killedBabyCentipede"] = new PlayerDataField {
                name = "killedBabyCentipede",
                type = "Boolean",
                shortCode = "PE"
            },
            ["killsBabyCentipede"] = new PlayerDataField {
                name = "killsBabyCentipede",
                type = "Int32",
                shortCode = "PF"
            },
            ["newDataBabyCentipede"] = new PlayerDataField {
                name = "newDataBabyCentipede",
                type = "Boolean",
                shortCode = "PG"
            },
            ["killedBigCentipede"] = new PlayerDataField {
                name = "killedBigCentipede",
                type = "Boolean",
                shortCode = "PH"
            },
            ["killsBigCentipede"] = new PlayerDataField {
                name = "killsBigCentipede",
                type = "Int32",
                shortCode = "PI"
            },
            ["newDataBigCentipede"] = new PlayerDataField {
                name = "newDataBigCentipede",
                type = "Boolean",
                shortCode = "PJ"
            },
            ["killedCentipedeHatcher"] = new PlayerDataField {
                name = "killedCentipedeHatcher",
                type = "Boolean",
                shortCode = "PK"
            },
            ["killsCentipedeHatcher"] = new PlayerDataField {
                name = "killsCentipedeHatcher",
                type = "Int32",
                shortCode = "PL"
            },
            ["newDataCentipedeHatcher"] = new PlayerDataField {
                name = "newDataCentipedeHatcher",
                type = "Boolean",
                shortCode = "PM"
            },
            ["killedLesserMawlek"] = new PlayerDataField {
                name = "killedLesserMawlek",
                type = "Boolean",
                shortCode = "PN"
            },
            ["killsLesserMawlek"] = new PlayerDataField {
                name = "killsLesserMawlek",
                type = "Int32",
                shortCode = "PO"
            },
            ["newDataLesserMawlek"] = new PlayerDataField {
                name = "newDataLesserMawlek",
                type = "Boolean",
                shortCode = "PP"
            },
            ["killedSlashSpider"] = new PlayerDataField {
                name = "killedSlashSpider",
                type = "Boolean",
                shortCode = "PQ"
            },
            ["killsSlashSpider"] = new PlayerDataField {
                name = "killsSlashSpider",
                type = "Int32",
                shortCode = "PR"
            },
            ["newDataSlashSpider"] = new PlayerDataField {
                name = "newDataSlashSpider",
                type = "Boolean",
                shortCode = "PS"
            },
            ["killedSpiderCorpse"] = new PlayerDataField {
                name = "killedSpiderCorpse",
                type = "Boolean",
                shortCode = "PT"
            },
            ["killsSpiderCorpse"] = new PlayerDataField {
                name = "killsSpiderCorpse",
                type = "Int32",
                shortCode = "PU"
            },
            ["newDataSpiderCorpse"] = new PlayerDataField {
                name = "newDataSpiderCorpse",
                type = "Boolean",
                shortCode = "PV"
            },
            ["killedShootSpider"] = new PlayerDataField {
                name = "killedShootSpider",
                type = "Boolean",
                shortCode = "PW"
            },
            ["killsShootSpider"] = new PlayerDataField {
                name = "killsShootSpider",
                type = "Int32",
                shortCode = "PX"
            },
            ["newDataShootSpider"] = new PlayerDataField {
                name = "newDataShootSpider",
                type = "Boolean",
                shortCode = "PY"
            },
            ["killedMiniSpider"] = new PlayerDataField {
                name = "killedMiniSpider",
                type = "Boolean",
                shortCode = "PZ"
            },
            ["killsMiniSpider"] = new PlayerDataField {
                name = "killsMiniSpider",
                type = "Int32",
                shortCode = "Q0"
            },
            ["newDataMiniSpider"] = new PlayerDataField {
                name = "newDataMiniSpider",
                type = "Boolean",
                shortCode = "Q1"
            },
            ["killedSpiderFlyer"] = new PlayerDataField {
                name = "killedSpiderFlyer",
                type = "Boolean",
                shortCode = "Q2"
            },
            ["killsSpiderFlyer"] = new PlayerDataField {
                name = "killsSpiderFlyer",
                type = "Int32",
                shortCode = "Q3"
            },
            ["newDataSpiderFlyer"] = new PlayerDataField {
                name = "newDataSpiderFlyer",
                type = "Boolean",
                shortCode = "Q4"
            },
            ["killedMimicSpider"] = new PlayerDataField {
                name = "killedMimicSpider",
                type = "Boolean",
                shortCode = "Q5"
            },
            ["killsMimicSpider"] = new PlayerDataField {
                name = "killsMimicSpider",
                type = "Int32",
                shortCode = "Q6"
            },
            ["newDataMimicSpider"] = new PlayerDataField {
                name = "newDataMimicSpider",
                type = "Boolean",
                shortCode = "Q7"
            },
            ["killedBeeHatchling"] = new PlayerDataField {
                name = "killedBeeHatchling",
                type = "Boolean",
                shortCode = "Q8"
            },
            ["killsBeeHatchling"] = new PlayerDataField {
                name = "killsBeeHatchling",
                type = "Int32",
                shortCode = "Q9"
            },
            ["newDataBeeHatchling"] = new PlayerDataField {
                name = "newDataBeeHatchling",
                type = "Boolean",
                shortCode = "QA"
            },
            ["killedBeeStinger"] = new PlayerDataField {
                name = "killedBeeStinger",
                type = "Boolean",
                shortCode = "QB"
            },
            ["killsBeeStinger"] = new PlayerDataField {
                name = "killsBeeStinger",
                type = "Int32",
                shortCode = "QC"
            },
            ["newDataBeeStinger"] = new PlayerDataField {
                name = "newDataBeeStinger",
                type = "Boolean",
                shortCode = "QD"
            },
            ["killedBigBee"] = new PlayerDataField {
                name = "killedBigBee",
                type = "Boolean",
                shortCode = "QE"
            },
            ["killsBigBee"] = new PlayerDataField {
                name = "killsBigBee",
                type = "Int32",
                shortCode = "QF"
            },
            ["newDataBigBee"] = new PlayerDataField {
                name = "newDataBigBee",
                type = "Boolean",
                shortCode = "QG"
            },
            ["killedHiveKnight"] = new PlayerDataField {
                name = "killedHiveKnight",
                type = "Boolean",
                shortCode = "QH"
            },
            ["killsHiveKnight"] = new PlayerDataField {
                name = "killsHiveKnight",
                type = "Int32",
                shortCode = "QI"
            },
            ["newDataHiveKnight"] = new PlayerDataField {
                name = "newDataHiveKnight",
                type = "Boolean",
                shortCode = "QJ"
            },
            ["killedBlowFly"] = new PlayerDataField {
                name = "killedBlowFly",
                type = "Boolean",
                shortCode = "QK"
            },
            ["killsBlowFly"] = new PlayerDataField {
                name = "killsBlowFly",
                type = "Int32",
                shortCode = "QL"
            },
            ["newDataBlowFly"] = new PlayerDataField {
                name = "newDataBlowFly",
                type = "Boolean",
                shortCode = "QM"
            },
            ["killedCeilingDropper"] = new PlayerDataField {
                name = "killedCeilingDropper",
                type = "Boolean",
                shortCode = "QN"
            },
            ["killsCeilingDropper"] = new PlayerDataField {
                name = "killsCeilingDropper",
                type = "Int32",
                shortCode = "QO"
            },
            ["newDataCeilingDropper"] = new PlayerDataField {
                name = "newDataCeilingDropper",
                type = "Boolean",
                shortCode = "QP"
            },
            ["killedGiantHopper"] = new PlayerDataField {
                name = "killedGiantHopper",
                type = "Boolean",
                shortCode = "QQ"
            },
            ["killsGiantHopper"] = new PlayerDataField {
                name = "killsGiantHopper",
                type = "Int32",
                shortCode = "QR"
            },
            ["newDataGiantHopper"] = new PlayerDataField {
                name = "newDataGiantHopper",
                type = "Boolean",
                shortCode = "QS"
            },
            ["killedGrubMimic"] = new PlayerDataField {
                name = "killedGrubMimic",
                type = "Boolean",
                shortCode = "QT"
            },
            ["killsGrubMimic"] = new PlayerDataField {
                name = "killsGrubMimic",
                type = "Int32",
                shortCode = "QU"
            },
            ["newDataGrubMimic"] = new PlayerDataField {
                name = "newDataGrubMimic",
                type = "Boolean",
                shortCode = "QV"
            },
            ["killedMawlekTurret"] = new PlayerDataField {
                name = "killedMawlekTurret",
                type = "Boolean",
                shortCode = "QW"
            },
            ["killsMawlekTurret"] = new PlayerDataField {
                name = "killsMawlekTurret",
                type = "Int32",
                shortCode = "QX"
            },
            ["newDataMawlekTurret"] = new PlayerDataField {
                name = "newDataMawlekTurret",
                type = "Boolean",
                shortCode = "QY"
            },
            ["killedOrangeScuttler"] = new PlayerDataField {
                name = "killedOrangeScuttler",
                type = "Boolean",
                shortCode = "QZ"
            },
            ["killsOrangeScuttler"] = new PlayerDataField {
                name = "killsOrangeScuttler",
                type = "Int32",
                shortCode = "R0"
            },
            ["newDataOrangeScuttler"] = new PlayerDataField {
                name = "newDataOrangeScuttler",
                type = "Boolean",
                shortCode = "R1"
            },
            ["killedHealthScuttler"] = new PlayerDataField {
                name = "killedHealthScuttler",
                type = "Boolean",
                shortCode = "R2"
            },
            ["killsHealthScuttler"] = new PlayerDataField {
                name = "killsHealthScuttler",
                type = "Int32",
                shortCode = "R3"
            },
            ["newDataHealthScuttler"] = new PlayerDataField {
                name = "newDataHealthScuttler",
                type = "Boolean",
                shortCode = "R4"
            },
            ["killedPigeon"] = new PlayerDataField {
                name = "killedPigeon",
                type = "Boolean",
                shortCode = "R5"
            },
            ["killsPigeon"] = new PlayerDataField {
                name = "killsPigeon",
                type = "Int32",
                shortCode = "R6"
            },
            ["newDataPigeon"] = new PlayerDataField {
                name = "newDataPigeon",
                type = "Boolean",
                shortCode = "R7"
            },
            ["killedZombieHive"] = new PlayerDataField {
                name = "killedZombieHive",
                type = "Boolean",
                shortCode = "R8"
            },
            ["killsZombieHive"] = new PlayerDataField {
                name = "killsZombieHive",
                type = "Int32",
                shortCode = "R9"
            },
            ["newDataZombieHive"] = new PlayerDataField {
                name = "newDataZombieHive",
                type = "Boolean",
                shortCode = "RA"
            },
            ["killedDreamGuard"] = new PlayerDataField {
                name = "killedDreamGuard",
                type = "Boolean",
                shortCode = "RB"
            },
            ["killsDreamGuard"] = new PlayerDataField {
                name = "killsDreamGuard",
                type = "Int32",
                shortCode = "RC"
            },
            ["newDataDreamGuard"] = new PlayerDataField {
                name = "newDataDreamGuard",
                type = "Boolean",
                shortCode = "RD"
            },
            ["killedHornet"] = new PlayerDataField {
                name = "killedHornet",
                type = "Boolean",
                shortCode = "RE"
            },
            ["killsHornet"] = new PlayerDataField {
                name = "killsHornet",
                type = "Int32",
                shortCode = "RF"
            },
            ["newDataHornet"] = new PlayerDataField {
                name = "newDataHornet",
                type = "Boolean",
                shortCode = "RG"
            },
            ["killedAbyssCrawler"] = new PlayerDataField {
                name = "killedAbyssCrawler",
                type = "Boolean",
                shortCode = "RH"
            },
            ["killsAbyssCrawler"] = new PlayerDataField {
                name = "killsAbyssCrawler",
                type = "Int32",
                shortCode = "RI"
            },
            ["newDataAbyssCrawler"] = new PlayerDataField {
                name = "newDataAbyssCrawler",
                type = "Boolean",
                shortCode = "RJ"
            },
            ["killedSuperSpitter"] = new PlayerDataField {
                name = "killedSuperSpitter",
                type = "Boolean",
                shortCode = "RK"
            },
            ["killsSuperSpitter"] = new PlayerDataField {
                name = "killsSuperSpitter",
                type = "Int32",
                shortCode = "RL"
            },
            ["newDataSuperSpitter"] = new PlayerDataField {
                name = "newDataSuperSpitter",
                type = "Boolean",
                shortCode = "RM"
            },
            ["killedSibling"] = new PlayerDataField {
                name = "killedSibling",
                type = "Boolean",
                shortCode = "RN"
            },
            ["killsSibling"] = new PlayerDataField {
                name = "killsSibling",
                type = "Int32",
                shortCode = "RO"
            },
            ["newDataSibling"] = new PlayerDataField {
                name = "newDataSibling",
                type = "Boolean",
                shortCode = "RP"
            },
            ["killedPalaceFly"] = new PlayerDataField {
                name = "killedPalaceFly",
                type = "Boolean",
                shortCode = "RQ"
            },
            ["killsPalaceFly"] = new PlayerDataField {
                name = "killsPalaceFly",
                type = "Int32",
                shortCode = "RR"
            },
            ["newDataPalaceFly"] = new PlayerDataField {
                name = "newDataPalaceFly",
                type = "Boolean",
                shortCode = "RS"
            },
            ["killedEggSac"] = new PlayerDataField {
                name = "killedEggSac",
                type = "Boolean",
                shortCode = "RT"
            },
            ["killsEggSac"] = new PlayerDataField {
                name = "killsEggSac",
                type = "Int32",
                shortCode = "RU"
            },
            ["newDataEggSac"] = new PlayerDataField {
                name = "newDataEggSac",
                type = "Boolean",
                shortCode = "RV"
            },
            ["killedMummy"] = new PlayerDataField {
                name = "killedMummy",
                type = "Boolean",
                shortCode = "RW"
            },
            ["killsMummy"] = new PlayerDataField {
                name = "killsMummy",
                type = "Int32",
                shortCode = "RX"
            },
            ["newDataMummy"] = new PlayerDataField {
                name = "newDataMummy",
                type = "Boolean",
                shortCode = "RY"
            },
            ["killedOrangeBalloon"] = new PlayerDataField {
                name = "killedOrangeBalloon",
                type = "Boolean",
                shortCode = "RZ"
            },
            ["killsOrangeBalloon"] = new PlayerDataField {
                name = "killsOrangeBalloon",
                type = "Int32",
                shortCode = "S0"
            },
            ["newDataOrangeBalloon"] = new PlayerDataField {
                name = "newDataOrangeBalloon",
                type = "Boolean",
                shortCode = "S1"
            },
            ["killedAbyssTendril"] = new PlayerDataField {
                name = "killedAbyssTendril",
                type = "Boolean",
                shortCode = "S2"
            },
            ["killsAbyssTendril"] = new PlayerDataField {
                name = "killsAbyssTendril",
                type = "Int32",
                shortCode = "S3"
            },
            ["newDataAbyssTendril"] = new PlayerDataField {
                name = "newDataAbyssTendril",
                type = "Boolean",
                shortCode = "S4"
            },
            ["killedHeavyMantis"] = new PlayerDataField {
                name = "killedHeavyMantis",
                type = "Boolean",
                shortCode = "S5"
            },
            ["killsHeavyMantis"] = new PlayerDataField {
                name = "killsHeavyMantis",
                type = "Int32",
                shortCode = "S6"
            },
            ["newDataHeavyMantis"] = new PlayerDataField {
                name = "newDataHeavyMantis",
                type = "Boolean",
                shortCode = "S7"
            },
            ["killedTraitorLord"] = new PlayerDataField {
                name = "killedTraitorLord",
                type = "Boolean",
                shortCode = "S8"
            },
            ["killsTraitorLord"] = new PlayerDataField {
                name = "killsTraitorLord",
                type = "Int32",
                shortCode = "S9"
            },
            ["newDataTraitorLord"] = new PlayerDataField {
                name = "newDataTraitorLord",
                type = "Boolean",
                shortCode = "SA"
            },
            ["killedMantisHeavyFlyer"] = new PlayerDataField {
                name = "killedMantisHeavyFlyer",
                type = "Boolean",
                shortCode = "SB"
            },
            ["killsMantisHeavyFlyer"] = new PlayerDataField {
                name = "killsMantisHeavyFlyer",
                type = "Int32",
                shortCode = "SC"
            },
            ["newDataMantisHeavyFlyer"] = new PlayerDataField {
                name = "newDataMantisHeavyFlyer",
                type = "Boolean",
                shortCode = "SD"
            },
            ["killedGardenZombie"] = new PlayerDataField {
                name = "killedGardenZombie",
                type = "Boolean",
                shortCode = "SE"
            },
            ["killsGardenZombie"] = new PlayerDataField {
                name = "killsGardenZombie",
                type = "Int32",
                shortCode = "SF"
            },
            ["newDataGardenZombie"] = new PlayerDataField {
                name = "newDataGardenZombie",
                type = "Boolean",
                shortCode = "SG"
            },
            ["killedRoyalGuard"] = new PlayerDataField {
                name = "killedRoyalGuard",
                type = "Boolean",
                shortCode = "SH"
            },
            ["killsRoyalGuard"] = new PlayerDataField {
                name = "killsRoyalGuard",
                type = "Int32",
                shortCode = "SI"
            },
            ["newDataRoyalGuard"] = new PlayerDataField {
                name = "newDataRoyalGuard",
                type = "Boolean",
                shortCode = "SJ"
            },
            ["killedWhiteRoyal"] = new PlayerDataField {
                name = "killedWhiteRoyal",
                type = "Boolean",
                shortCode = "SK"
            },
            ["killsWhiteRoyal"] = new PlayerDataField {
                name = "killsWhiteRoyal",
                type = "Int32",
                shortCode = "SL"
            },
            ["newDataWhiteRoyal"] = new PlayerDataField {
                name = "newDataWhiteRoyal",
                type = "Boolean",
                shortCode = "SM"
            },
            ["openedPalaceGrounds"] = new PlayerDataField {
                name = "openedPalaceGrounds",
                type = "Boolean",
                shortCode = "SN"
            },
            ["killedOblobble"] = new PlayerDataField {
                name = "killedOblobble",
                type = "Boolean",
                shortCode = "SO"
            },
            ["killsOblobble"] = new PlayerDataField {
                name = "killsOblobble",
                type = "Int32",
                shortCode = "SP"
            },
            ["newDataOblobble"] = new PlayerDataField {
                name = "newDataOblobble",
                type = "Boolean",
                shortCode = "SQ"
            },
            ["killedZote"] = new PlayerDataField {
                name = "killedZote",
                type = "Boolean",
                shortCode = "SR"
            },
            ["killsZote"] = new PlayerDataField {
                name = "killsZote",
                type = "Int32",
                shortCode = "SS"
            },
            ["newDataZote"] = new PlayerDataField {
                name = "newDataZote",
                type = "Boolean",
                shortCode = "ST"
            },
            ["killedBlobble"] = new PlayerDataField {
                name = "killedBlobble",
                type = "Boolean",
                shortCode = "SU"
            },
            ["killsBlobble"] = new PlayerDataField {
                name = "killsBlobble",
                type = "Int32",
                shortCode = "SV"
            },
            ["newDataBlobble"] = new PlayerDataField {
                name = "newDataBlobble",
                type = "Boolean",
                shortCode = "SW"
            },
            ["killedColMosquito"] = new PlayerDataField {
                name = "killedColMosquito",
                type = "Boolean",
                shortCode = "SX"
            },
            ["killsColMosquito"] = new PlayerDataField {
                name = "killsColMosquito",
                type = "Int32",
                shortCode = "SY"
            },
            ["newDataColMosquito"] = new PlayerDataField {
                name = "newDataColMosquito",
                type = "Boolean",
                shortCode = "SZ"
            },
            ["killedColRoller"] = new PlayerDataField {
                name = "killedColRoller",
                type = "Boolean",
                shortCode = "T0"
            },
            ["killsColRoller"] = new PlayerDataField {
                name = "killsColRoller",
                type = "Int32",
                shortCode = "T1"
            },
            ["newDataColRoller"] = new PlayerDataField {
                name = "newDataColRoller",
                type = "Boolean",
                shortCode = "T2"
            },
            ["killedColFlyingSentry"] = new PlayerDataField {
                name = "killedColFlyingSentry",
                type = "Boolean",
                shortCode = "T3"
            },
            ["killsColFlyingSentry"] = new PlayerDataField {
                name = "killsColFlyingSentry",
                type = "Int32",
                shortCode = "T4"
            },
            ["newDataColFlyingSentry"] = new PlayerDataField {
                name = "newDataColFlyingSentry",
                type = "Boolean",
                shortCode = "T5"
            },
            ["killedColMiner"] = new PlayerDataField {
                name = "killedColMiner",
                type = "Boolean",
                shortCode = "T6"
            },
            ["killsColMiner"] = new PlayerDataField {
                name = "killsColMiner",
                type = "Int32",
                shortCode = "T7"
            },
            ["newDataColMiner"] = new PlayerDataField {
                name = "newDataColMiner",
                type = "Boolean",
                shortCode = "T8"
            },
            ["killedColShield"] = new PlayerDataField {
                name = "killedColShield",
                type = "Boolean",
                shortCode = "T9"
            },
            ["killsColShield"] = new PlayerDataField {
                name = "killsColShield",
                type = "Int32",
                shortCode = "TA"
            },
            ["newDataColShield"] = new PlayerDataField {
                name = "newDataColShield",
                type = "Boolean",
                shortCode = "TB"
            },
            ["killedColWorm"] = new PlayerDataField {
                name = "killedColWorm",
                type = "Boolean",
                shortCode = "TC"
            },
            ["killsColWorm"] = new PlayerDataField {
                name = "killsColWorm",
                type = "Int32",
                shortCode = "TD"
            },
            ["newDataColWorm"] = new PlayerDataField {
                name = "newDataColWorm",
                type = "Boolean",
                shortCode = "TE"
            },
            ["killedColHopper"] = new PlayerDataField {
                name = "killedColHopper",
                type = "Boolean",
                shortCode = "TF"
            },
            ["killsColHopper"] = new PlayerDataField {
                name = "killsColHopper",
                type = "Int32",
                shortCode = "TG"
            },
            ["newDataColHopper"] = new PlayerDataField {
                name = "newDataColHopper",
                type = "Boolean",
                shortCode = "TH"
            },
            ["killedLobsterLancer"] = new PlayerDataField {
                name = "killedLobsterLancer",
                type = "Boolean",
                shortCode = "TI"
            },
            ["killsLobsterLancer"] = new PlayerDataField {
                name = "killsLobsterLancer",
                type = "Int32",
                shortCode = "TJ"
            },
            ["newDataLobsterLancer"] = new PlayerDataField {
                name = "newDataLobsterLancer",
                type = "Boolean",
                shortCode = "TK"
            },
            ["killedGhostAladar"] = new PlayerDataField {
                name = "killedGhostAladar",
                type = "Boolean",
                shortCode = "TL"
            },
            ["killsGhostAladar"] = new PlayerDataField {
                name = "killsGhostAladar",
                type = "Int32",
                shortCode = "TM"
            },
            ["newDataGhostAladar"] = new PlayerDataField {
                name = "newDataGhostAladar",
                type = "Boolean",
                shortCode = "TN"
            },
            ["killedGhostXero"] = new PlayerDataField {
                name = "killedGhostXero",
                type = "Boolean",
                shortCode = "TO"
            },
            ["killsGhostXero"] = new PlayerDataField {
                name = "killsGhostXero",
                type = "Int32",
                shortCode = "TP"
            },
            ["newDataGhostXero"] = new PlayerDataField {
                name = "newDataGhostXero",
                type = "Boolean",
                shortCode = "TQ"
            },
            ["killedGhostHu"] = new PlayerDataField {
                name = "killedGhostHu",
                type = "Boolean",
                shortCode = "TR"
            },
            ["killsGhostHu"] = new PlayerDataField {
                name = "killsGhostHu",
                type = "Int32",
                shortCode = "TS"
            },
            ["newDataGhostHu"] = new PlayerDataField {
                name = "newDataGhostHu",
                type = "Boolean",
                shortCode = "TT"
            },
            ["killedGhostMarmu"] = new PlayerDataField {
                name = "killedGhostMarmu",
                type = "Boolean",
                shortCode = "TU"
            },
            ["killsGhostMarmu"] = new PlayerDataField {
                name = "killsGhostMarmu",
                type = "Int32",
                shortCode = "TV"
            },
            ["newDataGhostMarmu"] = new PlayerDataField {
                name = "newDataGhostMarmu",
                type = "Boolean",
                shortCode = "TW"
            },
            ["killedGhostNoEyes"] = new PlayerDataField {
                name = "killedGhostNoEyes",
                type = "Boolean",
                shortCode = "TX"
            },
            ["killsGhostNoEyes"] = new PlayerDataField {
                name = "killsGhostNoEyes",
                type = "Int32",
                shortCode = "TY"
            },
            ["newDataGhostNoEyes"] = new PlayerDataField {
                name = "newDataGhostNoEyes",
                type = "Boolean",
                shortCode = "TZ"
            },
            ["killedGhostMarkoth"] = new PlayerDataField {
                name = "killedGhostMarkoth",
                type = "Boolean",
                shortCode = "U0"
            },
            ["killsGhostMarkoth"] = new PlayerDataField {
                name = "killsGhostMarkoth",
                type = "Int32",
                shortCode = "U1"
            },
            ["newDataGhostMarkoth"] = new PlayerDataField {
                name = "newDataGhostMarkoth",
                type = "Boolean",
                shortCode = "U2"
            },
            ["killedGhostGalien"] = new PlayerDataField {
                name = "killedGhostGalien",
                type = "Boolean",
                shortCode = "U3"
            },
            ["killsGhostGalien"] = new PlayerDataField {
                name = "killsGhostGalien",
                type = "Int32",
                shortCode = "U4"
            },
            ["newDataGhostGalien"] = new PlayerDataField {
                name = "newDataGhostGalien",
                type = "Boolean",
                shortCode = "U5"
            },
            ["killedWhiteDefender"] = new PlayerDataField {
                name = "killedWhiteDefender",
                type = "Boolean",
                shortCode = "U6"
            },
            ["killsWhiteDefender"] = new PlayerDataField {
                name = "killsWhiteDefender",
                type = "Int32",
                shortCode = "U7"
            },
            ["newDataWhiteDefender"] = new PlayerDataField {
                name = "newDataWhiteDefender",
                type = "Boolean",
                shortCode = "U8"
            },
            ["killedGreyPrince"] = new PlayerDataField {
                name = "killedGreyPrince",
                type = "Boolean",
                shortCode = "U9"
            },
            ["killsGreyPrince"] = new PlayerDataField {
                name = "killsGreyPrince",
                type = "Int32",
                shortCode = "UA"
            },
            ["newDataGreyPrince"] = new PlayerDataField {
                name = "newDataGreyPrince",
                type = "Boolean",
                shortCode = "UB"
            },
            ["killedZotelingBalloon"] = new PlayerDataField {
                name = "killedZotelingBalloon",
                type = "Boolean",
                shortCode = "UC"
            },
            ["killsZotelingBalloon"] = new PlayerDataField {
                name = "killsZotelingBalloon",
                type = "Int32",
                shortCode = "UD"
            },
            ["newDataZotelingBalloon"] = new PlayerDataField {
                name = "newDataZotelingBalloon",
                type = "Boolean",
                shortCode = "UE"
            },
            ["killedZotelingHopper"] = new PlayerDataField {
                name = "killedZotelingHopper",
                type = "Boolean",
                shortCode = "UF"
            },
            ["killsZotelingHopper"] = new PlayerDataField {
                name = "killsZotelingHopper",
                type = "Int32",
                shortCode = "UG"
            },
            ["newDataZotelingHopper"] = new PlayerDataField {
                name = "newDataZotelingHopper",
                type = "Boolean",
                shortCode = "UH"
            },
            ["killedZotelingBuzzer"] = new PlayerDataField {
                name = "killedZotelingBuzzer",
                type = "Boolean",
                shortCode = "UI"
            },
            ["killsZotelingBuzzer"] = new PlayerDataField {
                name = "killsZotelingBuzzer",
                type = "Int32",
                shortCode = "UJ"
            },
            ["newDataZotelingBuzzer"] = new PlayerDataField {
                name = "newDataZotelingBuzzer",
                type = "Boolean",
                shortCode = "UK"
            },
            ["killedHollowKnight"] = new PlayerDataField {
                name = "killedHollowKnight",
                type = "Boolean",
                shortCode = "UL"
            },
            ["killsHollowKnight"] = new PlayerDataField {
                name = "killsHollowKnight",
                type = "Int32",
                shortCode = "UM"
            },
            ["newDataHollowKnight"] = new PlayerDataField {
                name = "newDataHollowKnight",
                type = "Boolean",
                shortCode = "UN"
            },
            ["killedFinalBoss"] = new PlayerDataField {
                name = "killedFinalBoss",
                type = "Boolean",
                shortCode = "UO"
            },
            ["killsFinalBoss"] = new PlayerDataField {
                name = "killsFinalBoss",
                type = "Int32",
                shortCode = "UP"
            },
            ["newDataFinalBoss"] = new PlayerDataField {
                name = "newDataFinalBoss",
                type = "Boolean",
                shortCode = "UQ"
            },
            ["killedHunterMark"] = new PlayerDataField {
                name = "killedHunterMark",
                type = "Boolean",
                shortCode = "UR"
            },
            ["killsHunterMark"] = new PlayerDataField {
                name = "killsHunterMark",
                type = "Int32",
                shortCode = "US"
            },
            ["newDataHunterMark"] = new PlayerDataField {
                name = "newDataHunterMark",
                type = "Boolean",
                shortCode = "UT"
            },
            ["killedFlameBearerSmall"] = new PlayerDataField {
                name = "killedFlameBearerSmall",
                type = "Boolean",
                shortCode = "UU"
            },
            ["killsFlameBearerSmall"] = new PlayerDataField {
                name = "killsFlameBearerSmall",
                type = "Int32",
                shortCode = "UV"
            },
            ["newDataFlameBearerSmall"] = new PlayerDataField {
                name = "newDataFlameBearerSmall",
                type = "Boolean",
                shortCode = "UW"
            },
            ["killedFlameBearerMed"] = new PlayerDataField {
                name = "killedFlameBearerMed",
                type = "Boolean",
                shortCode = "UX"
            },
            ["killsFlameBearerMed"] = new PlayerDataField {
                name = "killsFlameBearerMed",
                type = "Int32",
                shortCode = "UY"
            },
            ["newDataFlameBearerMed"] = new PlayerDataField {
                name = "newDataFlameBearerMed",
                type = "Boolean",
                shortCode = "UZ"
            },
            ["killedFlameBearerLarge"] = new PlayerDataField {
                name = "killedFlameBearerLarge",
                type = "Boolean",
                shortCode = "V0"
            },
            ["killsFlameBearerLarge"] = new PlayerDataField {
                name = "killsFlameBearerLarge",
                type = "Int32",
                shortCode = "V1"
            },
            ["newDataFlameBearerLarge"] = new PlayerDataField {
                name = "newDataFlameBearerLarge",
                type = "Boolean",
                shortCode = "V2"
            },
            ["killedGrimm"] = new PlayerDataField {
                name = "killedGrimm",
                type = "Boolean",
                shortCode = "V3"
            },
            ["killsGrimm"] = new PlayerDataField {
                name = "killsGrimm",
                type = "Int32",
                shortCode = "V4"
            },
            ["newDataGrimm"] = new PlayerDataField {
                name = "newDataGrimm",
                type = "Boolean",
                shortCode = "V5"
            },
            ["killedNightmareGrimm"] = new PlayerDataField {
                name = "killedNightmareGrimm",
                type = "Boolean",
                shortCode = "V6"
            },
            ["killsNightmareGrimm"] = new PlayerDataField {
                name = "killsNightmareGrimm",
                type = "Int32",
                shortCode = "V7"
            },
            ["newDataNightmareGrimm"] = new PlayerDataField {
                name = "newDataNightmareGrimm",
                type = "Boolean",
                shortCode = "V8"
            },
            ["killedBindingSeal"] = new PlayerDataField {
                name = "killedBindingSeal",
                type = "Boolean",
                shortCode = "V9"
            },
            ["killsBindingSeal"] = new PlayerDataField {
                name = "killsBindingSeal",
                type = "Int32",
                shortCode = "VA"
            },
            ["newDataBindingSeal"] = new PlayerDataField {
                name = "newDataBindingSeal",
                type = "Boolean",
                shortCode = "VB"
            },
            ["killedFatFluke"] = new PlayerDataField {
                name = "killedFatFluke",
                type = "Boolean",
                shortCode = "VC"
            },
            ["killsFatFluke"] = new PlayerDataField {
                name = "killsFatFluke",
                type = "Int32",
                shortCode = "VD"
            },
            ["newDataFatFluke"] = new PlayerDataField {
                name = "newDataFatFluke",
                type = "Boolean",
                shortCode = "VE"
            },
            ["killedPaleLurker"] = new PlayerDataField {
                name = "killedPaleLurker",
                type = "Boolean",
                shortCode = "VF"
            },
            ["killsPaleLurker"] = new PlayerDataField {
                name = "killsPaleLurker",
                type = "Int32",
                shortCode = "VG"
            },
            ["newDataPaleLurker"] = new PlayerDataField {
                name = "newDataPaleLurker",
                type = "Boolean",
                shortCode = "VH"
            },
            ["killedNailBros"] = new PlayerDataField {
                name = "killedNailBros",
                type = "Boolean",
                shortCode = "VI"
            },
            ["killsNailBros"] = new PlayerDataField {
                name = "killsNailBros",
                type = "Int32",
                shortCode = "VJ"
            },
            ["newDataNailBros"] = new PlayerDataField {
                name = "newDataNailBros",
                type = "Boolean",
                shortCode = "VK"
            },
            ["killedPaintmaster"] = new PlayerDataField {
                name = "killedPaintmaster",
                type = "Boolean",
                shortCode = "VL"
            },
            ["killsPaintmaster"] = new PlayerDataField {
                name = "killsPaintmaster",
                type = "Int32",
                shortCode = "VM"
            },
            ["newDataPaintmaster"] = new PlayerDataField {
                name = "newDataPaintmaster",
                type = "Boolean",
                shortCode = "VN"
            },
            ["killedNailsage"] = new PlayerDataField {
                name = "killedNailsage",
                type = "Boolean",
                shortCode = "VO"
            },
            ["killsNailsage"] = new PlayerDataField {
                name = "killsNailsage",
                type = "Int32",
                shortCode = "VP"
            },
            ["newDataNailsage"] = new PlayerDataField {
                name = "newDataNailsage",
                type = "Boolean",
                shortCode = "VQ"
            },
            ["killedHollowKnightPrime"] = new PlayerDataField {
                name = "killedHollowKnightPrime",
                type = "Boolean",
                shortCode = "VR"
            },
            ["killsHollowKnightPrime"] = new PlayerDataField {
                name = "killsHollowKnightPrime",
                type = "Int32",
                shortCode = "VS"
            },
            ["newDataHollowKnightPrime"] = new PlayerDataField {
                name = "newDataHollowKnightPrime",
                type = "Boolean",
                shortCode = "VT"
            },
            ["killedGodseekerMask"] = new PlayerDataField {
                name = "killedGodseekerMask",
                type = "Boolean",
                shortCode = "VU"
            },
            ["killsGodseekerMask"] = new PlayerDataField {
                name = "killsGodseekerMask",
                type = "Int32",
                shortCode = "VV"
            },
            ["newDataGodseekerMask"] = new PlayerDataField {
                name = "newDataGodseekerMask",
                type = "Boolean",
                shortCode = "VW"
            },
            ["killedVoidIdol_1"] = new PlayerDataField {
                name = "killedVoidIdol_1",
                type = "Boolean",
                shortCode = "VX"
            },
            ["killsVoidIdol_1"] = new PlayerDataField {
                name = "killsVoidIdol_1",
                type = "Int32",
                shortCode = "VY"
            },
            ["newDataVoidIdol_1"] = new PlayerDataField {
                name = "newDataVoidIdol_1",
                type = "Boolean",
                shortCode = "VZ"
            },
            ["killedVoidIdol_2"] = new PlayerDataField {
                name = "killedVoidIdol_2",
                type = "Boolean",
                shortCode = "W0"
            },
            ["killsVoidIdol_2"] = new PlayerDataField {
                name = "killsVoidIdol_2",
                type = "Int32",
                shortCode = "W1"
            },
            ["newDataVoidIdol_2"] = new PlayerDataField {
                name = "newDataVoidIdol_2",
                type = "Boolean",
                shortCode = "W2"
            },
            ["killedVoidIdol_3"] = new PlayerDataField {
                name = "killedVoidIdol_3",
                type = "Boolean",
                shortCode = "W3"
            },
            ["killsVoidIdol_3"] = new PlayerDataField {
                name = "killsVoidIdol_3",
                type = "Int32",
                shortCode = "W4"
            },
            ["newDataVoidIdol_3"] = new PlayerDataField {
                name = "newDataVoidIdol_3",
                type = "Boolean",
                shortCode = "W5"
            },
            ["grubsCollected"] = new PlayerDataField {
                name = "grubsCollected",
                type = "Int32",
                shortCode = "W6"
            },
            ["grubRewards"] = new PlayerDataField {
                name = "grubRewards",
                type = "Int32",
                shortCode = "W7"
            },
            ["finalGrubRewardCollected"] = new PlayerDataField {
                name = "finalGrubRewardCollected",
                type = "Boolean",
                shortCode = "W8"
            },
            ["fatGrubKing"] = new PlayerDataField {
                name = "fatGrubKing",
                type = "Boolean",
                shortCode = "W9"
            },
            ["falseKnightDefeated"] = new PlayerDataField {
                name = "falseKnightDefeated",
                type = "Boolean",
                shortCode = "WA"
            },
            ["falseKnightDreamDefeated"] = new PlayerDataField {
                name = "falseKnightDreamDefeated",
                type = "Boolean",
                shortCode = "WB"
            },
            ["falseKnightOrbsCollected"] = new PlayerDataField {
                name = "falseKnightOrbsCollected",
                type = "Boolean",
                shortCode = "WC"
            },
            ["mawlekDefeated"] = new PlayerDataField {
                name = "mawlekDefeated",
                type = "Boolean",
                shortCode = "WD"
            },
            ["giantBuzzerDefeated"] = new PlayerDataField {
                name = "giantBuzzerDefeated",
                type = "Boolean",
                shortCode = "WE"
            },
            ["giantFlyDefeated"] = new PlayerDataField {
                name = "giantFlyDefeated",
                type = "Boolean",
                shortCode = "WF"
            },
            ["blocker1Defeated"] = new PlayerDataField {
                name = "blocker1Defeated",
                type = "Boolean",
                shortCode = "WG"
            },
            ["blocker2Defeated"] = new PlayerDataField {
                name = "blocker2Defeated",
                type = "Boolean",
                shortCode = "WH"
            },
            ["hornet1Defeated"] = new PlayerDataField {
                name = "hornet1Defeated",
                type = "Boolean",
                shortCode = "WI"
            },
            ["collectorDefeated"] = new PlayerDataField {
                name = "collectorDefeated",
                type = "Boolean",
                shortCode = "WJ"
            },
            ["hornetOutskirtsDefeated"] = new PlayerDataField {
                name = "hornetOutskirtsDefeated",
                type = "Boolean",
                shortCode = "WK"
            },
            ["mageLordDreamDefeated"] = new PlayerDataField {
                name = "mageLordDreamDefeated",
                type = "Boolean",
                shortCode = "WL"
            },
            ["mageLordOrbsCollected"] = new PlayerDataField {
                name = "mageLordOrbsCollected",
                type = "Boolean",
                shortCode = "WM"
            },
            ["infectedKnightDreamDefeated"] = new PlayerDataField {
                name = "infectedKnightDreamDefeated",
                type = "Boolean",
                shortCode = "WN"
            },
            ["infectedKnightOrbsCollected"] = new PlayerDataField {
                name = "infectedKnightOrbsCollected",
                type = "Boolean",
                shortCode = "WO"
            },
            ["whiteDefenderDefeated"] = new PlayerDataField {
                name = "whiteDefenderDefeated",
                type = "Boolean",
                shortCode = "WP"
            },
            ["whiteDefenderOrbsCollected"] = new PlayerDataField {
                name = "whiteDefenderOrbsCollected",
                type = "Boolean",
                shortCode = "WQ"
            },
            ["whiteDefenderDefeats"] = new PlayerDataField {
                name = "whiteDefenderDefeats",
                type = "Int32",
                shortCode = "WR"
            },
            ["greyPrinceDefeats"] = new PlayerDataField {
                name = "greyPrinceDefeats",
                type = "Int32",
                shortCode = "WS"
            },
            ["greyPrinceDefeated"] = new PlayerDataField {
                name = "greyPrinceDefeated",
                type = "Boolean",
                shortCode = "WT"
            },
            ["greyPrinceOrbsCollected"] = new PlayerDataField {
                name = "greyPrinceOrbsCollected",
                type = "Boolean",
                shortCode = "WU"
            },
            ["aladarSlugDefeated"] = new PlayerDataField {
                name = "aladarSlugDefeated",
                type = "Int32",
                shortCode = "WV"
            },
            ["xeroDefeated"] = new PlayerDataField {
                name = "xeroDefeated",
                type = "Int32",
                shortCode = "WW"
            },
            ["elderHuDefeated"] = new PlayerDataField {
                name = "elderHuDefeated",
                type = "Int32",
                shortCode = "WX"
            },
            ["mumCaterpillarDefeated"] = new PlayerDataField {
                name = "mumCaterpillarDefeated",
                type = "Int32",
                shortCode = "WY"
            },
            ["noEyesDefeated"] = new PlayerDataField {
                name = "noEyesDefeated",
                type = "Int32",
                shortCode = "WZ"
            },
            ["markothDefeated"] = new PlayerDataField {
                name = "markothDefeated",
                type = "Int32",
                shortCode = "X0"
            },
            ["galienDefeated"] = new PlayerDataField {
                name = "galienDefeated",
                type = "Int32",
                shortCode = "X1"
            },
            ["XERO_encountered"] = new PlayerDataField {
                name = "XERO_encountered",
                type = "Boolean",
                shortCode = "X2"
            },
            ["ALADAR_encountered"] = new PlayerDataField {
                name = "ALADAR_encountered",
                type = "Boolean",
                shortCode = "X3"
            },
            ["HU_encountered"] = new PlayerDataField {
                name = "HU_encountered",
                type = "Boolean",
                shortCode = "X4"
            },
            ["MUMCAT_encountered"] = new PlayerDataField {
                name = "MUMCAT_encountered",
                type = "Boolean",
                shortCode = "X5"
            },
            ["NOEYES_encountered"] = new PlayerDataField {
                name = "NOEYES_encountered",
                type = "Boolean",
                shortCode = "X6"
            },
            ["MARKOTH_encountered"] = new PlayerDataField {
                name = "MARKOTH_encountered",
                type = "Boolean",
                shortCode = "X7"
            },
            ["GALIEN_encountered"] = new PlayerDataField {
                name = "GALIEN_encountered",
                type = "Boolean",
                shortCode = "X8"
            },
            ["xeroPinned"] = new PlayerDataField {
                name = "xeroPinned",
                type = "Boolean",
                shortCode = "X9"
            },
            ["aladarPinned"] = new PlayerDataField {
                name = "aladarPinned",
                type = "Boolean",
                shortCode = "XA"
            },
            ["huPinned"] = new PlayerDataField {
                name = "huPinned",
                type = "Boolean",
                shortCode = "XB"
            },
            ["mumCaterpillarPinned"] = new PlayerDataField {
                name = "mumCaterpillarPinned",
                type = "Boolean",
                shortCode = "XC"
            },
            ["noEyesPinned"] = new PlayerDataField {
                name = "noEyesPinned",
                type = "Boolean",
                shortCode = "XD"
            },
            ["markothPinned"] = new PlayerDataField {
                name = "markothPinned",
                type = "Boolean",
                shortCode = "XE"
            },
            ["galienPinned"] = new PlayerDataField {
                name = "galienPinned",
                type = "Boolean",
                shortCode = "XF"
            },
            ["currentInvPane"] = new PlayerDataField {
                name = "currentInvPane",
                type = "Int32",
                shortCode = "XG"
            },
            ["showGeoUI"] = new PlayerDataField {
                name = "showGeoUI",
                type = "Boolean",
                shortCode = "XH"
            },
            ["showHealthUI"] = new PlayerDataField {
                name = "showHealthUI",
                type = "Boolean",
                shortCode = "XI"
            },
            ["promptFocus"] = new PlayerDataField {
                name = "promptFocus",
                type = "Boolean",
                shortCode = "XJ"
            },
            ["seenFocusTablet"] = new PlayerDataField {
                name = "seenFocusTablet",
                type = "Boolean",
                shortCode = "XK"
            },
            ["seenDreamNailPrompt"] = new PlayerDataField {
                name = "seenDreamNailPrompt",
                type = "Boolean",
                shortCode = "XL"
            },
            ["isFirstGame"] = new PlayerDataField {
                name = "isFirstGame",
                type = "Boolean",
                shortCode = "XM"
            },
            ["enteredTutorialFirstTime"] = new PlayerDataField {
                name = "enteredTutorialFirstTime",
                type = "Boolean",
                shortCode = "XN"
            },
            ["isInvincible"] = new PlayerDataField {
                name = "isInvincible",
                type = "Boolean",
                shortCode = "XO"
            },
            ["infiniteAirJump"] = new PlayerDataField {
                name = "infiniteAirJump",
                type = "Boolean",
                shortCode = "XP"
            },
            ["invinciTest"] = new PlayerDataField {
                name = "invinciTest",
                type = "Boolean",
                shortCode = "XQ"
            },
            ["currentArea"] = new PlayerDataField {
                name = "currentArea",
                type = "Int32",
                shortCode = "XR"
            },
            ["visitedDirtmouth"] = new PlayerDataField {
                name = "visitedDirtmouth",
                type = "Boolean",
                shortCode = "XS"
            },
            ["visitedCrossroads"] = new PlayerDataField {
                name = "visitedCrossroads",
                type = "Boolean",
                shortCode = "XT"
            },
            ["visitedGreenpath"] = new PlayerDataField {
                name = "visitedGreenpath",
                type = "Boolean",
                shortCode = "XU"
            },
            ["visitedFungus"] = new PlayerDataField {
                name = "visitedFungus",
                type = "Boolean",
                shortCode = "XV"
            },
            ["visitedHive"] = new PlayerDataField {
                name = "visitedHive",
                type = "Boolean",
                shortCode = "XW"
            },
            ["visitedCrossroadsInfected"] = new PlayerDataField {
                name = "visitedCrossroadsInfected",
                type = "Boolean",
                shortCode = "XX"
            },
            ["visitedRuins"] = new PlayerDataField {
                name = "visitedRuins",
                type = "Boolean",
                shortCode = "XY"
            },
            ["visitedMines"] = new PlayerDataField {
                name = "visitedMines",
                type = "Boolean",
                shortCode = "XZ"
            },
            ["visitedRoyalGardens"] = new PlayerDataField {
                name = "visitedRoyalGardens",
                type = "Boolean",
                shortCode = "Y0"
            },
            ["visitedFogCanyon"] = new PlayerDataField {
                name = "visitedFogCanyon",
                type = "Boolean",
                shortCode = "Y1"
            },
            ["visitedDeepnest"] = new PlayerDataField {
                name = "visitedDeepnest",
                type = "Boolean",
                shortCode = "Y2"
            },
            ["visitedRestingGrounds"] = new PlayerDataField {
                name = "visitedRestingGrounds",
                type = "Boolean",
                shortCode = "Y3"
            },
            ["visitedWaterways"] = new PlayerDataField {
                name = "visitedWaterways",
                type = "Boolean",
                shortCode = "Y4"
            },
            ["visitedAbyss"] = new PlayerDataField {
                name = "visitedAbyss",
                type = "Boolean",
                shortCode = "Y5"
            },
            ["visitedOutskirts"] = new PlayerDataField {
                name = "visitedOutskirts",
                type = "Boolean",
                shortCode = "Y6"
            },
            ["visitedWhitePalace"] = new PlayerDataField {
                name = "visitedWhitePalace",
                type = "Boolean",
                shortCode = "Y7"
            },
            ["visitedCliffs"] = new PlayerDataField {
                name = "visitedCliffs",
                type = "Boolean",
                shortCode = "Y8"
            },
            ["visitedAbyssLower"] = new PlayerDataField {
                name = "visitedAbyssLower",
                type = "Boolean",
                shortCode = "Y9"
            },
            ["visitedGodhome"] = new PlayerDataField {
                name = "visitedGodhome",
                type = "Boolean",
                shortCode = "YA"
            },
            ["visitedMines10"] = new PlayerDataField {
                name = "visitedMines10",
                type = "Boolean",
                shortCode = "YB"
            },
            ["scenesVisited"] = new PlayerDataField {
                name = "scenesVisited",
                type = "List`1",
                shortCode = "YC"
            },
            ["scenesMapped"] = new PlayerDataField {
                name = "scenesMapped",
                type = "List`1",
                shortCode = "YD"
            },
            ["scenesEncounteredBench"] = new PlayerDataField {
                name = "scenesEncounteredBench",
                type = "List`1",
                shortCode = "YE"
            },
            ["scenesGrubRescued"] = new PlayerDataField {
                name = "scenesGrubRescued",
                type = "List`1",
                shortCode = "YF"
            },
            ["scenesFlameCollected"] = new PlayerDataField {
                name = "scenesFlameCollected",
                type = "List`1",
                shortCode = "YG"
            },
            ["scenesEncounteredCocoon"] = new PlayerDataField {
                name = "scenesEncounteredCocoon",
                type = "List`1",
                shortCode = "YH"
            },
            ["scenesEncounteredDreamPlant"] = new PlayerDataField {
                name = "scenesEncounteredDreamPlant",
                type = "List`1",
                shortCode = "YI"
            },
            ["scenesEncounteredDreamPlantC"] = new PlayerDataField {
                name = "scenesEncounteredDreamPlantC",
                type = "List`1",
                shortCode = "YJ"
            },
            ["hasMap"] = new PlayerDataField {
                name = "hasMap",
                type = "Boolean",
                shortCode = "YK"
            },
            ["mapAllRooms"] = new PlayerDataField {
                name = "mapAllRooms",
                type = "Boolean",
                shortCode = "YL"
            },
            ["atMapPrompt"] = new PlayerDataField {
                name = "atMapPrompt",
                type = "Boolean",
                shortCode = "YM"
            },
            ["mapDirtmouth"] = new PlayerDataField {
                name = "mapDirtmouth",
                type = "Boolean",
                shortCode = "YN"
            },
            ["mapCrossroads"] = new PlayerDataField {
                name = "mapCrossroads",
                type = "Boolean",
                shortCode = "YO"
            },
            ["mapGreenpath"] = new PlayerDataField {
                name = "mapGreenpath",
                type = "Boolean",
                shortCode = "YP"
            },
            ["mapFogCanyon"] = new PlayerDataField {
                name = "mapFogCanyon",
                type = "Boolean",
                shortCode = "YQ"
            },
            ["mapRoyalGardens"] = new PlayerDataField {
                name = "mapRoyalGardens",
                type = "Boolean",
                shortCode = "YR"
            },
            ["mapFungalWastes"] = new PlayerDataField {
                name = "mapFungalWastes",
                type = "Boolean",
                shortCode = "YS"
            },
            ["mapCity"] = new PlayerDataField {
                name = "mapCity",
                type = "Boolean",
                shortCode = "YT"
            },
            ["mapWaterways"] = new PlayerDataField {
                name = "mapWaterways",
                type = "Boolean",
                shortCode = "YU"
            },
            ["mapMines"] = new PlayerDataField {
                name = "mapMines",
                type = "Boolean",
                shortCode = "YV"
            },
            ["mapDeepnest"] = new PlayerDataField {
                name = "mapDeepnest",
                type = "Boolean",
                shortCode = "YW"
            },
            ["mapCliffs"] = new PlayerDataField {
                name = "mapCliffs",
                type = "Boolean",
                shortCode = "YX"
            },
            ["mapOutskirts"] = new PlayerDataField {
                name = "mapOutskirts",
                type = "Boolean",
                shortCode = "YY"
            },
            ["mapRestingGrounds"] = new PlayerDataField {
                name = "mapRestingGrounds",
                type = "Boolean",
                shortCode = "YZ"
            },
            ["mapAbyss"] = new PlayerDataField {
                name = "mapAbyss",
                type = "Boolean",
                shortCode = "Z0"
            },
            ["mapZoneBools"] = new PlayerDataField {
                name = "mapZoneBools",
                type = "Dictionary`2",
                shortCode = "Z1"
            },
            ["hasPin"] = new PlayerDataField {
                name = "hasPin",
                type = "Boolean",
                shortCode = "Z2"
            },
            ["hasPinBench"] = new PlayerDataField {
                name = "hasPinBench",
                type = "Boolean",
                shortCode = "Z3"
            },
            ["hasPinCocoon"] = new PlayerDataField {
                name = "hasPinCocoon",
                type = "Boolean",
                shortCode = "Z4"
            },
            ["hasPinDreamPlant"] = new PlayerDataField {
                name = "hasPinDreamPlant",
                type = "Boolean",
                shortCode = "Z5"
            },
            ["hasPinGuardian"] = new PlayerDataField {
                name = "hasPinGuardian",
                type = "Boolean",
                shortCode = "Z6"
            },
            ["hasPinBlackEgg"] = new PlayerDataField {
                name = "hasPinBlackEgg",
                type = "Boolean",
                shortCode = "Z7"
            },
            ["hasPinShop"] = new PlayerDataField {
                name = "hasPinShop",
                type = "Boolean",
                shortCode = "Z8"
            },
            ["hasPinSpa"] = new PlayerDataField {
                name = "hasPinSpa",
                type = "Boolean",
                shortCode = "Z9"
            },
            ["hasPinStag"] = new PlayerDataField {
                name = "hasPinStag",
                type = "Boolean",
                shortCode = "ZA"
            },
            ["hasPinTram"] = new PlayerDataField {
                name = "hasPinTram",
                type = "Boolean",
                shortCode = "ZB"
            },
            ["hasPinGhost"] = new PlayerDataField {
                name = "hasPinGhost",
                type = "Boolean",
                shortCode = "ZC"
            },
            ["hasPinGrub"] = new PlayerDataField {
                name = "hasPinGrub",
                type = "Boolean",
                shortCode = "ZD"
            },
            ["hasMarker"] = new PlayerDataField {
                name = "hasMarker",
                type = "Boolean",
                shortCode = "ZE"
            },
            ["hasMarker_r"] = new PlayerDataField {
                name = "hasMarker_r",
                type = "Boolean",
                shortCode = "ZF"
            },
            ["hasMarker_b"] = new PlayerDataField {
                name = "hasMarker_b",
                type = "Boolean",
                shortCode = "ZG"
            },
            ["hasMarker_y"] = new PlayerDataField {
                name = "hasMarker_y",
                type = "Boolean",
                shortCode = "ZH"
            },
            ["hasMarker_w"] = new PlayerDataField {
                name = "hasMarker_w",
                type = "Boolean",
                shortCode = "ZI"
            },
            ["spareMarkers_r"] = new PlayerDataField {
                name = "spareMarkers_r",
                type = "Int32",
                shortCode = "ZJ"
            },
            ["spareMarkers_b"] = new PlayerDataField {
                name = "spareMarkers_b",
                type = "Int32",
                shortCode = "ZK"
            },
            ["spareMarkers_y"] = new PlayerDataField {
                name = "spareMarkers_y",
                type = "Int32",
                shortCode = "ZL"
            },
            ["spareMarkers_w"] = new PlayerDataField {
                name = "spareMarkers_w",
                type = "Int32",
                shortCode = "ZM"
            },
            ["placedMarkers_r"] = new PlayerDataField {
                name = "placedMarkers_r",
                type = "List`1",
                shortCode = "ZN"
            },
            ["placedMarkers_b"] = new PlayerDataField {
                name = "placedMarkers_b",
                type = "List`1",
                shortCode = "ZO"
            },
            ["placedMarkers_y"] = new PlayerDataField {
                name = "placedMarkers_y",
                type = "List`1",
                shortCode = "ZP"
            },
            ["placedMarkers_w"] = new PlayerDataField {
                name = "placedMarkers_w",
                type = "List`1",
                shortCode = "ZQ"
            },
            ["environmentType"] = new PlayerDataField {
                name = "environmentType",
                type = "Int32",
                shortCode = "ZR"
            },
            ["environmentTypeDefault"] = new PlayerDataField {
                name = "environmentTypeDefault",
                type = "Int32",
                shortCode = "ZS"
            },
            ["previousDarkness"] = new PlayerDataField {
                name = "previousDarkness",
                type = "Int32",
                shortCode = "ZT"
            },
            ["openedTramLower"] = new PlayerDataField {
                name = "openedTramLower",
                type = "Boolean",
                shortCode = "ZU"
            },
            ["openedTramRestingGrounds"] = new PlayerDataField {
                name = "openedTramRestingGrounds",
                type = "Boolean",
                shortCode = "ZV"
            },
            ["tramLowerPosition"] = new PlayerDataField {
                name = "tramLowerPosition",
                type = "Int32",
                shortCode = "ZW"
            },
            ["tramRestingGroundsPosition"] = new PlayerDataField {
                name = "tramRestingGroundsPosition",
                type = "Int32",
                shortCode = "ZX"
            },
            ["mineLiftOpened"] = new PlayerDataField {
                name = "mineLiftOpened",
                type = "Boolean",
                shortCode = "ZY"
            },
            ["menderDoorOpened"] = new PlayerDataField {
                name = "menderDoorOpened",
                type = "Boolean",
                shortCode = "ZZ"
            },
            ["vesselFragStagNest"] = new PlayerDataField {
                name = "vesselFragStagNest",
                type = "Boolean",
                shortCode = "100"
            },
            ["shamanPillar"] = new PlayerDataField {
                name = "shamanPillar",
                type = "Boolean",
                shortCode = "101"
            },
            ["crossroadsMawlekWall"] = new PlayerDataField {
                name = "crossroadsMawlekWall",
                type = "Boolean",
                shortCode = "102"
            },
            ["eggTempleVisited"] = new PlayerDataField {
                name = "eggTempleVisited",
                type = "Boolean",
                shortCode = "103"
            },
            ["crossroadsInfected"] = new PlayerDataField {
                name = "crossroadsInfected",
                type = "Boolean",
                shortCode = "104"
            },
            ["falseKnightFirstPlop"] = new PlayerDataField {
                name = "falseKnightFirstPlop",
                type = "Boolean",
                shortCode = "105"
            },
            ["falseKnightWallRepaired"] = new PlayerDataField {
                name = "falseKnightWallRepaired",
                type = "Boolean",
                shortCode = "106"
            },
            ["falseKnightWallBroken"] = new PlayerDataField {
                name = "falseKnightWallBroken",
                type = "Boolean",
                shortCode = "107"
            },
            ["falseKnightGhostDeparted"] = new PlayerDataField {
                name = "falseKnightGhostDeparted",
                type = "Boolean",
                shortCode = "108"
            },
            ["spaBugsEncountered"] = new PlayerDataField {
                name = "spaBugsEncountered",
                type = "Boolean",
                shortCode = "109"
            },
            ["hornheadVinePlat"] = new PlayerDataField {
                name = "hornheadVinePlat",
                type = "Boolean",
                shortCode = "10A"
            },
            ["infectedKnightEncountered"] = new PlayerDataField {
                name = "infectedKnightEncountered",
                type = "Boolean",
                shortCode = "10B"
            },
            ["megaMossChargerEncountered"] = new PlayerDataField {
                name = "megaMossChargerEncountered",
                type = "Boolean",
                shortCode = "10C"
            },
            ["megaMossChargerDefeated"] = new PlayerDataField {
                name = "megaMossChargerDefeated",
                type = "Boolean",
                shortCode = "10D"
            },
            ["dreamerScene1"] = new PlayerDataField {
                name = "dreamerScene1",
                type = "Boolean",
                shortCode = "10E"
            },
            ["slugEncounterComplete"] = new PlayerDataField {
                name = "slugEncounterComplete",
                type = "Boolean",
                shortCode = "10F"
            },
            ["defeatedDoubleBlockers"] = new PlayerDataField {
                name = "defeatedDoubleBlockers",
                type = "Boolean",
                shortCode = "10G"
            },
            ["oneWayArchive"] = new PlayerDataField {
                name = "oneWayArchive",
                type = "Boolean",
                shortCode = "10H"
            },
            ["defeatedMegaJelly"] = new PlayerDataField {
                name = "defeatedMegaJelly",
                type = "Boolean",
                shortCode = "10I"
            },
            ["summonedMonomon"] = new PlayerDataField {
                name = "summonedMonomon",
                type = "Boolean",
                shortCode = "10J"
            },
            ["sawWoundedQuirrel"] = new PlayerDataField {
                name = "sawWoundedQuirrel",
                type = "Boolean",
                shortCode = "10K"
            },
            ["encounteredMegaJelly"] = new PlayerDataField {
                name = "encounteredMegaJelly",
                type = "Boolean",
                shortCode = "10L"
            },
            ["defeatedMantisLords"] = new PlayerDataField {
                name = "defeatedMantisLords",
                type = "Boolean",
                shortCode = "10M"
            },
            ["encounteredGatekeeper"] = new PlayerDataField {
                name = "encounteredGatekeeper",
                type = "Boolean",
                shortCode = "10N"
            },
            ["deepnestWall"] = new PlayerDataField {
                name = "deepnestWall",
                type = "Boolean",
                shortCode = "10O"
            },
            ["queensStationNonDisplay"] = new PlayerDataField {
                name = "queensStationNonDisplay",
                type = "Boolean",
                shortCode = "10P"
            },
            ["cityBridge1"] = new PlayerDataField {
                name = "cityBridge1",
                type = "Boolean",
                shortCode = "10Q"
            },
            ["cityBridge2"] = new PlayerDataField {
                name = "cityBridge2",
                type = "Boolean",
                shortCode = "10R"
            },
            ["cityLift1"] = new PlayerDataField {
                name = "cityLift1",
                type = "Boolean",
                shortCode = "10S"
            },
            ["cityLift1_isUp"] = new PlayerDataField {
                name = "cityLift1_isUp",
                type = "Boolean",
                shortCode = "10T"
            },
            ["liftArrival"] = new PlayerDataField {
                name = "liftArrival",
                type = "Boolean",
                shortCode = "10U"
            },
            ["openedMageDoor"] = new PlayerDataField {
                name = "openedMageDoor",
                type = "Boolean",
                shortCode = "10V"
            },
            ["openedMageDoor_v2"] = new PlayerDataField {
                name = "openedMageDoor_v2",
                type = "Boolean",
                shortCode = "10W"
            },
            ["brokenMageWindow"] = new PlayerDataField {
                name = "brokenMageWindow",
                type = "Boolean",
                shortCode = "10X"
            },
            ["brokenMageWindowGlass"] = new PlayerDataField {
                name = "brokenMageWindowGlass",
                type = "Boolean",
                shortCode = "10Y"
            },
            ["mageLordEncountered"] = new PlayerDataField {
                name = "mageLordEncountered",
                type = "Boolean",
                shortCode = "10Z"
            },
            ["mageLordEncountered_2"] = new PlayerDataField {
                name = "mageLordEncountered_2",
                type = "Boolean",
                shortCode = "110"
            },
            ["mageLordDefeated"] = new PlayerDataField {
                name = "mageLordDefeated",
                type = "Boolean",
                shortCode = "111"
            },
            ["ruins1_5_tripleDoor"] = new PlayerDataField {
                name = "ruins1_5_tripleDoor",
                type = "Boolean",
                shortCode = "112"
            },
            ["openedCityGate"] = new PlayerDataField {
                name = "openedCityGate",
                type = "Boolean",
                shortCode = "113"
            },
            ["cityGateClosed"] = new PlayerDataField {
                name = "cityGateClosed",
                type = "Boolean",
                shortCode = "114"
            },
            ["bathHouseOpened"] = new PlayerDataField {
                name = "bathHouseOpened",
                type = "Boolean",
                shortCode = "115"
            },
            ["bathHouseWall"] = new PlayerDataField {
                name = "bathHouseWall",
                type = "Boolean",
                shortCode = "116"
            },
            ["cityLift2"] = new PlayerDataField {
                name = "cityLift2",
                type = "Boolean",
                shortCode = "117"
            },
            ["cityLift2_isUp"] = new PlayerDataField {
                name = "cityLift2_isUp",
                type = "Boolean",
                shortCode = "118"
            },
            ["city2_sewerDoor"] = new PlayerDataField {
                name = "city2_sewerDoor",
                type = "Boolean",
                shortCode = "119"
            },
            ["openedLoveDoor"] = new PlayerDataField {
                name = "openedLoveDoor",
                type = "Boolean",
                shortCode = "11A"
            },
            ["watcherChandelier"] = new PlayerDataField {
                name = "watcherChandelier",
                type = "Boolean",
                shortCode = "11B"
            },
            ["completedQuakeArea"] = new PlayerDataField {
                name = "completedQuakeArea",
                type = "Boolean",
                shortCode = "11C"
            },
            ["kingsStationNonDisplay"] = new PlayerDataField {
                name = "kingsStationNonDisplay",
                type = "Boolean",
                shortCode = "11D"
            },
            ["tollBenchCity"] = new PlayerDataField {
                name = "tollBenchCity",
                type = "Boolean",
                shortCode = "11E"
            },
            ["waterwaysGate"] = new PlayerDataField {
                name = "waterwaysGate",
                type = "Boolean",
                shortCode = "11F"
            },
            ["defeatedDungDefender"] = new PlayerDataField {
                name = "defeatedDungDefender",
                type = "Boolean",
                shortCode = "11G"
            },
            ["dungDefenderEncounterReady"] = new PlayerDataField {
                name = "dungDefenderEncounterReady",
                type = "Boolean",
                shortCode = "11H"
            },
            ["flukeMotherEncountered"] = new PlayerDataField {
                name = "flukeMotherEncountered",
                type = "Boolean",
                shortCode = "11I"
            },
            ["flukeMotherDefeated"] = new PlayerDataField {
                name = "flukeMotherDefeated",
                type = "Boolean",
                shortCode = "11J"
            },
            ["openedWaterwaysManhole"] = new PlayerDataField {
                name = "openedWaterwaysManhole",
                type = "Boolean",
                shortCode = "11K"
            },
            ["waterwaysAcidDrained"] = new PlayerDataField {
                name = "waterwaysAcidDrained",
                type = "Boolean",
                shortCode = "11L"
            },
            ["dungDefenderWallBroken"] = new PlayerDataField {
                name = "dungDefenderWallBroken",
                type = "Boolean",
                shortCode = "11M"
            },
            ["dungDefenderSleeping"] = new PlayerDataField {
                name = "dungDefenderSleeping",
                type = "Boolean",
                shortCode = "11N"
            },
            ["defeatedMegaBeamMiner"] = new PlayerDataField {
                name = "defeatedMegaBeamMiner",
                type = "Boolean",
                shortCode = "11O"
            },
            ["defeatedMegaBeamMiner2"] = new PlayerDataField {
                name = "defeatedMegaBeamMiner2",
                type = "Boolean",
                shortCode = "11P"
            },
            ["brokeMinersWall"] = new PlayerDataField {
                name = "brokeMinersWall",
                type = "Boolean",
                shortCode = "11Q"
            },
            ["encounteredMimicSpider"] = new PlayerDataField {
                name = "encounteredMimicSpider",
                type = "Boolean",
                shortCode = "11R"
            },
            ["steppedBeyondBridge"] = new PlayerDataField {
                name = "steppedBeyondBridge",
                type = "Boolean",
                shortCode = "11S"
            },
            ["deepnestBridgeCollapsed"] = new PlayerDataField {
                name = "deepnestBridgeCollapsed",
                type = "Boolean",
                shortCode = "11T"
            },
            ["spiderCapture"] = new PlayerDataField {
                name = "spiderCapture",
                type = "Boolean",
                shortCode = "11U"
            },
            ["deepnest26b_switch"] = new PlayerDataField {
                name = "deepnest26b_switch",
                type = "Boolean",
                shortCode = "11V"
            },
            ["openedRestingGrounds02"] = new PlayerDataField {
                name = "openedRestingGrounds02",
                type = "Boolean",
                shortCode = "11W"
            },
            ["restingGroundsCryptWall"] = new PlayerDataField {
                name = "restingGroundsCryptWall",
                type = "Boolean",
                shortCode = "11X"
            },
            ["dreamNailConvo"] = new PlayerDataField {
                name = "dreamNailConvo",
                type = "Boolean",
                shortCode = "11Y"
            },
            ["gladeGhostsKilled"] = new PlayerDataField {
                name = "gladeGhostsKilled",
                type = "Int32",
                shortCode = "11Z"
            },
            ["openedGardensStagStation"] = new PlayerDataField {
                name = "openedGardensStagStation",
                type = "Boolean",
                shortCode = "120"
            },
            ["extendedGramophone"] = new PlayerDataField {
                name = "extendedGramophone",
                type = "Boolean",
                shortCode = "121"
            },
            ["tollBenchQueensGardens"] = new PlayerDataField {
                name = "tollBenchQueensGardens",
                type = "Boolean",
                shortCode = "122"
            },
            ["blizzardEnded"] = new PlayerDataField {
                name = "blizzardEnded",
                type = "Boolean",
                shortCode = "123"
            },
            ["encounteredHornet"] = new PlayerDataField {
                name = "encounteredHornet",
                type = "Boolean",
                shortCode = "124"
            },
            ["savedByHornet"] = new PlayerDataField {
                name = "savedByHornet",
                type = "Boolean",
                shortCode = "125"
            },
            ["outskirtsWall"] = new PlayerDataField {
                name = "outskirtsWall",
                type = "Boolean",
                shortCode = "126"
            },
            ["abyssGateOpened"] = new PlayerDataField {
                name = "abyssGateOpened",
                type = "Boolean",
                shortCode = "127"
            },
            ["abyssLighthouse"] = new PlayerDataField {
                name = "abyssLighthouse",
                type = "Boolean",
                shortCode = "128"
            },
            ["blueVineDoor"] = new PlayerDataField {
                name = "blueVineDoor",
                type = "Boolean",
                shortCode = "129"
            },
            ["gotShadeCharm"] = new PlayerDataField {
                name = "gotShadeCharm",
                type = "Boolean",
                shortCode = "12A"
            },
            ["tollBenchAbyss"] = new PlayerDataField {
                name = "tollBenchAbyss",
                type = "Boolean",
                shortCode = "12B"
            },
            ["fountainGeo"] = new PlayerDataField {
                name = "fountainGeo",
                type = "Int32",
                shortCode = "12C"
            },
            ["fountainVesselSummoned"] = new PlayerDataField {
                name = "fountainVesselSummoned",
                type = "Boolean",
                shortCode = "12D"
            },
            ["openedBlackEggPath"] = new PlayerDataField {
                name = "openedBlackEggPath",
                type = "Boolean",
                shortCode = "12E"
            },
            ["enteredDreamWorld"] = new PlayerDataField {
                name = "enteredDreamWorld",
                type = "Boolean",
                shortCode = "12F"
            },
            ["duskKnightDefeated"] = new PlayerDataField {
                name = "duskKnightDefeated",
                type = "Boolean",
                shortCode = "12G"
            },
            ["whitePalaceOrb_1"] = new PlayerDataField {
                name = "whitePalaceOrb_1",
                type = "Boolean",
                shortCode = "12H"
            },
            ["whitePalaceOrb_2"] = new PlayerDataField {
                name = "whitePalaceOrb_2",
                type = "Boolean",
                shortCode = "12I"
            },
            ["whitePalaceOrb_3"] = new PlayerDataField {
                name = "whitePalaceOrb_3",
                type = "Boolean",
                shortCode = "12J"
            },
            ["whitePalace05_lever"] = new PlayerDataField {
                name = "whitePalace05_lever",
                type = "Boolean",
                shortCode = "12K"
            },
            ["whitePalaceMidWarp"] = new PlayerDataField {
                name = "whitePalaceMidWarp",
                type = "Boolean",
                shortCode = "12L"
            },
            ["whitePalaceSecretRoomVisited"] = new PlayerDataField {
                name = "whitePalaceSecretRoomVisited",
                type = "Boolean",
                shortCode = "12M"
            },
            ["tramOpenedDeepnest"] = new PlayerDataField {
                name = "tramOpenedDeepnest",
                type = "Boolean",
                shortCode = "12N"
            },
            ["tramOpenedCrossroads"] = new PlayerDataField {
                name = "tramOpenedCrossroads",
                type = "Boolean",
                shortCode = "12O"
            },
            ["openedBlackEggDoor"] = new PlayerDataField {
                name = "openedBlackEggDoor",
                type = "Boolean",
                shortCode = "12P"
            },
            ["unchainedHollowKnight"] = new PlayerDataField {
                name = "unchainedHollowKnight",
                type = "Boolean",
                shortCode = "12Q"
            },
            ["flamesCollected"] = new PlayerDataField {
                name = "flamesCollected",
                type = "Int32",
                shortCode = "12R"
            },
            ["flamesRequired"] = new PlayerDataField {
                name = "flamesRequired",
                type = "Int32",
                shortCode = "12S"
            },
            ["nightmareLanternAppeared"] = new PlayerDataField {
                name = "nightmareLanternAppeared",
                type = "Boolean",
                shortCode = "12T"
            },
            ["nightmareLanternLit"] = new PlayerDataField {
                name = "nightmareLanternLit",
                type = "Boolean",
                shortCode = "12U"
            },
            ["troupeInTown"] = new PlayerDataField {
                name = "troupeInTown",
                type = "Boolean",
                shortCode = "12V"
            },
            ["divineInTown"] = new PlayerDataField {
                name = "divineInTown",
                type = "Boolean",
                shortCode = "12W"
            },
            ["grimmChildLevel"] = new PlayerDataField {
                name = "grimmChildLevel",
                type = "Int32",
                shortCode = "12X"
            },
            ["elderbugConvoGrimm"] = new PlayerDataField {
                name = "elderbugConvoGrimm",
                type = "Boolean",
                shortCode = "12Y"
            },
            ["slyConvoGrimm"] = new PlayerDataField {
                name = "slyConvoGrimm",
                type = "Boolean",
                shortCode = "12Z"
            },
            ["iseldaConvoGrimm"] = new PlayerDataField {
                name = "iseldaConvoGrimm",
                type = "Boolean",
                shortCode = "130"
            },
            ["midwifeWeaverlingConvo"] = new PlayerDataField {
                name = "midwifeWeaverlingConvo",
                type = "Boolean",
                shortCode = "131"
            },
            ["metGrimm"] = new PlayerDataField {
                name = "metGrimm",
                type = "Boolean",
                shortCode = "132"
            },
            ["foughtGrimm"] = new PlayerDataField {
                name = "foughtGrimm",
                type = "Boolean",
                shortCode = "133"
            },
            ["metBrum"] = new PlayerDataField {
                name = "metBrum",
                type = "Boolean",
                shortCode = "134"
            },
            ["defeatedNightmareGrimm"] = new PlayerDataField {
                name = "defeatedNightmareGrimm",
                type = "Boolean",
                shortCode = "135"
            },
            ["grimmchildAwoken"] = new PlayerDataField {
                name = "grimmchildAwoken",
                type = "Boolean",
                shortCode = "136"
            },
            ["gotBrummsFlame"] = new PlayerDataField {
                name = "gotBrummsFlame",
                type = "Boolean",
                shortCode = "137"
            },
            ["brummBrokeBrazier"] = new PlayerDataField {
                name = "brummBrokeBrazier",
                type = "Boolean",
                shortCode = "138"
            },
            ["destroyedNightmareLantern"] = new PlayerDataField {
                name = "destroyedNightmareLantern",
                type = "Boolean",
                shortCode = "139"
            },
            ["gotGrimmNotch"] = new PlayerDataField {
                name = "gotGrimmNotch",
                type = "Boolean",
                shortCode = "13A"
            },
            ["nymmInTown"] = new PlayerDataField {
                name = "nymmInTown",
                type = "Boolean",
                shortCode = "13B"
            },
            ["nymmSpoken"] = new PlayerDataField {
                name = "nymmSpoken",
                type = "Boolean",
                shortCode = "13C"
            },
            ["nymmCharmConvo"] = new PlayerDataField {
                name = "nymmCharmConvo",
                type = "Boolean",
                shortCode = "13D"
            },
            ["nymmFinalConvo"] = new PlayerDataField {
                name = "nymmFinalConvo",
                type = "Boolean",
                shortCode = "13E"
            },
            ["elderbugNymmConvo"] = new PlayerDataField {
                name = "elderbugNymmConvo",
                type = "Boolean",
                shortCode = "13F"
            },
            ["slyNymmConvo"] = new PlayerDataField {
                name = "slyNymmConvo",
                type = "Boolean",
                shortCode = "13G"
            },
            ["iseldaNymmConvo"] = new PlayerDataField {
                name = "iseldaNymmConvo",
                type = "Boolean",
                shortCode = "13H"
            },
            ["nymmMissedEggOpen"] = new PlayerDataField {
                name = "nymmMissedEggOpen",
                type = "Boolean",
                shortCode = "13I"
            },
            ["elderbugTroupeLeftConvo"] = new PlayerDataField {
                name = "elderbugTroupeLeftConvo",
                type = "Boolean",
                shortCode = "13J"
            },
            ["elderbugBrettaLeft"] = new PlayerDataField {
                name = "elderbugBrettaLeft",
                type = "Boolean",
                shortCode = "13K"
            },
            ["jijiGrimmConvo"] = new PlayerDataField {
                name = "jijiGrimmConvo",
                type = "Boolean",
                shortCode = "13L"
            },
            ["metDivine"] = new PlayerDataField {
                name = "metDivine",
                type = "Boolean",
                shortCode = "13M"
            },
            ["divineFinalConvo"] = new PlayerDataField {
                name = "divineFinalConvo",
                type = "Boolean",
                shortCode = "13N"
            },
            ["gaveFragileHeart"] = new PlayerDataField {
                name = "gaveFragileHeart",
                type = "Boolean",
                shortCode = "13O"
            },
            ["gaveFragileGreed"] = new PlayerDataField {
                name = "gaveFragileGreed",
                type = "Boolean",
                shortCode = "13P"
            },
            ["gaveFragileStrength"] = new PlayerDataField {
                name = "gaveFragileStrength",
                type = "Boolean",
                shortCode = "13Q"
            },
            ["divineEatenConvos"] = new PlayerDataField {
                name = "divineEatenConvos",
                type = "Int32",
                shortCode = "13R"
            },
            ["pooedFragileHeart"] = new PlayerDataField {
                name = "pooedFragileHeart",
                type = "Boolean",
                shortCode = "13S"
            },
            ["pooedFragileGreed"] = new PlayerDataField {
                name = "pooedFragileGreed",
                type = "Boolean",
                shortCode = "13T"
            },
            ["pooedFragileStrength"] = new PlayerDataField {
                name = "pooedFragileStrength",
                type = "Boolean",
                shortCode = "13U"
            },
            ["completionPercentage"] = new PlayerDataField {
                name = "completionPercentage",
                type = "Single",
                shortCode = "13V"
            },
            ["disablePause"] = new PlayerDataField {
                name = "disablePause",
                type = "Boolean",
                shortCode = "13W"
            },
            ["backerCredits"] = new PlayerDataField {
                name = "backerCredits",
                type = "Boolean",
                shortCode = "13X"
            },
            ["unlockedCompletionRate"] = new PlayerDataField {
                name = "unlockedCompletionRate",
                type = "Boolean",
                shortCode = "13Y"
            },
            ["mapKeyPref"] = new PlayerDataField {
                name = "mapKeyPref",
                type = "Int32",
                shortCode = "13Z"
            },
            ["playerStory"] = new PlayerDataField {
                name = "playerStory",
                type = "List`1",
                shortCode = "140"
            },
            ["playerStoryOutput"] = new PlayerDataField {
                name = "playerStoryOutput",
                type = "String",
                shortCode = "141"
            },
            ["betaEnd"] = new PlayerDataField {
                name = "betaEnd",
                type = "Boolean",
                shortCode = "142"
            },
            ["newDatTraitorLord"] = new PlayerDataField {
                name = "newDatTraitorLord",
                type = "Boolean",
                shortCode = "143"
            },
            ["bossReturnEntryGate"] = new PlayerDataField {
                name = "bossReturnEntryGate",
                type = "String",
                shortCode = "144"
            },
            ["bossDoorStateTier1"] = new PlayerDataField {
                name = "bossDoorStateTier1",
                type = "Completion",
                shortCode = "145"
            },
            ["bossDoorStateTier2"] = new PlayerDataField {
                name = "bossDoorStateTier2",
                type = "Completion",
                shortCode = "146"
            },
            ["bossDoorStateTier3"] = new PlayerDataField {
                name = "bossDoorStateTier3",
                type = "Completion",
                shortCode = "147"
            },
            ["bossDoorStateTier4"] = new PlayerDataField {
                name = "bossDoorStateTier4",
                type = "Completion",
                shortCode = "148"
            },
            ["bossDoorStateTier5"] = new PlayerDataField {
                name = "bossDoorStateTier5",
                type = "Completion",
                shortCode = "149"
            },
            ["bossStatueTargetLevel"] = new PlayerDataField {
                name = "bossStatueTargetLevel",
                type = "Int32",
                shortCode = "14A"
            },
            ["currentBossStatueCompletionKey"] = new PlayerDataField {
                name = "currentBossStatueCompletionKey",
                type = "String",
                shortCode = "14B"
            },
            ["statueStateGruzMother"] = new PlayerDataField {
                name = "statueStateGruzMother",
                type = "Completion",
                shortCode = "14C"
            },
            ["statueStateVengefly"] = new PlayerDataField {
                name = "statueStateVengefly",
                type = "Completion",
                shortCode = "14D"
            },
            ["statueStateBroodingMawlek"] = new PlayerDataField {
                name = "statueStateBroodingMawlek",
                type = "Completion",
                shortCode = "14E"
            },
            ["statueStateFalseKnight"] = new PlayerDataField {
                name = "statueStateFalseKnight",
                type = "Completion",
                shortCode = "14F"
            },
            ["statueStateFailedChampion"] = new PlayerDataField {
                name = "statueStateFailedChampion",
                type = "Completion",
                shortCode = "14G"
            },
            ["statueStateHornet1"] = new PlayerDataField {
                name = "statueStateHornet1",
                type = "Completion",
                shortCode = "14H"
            },
            ["statueStateHornet2"] = new PlayerDataField {
                name = "statueStateHornet2",
                type = "Completion",
                shortCode = "14I"
            },
            ["statueStateMegaMossCharger"] = new PlayerDataField {
                name = "statueStateMegaMossCharger",
                type = "Completion",
                shortCode = "14J"
            },
            ["statueStateMantisLords"] = new PlayerDataField {
                name = "statueStateMantisLords",
                type = "Completion",
                shortCode = "14K"
            },
            ["statueStateOblobbles"] = new PlayerDataField {
                name = "statueStateOblobbles",
                type = "Completion",
                shortCode = "14L"
            },
            ["statueStateGreyPrince"] = new PlayerDataField {
                name = "statueStateGreyPrince",
                type = "Completion",
                shortCode = "14M"
            },
            ["statueStateBrokenVessel"] = new PlayerDataField {
                name = "statueStateBrokenVessel",
                type = "Completion",
                shortCode = "14N"
            },
            ["statueStateLostKin"] = new PlayerDataField {
                name = "statueStateLostKin",
                type = "Completion",
                shortCode = "14O"
            },
            ["statueStateNosk"] = new PlayerDataField {
                name = "statueStateNosk",
                type = "Completion",
                shortCode = "14P"
            },
            ["statueStateFlukemarm"] = new PlayerDataField {
                name = "statueStateFlukemarm",
                type = "Completion",
                shortCode = "14Q"
            },
            ["statueStateCollector"] = new PlayerDataField {
                name = "statueStateCollector",
                type = "Completion",
                shortCode = "14R"
            },
            ["statueStateWatcherKnights"] = new PlayerDataField {
                name = "statueStateWatcherKnights",
                type = "Completion",
                shortCode = "14S"
            },
            ["statueStateSoulMaster"] = new PlayerDataField {
                name = "statueStateSoulMaster",
                type = "Completion",
                shortCode = "14T"
            },
            ["statueStateSoulTyrant"] = new PlayerDataField {
                name = "statueStateSoulTyrant",
                type = "Completion",
                shortCode = "14U"
            },
            ["statueStateGodTamer"] = new PlayerDataField {
                name = "statueStateGodTamer",
                type = "Completion",
                shortCode = "14V"
            },
            ["statueStateCrystalGuardian1"] = new PlayerDataField {
                name = "statueStateCrystalGuardian1",
                type = "Completion",
                shortCode = "14W"
            },
            ["statueStateCrystalGuardian2"] = new PlayerDataField {
                name = "statueStateCrystalGuardian2",
                type = "Completion",
                shortCode = "14X"
            },
            ["statueStateUumuu"] = new PlayerDataField {
                name = "statueStateUumuu",
                type = "Completion",
                shortCode = "14Y"
            },
            ["statueStateDungDefender"] = new PlayerDataField {
                name = "statueStateDungDefender",
                type = "Completion",
                shortCode = "14Z"
            },
            ["statueStateWhiteDefender"] = new PlayerDataField {
                name = "statueStateWhiteDefender",
                type = "Completion",
                shortCode = "150"
            },
            ["statueStateHiveKnight"] = new PlayerDataField {
                name = "statueStateHiveKnight",
                type = "Completion",
                shortCode = "151"
            },
            ["statueStateTraitorLord"] = new PlayerDataField {
                name = "statueStateTraitorLord",
                type = "Completion",
                shortCode = "152"
            },
            ["statueStateGrimm"] = new PlayerDataField {
                name = "statueStateGrimm",
                type = "Completion",
                shortCode = "153"
            },
            ["statueStateNightmareGrimm"] = new PlayerDataField {
                name = "statueStateNightmareGrimm",
                type = "Completion",
                shortCode = "154"
            },
            ["statueStateHollowKnight"] = new PlayerDataField {
                name = "statueStateHollowKnight",
                type = "Completion",
                shortCode = "155"
            },
            ["statueStateElderHu"] = new PlayerDataField {
                name = "statueStateElderHu",
                type = "Completion",
                shortCode = "156"
            },
            ["statueStateGalien"] = new PlayerDataField {
                name = "statueStateGalien",
                type = "Completion",
                shortCode = "157"
            },
            ["statueStateMarkoth"] = new PlayerDataField {
                name = "statueStateMarkoth",
                type = "Completion",
                shortCode = "158"
            },
            ["statueStateMarmu"] = new PlayerDataField {
                name = "statueStateMarmu",
                type = "Completion",
                shortCode = "159"
            },
            ["statueStateNoEyes"] = new PlayerDataField {
                name = "statueStateNoEyes",
                type = "Completion",
                shortCode = "15A"
            },
            ["statueStateXero"] = new PlayerDataField {
                name = "statueStateXero",
                type = "Completion",
                shortCode = "15B"
            },
            ["statueStateGorb"] = new PlayerDataField {
                name = "statueStateGorb",
                type = "Completion",
                shortCode = "15C"
            },
            ["statueStateRadiance"] = new PlayerDataField {
                name = "statueStateRadiance",
                type = "Completion",
                shortCode = "15D"
            },
            ["statueStateSly"] = new PlayerDataField {
                name = "statueStateSly",
                type = "Completion",
                shortCode = "15E"
            },
            ["statueStateNailmasters"] = new PlayerDataField {
                name = "statueStateNailmasters",
                type = "Completion",
                shortCode = "15F"
            },
            ["statueStateMageKnight"] = new PlayerDataField {
                name = "statueStateMageKnight",
                type = "Completion",
                shortCode = "15G"
            },
            ["statueStatePaintmaster"] = new PlayerDataField {
                name = "statueStatePaintmaster",
                type = "Completion",
                shortCode = "15H"
            },
            ["statueStateZote"] = new PlayerDataField {
                name = "statueStateZote",
                type = "Completion",
                shortCode = "15I"
            },
            ["statueStateNoskHornet"] = new PlayerDataField {
                name = "statueStateNoskHornet",
                type = "Completion",
                shortCode = "15J"
            },
            ["statueStateMantisLordsExtra"] = new PlayerDataField {
                name = "statueStateMantisLordsExtra",
                type = "Completion",
                shortCode = "15K"
            },
            ["godseekerUnlocked"] = new PlayerDataField {
                name = "godseekerUnlocked",
                type = "Boolean",
                shortCode = "15L"
            },
            ["currentBossSequence"] = new PlayerDataField {
                name = "currentBossSequence",
                type = "BossSequenceData",
                shortCode = "15M"
            },
            ["bossRushMode"] = new PlayerDataField {
                name = "bossRushMode",
                type = "Boolean",
                shortCode = "15N"
            },
            ["bossDoorCageUnlocked"] = new PlayerDataField {
                name = "bossDoorCageUnlocked",
                type = "Boolean",
                shortCode = "15O"
            },
            ["blueRoomDoorUnlocked"] = new PlayerDataField {
                name = "blueRoomDoorUnlocked",
                type = "Boolean",
                shortCode = "15P"
            },
            ["blueRoomActivated"] = new PlayerDataField {
                name = "blueRoomActivated",
                type = "Boolean",
                shortCode = "15Q"
            },
            ["finalBossDoorUnlocked"] = new PlayerDataField {
                name = "finalBossDoorUnlocked",
                type = "Boolean",
                shortCode = "15R"
            },
            ["hasGodfinder"] = new PlayerDataField {
                name = "hasGodfinder",
                type = "Boolean",
                shortCode = "15S"
            },
            ["unlockedNewBossStatue"] = new PlayerDataField {
                name = "unlockedNewBossStatue",
                type = "Boolean",
                shortCode = "15T"
            },
            ["scaredFlukeHermitEncountered"] = new PlayerDataField {
                name = "scaredFlukeHermitEncountered",
                type = "Boolean",
                shortCode = "15U"
            },
            ["scaredFlukeHermitReturned"] = new PlayerDataField {
                name = "scaredFlukeHermitReturned",
                type = "Boolean",
                shortCode = "15V"
            },
            ["enteredGGAtrium"] = new PlayerDataField {
                name = "enteredGGAtrium",
                type = "Boolean",
                shortCode = "15W"
            },
            ["extraFlowerAppear"] = new PlayerDataField {
                name = "extraFlowerAppear",
                type = "Boolean",
                shortCode = "15X"
            },
            ["givenGodseekerFlower"] = new PlayerDataField {
                name = "givenGodseekerFlower",
                type = "Boolean",
                shortCode = "15Y"
            },
            ["givenOroFlower"] = new PlayerDataField {
                name = "givenOroFlower",
                type = "Boolean",
                shortCode = "15Z"
            },
            ["givenWhiteLadyFlower"] = new PlayerDataField {
                name = "givenWhiteLadyFlower",
                type = "Boolean",
                shortCode = "160"
            },
            ["givenEmilitiaFlower"] = new PlayerDataField {
                name = "givenEmilitiaFlower",
                type = "Boolean",
                shortCode = "161"
            },
            ["unlockedBossScenes"] = new PlayerDataField {
                name = "unlockedBossScenes",
                type = "List`1",
                shortCode = "162"
            },
            ["queuedGodfinderIcon"] = new PlayerDataField {
                name = "queuedGodfinderIcon",
                type = "Boolean",
                shortCode = "163"
            },
            ["godseekerSpokenAwake"] = new PlayerDataField {
                name = "godseekerSpokenAwake",
                type = "Boolean",
                shortCode = "164"
            },
            ["nailsmithCorpseAppeared"] = new PlayerDataField {
                name = "nailsmithCorpseAppeared",
                type = "Boolean",
                shortCode = "165"
            },
            ["godseekerWaterwaysSeenState"] = new PlayerDataField {
                name = "godseekerWaterwaysSeenState",
                type = "Int32",
                shortCode = "166"
            },
            ["godseekerWaterwaysSpoken1"] = new PlayerDataField {
                name = "godseekerWaterwaysSpoken1",
                type = "Boolean",
                shortCode = "167"
            },
            ["godseekerWaterwaysSpoken2"] = new PlayerDataField {
                name = "godseekerWaterwaysSpoken2",
                type = "Boolean",
                shortCode = "168"
            },
            ["godseekerWaterwaysSpoken3"] = new PlayerDataField {
                name = "godseekerWaterwaysSpoken3",
                type = "Boolean",
                shortCode = "169"
            },
            ["bossDoorEntranceTextSeen"] = new PlayerDataField {
                name = "bossDoorEntranceTextSeen",
                type = "Int32",
                shortCode = "16A"
            },
            ["seenDoor4Finale"] = new PlayerDataField {
                name = "seenDoor4Finale",
                type = "Boolean",
                shortCode = "16B"
            },
            ["zoteStatueWallBroken"] = new PlayerDataField {
                name = "zoteStatueWallBroken",
                type = "Boolean",
                shortCode = "16C"
            },
            ["seenGGWastes"] = new PlayerDataField {
                name = "seenGGWastes",
                type = "Boolean",
                shortCode = "16D"
            },
            ["ordealAchieved"] = new PlayerDataField {
                name = "ordealAchieved",
                type = "Boolean",
                shortCode = "16E"
            },

        };
    }
}
