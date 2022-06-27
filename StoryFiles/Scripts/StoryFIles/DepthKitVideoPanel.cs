using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using RenderHeads.Media.AVProVideo;
using UnityEngine.Networking;

#if SF_USING_DEPTHKIT
using Depthkit;
#endif


namespace StoryFiles
{
    public class DepthKitVideoPanel : MonoBehaviour, IVideoPanel
    {
        // UI elements
        public MediaPlayer waitingMediaPlayer;
        public MediaPlayer noAnswerMediaPlayer;
        public MediaPlayer answerMediaPlayer;

#if SF_USING_DEPTHKIT
        public Depthkit_Clip waitingClip;
        public Depthkit_Clip noAnswerClip;
        public Depthkit_Clip answerClip;
#endif

        private string waitingVideoURL = "";
        private bool isWaitingVideoLoaded = false;

        private string noAnswerVideoURL = "";
        private bool isNoAnswerVideoLoaded = false;

        private void Awake()
        {
            waitingMediaPlayer.Events.AddListener(OnMediaPlayerEvent);
            noAnswerMediaPlayer.Events.AddListener(OnMediaPlayerEvent);
            answerMediaPlayer.Events.AddListener(OnMediaPlayerEvent);
        }


        private void OnDestroy()
        {
            waitingMediaPlayer.Events.RemoveAllListeners();
            answerMediaPlayer.Events.RemoveAllListeners();
            noAnswerMediaPlayer.Events.RemoveAllListeners();
        }

        private void OnEnable()
        { 
            
        }

        private void OnDisable()
        {
#if SF_USING_AVPRO1
            waitingMediaPlayer.CloseVideo();
            noAnswerMediaPlayer.CloseVideo();
            answerMediaPlayer.CloseVideo();
#endif
#if SF_USING_AVPRO2
            waitingMediaPlayer.CloseMedia();
            noAnswerMediaPlayer.CloseMedia();
            answerMediaPlayer.CloseMedia();
#endif
#if SF_USING_AVPRO1

#endif
        }

        public void Setup(string waitingVideoURL, string noAnswerVideoURL, string waitingMetastring = "", string noAnswerMetastring = "")
        {
            isWaitingVideoLoaded = false;
            isNoAnswerVideoLoaded = false;
            this.waitingVideoURL = waitingVideoURL;
            this.noAnswerVideoURL = noAnswerVideoURL;
            
            var wmetadata = new TextAsset(waitingMetastring);
            var nmetadata = new TextAsset(noAnswerMetastring);
#if SF_USING_DEPTHKIT
            if (wmetadata  != null)
                waitingClip.Setup(AvailablePlayerType.AVProVideo, RenderType.Photo, wmetadata);
            if (nmetadata != null)
                noAnswerClip.Setup(AvailablePlayerType.AVProVideo, RenderType.Photo, nmetadata);
#endif
        }

        /// <summary>
        /// Video control
        /// </summary>
        public void PlayWaitingVideo()
        {
            answerMediaPlayer.Stop();
            noAnswerMediaPlayer.Stop();
            waitingMediaPlayer.gameObject.SetActive(true);
            Debug.Log(waitingVideoURL);
            if (isWaitingVideoLoaded)
            {
                waitingMediaPlayer.Play();
            }
            else
            {
#if SF_USING_AVPRO1
                waitingMediaPlayer.OpenVideoFromFile(MediaPlayer.FileLocation.AbsolutePathOrURL, waitingVideoURL, true);
#endif
#if SF_USING_AVPRO2
                waitingMediaPlayer.OpenMedia(MediaPathType.AbsolutePathOrURL, waitingVideoURL, true);
#endif
            }
        }

        public void PlayNoAnswerVideo()
        {
            answerMediaPlayer.Stop();
            noAnswerMediaPlayer.gameObject.SetActive(true);
            if (isNoAnswerVideoLoaded)
            {
                waitingMediaPlayer.Pause();
                noAnswerMediaPlayer.Play();
            }
            else
            {
                waitingMediaPlayer.Play();
#if SF_USING_AVPRO1
                waitingMediaPlayer.OpenVideoFromFile(MediaPlayer.FileLocation.AbsolutePathOrURL, noAnswerVideoURL, true);
#endif
#if SF_USING_AVPRO2
                noAnswerMediaPlayer.OpenMedia(MediaPathType.AbsolutePathOrURL, noAnswerVideoURL, true);
#endif
            }
        }

        public void PlayAnswerVideo(string url, string metastring = "")
        {
            answerMediaPlayer.gameObject.SetActive(true);
#if SF_USING_AVPRO1
            answerMediaPlayer.OpenVideoFromFile(MediaPlayer.FileLocation.AbsolutePathOrURL, url, true);
#endif
#if SF_USING_AVPRO2
            answerMediaPlayer.OpenMedia(MediaPathType.AbsolutePathOrURL, url, true);
#endif
            if (metastring != "")
            {
#if SF_USING_DEPTHKIT
                var metadata = new TextAsset(metastring);
                answerClip.Setup(AvailablePlayerType.AVProVideo, RenderType.Photo, metadata);
#endif
            }
        }

        public void SetVolume(float volume)
        {
            waitingMediaPlayer.Control.SetVolume(volume);
            noAnswerMediaPlayer.Control.SetVolume(volume);
            answerMediaPlayer.Control.SetVolume(volume);
        }

        public void OnMediaPlayerEvent(MediaPlayer mp, MediaPlayerEvent.EventType et, ErrorCode errorCode)
        {
            switch (et)
            {
                case MediaPlayerEvent.EventType.FirstFrameReady:
                    if (mp == answerMediaPlayer)
                    {
                        waitingMediaPlayer.Pause();
                        waitingMediaPlayer.gameObject.SetActive(false);
                        noAnswerMediaPlayer.gameObject.SetActive(false);
#if SF_USING_AVPRO1
                        SFEventManager.TriggerEvent(SFEventType.VideoStarted, SFEventVideoType.Answer, mp.m_VideoPath);
#endif
#if SF_USING_AVPRO2
                        SFEventManager.TriggerEvent(SFEventType.VideoStarted, SFEventVideoType.Answer, mp.MediaPath);
#endif
                    }
                    if (mp == noAnswerMediaPlayer)
                    {
                        isNoAnswerVideoLoaded = true;
                        waitingMediaPlayer.Pause();
                        waitingMediaPlayer.gameObject.SetActive(false);
                        answerMediaPlayer.gameObject.SetActive(false);
                        SFEventManager.TriggerEvent(SFEventType.VideoStarted, SFEventVideoType.NoAnswer);
                    }
                    if (mp == waitingMediaPlayer)
                    {
                        isWaitingVideoLoaded = true;
                        noAnswerMediaPlayer.gameObject.SetActive(false);
                        answerMediaPlayer.gameObject.SetActive(false);
                        SFEventManager.TriggerEvent(SFEventType.VideoStarted, SFEventVideoType.Waiting);
                    }
                    break;
                case MediaPlayerEvent.EventType.FinishedPlaying:
                    if (mp == answerMediaPlayer || mp == noAnswerMediaPlayer)
                    {
                        PlayWaitingVideo();
                        answerMediaPlayer.gameObject.SetActive(false);
                        SFEventManager.TriggerEvent(SFEventType.VideoEnded);
                    }

                    break;
                case MediaPlayerEvent.EventType.Error:
                    print(Helper.GetErrorMessage(errorCode));
                    PlayWaitingVideo();
                    break;
                default:
                    break;
            }
        }
    }

}