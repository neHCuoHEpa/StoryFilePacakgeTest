using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;


namespace StoryFiles
{
    public class SFRecorderer : MonoBehaviour, ISFRecorderer
    {
        private string micName = null;

        private VoiceDataUpdatedEvent dataReadyEvent = new VoiceDataUpdatedEvent();

        [HideInInspector]
        public VoiceDataUpdatedEvent DataReadyEvent { get => dataReadyEvent; }


        void Start()
        {
            if (Application.platform == RuntimePlatform.Android && Microphone.devices.Length > 0)
            {
                micName = Microphone.devices[0];
            }
        }

        public void StartRecording()
        {
            if (!Application.HasUserAuthorization(UserAuthorization.Microphone))
            {
                return;
            }

            recordingClip = Microphone.Start(micName, true, _recordingBufferSize, _recordingHZ);
            sending = true;
            lastSample = 0;
        }

        public void StopRecording()
        {
            sending = false;
            Microphone.End(micName);
        }


        private int _recordingBufferSize = 15;
        private int _recordingHZ = 16000;
        private bool sending = false;
        private int lastSample = 0;
        private float interval = 0;
        private AudioClip recordingClip;
        private void FixedUpdate()
        {
            if (sending)
            {
                if (interval > 0.6f)
                {
                    GetAndSendNewData();
                    interval = 0;
                }
                else
                {
                    interval += Time.fixedDeltaTime;
                }
            }
        }

        private void GetAndSendNewData()
        {
            int pos = Microphone.GetPosition(micName);
            int diff = pos - lastSample;

            if (diff > 0)
            {
                float[] samples = new float[diff * recordingClip.channels];
                recordingClip.GetData(samples, lastSample);

                var sendingClip = AudioClip.Create("clip", samples.Length, recordingClip.channels, recordingClip.frequency, false);
                sendingClip.SetData(samples, 0);

                var data = WavUtility.FromAudioClip(sendingClip);
                //ws.SendAudioMessage(data);
                DataReadyEvent.Invoke(data);
            }
            lastSample = pos;
        }
    }
}