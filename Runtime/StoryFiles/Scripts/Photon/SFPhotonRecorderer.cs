using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;


#if PHOTON_UNITY_NETWORKING
using Photon.Voice.Unity;
using Photon.Voice;

namespace StoryFiles
{
    [RequireComponent(typeof(Recorder))]
    [DisallowMultipleComponent]
    public class SFPhotonRecorderer : VoiceComponent, ISFRecorderer
    {

        private int clipCount = 0;

        private Buffering buffering = new Buffering();

        private VoiceInfo voiceInfo;

        private Recorder recorder;

        private VoiceDataUpdatedEvent dataReadyEvent = new VoiceDataUpdatedEvent();

        [HideInInspector]
        public VoiceDataUpdatedEvent DataReadyEvent { get => dataReadyEvent; }

        private void Start()
        {
            recorder = GetComponent<Recorder>();
        }

        private void Update()
        {
            
            if (buffering.buffer.Length > 20000)
            {
                var newBuffer = resample(buffering.buffer, voiceInfo.SamplingRate, 16000);

                var sendingClip = AudioClip.Create("clip", newBuffer.Length, voiceInfo.Channels, 16000, false);
                sendingClip.SetData(newBuffer, 0);

                clipCount++;

                var data = WavUtility.FromAudioClip(sendingClip);
                //ws.SendAudioMessage(data);
                DataReadyEvent.Invoke(data);
                buffering.buffer = new float[0];
            }
        }

        private float[] resample(float[] buffer, int currentRate, int rate)
        {
            if (rate == currentRate)
            {
                return buffer;
            }
            if (rate > currentRate)
            {
                return buffer;
            }
            float sampleRateRatio = currentRate * 1.0f / rate;
            var newLength = Mathf.RoundToInt(buffer.Length / sampleRateRatio);
            var result = new float[newLength];
            var offsetResult = 0;
            var offsetBuffer = 0f;
            while (offsetResult < result.Length)
            {
                var nextOffsetBuffer = Mathf.Round((offsetResult + 1) * sampleRateRatio);
                // Use average value of skipped samples
                float accum = 0;
                var count = 0;
                for (var i = offsetBuffer; i < nextOffsetBuffer && i < buffer.Length; i++)
                {
                    accum += buffer[(int)i];
                    count++;
                }
                result[offsetResult] = accum / count;
                // Or you can simply get rid of the skipped samples:
                // result[offsetResult] = buffer[nextOffsetBuffer];
                offsetResult++;
                offsetBuffer = nextOffsetBuffer;
            }
            return result;
        }

        private void PhotonVoiceCreated(PhotonVoiceCreatedParams photonVoiceCreatedParams)
        {
            voiceInfo = photonVoiceCreatedParams.Voice.Info;

            if (photonVoiceCreatedParams.Voice is LocalVoiceAudioFloat)
            {
                LocalVoiceAudioFloat localVoiceAudioFloat = photonVoiceCreatedParams.Voice as LocalVoiceAudioFloat;
                localVoiceAudioFloat.AddPreProcessor(new SFOutgoingStreamFloat(buffering));
            }
            else if (photonVoiceCreatedParams.Voice is LocalVoiceAudioShort)
            {
                LocalVoiceAudioShort localVoiceAudioShort = photonVoiceCreatedParams.Voice as LocalVoiceAudioShort;
                localVoiceAudioShort.AddPreProcessor(new SFOutgoingStreamShort());
            }
        }

        public void StartRecording()
        {
            recorder.TransmitEnabled = true;
        }

        public void StopRecording()
        {
            recorder.TransmitEnabled = false;
        }

        class Buffering
        {
            public float[] buffer = new float[0];
            public float[] total = new float[0];
        }

        class SFOutgoingStreamFloat : IProcessor<float>
        {
            Buffering buffering;

            public SFOutgoingStreamFloat(Buffering b)
            {
                buffering = b;
            }

            public float[] Process(float[] buf)
            {
                buffering.buffer = buffering.buffer.Concat(buf).ToArray();
                buffering.total = buffering.total.Concat(buf).ToArray();
                return buf;
            }

            public void Dispose()
            {
            }
        }

        class SFOutgoingStreamShort : IProcessor<short>
        {
            public short[] Process(short[] buf)
            {
                return buf;
            }

            public void Dispose()
            {
            }
        }
    }
}
#endif