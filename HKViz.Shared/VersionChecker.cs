using HKViz.Shared;

namespace HKViz.Shared;

public class VersionChecker {
    private readonly ServerApi serverApi;
    private readonly IHkVizLogger logger;
    
    public VersionCheckResponse? checkResponse = null;

    public class VersionCheckResponse {
        public string message;
        public string color;
        public bool show;
    }
    
    public VersionChecker(ServerApi serverApi, IHkVizLogger logger) {
        this.serverApi = serverApi;
        this.logger = logger;
    }

    public void Initialize() {
        serverApi.ApiGet<VersionCheckResponse>(
            path: $"modversioncheck/{GameFlavor.GAME_ID}/{HkVizSharedConstants.GetModVersion()}",
            onSuccess: data => {
                checkResponse = data;
            },
            onError: request => {
                logger.LogError("Could not check version of HKViz mod");
                logger.LogError(request.error);
                logger.LogError(request.downloadHandler.text);
            }
        ).StartGlobal();
    }
}
