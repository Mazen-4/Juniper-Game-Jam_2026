using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class Health : MonoBehaviour
{
    [Header("Health Settings")]
    [SerializeField] private int maxHealth = 100;
    private int currentHealth;

    // Events allow other scripts (like UI or VFX) to listen for changes without tightly coupling code
    public static event Action OnPlayerDeath; // Global event for game managers
    public event Action<int, int> OnHealthChanged; // Local event (current, max) for UI bars

    public bool IsDead { get; private set; }

    private void Start()
    {
        currentHealth = maxHealth;
        IsDead = false;

        // Trigger initial UI update if anyone is listening
        OnHealthChanged?.Invoke(currentHealth, maxHealth);
    }

    public void TakeDamage(int damageAmount)
    {
        if (IsDead) return;

        // Ensure damage is never negative
        currentHealth -= Mathf.Max(damageAmount, 0);
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        // Notify UI or health bars
        OnHealthChanged?.Invoke(currentHealth, maxHealth);

        Debug.Log($"{gameObject.name} took {damageAmount} damage. Current Health: {currentHealth}");

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void Heal(int healAmount)
    {
        if (IsDead) return;

        currentHealth += Mathf.Max(healAmount, 0);
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        OnHealthChanged?.Invoke(currentHealth, maxHealth);
        Debug.Log($"{gameObject.name} healed by {healAmount}. Current Health: {currentHealth}");
    }

    private void Die()
    {
        IsDead = true;
        Debug.Log($"{gameObject.name} has died!");

        if (gameObject.CompareTag("Player"))
        {
            OnPlayerDeath?.Invoke(); // Trigger game over / reload screen
        }

        // Handle death behavior here (e.g., play animation, disable object, drop loot)
        gameObject.SetActive(false);
    }
}
