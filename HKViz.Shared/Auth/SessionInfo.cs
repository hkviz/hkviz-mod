using System;

namespace HKViz.Shared.Auth;

[Serializable]
internal class SessionInfo {
    public string id;
    public string name;
    public UserInfo? user;

}