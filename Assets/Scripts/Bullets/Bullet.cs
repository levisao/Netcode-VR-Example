using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class Bullet : NetworkBehaviour
{
    public Vector3 bulletMoveDirection;

    private Rigidbody bulletRB;

    [SerializeField] private float bulletSpeed = 2;

    [SerializeField] private float bulletDestroyDelay = 3f;


    public override void OnNetworkSpawn()
    {
        //if (!IsOwner) return;

    }

    void OnEnable()
    {
        //bulletRB = GetComponent<Rigidbody>();
        //bulletRB.AddForce(bulletMoveDirection * bulletSpeed, ForceMode.Impulse);
        //Debug.Log("DESGRA�A, Bullet Move Dir: " + bulletMoveDirection);

        Destroy(gameObject, 3f);
    }
  


    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("BALA COLIDIU");
        if (collision.transform.tag == "Player" && !IsOwner)
        {
            // take damage
            Destroy(gameObject);
        }
    }
}
