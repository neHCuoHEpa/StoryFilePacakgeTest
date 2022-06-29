using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StoryFiles
{
    public interface IVideoPanel
    {
        void Setup(string waitingVideoURL, string noAnswerVideoURL, string waitingMetastring = "", string noAnswerMetastring = "");
        void PlayWaitingVideo();
        void PlayNoAnswerVideo();
        void PlayAnswerVideo(string url, string metastring = "");
        void SetVolume(float volume);
    }
}
