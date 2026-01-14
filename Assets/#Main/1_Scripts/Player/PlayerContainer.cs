using UnityEngine;

public class PlayerContainer : MonoBehaviour
{
    [SerializeField] protected int maxHealth = 100;
    [SerializeField] protected int currentHealth;

    private void Awake()
    {
        currentHealth = maxHealth;
    }

    public int Max
    {
        get { return maxHealth; }
    }

    public int GetCurrentHealth()
    {
        return currentHealth;
    }

    //player death
    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        // Handle player death (e.g., respawn, game over, etc.)
        Debug.Log("Player has died.");
        Debug.Break();
    }

    public void Heal(int amount)
    {
        currentHealth += amount;
        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
    }

}
