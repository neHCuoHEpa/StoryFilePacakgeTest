using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NativeWebSocket;


namespace StoryFiles
{
    public class WSCommunication : MonoBehaviour
    {

        WebSocket websocket;

        async void Start()
        {
            websocket = new WebSocket(EnvironmentManager.SpeachSocketUrl(Environment.Production));

            websocket.OnOpen += () =>
            {
                if (StoryFilesInfo.CheckDebug(StoryFilesInfo.DebugLevel.Logs, StoryFilesInfo.DebugComponent.WebSocket))
                {
                    Debug.Log("Websocket Connection open!");
                }
            };

            websocket.OnError += (e) =>
            {
                Debug.Log("Error! " + e);
            };

            websocket.OnClose += (e) =>
            {
                if (StoryFilesInfo.CheckDebug(StoryFilesInfo.DebugLevel.Logs, StoryFilesInfo.DebugComponent.WebSocket))
                {
                    Debug.Log("Websocket Connection closed!");
                }
            };

            websocket.OnMessage += (bytes) =>
            {
            // getting the message as a string
                string message = System.Text.Encoding.UTF8.GetString(bytes);
                var result = JsonUtility.FromJson<SpeachAnswerData>(message);
                SFEventManager.TriggerEvent(SFEventType.VoiceRecordingUpdated, result.data);
            };

            // waiting for messages
            await websocket.Connect();
        }

        private async void OnEnable()
        {
            if (websocket == null) return;
            await websocket.Connect();
        }

        private async void OnDisable()
        {
            if (websocket == null) return;
            await websocket.Close();
        }

        // Update is called once per frame
        void Update()
        {
#if !UNITY_WEBGL || UNITY_EDITOR
            websocket.DispatchMessageQueue();
#endif
        }

        public async void SendStartMessage()
        {
            if (websocket != null && websocket.State == WebSocketState.Open)
            {
                SFEventManager.TriggerEvent(SFEventType.VoiceRecordingStart);
                await websocket.SendText(@"{""type"":""RECOGNIZE_START"",""lang"":""en""}");
            }

        }

        public async void SendStopMessage()
        {
            if (websocket != null && websocket.State == WebSocketState.Open)
            {
                SFEventManager.TriggerEvent(SFEventType.VoiceRecordingEnd);
                await websocket.SendText(@"{""type"":""RECOGNIZE_END""}");
            }
        }

        public async void SendAudioMessage(byte[] ba)
        {
            if (websocket != null && websocket.State == WebSocketState.Open)
            {
                await websocket.Send(ba);
            }

        }

        private async void OnApplicationQuit()
        {
            await websocket.Close();
        }
    }
}