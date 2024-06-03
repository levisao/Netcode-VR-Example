using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class PlayerNetwork : NetworkBehaviour
{

    [SerializeField] private LocomotionSystem locomotionSystem;

    [SerializeField] private ContinuousMoveProviderBase continuousMoveProvider;

    public override void OnNetworkSpawn()
    {
        if (IsOwner) return;

        locomotionSystem.enabled = false;
        continuousMoveProvider.enabled = false;
    }
}
