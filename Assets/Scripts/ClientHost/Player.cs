using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public int id;
    public string username;
    public GameObject model;
    public Transform canon;
    public Transform canonBarrel;
    public Transform shootOrigin;
    public Canvas canvas;
    public Text usernameText;
    public Slider healthbar;
    public float moveSpeed = 0.75f;
    public float rotationSpeed = 75f;
    public float throwForce = 0.05f;
    public float boostForce = 100000f;
    public float health;
    public float maxHealth = 100f;

    public GameObject shotEffect;

    private bool[] inputs;

    private bool reload = false;
    private float reloadDistance = -1f;
    private Vector3 canonBarrelLocalPos;

    private float boostTimer = 0f;

    private void Start()
    {
        moveSpeed *= Time.fixedDeltaTime;
        rotationSpeed *= Time.fixedDeltaTime;

        canonBarrelLocalPos = canonBarrel.localPosition;
    }

    public void Initialize(int _id, string _username)
    {
        id = _id;
        username = _username;
        health = maxHealth;

        usernameText.text = username;

        inputs = new bool[5];
    }

    public void FixedUpdate()
    {
        if (health <= 0f) 
        {
            return;
        }

        if (boostTimer > 0f) boostTimer -= Time.deltaTime;

        Reload();

        int move = 0;
        int rot = 0;
        if (inputs[0]) move += 1;
        if (inputs[1]) move -= 1;
        if (inputs[2]) rot -= 1;
        if (inputs[3]) rot += 1;
        if (inputs[4] && boostTimer <= 0f)
        {
            GetComponent<Rigidbody>().AddForce(transform.forward * boostForce);
            boostTimer = 2f;
        }

        transform.position += transform.forward * move * moveSpeed;
        transform.Rotate(new Vector3(0f, rot * rotationSpeed, 0f));

        canvas.transform.eulerAngles = new Vector3(90f, 0f, 0f);

        ServerSend.PlayerPosition(this);
        ServerSend.PlayerRotation(this);
        ServerSend.PlayerCanonRotation(this);
    }

    public void SetInput(bool[] _inputs, Vector3 _shootDirection)
    {
        inputs = _inputs;
        canon.rotation = Quaternion.LookRotation(_shootDirection, transform.up);
    }

    public void Shoot(Vector3 _shootDirection)
    {
        if (health <= 0 || reload)
        {
            return;
        }

        canon.rotation = Quaternion.LookRotation(_shootDirection, transform.up);

        reload = true;
        canonBarrel.position -= canon.forward * 0.05f;
        Instantiate(shotEffect, shootOrigin.position, shootOrigin.rotation);

        NetworkManager.instance.InstantiateMissil(shootOrigin).Initialize(_shootDirection, throwForce, id);

        ServerSend.PlayerShot(this);
    }

    public void TakeDamage(float _damage)
    {
        if (health <= 0f)
        {
            return;
        }

        health -= _damage;
        if (health <= 0f)
        {
            health = 0f;

            model.SetActive(false);
            canvas.enabled = false;

            canonBarrel.localPosition = canonBarrelLocalPos;
            reload = false;
            reloadDistance = -1f;

            boostTimer = 0f;

            transform.position = new Vector3(0f, 1f, 0f);
            transform.rotation = Quaternion.identity;
            GetComponent<Collider>().enabled = false;
            GetComponent<Rigidbody>().isKinematic = true;
            ServerSend.PlayerPosition(this);
            ServerSend.PlayerRotation(this);
            StartCoroutine(Respawn());
        }

        healthbar.value = health / maxHealth;

        ServerSend.PlayerHealth(this);
    }

    private IEnumerator Respawn()
    {
        yield return new WaitForSeconds(5f);

        model.SetActive(true);
        canvas.enabled = true;

        health = maxHealth;
        healthbar.value = health / maxHealth;

        GetComponent<Collider>().enabled = true;
        GetComponent<Rigidbody>().isKinematic = false;
        ServerSend.PlayerRespawned(this);
    }

    private void Reload()
    {
        if (reload)
        {
            canonBarrel.position += canon.forward * 0.001f;

            float remaingDistance = Vector3.Distance(canonBarrel.localPosition, canonBarrelLocalPos);
            if (reloadDistance != -1 && remaingDistance > reloadDistance)
            {
                canonBarrel.localPosition = canonBarrelLocalPos;
                reload = false;
                reloadDistance = -1f;
            }
            else
            {
                reloadDistance = remaingDistance;
            }
        }
    }

}
