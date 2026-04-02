using Modding;
using Steamworks;
using System;

namespace HKViz {
    internal class GameLauncherUser : Loggable {
        private static GameLauncherUser? instance;
        public static GameLauncherUser Instance { 
            get {
                instance ??= new GameLauncherUser();
                return instance;
            }
        }

        private string? userId;

        // the steam user id is used to store a few settings per steam user
        // so one can switch accounts and the modded settings are not the same between users
        // this is especially important for the hkviz login, and for keeping the local uuid generated
        // for each gameplay/profile different between users
        public string GetUserId() {
            if (userId != null) return userId;
            try {
                Steamworks.CSteamID steamId = SteamUser.GetSteamID();
                string id = steamId.GetAccountID().ToString();
                userId = string.IsNullOrEmpty(id) ? "no-steam-id" : id;
            } catch (Exception e) {
                LogError("Could not get steam id " + e.Message);
                userId = "no-steam-id";
            }
            return userId;
        }
    }
}
