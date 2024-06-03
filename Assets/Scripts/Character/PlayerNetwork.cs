using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

public class PlayerNetwork : NetworkBehaviour
{

    [SerializeField] private LocomotionSystem locomotionSystem;

    [SerializeField] private ContinuousMoveProviderBase continuousMoveProvider;

    [SerializeField] private InputActionReference moveInputAction;

    [SerializeField] private CharacterController characterController;

    [SerializeField] private float speed = 1.0f;

    public override void OnNetworkSpawn()
    {
        if (IsOwner) return;

        locomotionSystem.enabled = false;
        continuousMoveProvider.enabled = false;
    }

    private void Update()
    {
        if (NetworkManager.IsListening)
        {
            if (!IsOwner) return;
        }


        Vector2 moveInput = moveInputAction.action.ReadValue<Vector2>();


        // Translate input to movement direction
        Vector3 movementDirection = new Vector3(moveInput.x, 0f, moveInput.y);

        // Move the character
        characterController.Move(movementDirection * speed * Time.deltaTime);
    }
}
