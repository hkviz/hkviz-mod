using System;
using HKViz.Shared;
using HKViz.Shared.Auth;
using HKViz.Shared.Recording;
using HKViz.Shared.Upload;

namespace HKViz;

public class HkVizSharedInstances {
    public static HkVizSharedInstances? Instance { get; private set; }

    public static void CreateInstance(HkVizSharedInstances sharedInstances) {
        Instance = sharedInstances;
    }

    public readonly ServerApi serverApi;
    public readonly IHkVizLogger logger;
    public readonly AuthManager authManager;
    public readonly IRecordingManager recordingManager;
    public readonly IUploadManager uploadManager;
    public readonly VersionChecker versionChecker;

    public HkVizSharedInstances(
        IHkVizLogger logger,
        Func<ServerApi, AuthManager, IHkVizLogger, IUploadManager> createUploadManager,
        IRecordingManager recordingManager
    ) {
        this.logger = logger;
        this.recordingManager = recordingManager;
        serverApi = new ServerApi(logger);
        authManager = new AuthManager(serverApi, logger);
        uploadManager = createUploadManager(serverApi, authManager, logger);
        versionChecker = new VersionChecker(serverApi, logger);
    }
    
    public void Initialize() {
        uploadManager.Initialize();
        versionChecker.Initialize();
        
        BehaviourManager.Instance.gameObject.AddComponent<HkVizIMGUI>();
    }
}
