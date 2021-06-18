using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class TankMove : MonoBehaviour
{
    public GameObject body;
    public GameObject canon;
    public GameObject canon_barrel;
    public GameObject effect;
    public Transform shootOrigin;

    public GameObject shotEffect;

    public bool reloading = false;
    public float distance = -1f;

    public float moveSpeed = 1f;
    public float rotationSpeed = 75f;
    public float reloadSpeed = 10000000000000000000000000000000000000f;
    public float force = 50f;

    public float timer = 0f;

    public Vector3 canonOffset;

    public Canvas canvas;
    public Slider slider;

    // Start is called before the first frame update
    void Start()
    {
        canonOffset = canon_barrel.transform.localPosition;

        moveSpeed *= Time.fixedDeltaTime;
        rotationSpeed *= Time.fixedDeltaTime;
        reloadSpeed *= Time.fixedDeltaTime;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (timer > 0f) timer -= Time.deltaTime;

        if (reloading)
        {
            canon_barrel.transform.position += canon.transform.forward * 0.001f;

            float remaingDistance = Vector3.Distance(canon_barrel.transform.localPosition, canonOffset);
            if (distance != -1 && remaingDistance > distance)
            {
                canon_barrel.transform.localPosition = canonOffset;
                reloading = false;
                distance = -1f;
            }
            else
            {
                distance = remaingDistance;
            }
        }

        int move = 0;
        int rot = 0;
        if (Input.GetKey(KeyCode.W)) move += 1;
        if (Input.GetKey(KeyCode.S)) move -= 1;
        if (Input.GetKey(KeyCode.A)) rot -= 1;
        if (Input.GetKey(KeyCode.D)) rot += 1;

        if (Input.GetKey(KeyCode.Space))
        {
            if (timer <= 0f)
            {
                Debug.Log("Space!");
                GetComponent<Rigidbody2D>().AddForce(body.transform.forward * force);
                timer = 5f;
            }
        }

        transform.position += move * transform.forward * moveSpeed;
        transform.Rotate(new Vector3(0f, rotationSpeed * rot, 0f));

        canvas.transform.position = transform.position - new Vector3(0f, 0f, 0.2f);
        canvas.transform.eulerAngles = new Vector3(90f, 0f, 0f);

        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        worldPosition.y = transform.position.y;
        Vector3 dir = worldPosition - transform.position;
        Debug.DrawRay(transform.position, dir, Color.red);

        canon.transform.rotation = Quaternion.LookRotation(dir, transform.up);

        if (Input.GetKey(KeyCode.Mouse0) && !reloading)
        {
            reloading = true;
            canon_barrel.transform.position -= canon.transform.forward * 0.05f;
            Instantiate(shotEffect, shootOrigin.position, shootOrigin.rotation);
            Instantiate(effect, worldPosition, Quaternion.identity);
        }
    }
}
