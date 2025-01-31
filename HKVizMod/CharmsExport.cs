using Modding;
using Satchel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace HKViz {
    internal class CharmsExport : Loggable {
        class CharmInfo {
            public string charmId;
            public string spriteName;
        }


        private static CharmsExport? _instance;
        public static CharmsExport Instance {
            get {
                if (_instance != null) return _instance;
                _instance = new CharmsExport();
                return _instance;
            }
        }


        public void Export() {
            var go = GameObject.FindObjectOfType<CharmVibrations>().transform.Find("Collected Charms");
            var sprites = go.GetComponentsInChildren<SpriteRenderer>();
            Log("sprites length" + sprites.Length);

            var infos = sprites.Where(it => it.gameObject.name == "Sprite").Select(it => new CharmInfo {
                charmId = it.transform.parent.gameObject.name,
                spriteName = it.sprite.name,
            }).ToList();

            Debug.Log("infos" + infos.Count);

            var json = Json.Stringify(infos);
            using (var writer = new StreamWriter(StoragePaths.GetUserFilePath("charms-inventory-export.txt"))) {
                writer.Write(json);
            }
        }
    }
}
