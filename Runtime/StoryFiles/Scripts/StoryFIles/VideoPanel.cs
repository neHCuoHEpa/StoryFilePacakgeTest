using UnityEngine;

#if SF_USING_AVPRO2
using RenderHeads.Media.AVProVideo;
#endif

namespace StoryFiles
{
    public class VideoPanel : MonoBehaviour, IVideoPanel
    {
        // UI elements
#if SF_USING_AVPRO2
        public MediaPlayer waitingMediaPlayer;
        public MediaPlayer noAnswerMediaPlayer;
        public MediaPlayer answerMediaPlayer;

        public DisplayUGUI waitingDisplayGUI;
        public DisplayUGUI noAnswerDisplayGUI;
        public DisplayUGUI answerDisplayGUI;
#endif

        private string waitingVideoURL = "";
        private bool isWaitingVideoLoaded = false;

        private string noAnswerVideoURL = "";
        private bool isNoAnswerVideoLoaded = false;

        public float videoTransitionTime = 0.7f;


        private VideoState waitingVideoState = VideoState.Idle;
        private VideoState answerVideoState = VideoState.Idle;
        private VideoState noAnswerVideoState = VideoState.Idle;
        private float waitingTimeElapsed = 0;
        private float answerTimeElapsed = 0;
        private float noanswerTimeElapsed = 0;

        private Color clear = new Color(1, 1, 1, 0);

        enum VideoState
        {
            Idle,
            FadeIn,
            FadeOut
        }

#if SF_USING_AVPRO2
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

            waitingDisplayGUI.color = Color.black;
            noAnswerDisplayGUI.color = clear;
            answerDisplayGUI.color = clear;

