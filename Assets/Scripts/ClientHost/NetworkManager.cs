using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkManager : MonoBehaviour
{
    public static NetworkManager instance;

    public GameObject playerPrefab;
    public GameObject missilPrefab;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Debug.Log("Instance already exists, destroying object!");
            Destroy(this);
        }
    }

    private void Start()
    {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 30;

        Server.Start(2, 26950);
        //https://en.wikipedia.org/wiki/List_of_TCP_and_UDP_port_numbers
    }

    private void OnApplicationQuit()
    {
        //TO DO: OnSceneChange
        Server.Stop();
    }

    public Player InstantiatePlayer()
    {
        return Instantiate(playerPrefab, new Vector3(0f, 0f, 0f), Quaternion.identity).GetComponent<Player>();
    }

    public Missil InstantiateMissil(Transform _shootOrigin)
    {
        return Instantiate(missilPrefab, _shootOrigin.position + _shootOrigin.forward*0.25f, Quaternion.identity).GetComponent<Missil>();
    }
}
