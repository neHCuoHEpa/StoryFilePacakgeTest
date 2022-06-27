using System.Collections;
using System.Collections.Generic;
using SoarSDK;
using StoryFiles;
using UnityEngine;

public class SOARVideoPanel : MonoBehaviour, IVideoPanel
{
    public VolumetricRender waitingPlayback;
    public VolumetricRender noAnswerPlayback;
    public VolumetricRender answerPlayback;

    private int noAnswerPlaybackInstanceIndex = -1;
    private int answerPlaybackInstanceIndex = -1;

    private bool isAnswerPlayed = false;
    private bool isNoAnswerPlayed = false;

    void Start()
    {
        waitingPlayback.autoLoop = true;
        noAnswerPlayback.autoLoop = false;
        answerPlayback.autoLoop = false;

        PlayWaitingVideo();
    }

    void Update()
    {

        if (answerPlaybackInstanceIndex == -1)
        {
            var instance = answerPlayback.GetComponent<PlaybackInstance>();
            print(instance);
            if (instance != null) answerPlaybackInstanceIndex = answerPlayback.instanceRef.IndexOf(instance);
        }
        else
        {
            print(answerPlayback.Instance.PlaybackState);
            print(answerPlayback.GetInstanceState(answerPlaybackInstanceIndex));
            if (isAnswerPlayed && noAnswerPlayback.GetInstanceState(answerPlaybackInstanceIndex) == PlaybackState.READY)
            {
                waitingPlayback.gameObject.SetActive(false);
            }
            if (isAnswerPlayed && answerPlayback.Instance.IsFinished)
            {
                PlayWaitingVideo();
                isAnswerPlayed = false;
            }
        }

        if (noAnswerPlaybackInstanceIndex == -1)
        {
            var instance = noAnswerPlayback.GetComponent<PlaybackInstance>();
            print(instance);
            if (instance != null) noAnswerPlaybackInstanceIndex = noAnswerPlayback.instanceRef.IndexOf(instance);
        }
        else
        {
            print(noAnswerPlayback.Instance.PlaybackState);
            print(noAnswerPlayback.GetInstanceState(noAnswerPlaybackInstanceIndex));
            if (isNoAnswerPlayed && noAnswerPlayback.GetInstanceState(noAnswerPlaybackInstanceIndex) == PlaybackState.READY)
            {
                waitingPlayback.gameObject.SetActive(false);
            }
            if (isNoAnswerPlayed && noAnswerPlayback.Instance.IsFinished)
            {
                PlayWaitingVideo();
                isNoAnswerPlayed = false;
            }
        }
    }

    public void PlayAnswerVideo(string url, string metastring = "")
    {
        isAnswerPlayed = true;
        answerPlayback.gameObject.SetActive(true);
        var index = answerPlayback.instanceRef.IndexOf(answerPlayback.GetComponent<PlaybackInstance>());
        Debug.Log(index);
        answerPlayback.StartPlayback(0);
    }

    public void PlayNoAnswerVideo()
    {
        isNoAnswerPlayed = true;
        noAnswerPlayback.gameObject.SetActive(true);
        var index = noAnswerPlayback.instanceRef.IndexOf(noAnswerPlayback.GetComponent<PlaybackInstance>());
        noAnswerPlayback.StartPlayback(index);
    }

    public void PlayWaitingVideo()
    {
        waitingPlayback.gameObject.SetActive(true);
        noAnswerPlayback.gameObject.SetActive(false);
        answerPlayback.gameObject.SetActive(false);
        waitingPlayback.StartPlayback(0);
    }

    public void Setup(string waitingVideoURL, string noAnswerVideoURL, string waitingMetastring = "", string noAnswerMetastring = "")
    {
        
    }

    public void SetVolume(float volume)
    {
        
    }

}
