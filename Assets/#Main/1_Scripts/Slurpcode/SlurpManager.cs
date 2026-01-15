using UnityEngine;
using System;
using UnityEngine.Events;

public class SlurpManager : MonoBehaviour
{
    // The maximum health of the McD base
    [SerializeField] public int maxHealth = 100;
    // The current health of the McD base
    private int currentHealth = 0;

    //Events for other systems
    public UnityEvent<int> OnBaseHealthChanged;
    public UnityEvent<int> OnBaseDamageTaken;


    private void Awake()
    {
        currentHealth = maxHealth;
    }

    /// Add slurp to the silo (player collects slurp)
    public void AddSlurp(int amount)
    {
        if (amount <= 0) return;

        int previousHealth = currentHealth;
        currentHealth = Mathf.Min(currentHealth + amount, maxHealth);
        int actualAdded = currentHealth - (currentHealth - previousHealth);

        if (actualAdded > 0)
        {
            OnBaseHealthChanged.Invoke(amount);
            Debug.Log($"Added {actualAdded} slurp to the base. Current Health: {currentHealth}/{maxHealth}");
        }  
    }

    /// Remove slurp from the silo (base takes damage)
    /// Returns the actual damage dealt
    public int TakeDamage(int damageAmount)
    {
        if (damageAmount <=0) return 0;

        int actualDamage = Mathf.Min(damageAmount, currentHealth);
        currentHealth -= actualDamage;

        OnBaseHealthChanged?.Invoke(currentHealth);
        OnBaseDamageTaken?.Invoke(actualDamage);

        Debug.Log($"Base took {actualDamage} damage. Current Health: {currentHealth}/{maxHealth}");

        //Base is destroyed when health reaches zero
        if (currentHealth <= 0)
        {
            Debug.Log("Base destroyed!");
        }
        return actualDamage;  
    } 

    //Getter methods
    public int GetCurrentHealth() => currentHealth;
    public int GetMaxHealth() => maxHealth;
    public bool IsAlive() => currentHealth > 0; 
}
