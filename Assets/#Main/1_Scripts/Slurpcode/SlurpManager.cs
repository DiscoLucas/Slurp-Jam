using UnityEngine;
using System;
using UnityEngine.Events;

public class SlurpManager : MonoBehaviour
{
    [Header("Health Settings")]
    [SerializeField] private int maxHealth = 100;
    [SerializeField] private int currentHealth = 0;

    [Header("Events")]
    //Events for other systems
    public UnityEvent<int> OnBaseHealthChanged;
    public UnityEvent<int> OnBaseDamageTaken;
    public UnityEvent OnBaseDestroyed;


    private void Awake()
    {
        currentHealth = maxHealth;
    }

    /// <summary>
    /// Add slurp to the silo (player collects slurp)
    /// </summary>
    /// <param name="amount"></param>
    public void AddSlurp(int amount)
    {
        if (amount <= 0) return;
        currentHealth += Mathf.Max(maxHealth,currentHealth);
        OnBaseHealthChanged?.Invoke(currentHealth);
    }

    /// <summary>
    /// Remove slurp from the silo (base takes damage)
    /// Returns the actual damage dealt
    /// </summary>
    /// <param name="damageAmount"></param>
    /// <returns></returns>
    public void TakeDamage(int damageAmount)
    {
        if (damageAmount <=0) return;

        currentHealth -= damageAmount;

        OnBaseHealthChanged?.Invoke(currentHealth);
        OnBaseDamageTaken?.Invoke(damageAmount);

        Debug.Log($"Base took {damageAmount} damage. Current Health: {currentHealth}/{maxHealth}");

        //Base is destroyed when health reaches zero
        if (currentHealth <= 0)
        {
            OnBaseDestroyed?.Invoke();
            Debug.Log("Base destroyed!");
        }
    } 


    public int GetCurrentHealth() => currentHealth;
    public int GetMaxHealth() => maxHealth;
    public bool IsAlive() => currentHealth > 0; 
}
