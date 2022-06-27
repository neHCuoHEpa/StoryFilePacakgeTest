using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OculusPlayer : MonoBehaviour
{

    //public SFRoomManager roomManager;

    //public List<Mural> closestMurals = new List<Mural>();
    //public Mural lookedMural;

    //public GameObject uiHelper;
    //public Transform forwodDirection;

    //bool looking = false;
    //bool recording = false;

    //void Start()
    //{
    //}

    //int layerMask = 1 << 8;
    //// Update is called once per frame
    //void Update()
    //{

    //    Debug.DrawRay(forwodDirection.position, forwodDirection.TransformDirection(Vector3.forward) * 100, Color.red);
    //    RaycastHit hit;
    //    if (Physics.Raycast(forwodDirection.position, forwodDirection.TransformDirection(Vector3.forward), out hit, Mathf.Infinity, layerMask))
    //    {
    //        lookedMural = hit.collider.transform.parent.GetComponent<Mural>();
    //        print(lookedMural.gameObject.name);
    //        Debug.DrawRay(forwodDirection.position, forwodDirection.TransformDirection(Vector3.forward) * hit.distance, Color.blue);
    //    }

    //    if (roomManager.currentVideo)
    //    {
            
    //        var distance = Vector3.Distance(transform.position, roomManager.currentVideo.transform.position);
    //        var volume = Mathf.Clamp01(3 - distance);
    //        roomManager.currentVideo.SetVolume(volume);
    //    }

    //    if (closestMurals.Contains(lookedMural))
    //    {
    //        lookedMural.lighter.material.SetColor("_EmissionColor", Color.green);
    //        uiHelper.SetActive(true);
    //        if (OVRInput.GetDown(OVRInput.RawButton.A))
    //        {
    //            recording = true;
    //            lookedMural.videoPanel.PushToTalkPointedDown();
    //            lookedMural.recording.StartRecord();
    //        }
    //    }
    //    else
    //    {
    //        uiHelper.SetActive(false);
    //        lookedMural.lighter.material.SetColor("_EmissionColor", Color.white);
    //    }

    //    if (recording)
    //    {
    //        if (OVRInput.GetUp(OVRInput.RawButton.A))
    //        {
    //            lookedMural.videoPanel.PushToTalkPointedUp();
    //            lookedMural.recording.StopRecord();
    //            recording = false;
    //        }
    //    }
    //}

    //void OnTriggerEnter(Collider other)
    //{
    //    closestMurals.Add(other.GetComponent<Mural>());
    //}

    //void OnTriggerExit(Collider other)
    //{
    //    lookedMural.lighter.material.SetColor("_EmissionColor", Color.white);
    //    closestMurals.Remove(other.GetComponent<Mural>());
    //}
}
