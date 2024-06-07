using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class DeathCanvas : NetworkBehaviour
{
    [SerializeField] private Health playerHealth;
    [SerializeField] private GameObject deathCanvas;

    public override void OnNetworkSpawn()
    {
        if (playerHealth == null)
        {
            playerHealth = GetComponent<Health>();
        }

        if (playerHealth != null)
        {
            playerHealth.OnDie += HandleShowDeathCanvas;
        }
        else
        {
            Debug.LogError("Player Health Component is null");
        }
    }

    public void OnDisable()
    {
        if (playerHealth != null)
        {
            playerHealth.OnDie -= HandleShowDeathCanvas;
        }
    }

    private void HandleShowDeathCanvas(Health health)
    {
        if (health.transform != gameObject.transform) return;

        // Get the client ID from the Health component's NetworkObject
        ulong clientId = health.GetComponent<NetworkObject>().OwnerClientId;
        
        // Call the server RPC to notify the relevant client
        ShowDeathCanvasServerRpc(clientId);
    }

    [ServerRpc(RequireOwnership = false)]
    private void ShowDeathCanvasServerRpc(ulong clientId)
    {
        // Call the client RPC to update the UI on the specific client
        ShowDeathCanvasClientRpc(new ClientRpcParams
        {
            Send = new ClientRpcSendParams
            {
                TargetClientIds = new List<ulong> { clientId }
            }
        });
    }

    [ClientRpc]
    private void ShowDeathCanvasClientRpc(ClientRpcParams rpcParams = default)
    {
        Debug.Log("Showing Death Canvas on Client");
        deathCanvas.SetActive(true);
    }
}
