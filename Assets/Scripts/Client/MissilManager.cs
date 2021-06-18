using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissilManager : MonoBehaviour
{
    public int id;
    public GameObject explosionPrefab;

    public void Initialize(int _id)
    {
        id = _id;
    }

    public void Explode(Vector3 _position)
    {
        transform.position = _position;
        Instantiate(explosionPrefab, transform.position, Quaternion.identity);
        GameManager.missils.Remove(id);
        Destroy(gameObject);
    }
}
