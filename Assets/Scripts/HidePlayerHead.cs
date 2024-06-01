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
    private void OnEnable()
    {

        
    }

    private void HideHead(int index)
    {
        int avatarIndex = AvatarIndexInfo.instance.AvatarIndex;

        objsPfToHide[avatarIndex].GetComponent<Transform>().gameObject.layer = hideObjectLayerIndex;


        if (objsPfToHide[avatarIndex].GetChild(0) != null)
        {
            for (int i = 0; i < objsPfToHide[avatarIndex].childCount; i++)
            {
                objsPfToHide[avatarIndex].GetChild(i).GetComponent<Transform>().gameObject.layer = hideObjectLayerIndex;
            }
        }
    }

    private void Start()
    {
        

        if (!TestRelay.instance.GameStarted)
        {
            Initialization();

        }
    }

    private void Initialization()
    {
        AvatarIndexInfo.instance.onAvatarIndexChange += HideHead;

        playerCamera = GetComponent<Camera>();
        CameraHideObjectMask();

        avatarInitialIndex = AvatarIndexInfo.instance.AvatarIndex;

        HideHead(avatarInitialIndex);
    }

    private void HideObjectsInArray()
    {
        foreach (Transform obj in objsPfToHide)
        {
            if (obj != null)
            {
                Debug.Log("Hiding the obj: " + obj.name);
                obj.GetComponent<Transform>().gameObject.layer = hideObjectLayerIndex; //HideObjectLayer
            }
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.X))
        {
            Debug.Log("PRESSEEEEEED");
            CameraHideObjectMask();
        }
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
