using HKViz.Websockets.NativeWebSocket;
using Modding;
using System;
using UnityEngine;

namespace HKViz.Live {
    internal class HKVizLiveManager: Loggable {
        private RecordingFileManager recording = RecordingFileManager.Instance;
        private LiveDataBuffer liveDataBuffer = LiveDataBuffer.Instance;
        private UploadManager uploadManager = UploadManager.Instance;

        private static HKVizLiveManager? _instance;
        public static HKVizLiveManager Instance {
            get {
                if (_instance != null) return _instance;
                _instance = new HKVizLiveManager();
                return _instance;
            }
        }

        private WebSocket? websocket = null;

        private float lastWebsockerSendTime = 0;
        private float websocketSendAfterSeconds = 10; // 5 minutes

        public void Send<T> (T message) {
            if (websocket is null) {
                LogError("Tried sending without websocket being ready");
                return;
            }
            websocket.SendText(JsonUtility.ToJson(message));
        }

        public void StartStreaming() {
            if (websocket is not null) return;
            Log("Start Streaming");

            var url = "ws://127.0.0.1:8787/websocket?mode=host&accessKey=abc";
            websocket = new WebSocket(url);
            Log(url);

            websocket.OnOpen += () =>
            {
                Log("Connection open!");
                Send(new HostSwitchFileMessage(recording.currentPart));
                liveDataBuffer.IsLive = true;


                // TODO either swap file or read to upload
            };

            websocket.OnError += (e) =>
            {
                Log("Error! " + e);
            };

            websocket.OnClose += (e) =>
            {
                Log("Connection closed!");
                liveDataBuffer.IsLive = false;
                websocket = null;
            };

            _ = websocket.Connect();
        }

        public void StopStreaming() {
            Log("Stop Streaming");
            if (websocket is not null) {
                _ = websocket.Close();
                liveDataBuffer.IsLive = false;
                websocket = null;
            }
        }

        internal void Init() {
            Log("Init");
            recording.AfterSwitchedFile += AfterSwitchedFile;
            recording.AfterCloseLastSessionFile += AfterCloseLastSessionFile;
            uploadManager.UploadPartFinished += UploadPartFinished;
        }


        internal void AfterUpdate() {
            // TODO pause when game is paused
            if (!doLiveWork()) return;
            // only this check should not be done when paused, the other events below might happen when paused,
            // and must not be ignored.
            if (GameManager.instance.isPaused) return;
            if ((Time.unscaledTime - lastWebsockerSendTime) > websocketSendAfterSeconds) {
                SendBufferContents();
            }
        }

        private void SendBufferContents() {
            Log("Send Buffer Contents");
            var content = liveDataBuffer.GetContentsAndClear();

            // data is not send when its empty, but the last time is still set,
            // since otherwise we would send with the first write, but some delay there is fine.
            lastWebsockerSendTime = Time.unscaledTime;
            if (string.IsNullOrEmpty(content)) return;
            Send(new HostAppendDataMessage(content));
        }

        private void AfterCloseLastSessionFile() {
            StopStreaming();
        }

        private void AfterSwitchedFile() {
            if (!doLiveWork()) return;
            var part = recording.currentPart;
            SendBufferContents();
            Log("Mark switch part");
            Send(new HostSwitchFileMessage(part));
        }

        private void UploadPartFinished(int part) {
            if (!doLiveWork()) return;
            Log("Mark finished part");
            Send(new HostMarkFilePartUploadedMessage(part));
        }


        private bool doLiveWork() {
            return recording.isRecording && liveDataBuffer.IsLive;
        }
    }
}
