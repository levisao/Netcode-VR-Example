using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class AvatarRigSelector : NetworkBehaviour
{
    [SerializeField] private GameObject[] avatarPrefabs;



    [SerializeField] private Transform xrRig;

    private AvatarInputConverter avatarInputConverter;



    private void OnEnable()
    {
        AvatarIndexInfo.instance.onAvatarIndexChange += ChangeAvatar; // O OnEnable tava chamando antes do awake??

        avatarInputConverter = xrRig.GetComponent<AvatarInputConverter>();

    }

    private void Start()
    {
        int avatarIndex = AvatarIndexInfo.instance.AvatarIndex;
        Debug.Log("Avatar Index: " + avatarIndex);
        ChangeAvatar(avatarIndex);
    }

    private void ChangeAvatar(int avatarIndex)
    {
        for (int i = 0; i < avatarPrefabs.Length; i++)
        {
            avatarPrefabs[i].SetActive(i == avatarIndex);

            if (i == avatarIndex)
            {
                avatarInputConverter.MainAvatarTransform = transform.Find(avatarPrefabs[i].name);
                avatarInputConverter.AvatarBody = transform.Find(avatarPrefabs[i].name).GetChild(0);
                avatarInputConverter.AvatarHand_Left = transform.Find(avatarPrefabs[i].name).GetChild(1);
                avatarInputConverter.AvatarHand_Right = transform.Find(avatarPrefabs[i].name).GetChild(2);
                avatarInputConverter.AvatarHead = transform.Find(avatarPrefabs[i].name).GetChild(3);
            }
        }
    }
}

