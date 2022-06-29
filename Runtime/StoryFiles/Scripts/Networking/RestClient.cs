using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;


namespace StoryFiles
{
    public class RestClient : MonoBehaviour
    {

        public IEnumerator Get<T>(string url, string token = null, Action<T, string> completion = null)
        {
            if (StoryFilesInfo.CheckDebug(StoryFilesInfo.DebugLevel.Logs, StoryFilesInfo.DebugComponent.REST))
            {
                Debug.Log("---------------- URL ----------------");
                Debug.Log(url);
            }
            

            using (UnityWebRequest request = UnityWebRequest.Get(url))
            {

                request.SetRequestHeader("Content-Type", "application/json");
                request.SetRequestHeader("Accept", "application/json;v=1.0.3");
                if (token != null)
                {
                    request.SetRequestHeader("Authorization", token);
                }

                yield return request.SendWebRequest();

                if (request.error != null)
                {
                    if (StoryFilesInfo.CheckDebug(StoryFilesInfo.DebugLevel.Errors, StoryFilesInfo.DebugComponent.REST))
                    {
                        Debug.Log("---------------- ERROR ----------------");
                        Debug.LogError(request.error);
                    }
                    completion(default(T), request.error);
                }
                else if (request.isDone)
                {
                    Debug.Log("---------------- Response Raw ----------------");
                    var data = request.downloadHandler.data;
                    var json = Encoding.UTF8.GetString(data);
                    Debug.Log(json);

                    if (StoryFilesInfo.CheckDebug(StoryFilesInfo.DebugLevel.Logs, StoryFilesInfo.DebugComponent.REST))
                    {
                        Debug.Log("---------------- Response JSON ----------------");
                        Debug.Log(json);
                    }
                    var result = JsonUtility.FromJson<T>(json);
                    completion(result, null);
                }
            }
        }

        public IEnumerator Post<T, R>(string url, T p, string token = null, Action<R, string> completion = null)
        {
            if (StoryFilesInfo.CheckDebug(StoryFilesInfo.DebugLevel.Logs, StoryFilesInfo.DebugComponent.REST))
            {
                Debug.Log("---------------- URL ----------------");
                Debug.Log(url);
            }

            var sendJson = JsonUtility.ToJson(p);
            var sendData = Encoding.UTF8.GetBytes(sendJson);

            if (StoryFilesInfo.CheckDebug(StoryFilesInfo.DebugLevel.Logs, StoryFilesInfo.DebugComponent.REST))
            {
                Debug.Log("---------------- BODY ----------------");
                Debug.Log(sendJson);
            }

            UnityWebRequest request = new UnityWebRequest();

            request.url = url;
            request.method = "POST";
            request.downloadHandler = new DownloadHandlerBuffer();
            request.uploadHandler = new UploadHandlerRaw(sendData);

            request.SetRequestHeader("Content-Type", "application/json");
            request.SetRequestHeader("Accept", "application/json;v=1.0.3");
            if (token != null)
            {
                request.SetRequestHeader("Authorization", token);
            }

            yield return request.SendWebRequest();

            if (request.error != null)
            {
                if (StoryFilesInfo.CheckDebug(StoryFilesInfo.DebugLevel.Errors, StoryFilesInfo.DebugComponent.REST))
                {
                    Debug.Log("---------------- ERROR ----------------");
                    Debug.LogError(request.error);
                }
                completion(default(R), request.error);
            }
            else if (request.isDone)
            {
                if (StoryFilesInfo.CheckDebug(StoryFilesInfo.DebugLevel.Logs, StoryFilesInfo.DebugComponent.REST))
                {
                    Debug.Log("---------------- Response Raw ----------------");
                    Debug.Log(request.downloadHandler.data);
                }
                var data = request.downloadHandler.data;
                var json = Encoding.UTF8.GetString(data);

                if (StoryFilesInfo.CheckDebug(StoryFilesInfo.DebugLevel.Logs, StoryFilesInfo.DebugComponent.REST))
                {
                    Debug.Log("---------------- Response JSON ----------------");
                    Debug.Log(json);
                }
                var result = JsonUtility.FromJson<R>(json);
                completion(result, null);
            }
        }

    }
}