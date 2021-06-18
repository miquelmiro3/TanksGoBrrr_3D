using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerManager : MonoBehaviour
{
    public int id;
    public string username;
    public float health;
    public float maxHealth = 100f;
    public GameObject model;
    public Transform canon;
    public Transform canonBarrel;
    public Transform shootOrigin;
    public Canvas canvas;
    public Text usernameText;
    public Slider healthbar;

    public GameObject shotEffect;

    private bool reload = false;
    private float reloadDistance = -1f;
    private Vector3 canonBarrelLocalPos;

    private void Start()
    {
        canonBarrelLocalPos = canonBarrel.localPosition;
    }

    private void FixedUpdate()
    {
        Reload();
    }

    public void Initialize(int _id, string _username)
    {
        id = _id;
        username = _username;
        health = maxHealth;

        usernameText.text = username;
    }

    public void SetHealth(float _health)
    {
        health = _health;

        if (health <= 0)
        {
            Die();
        }
        else
        {
            healthbar.value = health / maxHealth;
        }
    }

    public void Die()
    {
        canonBarrel.localPosition = canonBarrelLocalPos;
        reload = false;
        reloadDistance = -1f;

        model.SetActive(false);
        canvas.enabled = false;
    }

    public void Respawn()
    {
        model.SetActive(true);
        canvas.enabled = true;
        SetHealth(maxHealth);
    }

    public void RotateCanon(Quaternion _canonRotation)
    {
        if (health <= 0f)
        {
            return;
        }

        canon.rotation = _canonRotation;
    }

    public void Shoot() 
    {
        reload = true;
        canonBarrel.position -= canon.forward * 0.05f;

        Instantiate(shotEffect, shootOrigin.position, shootOrigin.rotation);
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
