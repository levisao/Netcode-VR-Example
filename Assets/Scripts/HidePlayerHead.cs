using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class HidePlayerHead : NetworkBehaviour
{
    [SerializeField] private Transform[] objsPfToHide;

    [SerializeField] private int hideObjectLayerIndex = 8;

    private Camera playerCamera;

    private int avatarInitialIndex;
    public override void OnNetworkSpawn() 
    {
        base.OnNetworkSpawn();

        if (!IsOwner) return;

        Initialization();



    }

    private void HideObjects()
    {
        for (int i = 0; i < objsPfToHide.Length; i++)
        {
            objsPfToHide[i].GetComponent<Transform>().gameObject.layer = hideObjectLayerIndex;

            if (objsPfToHide[i].GetChild(0) != null)
            {
                for (int j = 0; j < objsPfToHide[i].childCount; j++)
                {
                    objsPfToHide[j].GetChild(i).GetComponent<Transform>().gameObject.layer = hideObjectLayerIndex;
                }
            }
        }


    }

    private void Start()
    {
       /* 
        if (!TestRelay.instance.GameStarted)
        {
            Initialization();
        }
       */
    }

    private void Initialization()
    {
        playerCamera = GetComponent<Camera>();
        CameraHideObjectMask();
        HideObjects();
    }


    private void Update()
    {
    }


    // Turn on the bit using an OR operation:
    private void Show()
    {
        playerCamera.cullingMask |= 1 << LayerMask.NameToLayer("SomeLayer");
    }

    // Turn off the bit using an AND operation with the complement of the shifted int:
    private void CameraHideObjectMask()
    {
        //playerCamera = GetComponent<Camera>();
        playerCamera.cullingMask &= ~(1 << LayerMask.NameToLayer("HideObject"));
    }

    // Toggle the bit using a XOR operation:
    private void Toggle()
    {
        playerCamera.cullingMask ^= 1 << LayerMask.NameToLayer("SomeLayer");
    }
}
