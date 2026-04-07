using Modding;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace HKViz {
    internal class EnemiesExport : Loggable {
        class EnemyJournalInfo {
            public string portraitName;
            public string convoName;
            public string descConvo;
            public string nameConvo;
            public string notesConvo;
            public string playerDataBoolName;
            public string playerDataKillsName;
            public string playerDataName;
            public string playerDataNewDataName;
        }


        private static EnemiesExport? _instance;
        public static EnemiesExport Instance {
            get {
                if (_instance != null) return _instance;
                _instance = new EnemiesExport();
                return _instance;
            }
        }


        public void Export() {
            var journalList = GameObject.FindObjectOfType<JournalList>();
            var listItems = journalList.transform.GetComponentsInChildren<JournalEntryStats>(true);

            var enemies = listItems.Select(e => new EnemyJournalInfo {
                portraitName = e.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite.name,
                convoName = e.convoName,
                descConvo = e.descConvo,
                nameConvo = e.nameConvo,
                notesConvo = e.notesConvo,
                playerDataBoolName = e.playerDataBoolName,
                playerDataKillsName = e.playerDataKillsName,
                playerDataName = e.playerDataName,
                playerDataNewDataName = e.playerDataNewDataName,
            }).ToList();


            var json = Json.Stringify(enemies);
            using (var writer = new StreamWriter(StoragePaths.GetUserFilePath("enemies-journal-export.txt"))) {
                writer.Write(json);
            }
        }
    }
}
