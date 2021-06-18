using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServerCameraControl : MonoBehaviour
{

    public float speed = 0.5f;

    void FixedUpdate()
    {
        int up = 0;
        int right = 0;

        if (Input.GetKey(KeyCode.W)) up += 1;
        if (Input.GetKey(KeyCode.S)) up -= 1;
        if (Input.GetKey(KeyCode.A)) right -= 1;
        if (Input.GetKey(KeyCode.D)) right += 1;

        transform.position += transform.up * up * speed + transform.right * right * speed;
    }
}
