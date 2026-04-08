using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HKViz.Shared;
using static HKViz.HkVizAuthManager;
using UnityEngine;
using Modding;

namespace HKViz {
    internal class HKVizVersionChecker: Loggable {
        private static HKVizVersionChecker _instance;
        public static HKVizVersionChecker Instance {
            get {
                if (_instance == null) {
                    _instance = new HKVizVersionChecker();
                }
                return _instance;
            }
        }

        private readonly ServerApi serverApi = HkVizInstances.Instance.serverApi;
        public VersionCheckResponse? checkResponse = null;

        public class VersionCheckResponse {
            public string message;
            public string color;
            public bool show;
        }

        public void Init() {
            serverApi.ApiGet<VersionCheckResponse>(
                path: "modversioncheck/" + HkVizHollowConstants.GetVersion(),
                onSuccess: data => {
                    checkResponse = data;
                },
                onError: request => {
                    Log("Could not check version of HKViz mod");
                    Log(request.error);
                    Log(request.downloadHandler.text);
                }
            ).StartGlobal();
        }
    }
}
