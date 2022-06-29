using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;


namespace StoryFiles
{
    
    [Serializable]
    public class ConversationalStarters
    {
        public string name;
        public Question[] question;
    }

    [Serializable]
    public class Question
    {
        public int id;
        public string text;
        public int conversationStarterSortOrder;
        public string conversationStarterTag;
    }

    [Serializable]
    public class AnswerData
    {
        public Answer data;
    }

    [Serializable]
    public class Answer
    {
        public string result;
        public string question_transcription;
        public string message;
        public AnswerVideo video;
    }

    [Serializable]
    public class AnswerVideo
    {
        public string transcription;
        public string[] url;

    }

    [Serializable]
    public class SpeachAnswerData
    {
        public SpeachAnswer data;

    }

    [Serializable]
    public class SpeachAnswer: IDisposable
    {
        public string text;
        public bool isFinal;

        public void Dispose(){}
    }

    [Serializable]
    public class QuestionData
    {
        public string transcription;
        public int userId;
        public int quality;
        public string sessionId;
    }

    // Migration
    [Serializable]
    public class DialogueStartRequest
    {
        public int storyfileId;
    }

    [Serializable]
    public class DialogueStartResponse
    {
        public string sessionId;
    }

    [Serializable]
    public class DialogueInteractRequest
    {
        public string sessionId;
        public string userUtterance;
        public string lang;
        public string clientId;
        public string quality;
    }

    [Serializable]
    public class DialogueInteractResponse
    {
        public string type;
        public int prob;
        public object value;
        public string actionType;
        public long timestamp;
        public string sessionId;
        public string source;
        public string topicLabel;
        public VideoAnswer video;
        //public string[] subtitles: Subtitle[]
        //public InkLine[] conversationStarters?: InkLine[]
        public string newCannotAnswerVideoUrl;
        public string newIdleVideoUrl;

        public string videoDepthMetadata;
    }

    [Serializable]
    public class DialogueConfigResponse
    {
        public string id;
        public string username;
        public string cannotAnswerVideo;
        public string introVideo;
        public string waitingVideo;

        public string waitingVideoDepthMetadata;
        public string cannotAnswerVideoDepthMetadata;
    }

    [Serializable]
    public class VideoAnswer
    {
        public long id;
        public string transcription;
        public string url;
    }
}