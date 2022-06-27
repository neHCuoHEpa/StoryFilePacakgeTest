using System.Collections;
using System.Collections.Generic;
using Arcturus.Volumetric;
using StoryFiles;
using UnityEngine;

public class HoloVideoPanel : MonoBehaviour, IVideoPanel
{
    public HoloStreamPlayer waitingPlayer;
    public HoloStreamPlayer answerPlayer;
    public HoloStreamPlayer noAnswerPlayer;

    private string waitingVideoURL = "";
    private bool isWaitingVideoLoaded = false;

    private string noAnswerVideoURL = "";
    private bool isNoAnswerVideoLoaded = false;

    void Awake()
    {
        waitingPlayer.onClipLoaded.AddListener(ClipLoaded);
        waitingPlayer.onPlayStateChanged.AddListener(WaitingPlayerStatusChanged);
        answerPlayer.onClipLoaded.AddListener(AnswerClipLoaded);
    }

    void Update()
    {

    }

    public void PlayAnswerVideo(string url, string metastring = "")
    {
        answerPlayer.gameObject.SetActive(true);
        answerPlayer.LoadURI(url);
    }

    public void PlayNoAnswerVideo()
    {
        answerPlayer.Stop();
        noAnswerPlayer.gameObject.SetActive(true);
        if (isNoAnswerVideoLoaded)
        {
            waitingPlayer.Stop();
            noAnswerPlayer.Play();
        }
        else
        {
            waitingPlayer.Play();
        }
    }

    public void PlayWaitingVideo()
    {
        //answerPlayer.Stop();
        //noAnswerPlayer.Stop();
        if (isWaitingVideoLoaded)
        {
            waitingPlayer.Play();
        }
        else
        {
            print("start Load " + waitingVideoURL);
            waitingPlayer.LoadURI(waitingVideoURL);
            waitingPlayer.gameObject.SetActive(true);
            print("start Load 2");
        }

    }

    public void Setup(string waitingVideoURL, string noAnswerVideoURL, string waitingMetastring = "", string noAnswerMetastring = "")
    {
        isWaitingVideoLoaded = false;
        isNoAnswerVideoLoaded = false;
        this.waitingVideoURL = "https://holostream-dev.arcturus.studio/media/4434a211-ec04-46d4-86ac-2cf9420d8b96/stream/dash/manifest.mpd";//waitingVideoURL;
        this.noAnswerVideoURL = noAnswerVideoURL;
    }

    public void SetVolume(float volume)
    {
        
    }

    private void ClipLoaded(float index)
    {
        print(index);

    }

    private void AnswerClipLoaded(float index)
    {
        waitingPlayer.Stop();
        waitingPlayer.gameObject.SetActive(false);
        noAnswerPlayer.gameObject.SetActive(false);
    }

    private void WaitingPlayerStatusChanged(OMSPlayer.State state)
    {
        print(state);
    }
}
