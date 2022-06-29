using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;


namespace StoryFiles
{
    public class SFEventManager : MonoBehaviour
    {

        Dictionary<string, StoryFileEvent> eventDictionary;

        static SFEventManager eventManager;

        public static SFEventManager Instance
        {
            get
            {
                if (!eventManager)
                {
                    eventManager = FindObjectOfType<SFEventManager>();

                    if (!eventManager)
                    {
                        var go = new GameObject("SFEventManager");
                        eventManager = go.AddComponent<SFEventManager>();
                    }

                    eventManager.Init();
                }

                return eventManager;
            }
        }

        void Init()
        {
            if (eventDictionary == null)
            {
                eventDictionary = new Dictionary<string, StoryFileEvent>();
            }
        }

        public static void StartListening(SFEventType eventType, UnityAction<object[]> listener)
        {
            StoryFileEvent thisEvent = null;
            if (Instance.eventDictionary.TryGetValue(eventType.ToString(), out thisEvent))
            {
                thisEvent.AddListener(listener);
            }
            else
            {
                thisEvent = new StoryFileEvent();
                thisEvent.AddListener(listener);
                Instance.eventDictionary.Add(eventType.ToString(), thisEvent);
            }
        }

        public static void StopListening(SFEventType eventType, UnityAction<object[]> listener)
        {
            if (eventManager == null) return;
            StoryFileEvent thisEvent = null;
            if (Instance.eventDictionary.TryGetValue(eventType.ToString(), out thisEvent))
            {
                thisEvent.RemoveListener(listener);
            }
        }

        public static void TriggerEvent(SFEventType eventType, params object[] args)
        {
            StoryFileEvent thisEvent = null;
            if (Instance.eventDictionary.TryGetValue(eventType.ToString(), out thisEvent))
            {
                thisEvent.Invoke(args);
            }
        }
    }
}