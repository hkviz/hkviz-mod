using HKViz.Shared;
using Modding;

namespace HKViz;

public class HkVizInstances: Loggable {
    private static HkVizInstances? _instance;
    public static HkVizInstances Instance {
        get {
            _instance ??= new HkVizInstances();
            return _instance;
        }
    }

    public readonly ServerApi serverApi;

    private HkVizInstances() {
        serverApi = new ServerApi(
            log: Log
        );
    }
}
