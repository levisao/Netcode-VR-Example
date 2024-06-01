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

    private GameObject chosenAvatar;


    [SerializeField] private Transform xrRig;

    private AvatarInputConverter avatarInputConverter;



    private void OnEnable() //OnEnable fica "bugando" trocar para o start conserta
    {


    }

    private void Start()
    {
        if (!TestRelay.instance.GameStarted)
        {
            AvatarIndexInfo.instance.onAvatarIndexChange += ChangeAvatar; // O OnEnable tava chamando antes do awake??

            avatarInputConverter = xrRig.GetComponent<AvatarInputConverter>();

            int avatarIndex = AvatarIndexInfo.instance.AvatarIndex; //aqui no start chamandp só na primeira cena

            ChangeAvatar(avatarIndex);

        }
    }

    public override void OnNetworkSpawn() //chamará quando o player spawnar na network
    {
        base.OnNetworkSpawn();
        if (!IsOwner) return;

        avatarInputConverter = xrRig.GetComponent<AvatarInputConverter>();

        int avatarIndex = AvatarIndexInfo.instance.AvatarIndex;

        ChangeAvatar(avatarIndex);
    }

    private void ChangeAvatar(int avatarIndex)
    {
        if (chosenAvatar != null)
        {
            Destroy(chosenAvatar);
        }

        chosenAvatar = Instantiate(avatarPrefabs[avatarIndex], xrRig.position, Quaternion.identity, xrRig);

        avatarInputConverter.MainAvatarTransform = chosenAvatar.transform.Find(avatarPrefabs[avatarIndex].name);
        avatarInputConverter.AvatarBody = chosenAvatar.transform.Find(avatarPrefabs[avatarIndex].name).GetChild(0);
        avatarInputConverter.AvatarHand_Left = chosenAvatar.transform.Find(avatarPrefabs[avatarIndex].name).GetChild(1);
        avatarInputConverter.AvatarHand_Right = chosenAvatar.transform.Find(avatarPrefabs[avatarIndex].name).GetChild(2);
        avatarInputConverter.AvatarHead = chosenAvatar.transform.Find(avatarPrefabs[avatarIndex].name).GetChild(3);
            
    }
}

