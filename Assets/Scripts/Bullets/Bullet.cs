using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class Bullet : NetworkBehaviour
{

    [SerializeField] private float bulletDestroyDelay = 2.5f;


    public override void OnNetworkSpawn()
    {
        //if (!IsOwner) return;

    }

    void OnEnable()
    {
        //bulletRB = GetComponent<Rigidbody>();
        //bulletRB.AddForce(bulletMoveDirection * bulletSpeed, ForceMode.Impulse);
        //Debug.Log("DESGRAÇA, Bullet Move Dir: " + bulletMoveDirection);

        Destroy(gameObject, bulletDestroyDelay);
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
