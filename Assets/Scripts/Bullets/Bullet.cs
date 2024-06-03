using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class Bullet : MonoBehaviour
{

    //[SerializeField] private float bulletDestroyDelay = 2.5f;
    [SerializeField] private int damage = 5;

    private ulong ownerClientId; /// a very longe int
    public void SetOwner(ulong ownerClientId)
    {
        this.ownerClientId = ownerClientId;
    }



    void OnEnable()
    {
        //bulletRB = GetComponent<Rigidbody>();
        //bulletRB.AddForce(bulletMoveDirection * bulletSpeed, ForceMode.Impulse);
        //Debug.Log("DESGRAÇA, Bullet Move Dir: " + bulletMoveDirection);

        //Destroy(gameObject, bulletDestroyDelay);
    }
  


    private void OnCollisionEnter(Collision collision)
    {
        /// um tip: só lidar com o take damage no serverProjectile (NormalBullet Server) o que não tem mesh
        if (collision.transform.GetComponent<CharacterController>() == null) return;
        Debug.Log("COLIDIU COM RIGIDBODY EM");

        if(collision.transform.TryGetComponent<NetworkObject>(out NetworkObject netObj)) /// pegando o networkobject de quem recebeu a colisao e pegando seu id
        {
            if (ownerClientId == netObj.OwnerClientId) return; /// se o id passado do dono da bullet for igual ao id de quem foi colidido retorna (se colidiu com algo owned by the player)
        }

        if (collision.transform.TryGetComponent<Health>(out Health health)) /// vendo se colidiu com alguém que tenha o Health e pegando esse componente
        {
            health.TakeDamage(damage); /// chamando o método para o dono do Health tomar dano
            Destroy(gameObject);
        }

    }
}
