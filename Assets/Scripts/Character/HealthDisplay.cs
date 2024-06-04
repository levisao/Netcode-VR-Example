//using Microsoft.Unity.VisualStudio.Editor;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class HealthDisplay :    NetworkBehaviour
{
    [Header("References")]
    [SerializeField] private Health health;
    [SerializeField] private Image healthBarImage;

    public override void OnNetworkSpawn()
    {
        /// evento criado automaticamente para networkvariables (OnValueChanged)
        if (!IsClient) return; ///só otimização, não é obrigatório

        healthBarImage.fillAmount = 1;

        health.currentHealth.OnValueChanged += HandleHealthChanged;
        HandleHealthChanged(0, health.currentHealth.Value); ///oldHeatlh n i,porta, n usamos, mas para o evento é obrigatorio
    }

    private void Update()
    {
        Debug.Log(healthBarImage.fillAmount);
    }
    public override void OnNetworkDespawn()
    {
        if (!IsClient) return; ///só otimização, não é obrigatório

        health.currentHealth.OnValueChanged -= HandleHealthChanged;
    }

    private void HandleHealthChanged(int oldHealth, int newHealth)
    {                               ///fillAmount vai de 0 a 1
        healthBarImage.fillAmount = (float)newHealth / health.MaxHealth; //resultará na porcentagem

        if (healthBarImage.fillAmount <= 0.5f)
        {
            healthBarImage.color = Color.red;
        }
        //Debug.Log("EVENT BEING CALLED. FILL AMOUNT: " + healthBarImage.fillAmount);
    }


}
