using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFRoomManager : MonoBehaviour
{

    //VideoPanel[] videoPanels;

    //public VideoPanel currentVideo;

    //void Start()
    //{
    //    videoPanels = GameObject.FindObjectsOfType<VideoPanel>();
    //    LoadStoryFiles();
    //}

    //private void Update()
    //{
    //    if (currentVideo)
    //    {
            
    //    }
    //}

    //private void OnEnable()
    //{
    //    SFEventManager.StartListening(SFEventManager.SFEventType.VideoPlayed, VideoPlayed);
    //}

    //private void OnDisable()
    //{
    //    SFEventManager.StopListening(SFEventManager.SFEventType.VideoPlayed, VideoPlayed);
    //}

    //void VideoPlayed(VideoPanel videoPanel)
    //{
    //    if (currentVideo && currentVideo != videoPanel)
    //    {
    //        currentVideo.Unload();
    //    }
    //    this.currentVideo = videoPanel;
    //}

    //void LoadStoryFiles()
    //{
    //    APIManager.shared.LoadAllStoryFiles((loadedStoryFiles) =>
    //    {
    //        var i = 0;
    //        print("loading");
    //        while (i < videoPanels.Length)
    //        {
    //            if (loadedStoryFiles[i].id == 1961)
    //            {
    //                i++;
    //                continue;
    //            }
    //            if (loadedStoryFiles.Length > i)
    //            {
    //                if (videoPanels[i].StoryFileID == -1)
    //                {
    //                    videoPanels[i].LoadData(loadedStoryFiles[i].id, false);
    //                }
    //                i++;
    //            }
    //            else
    //            {
    //                break;
    //            }
    //        }
    //    });
    //}
}
