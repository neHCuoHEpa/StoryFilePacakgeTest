using System.Collections;
using System.Collections.Generic;

using UnityEngine;


public class RotateBillboard : MonoBehaviour
{
    public Transform target;
    public float minAngle;
    public float maxAngle;
    public bool useLimits;
    public bool relativeToPosition;


    void Start()
    {
    }

    void Update()
    {
        if (target != null)
        {
            transform.LookAt(target, Vector3.up);
            var angles = transform.eulerAngles;
            angles.x = 0;
            if (useLimits)
            {
                angles.y = Mathf.Clamp(angles.y, minAngle, maxAngle);
            }
            angles.z = 0;
            transform.eulerAngles = angles;
        }
    }

    void OnDrawGizmosSelected()
    {
        float rayRange = 10.0f;
        Quaternion leftRayRotation = Quaternion.AngleAxis(minAngle, Vector3.up);
        Quaternion rightRayRotation = Quaternion.AngleAxis(maxAngle, Vector3.up);

        //Vector3 leftRayDirection = leftRayRotation * Vector3.forward;
        //Vector3 rightRayDirection = rightRayRotation * Vector3.forward;
        //Gizmos.color = Color.green;
        //Gizmos.DrawRay(transform.position, leftRayDirection * rayRange);
        //Gizmos.color = Color.blue;
        //Gizmos.DrawRay(transform.position, rightRayDirection * rayRange);
    }
}