            waitingVideoState = VideoState.Idle;
            answerVideoState = VideoState.Idle;
            noAnswerVideoState = VideoState.Idle;
        }
#endif

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
        }

        private void Update()
        {
#if SF_USING_AVPRO2
            float alpha = 0;
            if (waitingVideoState != VideoState.Idle)
            {
                if ((waitingVideoState == VideoState.FadeOut && waitingDisplayGUI.color.a == 0) ||
                    (waitingVideoState == VideoState.FadeIn && waitingDisplayGUI.color.a == 1))
                {
                    waitingVideoState = VideoState.Idle;
                    waitingTimeElapsed = 0;
                }
                else
                {
                    alpha = AlphaFor(waitingVideoState, waitingTimeElapsed);
                    waitingTimeElapsed += Time.deltaTime;
                    waitingDisplayGUI.color = new Color(1, 1, 1, alpha);
                    if (alpha < 0 || alpha > 1)
                    {
                        waitingVideoState = VideoState.Idle;
                        waitingTimeElapsed = 0;
                    }
                }
            }

            if (answerVideoState != VideoState.Idle)
            {
                if ((answerVideoState == VideoState.FadeOut && answerDisplayGUI.color.a == 0) ||
                    (answerVideoState == VideoState.FadeIn && answerDisplayGUI.color.a == 1))
                {
                    answerVideoState = VideoState.Idle;
                    noanswerTimeElapsed = 0;
                }
                else
                {
                    alpha = AlphaFor(answerVideoState, answerTimeElapsed);
                    answerTimeElapsed += Time.deltaTime;
                    answerDisplayGUI.color = new Color(1, 1, 1, alpha);
                    if (alpha < 0 || alpha > 1)
                    {
                        answerVideoState = VideoState.Idle;
                        answerTimeElapsed = 0;
                    }
                }
            }

            if (noAnswerVideoState != VideoState.Idle)
            {
                if ((noAnswerVideoState == VideoState.FadeOut && noAnswerDisplayGUI.color.a == 0) ||
                    (noAnswerVideoState == VideoState.FadeIn && noAnswerDisplayGUI.color.a == 1))
                {
                    noAnswerVideoState = VideoState.Idle;
                    noanswerTimeElapsed = 0;
                }
                else
                {
                    alpha = AlphaFor(noAnswerVideoState, noanswerTimeElapsed);
                    noanswerTimeElapsed += Time.deltaTime;
                    noAnswerDisplayGUI.color = new Color(1, 1, 1, alpha);
                    if (alpha < 0 || alpha > 1)
                    {
                        noAnswerVideoState = VideoState.Idle;
                        noanswerTimeElapsed = 0;
                    }
                }
            }
#endif
        }

        float AlphaFor(VideoState state, float time)
        {
            switch (state)
            {
                case VideoState.Idle:
                    return 0;
                case VideoState.FadeIn:
                    return Mathf.Pow(time / videoTransitionTime, 0.25f);
                case VideoState.FadeOut:
                    return 1 - Mathf.Pow(time / videoTransitionTime, 0.25f);
            }
            return 0;
        }

        public void Setup(string waitingVideoURL, string noAnswerVideoURL, string waitingMetastring = "", string noAnswerMetastring = "")
        {
            isWaitingVideoLoaded = false;
            isNoAnswerVideoLoaded = false;
            this.waitingVideoURL = waitingVideoURL;
            this.noAnswerVideoURL = noAnswerVideoURL;
        }

        /// <summary>
        /// Video control
        /// </summary>
        public void PlayWaitingVideo()
        {
#if SF_USING_AVPRO2
            answerMediaPlayer.Stop();
            noAnswerMediaPlayer.Stop();
            if (isWaitingVideoLoaded)
            {
                waitingMediaPlayer.Play();
                waitingVideoState = VideoState.FadeIn;
                noAnswerVideoState = VideoState.FadeOut;
                answerVideoState = VideoState.FadeOut;
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
#endif
        }

        public void PlayNoAnswerVideo()
        {
#if SF_USING_AVPRO2
            answerMediaPlayer.Stop();
            if (isNoAnswerVideoLoaded)
            {
                waitingMediaPlayer.Pause();
                noAnswerMediaPlayer.Play();
                waitingVideoState = VideoState.FadeOut;
                noAnswerVideoState = VideoState.FadeIn;
            }
            else
            {
                waitingVideoState = VideoState.FadeIn;
                waitingMediaPlayer.Play();
                noAnswerMediaPlayer.transform.parent.gameObject.SetActive(true);
#if SF_USING_AVPRO1
                waitingMediaPlayer.OpenVideoFromFile(MediaPlayer.FileLocation.AbsolutePathOrURL, noAnswerVideoURL, true);
#endif
#if SF_USING_AVPRO2
                noAnswerMediaPlayer.OpenMedia(MediaPathType.AbsolutePathOrURL, noAnswerVideoURL, true);
#endif
            }
#endif
        }

        public void PlayAnswerVideo(string url, string metafile = "")
        {
            if (!string.IsNullOrEmpty(url))
            {
#if SF_USING_AVPRO1
                waitingMediaPlayer.OpenVideoFromFile(MediaPlayer.FileLocation.AbsolutePathOrURL, url, true);
#endif
#if SF_USING_AVPRO2
                answerMediaPlayer.OpenMedia(MediaPathType.AbsolutePathOrURL, url, true);
#endif
            }
        }

        public void SetVolume(float volume)
        {
#if SF_USING_AVPRO2
            waitingMediaPlayer.Control.SetVolume(volume);
            noAnswerMediaPlayer.Control.SetVolume(volume);
            answerMediaPlayer.Control.SetVolume(volume);
#endif
        }

#if SF_USING_AVPRO2
        public void OnMediaPlayerEvent(MediaPlayer mp, MediaPlayerEvent.EventType et, ErrorCode errorCode)
        {
            switch (et)
            {
                case MediaPlayerEvent.EventType.FirstFrameReady:
                    if (mp == answerMediaPlayer)
                    {
                        waitingMediaPlayer.Pause();
                        waitingVideoState = VideoState.FadeOut;
                        answerVideoState = VideoState.FadeIn;
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
                        waitingVideoState = VideoState.FadeOut;
                        noAnswerVideoState = VideoState.FadeIn;
                        SFEventManager.TriggerEvent(SFEventType.VideoStarted, SFEventVideoType.NoAnswer);
                    }
                    if (mp == waitingMediaPlayer)
                    {
                        isWaitingVideoLoaded = true;
                        waitingDisplayGUI.color = Color.white;
                        SFEventManager.TriggerEvent(SFEventType.VideoStarted, SFEventVideoType.Waiting);
                    }
                    break;
                case MediaPlayerEvent.EventType.FinishedPlaying:
                    if (mp == answerMediaPlayer || mp == noAnswerMediaPlayer)
                    {
                        PlayWaitingVideo();
                        SFEventManager.TriggerEvent(SFEventType.VideoEnded);
                    }

                    break;
                case MediaPlayerEvent.EventType.Error:
                    PlayWaitingVideo();
                    break;
                default:
                    break;
            }
        }
#endif
    }

}