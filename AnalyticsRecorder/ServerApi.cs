using Modding;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.Networking;

namespace AnalyticsRecorder {
    internal class ServerApi: Loggable {
        private static ServerApi? instance;
        public static ServerApi Instance {
            get {
                instance ??= new ServerApi();
                return instance;
            }
        }

        public IEnumerator ApiPost<TBody, TResponse>(string path, TBody body, Action<TResponse> onSuccess, Action<UnityWebRequest> onError) {
            var json = Json.Stringify(body);
            var url = Constants.API_URL + path;

            var request = new UnityWebRequest(url, "POST");
            byte[] bodyRaw = Encoding.UTF8.GetBytes(json);
            request.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");
            return WrapWWW(request, onSuccess, onError);
        }

        public IEnumerator ApiGet<TResponse>(string path, Action<TResponse> onSuccess, Action<UnityWebRequest> onError) {
            var url = Constants.API_URL + path;
            Log(url);
            var request = UnityWebRequest.Get(url);
            return WrapWWW(request, onSuccess, onError);
        }

        public IEnumerator ApiDelete<TResponse>(string path, Action<TResponse> onSuccess, Action<UnityWebRequest> onError) {
            var url = Constants.API_URL + path;
            Log(url);
            var request = UnityWebRequest.Delete(url);
            return WrapWWW(request, onSuccess, onError);
        }


        private IEnumerator WrapWWW<TResponse>(UnityWebRequest request, Action<TResponse> onSuccess, Action<UnityWebRequest> onError) {
            yield return request.SendWebRequest();
            Log("Status Code: " + request.responseCode);

            if (request.result != UnityWebRequest.Result.Success) {
                Log(request.error);
                Log(request.downloadHandler.text);
                onError(request);
            } else {
                var result = Json.Parse<TResponse>(request.downloadHandler.text) ?? throw new Exception("No response data");
                onSuccess(result);
            }

            request.uploadHandler.Dispose();
            request.downloadHandler.Dispose();
            request.Dispose();
        }

        public IEnumerator R2Upload(string signedUploadUrl, string filePath, Action onSuccess, Action<UnityWebRequest> onError) {
            using(var request = new UnityWebRequest(signedUploadUrl, UnityWebRequest.kHttpVerbPUT)) {
                request.uploadHandler = new UploadHandlerFile(filePath);
                yield return request.SendWebRequest();
                Log("r2 upload result " + request.result);
                if (request.result == UnityWebRequest.Result.Success) {
                    onSuccess();
                } else {
                    onError(request);
                }
            }
        }
    }
}
