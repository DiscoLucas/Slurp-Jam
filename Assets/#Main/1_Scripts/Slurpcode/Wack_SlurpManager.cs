using UnityEngine;
using System;

public class SlurpManager : MonoBehaviour
{
    // The maximum health of the McD base
    [SerializeField] private int maxHealth = 100;
    // The current health of the McD base
    private int currentHealth = 0;

    //Events for other systems
    public static event Action<int> OnBaseHealthChanged;
    public static event Action<int> OnBaseDamageTaken;

    //Singleton instance
    public static SlurpManager Instance 
    {
        get 
        {
            if (instance == null)
            {
                instance = FindObjectOfType<SlurpManager>();
                    if (instance == null)
                    {
                        Debug.LogError("No SlurpManager found in the scene.");
                    }
            }   
            return instance;
        }
    }

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
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
            OnBaseHealthChanged?.Invoke(currentHealth);
            Debug.Log($"Added {actualAdded} slurp to the base. Current Health: {currentHealth}/{maxHealth}");
        }  
    }

    /// Remove slurp from the silo (base takes damage)
    /// Returns the actual damage dealt
    public int TakeDamage(int damageAmount)
    {
        if (damage <=0) return 0;

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
