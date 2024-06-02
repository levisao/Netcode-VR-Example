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

        _triggerRightHand = triggerRightHand.action.ReadValue<float>();

        //if (_triggerRightHand < 0.6f) return;
        // Button is pressed
        if (_triggerRightHand > 0.6 && canShoot)
        {
            StartCoroutine(NormalShoot());
        }
        

        //_triggerRightHand = triggerRightHand.action.ReadValue<bool>();

        //if (!_triggerRightHand) return;

        //StartCoroutine(NormalShoot());
        //if (Input.GetKey(KeyCode.Mouse0) && canShoot && TestRelay.instance.GameStarted)
        //{
        //   ShootCoroutine();
        // }
    }


    private IEnumerator NormalShoot()
    {
        canShoot = false;

        SpawnBulletServerRpc(rightHandController.position, rightHandController.forward);

        SpawnBullet(rightHandController.position, rightHandController.forward);

        yield return new WaitForSeconds(bulletShootDelay);

        canShoot = true;

    }

    [ServerRpc]
    private void SpawnBulletServerRpc(Vector3 spawnPos, Vector3 direction) // sempre passar as varaiveis que vai usar, parece que só funciona assim?
    {
        GameObject spawnedBulletTransform = Instantiate(serverBulletPrefab, spawnPos, Quaternion.identity); //fake projectile?

        spawnedBulletTransform.transform.forward = direction;

        Physics.IgnoreCollision(playerCollider, spawnedBulletTransform.GetComponent<Collider>()); //ignoring collision with player

        if (spawnedBulletTransform.TryGetComponent<Rigidbody>(out Rigidbody rb))
        {
            rb.AddForce(direction * bulletSpeed, ForceMode.Impulse);
        }

        SpawnBulletClientRpc(spawnPos, direction);
    }

    [ClientRpc]
    private void SpawnBulletClientRpc(Vector3 spawnPos, Vector3 direction)
    {
        if (IsOwner) return;  // aqui será spanada a bullet quando alguém que n é o player local atirar

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
    }
}

