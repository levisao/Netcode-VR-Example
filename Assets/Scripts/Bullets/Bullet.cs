using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class Bullet : NetworkBehaviour
{
    private Camera playerCamera;


    public Vector3 bulletMoveDirection;

    [SerializeField] private float bulletSpeed = 2;

    //public Transform directionTransform;
    void Start()
    {
        playerCamera = Camera.main;
    }

    void Update()
    {
        transform.position += bulletMoveDirection * bulletSpeed * Time.deltaTime;
    }

    private void OnCollisionEnter(Collision collision)
    {
            Debug.Log("BALA COLIDIU");
        if (collision.transform.tag == "Player" && !IsOwner)
        {
            // take damage
        }
    }
}
