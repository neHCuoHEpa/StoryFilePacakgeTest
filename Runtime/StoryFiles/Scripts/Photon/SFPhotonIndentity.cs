using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StoryFiles
{
    using System.Linq;
#if PHOTON_UNITY_NETWORKING

    using Photon.Pun;

    [RequireComponent(typeof(PhotonView))]
    public class SFPhotonIndentity : MonoBehaviour
    {
        private PhotonView photonView;

        private List<SFBillboard> billboards;

        private void Awake()
        {
            photonView = GetComponent<PhotonView>();
            billboards = FindObjectsOfType<SFBillboard>().ToList();
        }

        void Start()
        {
            if (photonView.IsMine == false && PhotonNetwork.IsConnected == true)
            {
                return;
            }

            SFEventManager.StartListening(SFEventType.VoiceRecordingUpdated, VoiceUpdated);
            SFEventManager.StartListening(SFEventType.VideoStarted, VideoStarted);
        }

        private void VoiceUpdated(object[] args)
        {
            if (args.Length > 0)
            {
                using (var answer = args[0] as SpeachAnswer)
                {
                    photonView.RPC("RPCUpdateSpeachTextAll", RpcTarget.Others, answer.text);
                }
            }
        }

        [PunRPC]
        private void RPCUpdateSpeachTextAll(string id, string text)
        {
            var billboard = billboards.FirstOrDefault(b => b.storyFileID == id);
            if (billboard != null)
            {
                //Debug.Log("All speach: " + text);
                if (photonView.IsMine)
                {
                    //Debug.Log("Mine speach: " + text);
                    //billboard.SetRecSepachText(text);
                    var answer = new SpeachAnswer();
                    answer.text = text;
                    answer.isFinal = false;
                    SFEventManager.TriggerEvent(SFEventType.VoiceRecordingUpdated, new object[] { answer });
                }
            }
        }

        private void VideoStarted(object[] args)
        {
            if (args.Length > 0 && args[0] is SFEventVideoType)
            {
                var type = (SFEventVideoType)args[0];
                var ti = (int)type;
                switch (type)
                {
                    case SFEventVideoType.Answer:
                        photonView.RPC("RPCVideoStart", RpcTarget.Others, ti, args[1]);
                        break;
                    case SFEventVideoType.NoAnswer:
                        photonView.RPC("RPCVideoStart", RpcTarget.Others, ti, "");
                        break;
                    case SFEventVideoType.Waiting:
                        break;
                }
            }
        }

        [PunRPC]
        private void RPCVideoStart(int type, string url)
        {
            var billboard = billboards.FirstOrDefault(b => b.storyFileID == "1");
            var realType = (SFEventVideoType)type;

            billboard.PlayVideo(realType, url);
        }
    }
#endif
}
