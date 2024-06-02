using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode.Components;

public class NetworkTransformClient : NetworkTransform
{
    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        ///Network transform and NetworkBehaviour components
        CanCommitToTransform = IsOwner; // dizendo que o owner can commit to transform
    }

    protected override void Update()
    {
        CanCommitToTransform = IsOwner;
        base.Update();

        ///safety checks (extra safe)
        if (NetworkManager != null) // não precisa do singleton pq esta herdando já da NetworkBehaviour
        {
            if (NetworkManager.IsConnectedClient || NetworkManager.IsListening) //are we connected to the server as a client
            {
                if (CanCommitToTransform)
                {
                    TryCommitTransformToServer(transform, NetworkManager.LocalTime.Time);
                }
            }
        }
    }
    protected override bool OnIsServerAuthoritative()
    {
        return false;
    }
}
