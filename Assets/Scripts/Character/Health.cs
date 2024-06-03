using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class Health : NetworkBehaviour
{
    /// <summary>
    /// "field:" makes it visible on inspector
    /// </summary>
    [field: SerializeField] public int MaxHealth { get; private set; } = 100;
    /// Long way to do the property
    ///[SerializeField] private int maxHealth = 100;
    ///public int MaxHealth => maxHealth;
    /// </summary>

    public NetworkVariable<int> currentHealth = new NetworkVariable<int>(); //only the server can modify

    private bool isDead;

    public Action<Health> OnDie; //event dizendo quem morreu para quem tiver ouvindo

    public override void OnNetworkSpawn()
    {
        if (!IsServer) return;

        // .Value para pegar o valor de variáveis de network
        currentHealth.Value = MaxHealth;
    }

    public void TakeDamage(int damageValue)
    {
        ModifyHealth(-damageValue);
    }

    private void ModifyHealth(int value)
    {
        if(isDead) return;

        int newValue = currentHealth.Value + value;
        currentHealth.Value = Mathf.Clamp(newValue, 0, MaxHealth); //limita o minimo e maximo do life

        if (currentHealth.Value == 0)
        {
            OnDie?.Invoke(this); //chamando evento da morte, passando quem morreu
            isDead = true;
        }
    }
}

