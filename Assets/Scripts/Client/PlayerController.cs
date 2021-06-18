using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Transform canon;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            ClientSend.PlayerShoot(canon.forward);
        }
    }

    void FixedUpdate()
    {
        SendInputToServer();

        if (Input.GetKey(KeyCode.Mouse0))
        {
            ClientSend.PlayerShoot(canon.forward);
        }
    }

    private void SendInputToServer()
    {
        bool[] _inputs = new bool[]
        {
            Input.GetKey(KeyCode.W),
            Input.GetKey(KeyCode.S),
            Input.GetKey(KeyCode.A),
            Input.GetKey(KeyCode.D),
            Input.GetKey(KeyCode.Space),
        };

        Vector3 _inputMouse = Input.mousePosition;
        _inputMouse.z = 5;
        Vector3 _mousePosition = Camera.main.ScreenToWorldPoint(_inputMouse);
        _mousePosition.y = transform.position.y;
        Vector3 _shootDirection = _mousePosition - transform.position;

        ClientSend.PlayerMovement(_inputs, _shootDirection);
    }
}
