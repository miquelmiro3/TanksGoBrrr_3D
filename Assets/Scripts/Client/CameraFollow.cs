using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;

    public float smoothSpeed = 0.000125f;
    public Vector3 offset;

    private void FixedUpdate()
    {
        if (target != null)
        {
            Vector3 _desiredPosition = target.position + offset;
            Vector3 _smoothPosition = Vector3.Lerp(transform.position, _desiredPosition, smoothSpeed);
            //transform.position = _smoothPosition;
            transform.position = _desiredPosition;
        }
        //transform.LookAt(target);
    }
}
