using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.AffordanceSystem.Receiver.Primitives;

public class Shoot : NetworkBehaviour
{
    [SerializeField] Transform bulletPrefab;

    [SerializeField] private Transform rightHandController;

    [SerializeField] private float bulletShootDelay = 0.3f;

    [SerializeField] private float bulletDestroyDelay = 3f;

    private Bullet bullet;

    private bool canShoot = true;
    void Start()
    {
        bullet = bulletPrefab.GetComponent<Bullet>();
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.Mouse0) && canShoot)
        {
            bullet.bulletMoveDirection = rightHandController.transform.forward;
            StartCoroutine(NormalShoot());
        }
    }

    private IEnumerator NormalShoot()
    {
        canShoot = false;

        Transform spawnedBulletTransform = Instantiate(bulletPrefab, rightHandController.position, Quaternion.identity);
        spawnedBulletTransform.GetComponent<NetworkObject>().Spawn(true);

        yield return new WaitForSeconds(bulletShootDelay);

        canShoot = true;

        Destroy(spawnedBulletTransform.gameObject, bulletDestroyDelay);
    }
}
