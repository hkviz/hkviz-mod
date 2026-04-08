using System;
using System.Collections;
using System.Text;
using HKViz.Shared.Json;
using UnityEngine.Networking;

namespace HKViz.Shared;

public class ServerApi(
    Action<string> log
) {

    public IEnumerator ApiPost<TBody, TResponse>(string path, TBody body, Action<TResponse> onSuccess, Action<UnityWebRequest> onError) {
        var json = HkVizJson.Stringify(body);
        var url = HkVizSharedConstants.API_URL + path + HkVizSharedConstants.API_URL_SUFFIX;

        var request = new UnityWebRequest(url, "POST");
        byte[] bodyRaw = Encoding.UTF8.GetBytes(json);
        request.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");
        return WrapWWW(request, onSuccess, onError);
    }

    public IEnumerator ApiGet<TResponse>(string path, Action<TResponse> onSuccess, Action<UnityWebRequest> onError) {
        var url = HkVizSharedConstants.API_URL + path + HkVizSharedConstants.API_URL_SUFFIX;
        log(url);
        var request = UnityWebRequest.Get(url);
        return WrapWWW(request, onSuccess, onError);
    }

    public IEnumerator ApiDelete<TResponse>(string path, Action<TResponse> onSuccess, Action<UnityWebRequest> onError) {
        var url = HkVizSharedConstants.API_URL + path + HkVizSharedConstants.API_URL_SUFFIX;
        log(url);
        var request = UnityWebRequest.Delete(url);
        return WrapWWW(request, onSuccess, onError);
    }


    private IEnumerator WrapWWW<TResponse>(UnityWebRequest request, Action<TResponse> onSuccess, Action<UnityWebRequest> onError) {
        yield return request.SendWebRequest();
        log("Status Code: " + request.responseCode);

        if (request.result != UnityWebRequest.Result.Success) {
            log(request.error);
            log(request.downloadHandler.text);
            onError(request);
        } else {
            var result = HkVizJson.Parse<TResponse>(request.downloadHandler.text) ?? throw new Exception("No response data");
            onSuccess(result);
        }

        request.uploadHandler?.Dispose();
        request.downloadHandler?.Dispose();
        request.Dispose();
    }

    public IEnumerator R2Upload(string signedUploadUrl, string filePath, Action onSuccess, Action<UnityWebRequest> onError) {
        using (var request = new UnityWebRequest(signedUploadUrl, UnityWebRequest.kHttpVerbPUT)) {
            request.uploadHandler = new UploadHandlerFile(filePath);
            yield return request.SendWebRequest();
            log("r2 upload result " + request.result);
            if (request.result == UnityWebRequest.Result.Success) {
                onSuccess();
            } else {
                onError(request);
            }
        }
    }
}
