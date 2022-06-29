using UnityEngine;
using System.Collections;
using UnityEngine.Events;

namespace StoryFiles
{
    [RequireComponent(typeof(WSCommunication))]
    public class SFRecoredererCoordinator : MonoBehaviour
    {
        private ISFRecorderer recorderer;
        private WSCommunication communication;

        private SpeachAnswer lastAnswer;
        private bool recordingOnProggress = false;


        void Awake()
        {
            communication = GetComponent<WSCommunication>();
            recorderer = GetComponent<SFRecorderer>();
            
#if PHOTON_UNITY_NETWORKING
            var photonRecorderer = FindObjectOfType<SFPhotonRecorderer>();
            
            if (photonRecorderer != null)
            {
                recorderer = photonRecorderer;
            }
                
#endif

            SFEventManager.StartListening(SFEventType.VoiceRecordingUpdated, VoiceUpdated);
            recorderer.DataReadyEvent.AddListener(VoiceDataUpdated);
        }

        public void StartRecording()
        {
            recordingOnProggress = true;
            communication.SendStartMessage();
            recorderer.StartRecording();
        }

        public void StopRecording()
        {
            recordingOnProggress = false;
            StartCoroutine(WatingForLastAnswer());
        }

        private IEnumerator WatingForLastAnswer()
        {
            while (recordingOnProggress == true || (lastAnswer != null && lastAnswer.isFinal == false))
            {
                yield return null;
            }

            communication.SendStopMessage();
            recorderer.StopRecording();
        }

        private void VoiceUpdated(object[] args)
        {
            if (args.Length > 0)
            {
                using (var answer = args[0] as SpeachAnswer) {
                    lastAnswer = answer;
                }
            }
        }

        private void VoiceDataUpdated(byte[] data)
        {
            communication.SendAudioMessage(data);
        }
    }
}
