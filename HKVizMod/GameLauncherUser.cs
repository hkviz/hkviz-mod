using Modding;
using Steamworks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Mono.Security.X509.X520;

namespace HKViz {
    internal class GameLauncherUser : Loggable {
        private static GameLauncherUser instance;
        public static GameLauncherUser Instance { 
            get {
                if (instance == null) {
                    instance = new GameLauncherUser();
                }
                return instance;
            }
        }

        private string userId;

        // the steam user id is used to store a few settings per steam user
        // so one can switch accounts and the modded settings are not the same between users
        // this is especially important for the hkviz login, and for keeping the local uuid generated
        // for each gameplay/profile different between users
        public string GetUserId() {
            if (userId == null) {
                try {
                    Steamworks.CSteamID SteamID = SteamUser.GetSteamID();
                    string id = SteamID.GetAccountID().ToString();
                    if (string.IsNullOrEmpty(id)) {
                        userId = "no-steam-id";
                    } else {
                        userId = id;
                    }
                } catch (Exception e) {
                    LogError("Could not get steam id " + e.Message);
                    userId = "no-steam-id";
                }
            }
            return userId;
        }
    }
}
