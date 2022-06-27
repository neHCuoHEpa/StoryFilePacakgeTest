using UnityEngine;
using System.Collections;
using static StoryFiles.StoryFilesInfo;
using System;

namespace StoryFiles
{
    public class SFBillboard : MonoBehaviour
    {
        
        public string storyFileID = "";
        public VideoQuality quality = VideoQuality.qaulity1080;
        public PushToTalkVisibility showPushToTalkButton = PushToTalkVisibility.Enable;
        public StarterQuestionsPresenting showStarterQuestion;
        public StarterQuestionsPosition starterQuestionPosition;
        public StarterQuestionsShowButtonPosition starterQuestionShowButtonPosition;

        public Environment environment = Environment.Production;

        [SerializeField]
        public DebugLevel debugLevelFlag;
        [SerializeField]
        public DebugComponent debugComponentsFlag;

        [SerializeField]
        public int debugComponentsFlagInt;

        private string sessionId;

        private IVideoPanel videoPanel;
        private SFPushToTalk pushToTalkButton;
        private StarterQuestionsPanel starterQuestionsPanel;
        private SFRecoredererCoordinator recorderer;

        private string speachRecorded = "";


        private void Awake()
        {
            videoPanel = GetComponentInChildren<IVideoPanel>(true);
            recorderer = GetComponentInChildren<SFRecoredererCoordinator>();
            pushToTalkButton = GetComponentInChildren<SFPushToTalk>(true);
            starterQuestionsPanel = GetComponentInChildren<StarterQuestionsPanel>();

            if (starterQuestionsPanel != null)
            {
                starterQuestionsPanel.sfBillboard = this;
                starterQuestionsPanel.gameObject.SetActive(showStarterQuestion == StarterQuestionsPresenting.Always);
            }
        }

        void Start()
        {
            var id = -1;
            if (int.TryParse(storyFileID, out id))
            {
                if (id > -1)
                {
                    SFAPIManager.Instance.DialogueStart(environment, id, (response) =>
                    {
                        this.sessionId = response.sessionId;
                    });
                    SFAPIManager.Instance.DialogueConfig(environment, id, (response) =>
                    {
                        //print(response.waitingVideoDepthMetadata);
                        SetupUser(response);
                    });

                }
            }

        }

        private void OnEnable()
        {
            SFEventManager.StartListening(SFEventType.VoiceRecordingUpdated, VoiceUpdated);
            SFEventManager.StartListening(SFEventType.VoiceRecordingEnd, VoiceRecorded);
        }


        public void StartVoiceAsk()
        {
            if (recorderer != null)
            {
                recorderer.StartRecording();
            }
        }

        public void StopVoiceAsk()
        {
            if (recorderer != null)
            {
                recorderer.StopRecording();
            }
        }

        /// <summary>
        /// EVENTS
        /// </summary>
        private void VoiceUpdated(object[] args)
        {
            if (args.Length > 0)
            {
                using (var answer = args[0] as SpeachAnswer)
                {
                    speachRecorded = answer.text;
                }
            }
        }
        private void VoiceRecorded(object[] args)
        {
            Debug.Log(speachRecorded);
            if (speachRecorded != "")
            {
                Answer(speachRecorded);
                speachRecorded = "";
            }
        }

        /// <summary>
        /// SETUP
        /// </summary>
        public void SetupUser(DialogueConfigResponse response)
        {
            videoPanel.Setup(response.waitingVideo, response.cannotAnswerVideo, response.waitingVideoDepthMetadata, response.cannotAnswerVideoDepthMetadata);
            videoPanel.PlayWaitingVideo();
        }

        /// <summary>
        /// Ansewrs
        /// </summary>
        public void Answer(Question question)
        {
            Answer(question.text);
        }

        public void Answer(string question)
        {
            videoPanel.PlayWaitingVideo();
            SFAPIManager.Instance.DialogueInteract(environment, sessionId, question, quality, (response) =>
            {
                Debug.Log(response.video.url);
                if (response == null || response.video == null)
                {
                    videoPanel.PlayNoAnswerVideo();
                    return;
                }
                videoPanel.PlayAnswerVideo(response.video.url, response.videoDepthMetadata);
            });
        }

        public void LoadStarterQuestion()
        {
            //if (showStarterQuestion != StarterQuestionsPresenting.Never && starterQuestionsPanel != null)
            //{
            //    starterQuestionsPanel.SetupStarterQuestions(user.conversationalStarters.question);
            //}
        }

        public void PlayVideo(SFEventVideoType type, string url)
        {
            switch (type)
            {
                case SFEventVideoType.Answer:
                    videoPanel.PlayAnswerVideo(url);
                    break;
                case SFEventVideoType.NoAnswer:
                    videoPanel.PlayNoAnswerVideo();
                    break;
                case SFEventVideoType.Waiting:
                    videoPanel.PlayWaitingVideo();
                    break;
            }
        }
    }
}