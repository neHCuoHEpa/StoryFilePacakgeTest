using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

#if SF_USING_ARFOUNDATION
using UnityEngine.XR.ARFoundation;
#endif

public class ARController : MonoBehaviour
{

#if SF_USING_ARFOUNDATION
    public ARRaycastManager raycastManager;
    public ARPlaneManager planeManager;

    List<ARRaycastHit> hits = new List<ARRaycastHit>();

    public GameObject targetCircle;

    public GameObject billboardAlphaPrefab;
    public GameObject billboardDepthKitPrefab;

    public GameObject initialHelpTest;
    public GameObject movementHelpTest;

    private GameObject billboard;
    private bool isBillboardSet = false;

    public bool isDepthKit = false;
    public Image dkButton;
    public Image alButton;

    private Pose lastPose;

    void Start()
    {
        raycastManager = GetComponent<ARRaycastManager>();
        planeManager = GetComponent<ARPlaneManager>();

        alButton.color = isDepthKit ? Color.white : Color.blue;

        targetCircle.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        //if (Input.touchCount > 0 && Input.touches[0].phase == TouchPhase.Began)
        //{
        //    PlaceBillboard(Input.GetTouch(0).position);
        //}
        if (isBillboardSet == false)
        {
            if (IsPointerOverUIObject())
            {
                return;
            }

            if (Input.touchCount > 0 && Input.touches[0].phase == TouchPhase.Began && targetCircle.activeSelf == false)
            {

                initialHelpTest.SetActive(false);
                movementHelpTest.SetActive(true);
                targetCircle.SetActive(true);
                PlaceTargetPosition(Input.GetTouch(0).position);
            }

            if (Input.touchCount > 0 && Input.touches[0].phase == TouchPhase.Moved)
            {
                PlaceTargetPosition(Input.GetTouch(0).position);
            }
        }
        
    }

    void PlaceTargetPosition(Vector2 touchPosition)
    {
        if (raycastManager.Raycast(touchPosition, hits, UnityEngine.XR.ARSubsystems.TrackableType.Planes))
        {
            lastPose = hits[0].pose;
            targetCircle.transform.position = lastPose.position;
            //targetCircle.transform.rotation = lastPose.rotation;
        }
    }

    public void PlaceBillboard()
    {
        initialHelpTest.SetActive(false);
        movementHelpTest.SetActive(false);
        targetCircle.SetActive(false);

        billboard = Instantiate(billboardDepthKitPrefab, lastPose.position, Quaternion.Euler(0, -90, 0));
        billboard.transform.LookAt(Camera.main.transform);
        billboard.transform.Rotate(new Vector3(0, 1), 180);
        billboard.transform.eulerAngles = new Vector3(0, billboard.transform.eulerAngles.y, 0);
        billboard.transform.localScale *= 2f;

        //billboard = Instantiate(billboardAlphaPrefab, lastPose.position, Quaternion.identity);
        //billboard.GetComponentInChildren<RotateBillboard>().target = Camera.main.transform;
        //billboard.transform.localScale *= 0.3f;

        DisablePlanes();
        isBillboardSet = true;
    }

    void PlaceBillboard(Vector2 touchPosition)
    {
        if (raycastManager.Raycast(touchPosition, hits, UnityEngine.XR.ARSubsystems.TrackableType.Planes))
        {
            var pose = hits[0].pose;

            if (billboard == null)
            {
                if (isDepthKit)
                {
                    billboard = Instantiate(billboardDepthKitPrefab, pose.position, Quaternion.identity);
                }
                else
                {
                    billboard = Instantiate(billboardAlphaPrefab, pose.position, Quaternion.identity);
                    billboard.GetComponentInChildren<RotateBillboard>().target = Camera.main.transform;
                    billboard.transform.localScale *= 0.3f;
                }
                
                
                DisablePlanes();
                //var c = GameObject.CreatePrimitive(PrimitiveType.Cube);
                //c.transform.position = pose.position;
                //c.transform.rotation = pose.rotation;
            }
            else
            {
                //billboard.transform.position = pose.position;
                //billboard.transform.rotation = pose.rotation;
            }
        }
    }

    public void ChangeBillboardType(bool dk)
    {
        isDepthKit = dk;
        alButton.color = dk ? Color.white : Color.blue;
        dkButton.color = dk ? Color.blue : Color.white;

        if (billboard != null)
        {
            DestroyImmediate(billboard);
            billboard = null;
        }
    }

    void DisablePlanes()
    {
        //List<ARPlane> allPlanes = new List<ARPlane>();

        var plane = planeManager.GetPlane(hits[0].trackableId);
        planeManager.enabled = false;
        plane.gameObject.SetActive(false);
    }

    private bool IsPointerOverUIObject()
    {
        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
        eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
        return results.Count > 0;
    }

#endif
}
