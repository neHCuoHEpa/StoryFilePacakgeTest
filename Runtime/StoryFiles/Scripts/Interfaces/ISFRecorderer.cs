using System;
using UnityEngine.Events;

namespace StoryFiles
{
    public interface ISFRecorderer
    {
        VoiceDataUpdatedEvent DataReadyEvent { get; }

        void StartRecording();
        void StopRecording();
    }

    [System.Serializable]
    public class VoiceDataUpdatedEvent : UnityEvent<byte[]> { }
}
