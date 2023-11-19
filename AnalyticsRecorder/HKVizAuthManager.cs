using MapChanger;
using Modding;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SocialPlatforms.Impl;

namespace AnalyticsRecorder {
    [Serializable]
    internal class InitSessionBodyPayload {
        public string name;
    }

    [Serializable]
    internal class InitSessionResult {
        public string id;
    }

    internal class HKVizAuthManager: Loggable {
        private static HKVizAuthManager instance;

        public static HKVizAuthManager Instance {
            get {
                if (instance == null) {
                    instance = new HKVizAuthManager();
                }
                return instance;
            }
        }


        public void Login() {
            InitSessionToken();
        }

        private void InitSessionToken() {
            GameManager.instance.StartCoroutine(ApiPost<InitSessionBodyPayload, InitSessionResult>(
                path: "ingamesession/init",
                body: new() {
                    name = SystemInfo.deviceName,
                },
                onSuccess: data => {
                    Log("Got session token " + data.id);
                    Application.OpenURL(Constants.LOGIN_URL + data.id);
                },
                onError: request => {
                    Debug.Log(request.error);
                    Debug.Log(request.downloadHandler.text);
                }
            ));
        }

        private IEnumerator ApiPost<TBody, TResponse>(string path, TBody body, Action<TResponse> onSuccess, Action<UnityWebRequest> onError) {
            var json = Json.Stringify(body);
            var url = Constants.API_URL + path;

            //WWWForm form = new WWWForm();
            //form.AddField("data", json);


            // using (var www = UnityWebRequest.Put(url, json)) {
            //using (var www = UnityWebRequest.Post(url, form)) {
            //        // www.method = "POST";
            //    // www.SetRequestHeader("Content-Type", "application/json");
            //    // www.SetRequestHeader("Accept", "application/json");
            //    yield return www.SendWebRequest();

            //    if (www.result != UnityWebRequest.Result.Success) {
            //        Debug.Log(www.error);
            //        Debug.Log(www.downloadHandler.text);
            //        onError(www);
            //    } else {
            //        Log("Got session token " + www.downloadHandler.text);
            //        var result = Json.Parse<TResponse>(www.downloadHandler.text) ?? throw new Exception("No response data");
            //        onSuccess(result);
            //    }
            //}


            var request = new UnityWebRequest(url, "POST");
            //var json = Json.Stringify(body);
            Log(json);
            byte[] bodyRaw = Encoding.UTF8.GetBytes(json);
            request.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");
            yield return request.SendWebRequest();
            Debug.Log("Status Code: " + request.responseCode);

            if (request.result != UnityWebRequest.Result.Success) {
                Debug.Log(request.error);
                Debug.Log(request.downloadHandler.text);
                onError(request);
            } else {
                Log("Got session token " + request.downloadHandler.text);
                var result = Json.Parse<TResponse>(request.downloadHandler.text) ?? throw new Exception("No response data");
                onSuccess(result);
            }

            request.uploadHandler.Dispose();
            request.downloadHandler.Dispose();
            request.Dispose();
        }
    }
}
