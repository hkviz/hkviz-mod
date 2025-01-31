using UnityEngine;

namespace HKViz {
    internal class KnightManager {
        private static KnightManager instance;
        public static KnightManager Instance {
            get {
                if (instance == null) {
                    instance = new KnightManager();
                }
                return instance;
            }
        }


        private Transform? knight;

        public Transform? Knight => knight;

        public void UpdateKnight() {
            if (knight == null) {
                // if destroyed needs to find new player
                knight = GameObject.Find("Knight").transform;
                if (knight != null) {
                    //OnNewKnightFound?.Invoke(knight);
                    PlayerHealthWriter.Instance.InitNewKnight(knight);
                }
            }
        }

        //public event Action<Transform>? OnNewKnightFound;
    }
}
