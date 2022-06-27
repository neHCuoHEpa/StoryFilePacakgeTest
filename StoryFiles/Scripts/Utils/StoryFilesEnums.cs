using UnityEngine;
using UnityEngine.Events;

namespace StoryFiles
{
    public enum StarterQuestionsPresenting
    {
        Always,
        OnClick,
        Never
    }

    public enum StarterQuestionsPosition
    {
        LeftOutside,
        LeftInside,
        RightInside,
        RightOutside
    }
    public enum PushToTalkVisibility
    {
        Enable,
        Disable
    }
    public enum StarterQuestionsShowButtonPosition
    {
        Top,
        Bottom
    }


    public enum Environment
    {
        Development,
        Stage,
        Production
    }

    public enum VideoQuality
    {
        qaulity360 = 360,
        qaulity480 = 480,
        qaulity720 = 720,
        qaulity1080 = 1080
    }

    public enum Language
    {
        en,
        es
    }

    [System.Serializable]
    public class StoryFileEvent : UnityEvent<object[]>
    {
    }

    public enum SFEventType
    {
        VideoStarted,
        VideoEnded,

        PushToTalkStart,
        PushToTalkEnd,
        VoiceRecordingStart,
        VoiceRecordingUpdated,
        VoiceRecordingEnd
    }

    public enum SFEventVideoType
    {
        Waiting,
        NoAnswer,
        Answer
    }
}