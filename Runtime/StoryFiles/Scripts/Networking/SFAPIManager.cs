using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StoryFiles
{
    [RequireComponent(typeof(RestClient))]
    public class SFAPIManager : MonoBehaviour
    {

        private static SFAPIManager instance;
        public static SFAPIManager Instance
        {
            get
            {
                if (instance == null)
                {
                    var go = new GameObject("SFAPIManager");
                    var rc = go.AddComponent<RestClient>();
                    instance = go.AddComponent<SFAPIManager>();
                    instance.restClient = rc;
                }
                return instance;
            }
        }

        private RestClient restClient;

        // Migration
        public void DialogueConfig(Environment environment, int id, System.Action<DialogueConfigResponse> callback)
        {
            var url = EnvironmentManager.ConfigUrl(environment, id);

            StartCoroutine(restClient.Get<DialogueConfigResponse>(url, null, (response, error) =>
            {
                if (response != null)
                {
                    callback(response);
                }
                else
                {
                    if (StoryFilesInfo.CheckDebug(StoryFilesInfo.DebugLevel.Errors, StoryFilesInfo.DebugComponent.StoryFiles))
                    {
                        Debug.Log($"Storyfile cannot load: {error}");
                    }
                }
                
            }));
        }

        public void DialogueStart(Environment environment, int id, System.Action<DialogueStartResponse> callback)
        {
            var url = EnvironmentManager.DialogStartUrl(environment);
            var param = new DialogueStartRequest();
            param.storyfileId = id;

            StartCoroutine(restClient.Post<DialogueStartRequest, DialogueStartResponse>(url, param, null, (response, error) =>
            {
                callback(response);
            }));
        }

        public void DialogueInteract(Environment environment, string session, string question, VideoQuality quality, System.Action<DialogueInteractResponse> callback)
        {
            var url = EnvironmentManager.InteractUrl(environment);
            var param = new DialogueInteractRequest();
            param.sessionId = session;
            param.quality = quality.GetHashCode().ToString();
            param.userUtterance = question;
            param.lang = "en";
            param.clientId = "unityplugin";


            StartCoroutine(restClient.Post<DialogueInteractRequest, DialogueInteractResponse>(url, param, null, (response, error) =>
            {
                callback(response);
            }));

        }
    }
}