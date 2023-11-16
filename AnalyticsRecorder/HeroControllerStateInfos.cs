﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnalyticsRecorder {
    internal class HeroControllerStateInfos {

        public static readonly Dictionary<string, HeroControllerStat> stats = new() {
            ["facingRight"] = new HeroControllerStat {
                name = "facingRight",
                shortCode = "1",
                notPartOfLog = true,
                onlyTruthLogged = false,
            },
            ["onGround"] = new HeroControllerStat {
                name = "onGround",
                shortCode = "2",
                notPartOfLog = true,
                onlyTruthLogged = false,
            },
            ["jumping"] = new HeroControllerStat {
                name = "jumping",
                shortCode = "3",
                notPartOfLog = false,
                onlyTruthLogged = true,
            },
            ["wallJumping"] = new HeroControllerStat {
                name = "wallJumping",
                shortCode = "4",
                notPartOfLog = false,
                onlyTruthLogged = true,
            },
            ["doubleJumping"] = new HeroControllerStat {
                name = "doubleJumping",
                shortCode = "5",
                notPartOfLog = false,
                onlyTruthLogged = true,
            },
            ["nailCharging"] = new HeroControllerStat {
                name = "nailCharging",
                shortCode = "6",
                notPartOfLog = false,
                onlyTruthLogged = false,
            },
            ["shadowDashing"] = new HeroControllerStat {
                name = "shadowDashing",
                shortCode = "7",
                notPartOfLog = false,
                onlyTruthLogged = true,
            },
            ["swimming"] = new HeroControllerStat {
                name = "swimming",
                shortCode = "8",
                notPartOfLog = false,
                onlyTruthLogged = false,
            },
            ["falling"] = new HeroControllerStat {
                name = "falling",
                shortCode = "9",
                notPartOfLog = true,
                onlyTruthLogged = false,
            },
            ["dashing"] = new HeroControllerStat {
                name = "dashing",
                shortCode = "A",
                notPartOfLog = false,
                onlyTruthLogged = true,
            },
            ["superDashing"] = new HeroControllerStat {
                name = "superDashing",
                shortCode = "B",
                notPartOfLog = false,
                onlyTruthLogged = false,
            },
            ["superDashOnWall"] = new HeroControllerStat {
                name = "superDashOnWall",
                shortCode = "C",
                notPartOfLog = false,
                onlyTruthLogged = false,
            },
            ["backDashing"] = new HeroControllerStat {
                name = "backDashing",
                shortCode = "D",
                notPartOfLog = false,
                onlyTruthLogged = false,
            },
            ["touchingWall"] = new HeroControllerStat {
                name = "touchingWall",
                shortCode = "E",
                notPartOfLog = true,
                onlyTruthLogged = false,
            },
            ["wallSliding"] = new HeroControllerStat {
                name = "wallSliding",
                shortCode = "F",
                notPartOfLog = true,
                onlyTruthLogged = false,
            },
            ["transitioning"] = new HeroControllerStat {
                name = "transitioning",
                shortCode = "G",
                notPartOfLog = true,
                onlyTruthLogged = false,
            },
            ["attacking"] = new HeroControllerStat {
                name = "attacking",
                shortCode = "H",
                notPartOfLog = false,
                onlyTruthLogged = true,
            },
            ["lookingUp"] = new HeroControllerStat {
                name = "lookingUp",
                shortCode = "I",
                notPartOfLog = true,
                onlyTruthLogged = false,
            },
            ["lookingDown"] = new HeroControllerStat {
                name = "lookingDown",
                shortCode = "J",
                notPartOfLog = true,
                onlyTruthLogged = false,
            },
            ["lookingUpAnim"] = new HeroControllerStat {
                name = "lookingUpAnim",
                shortCode = "K",
                notPartOfLog = false,
                onlyTruthLogged = false,
            },
            ["lookingDownAnim"] = new HeroControllerStat {
                name = "lookingDownAnim",
                shortCode = "L",
                notPartOfLog = false,
                onlyTruthLogged = false,
            },
            ["altAttack"] = new HeroControllerStat {
                name = "altAttack",
                shortCode = "M",
                notPartOfLog = false,
                onlyTruthLogged = true,
            },
            ["upAttacking"] = new HeroControllerStat {
                name = "upAttacking",
                shortCode = "N",
                notPartOfLog = false,
                onlyTruthLogged = true,
            },
            ["downAttacking"] = new HeroControllerStat {
                name = "downAttacking",
                shortCode = "O",
                notPartOfLog = false,
                onlyTruthLogged = true,
            },
            ["bouncing"] = new HeroControllerStat {
                name = "bouncing",
                shortCode = "P",
                notPartOfLog = false,
                onlyTruthLogged = true,
            },
            ["shroomBouncing"] = new HeroControllerStat {
                name = "shroomBouncing",
                shortCode = "Q",
                notPartOfLog = false,
                onlyTruthLogged = false,
            },
            ["recoilingRight"] = new HeroControllerStat {
                name = "recoilingRight",
                shortCode = "R",
                notPartOfLog = true,
                onlyTruthLogged = false,
            },
            ["recoilingLeft"] = new HeroControllerStat {
                name = "recoilingLeft",
                shortCode = "S",
                notPartOfLog = true,
                onlyTruthLogged = false,
            },
            ["dead"] = new HeroControllerStat {
                name = "dead",
                shortCode = "T",
                notPartOfLog = false,
                onlyTruthLogged = false,
            },
            ["hazardDeath"] = new HeroControllerStat {
                name = "hazardDeath",
                shortCode = "U",
                notPartOfLog = false,
                onlyTruthLogged = false,
            },
            ["hazardRespawning"] = new HeroControllerStat {
                name = "hazardRespawning",
                shortCode = "V",
                notPartOfLog = false,
                onlyTruthLogged = false,
            },
            ["willHardLand"] = new HeroControllerStat {
                name = "willHardLand",
                shortCode = "W",
                notPartOfLog = false,
                onlyTruthLogged = false,
            },
            ["recoilFrozen"] = new HeroControllerStat {
                name = "recoilFrozen",
                shortCode = "X",
                notPartOfLog = false,
                onlyTruthLogged = false,
            },
            ["recoiling"] = new HeroControllerStat {
                name = "recoiling",
                shortCode = "Y",
                notPartOfLog = false,
                onlyTruthLogged = false,
            },
            ["invulnerable"] = new HeroControllerStat {
                name = "invulnerable",
                shortCode = "Z",
                notPartOfLog = false,
                onlyTruthLogged = false,
            },
            ["casting"] = new HeroControllerStat {
                name = "casting",
                shortCode = "10",
                notPartOfLog = false,
                onlyTruthLogged = false,
            },
            ["castRecoiling"] = new HeroControllerStat {
                name = "castRecoiling",
                shortCode = "11",
                notPartOfLog = false,
                onlyTruthLogged = false,
            },
            ["preventDash"] = new HeroControllerStat {
                name = "preventDash",
                shortCode = "12",
                notPartOfLog = false,
                onlyTruthLogged = false,
            },
            ["preventBackDash"] = new HeroControllerStat {
                name = "preventBackDash",
                shortCode = "13",
                notPartOfLog = false,
                onlyTruthLogged = false,
            },
            ["dashCooldown"] = new HeroControllerStat {
                name = "dashCooldown",
                shortCode = "14",
                notPartOfLog = true,
                onlyTruthLogged = false,
            },
            ["backDashCooldown"] = new HeroControllerStat {
                name = "backDashCooldown",
                shortCode = "15",
                notPartOfLog = true,
                onlyTruthLogged = false,
            },
            ["nearBench"] = new HeroControllerStat {
                name = "nearBench",
                shortCode = "16",
                notPartOfLog = false,
                onlyTruthLogged = false,
            },
            ["inWalkZone"] = new HeroControllerStat {
                name = "inWalkZone",
                shortCode = "17",
                notPartOfLog = false,
                onlyTruthLogged = false,
            },
            ["isPaused"] = new HeroControllerStat {
                name = "isPaused",
                shortCode = "18",
                notPartOfLog = false,
                onlyTruthLogged = false,
            },
            ["onConveyor"] = new HeroControllerStat {
                name = "onConveyor",
                shortCode = "19",
                notPartOfLog = false,
                onlyTruthLogged = false,
            },
            ["onConveyorV"] = new HeroControllerStat {
                name = "onConveyorV",
                shortCode = "1A",
                notPartOfLog = false,
                onlyTruthLogged = false,
            },
            ["inConveyorZone"] = new HeroControllerStat {
                name = "inConveyorZone",
                shortCode = "1B",
                notPartOfLog = false,
                onlyTruthLogged = false,
            },
            ["spellQuake"] = new HeroControllerStat {
                name = "spellQuake",
                shortCode = "1C",
                notPartOfLog = false,
                onlyTruthLogged = false,
            },
            ["freezeCharge"] = new HeroControllerStat {
                name = "freezeCharge",
                shortCode = "1D",
                notPartOfLog = false,
                onlyTruthLogged = false,
            },
            ["focusing"] = new HeroControllerStat {
                name = "focusing",
                shortCode = "1E",
                notPartOfLog = false,
                onlyTruthLogged = false,
            },
            ["inAcid"] = new HeroControllerStat {
                name = "inAcid",
                shortCode = "1F",
                notPartOfLog = false,
                onlyTruthLogged = false,
            },
            ["slidingLeft"] = new HeroControllerStat {
                name = "slidingLeft",
                shortCode = "1G",
                notPartOfLog = false,
                onlyTruthLogged = false,
            },
            ["slidingRight"] = new HeroControllerStat {
                name = "slidingRight",
                shortCode = "1H",
                notPartOfLog = false,
                onlyTruthLogged = false,
            },
            ["touchingNonSlider"] = new HeroControllerStat {
                name = "touchingNonSlider",
                shortCode = "1I",
                notPartOfLog = false,
                onlyTruthLogged = false,
            },
            ["wasOnGround"] = new HeroControllerStat {
                name = "wasOnGround",
                shortCode = "1J",
                notPartOfLog = true,
                onlyTruthLogged = false,
            },
        };
    }
}