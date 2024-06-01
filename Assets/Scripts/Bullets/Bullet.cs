using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class Bullet : NetworkBehaviour
{
    public Vector3 bulletMoveDirection;

    private Rigidbody bulletRB;

    [SerializeField] private float bulletSpeed = 2;

    //public Transform directionTransform;
    void Start()
    {
        bulletRB = GetComponent<Rigidbody>();

        bulletRB.AddForce(bulletMoveDirection * bulletSpeed, ForceMode.Impulse);
    }

    void Update()
    {
        //transform.position += bulletMoveDirection * bulletSpeed * Time.deltaTime;
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
