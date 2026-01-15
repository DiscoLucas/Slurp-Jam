using Unity.VisualScripting;
using UnityEngine;

public class PlayerContainer : MonoBehaviour
{
    [SerializeField] SlurpManager slurpManager;
    [SerializeField] protected int maxHealth = 100;
    [SerializeField] protected int currentHealth;

    [SerializeField] public int scrapCount = 0;
    //UI stuff

    void Start()
    {
        slurpManager = FindFirstObjectByType<SlurpManager>();
        if (slurpManager == null)
        {
            Debug.LogError("PlayerContainer could not find SlurpManager in the scene!", this);
        }
    }
    private void Awake()
    {
        currentHealth = maxHealth;
        scrapCount = 0;
    }

    public int Max
    {
        get { return maxHealth; }
    }

    public int GetCurrentHealth()
    {
        return currentHealth;
    }

    public void changeScrap(int amount)
    {
        scrapCount += amount;
        UI_PlayerStats uiPlayerStats = FindObjectOfType<UI_PlayerStats>();
        uiPlayerStats.RefreshScrapAmount();
    }

    public bool HasEnoughScrap(int amount)
    {
        return scrapCount >= amount;
    }

    public void changeBaseHealth(int amount)
    {
        UI_PlayerStats uiPlayerStats = FindObjectOfType<UI_PlayerStats>();
        uiPlayerStats.RefreshBaseHealth();
        if (amount > 0)
        {
            slurpManager.AddSlurp(amount);
        }
        else if(amount < 0)
        {
            slurpManager.TakeDamage(amount);
        }
    }
    //player death
    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        UI_PlayerStats uiPlayerStats = FindObjectOfType<UI_PlayerStats>();
        uiPlayerStats.RefreshPlayerHealth();
        if (currentHealth <= 0)
        {
            currentHealth = 0;
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
        UI_PlayerStats uiPlayerStats = FindObjectOfType<UI_PlayerStats>();
        uiPlayerStats.RefreshPlayerHealth();
        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
    }

}
