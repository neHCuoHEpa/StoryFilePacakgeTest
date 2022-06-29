using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using TMP;


namespace StoryFiles
{
    public class SFPushToTalk : MonoBehaviour
    {
        // PUBLIC
        public TextMeshProUGUI speachLabel;
        public RectTransform pushButton;
        public RectTransform pushButtonShadow;

        public string holdText = "HOLD TO TALK";

        private bool pushDown = false;

        public float scaleFactor = 2.3f;
        public float time = 0.8f;

        // PRIVATE
        //private string speachText = "";

        private void OnEnable()
        {
            SFEventManager.StartListening(SFEventType.VoiceRecordingUpdated, VoiceUpdated);
            SFEventManager.StartListening(SFEventType.VideoEnded, VideoEnded);

            speachLabel.text = holdText;
        }
        private void OnDisable()
        {
            SFEventManager.StopListening(SFEventType.VoiceRecordingUpdated, VoiceUpdated);
            SFEventManager.StopListening(SFEventType.VideoEnded, VideoEnded);
        }

        private float totalTime = 0;
        private Vector3 scale;
        public void PushToTalkPointedDown()
        {
            totalTime = 0;
            pushDown = true;
            pushButtonShadow.gameObject.SetActive(true);
            scale = new Vector3(1, 1) * scaleFactor;
        }

        public void PushToTalkPointedUp()
        {
            totalTime = 0;
            pushDown = false;
            scale = new Vector3(1, 1);
        }

        private void Update()
        {
            totalTime += Time.deltaTime / time;
            pushButtonShadow.localScale = Vector3.Lerp(pushButtonShadow.localScale, scale, totalTime);
            if (!pushDown && pushButtonShadow.localScale == scale)
            {
                pushButtonShadow.gameObject.SetActive(false);
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
                    speachLabel.text = answer.text;
                }
            }
        }

        private void VideoEnded(object[] args)
        {
            speachLabel.text = holdText;
        }
    }
}