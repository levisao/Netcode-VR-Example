using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit.AffordanceSystem.Receiver.Primitives;
using static UnityEngine.Rendering.DebugUI;

public class Shoot : NetworkBehaviour
{

    //[SerializeField] GameObject bulletPrefab;
    [Header("References:")]
    [SerializeField] private InputActionReference triggerRightHand;
    [SerializeField] private GameObject clientBulletPrefab;
    [SerializeField] private GameObject serverBulletPrefab;
    [SerializeField] private Transform rightHandController;
    [SerializeField] private Collider playerCollider;

    [Header("Settings:")]
    [SerializeField] private float bulletShootDelay = 0.3f;
    [SerializeField] private float bulletSpeed = 20f;
    [SerializeField] private float bulletDestroyDelay = 2.5f;


    private float _triggerRightHand;
    private bool canShoot = true;


    public override void OnNetworkSpawn()
    {
        if (!IsOwner) return;

        
    }

    public override void OnNetworkDespawn()
    {
        if (!IsOwner) return;
    }

    void Update()
    {
        //Debug.Log(triggerRightHand.action.ReadValue<float>());
        if (!IsOwner) return;

        _triggerRightHand = triggerRightHand.action.ReadValue<float>(); // 0 or 1

        // Button is pressed
        if (_triggerRightHand > 0.6 && canShoot)
        {
            StartCoroutine(NormalShoot());
        }
    }


    private IEnumerator NormalShoot()
    {
        canShoot = false;

        SpawnBulletServerRpc(rightHandController.position, rightHandController.forward);

        SpawnBullet(rightHandController.position, rightHandController.forward);

        yield return new WaitForSeconds(bulletShootDelay);

        canShoot = true;

    }

    /// <summary>
    /// Os rpcs funcionam assim: O cliente vai atirar um proj�til que s� ele est� vendo (acho)
    /// e logo em seguida um proj�til "invis�vel" atirado pelo server ir� ser lan�ado tambem
    /// O proj�til do cliente n�o da dano, � s� para o cliente achar o jogo mais responsivo, sem lag
    /// </summary>
    /// <param name="spawnPos"></param>
    /// <param name="direction"></param>

    [ServerRpc]
    private void SpawnBulletServerRpc(Vector3 spawnPos, Vector3 direction) // sempre passar as varaiveis que vai usar, parece que s� funciona assim?
    {
        Debug.Log("CRIANDO SERVER BULLET?");
        GameObject spawnedBulletTransform = Instantiate(serverBulletPrefab, spawnPos, Quaternion.identity); //fake projectile?

        spawnedBulletTransform.transform.forward = direction;

        Physics.IgnoreCollision(playerCollider, spawnedBulletTransform.GetComponent<Collider>()); //ignoring collision with player

        if(spawnedBulletTransform.TryGetComponent<Bullet>(out Bullet bullet))
        {
            bullet.SetOwner(OwnerClientId);  /// passando o id do client que atirou para o Bullet
        }

        if (spawnedBulletTransform.TryGetComponent<Rigidbody>(out Rigidbody rb))
        {
            rb.AddForce(direction * bulletSpeed, ForceMode.Impulse);
        }

        Destroy(spawnedBulletTransform, bulletDestroyDelay);

        SpawnBulletClientRpc(spawnPos, direction);
    }

    [ClientRpc]
    private void SpawnBulletClientRpc(Vector3 spawnPos, Vector3 direction)
    {
        if (IsOwner) return;  // aqui ser� spanada a bullet quando algu�m que n � o player local atirar

        SpawnBullet(spawnPos, direction);
    }

    private void SpawnBullet(Vector3 spawnPos , Vector3 direction)
    {
        GameObject spawnedBulletTransform = Instantiate(clientBulletPrefab, spawnPos, Quaternion.identity); //fake projectile?
        

        spawnedBulletTransform.transform.forward = direction;

        Physics.IgnoreCollision(playerCollider, spawnedBulletTransform.GetComponent<Collider>()); //ignoring collision with player

        if (spawnedBulletTransform.TryGetComponent<Rigidbody>(out Rigidbody rb))
        {
            rb.AddForce(direction * bulletSpeed, ForceMode.Impulse);
        }

        Destroy(spawnedBulletTransform, bulletDestroyDelay);
    }
}

