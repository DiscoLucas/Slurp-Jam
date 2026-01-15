using Unity.VisualScripting;
using UnityEngine;

public class PlayerContainer : MonoBehaviour
{
    [SerializeField] SlurpManager slurpManager;
    [SerializeField] protected int maxHealth = 100;
    [SerializeField] protected int currentHealth;

    [SerializeField] protected int currentSlurp = 0;
    [SerializeField] protected int maxSlurp = 100;
    [SerializeField] protected int scrapCount = 0;
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
    }

    public bool HasEnoughScrap(int amount)
    {
        return scrapCount >= amount;
    }

    public void changeSlurpLocalAmount(int amount)
    {
        currentSlurp = Mathf.Clamp(currentSlurp + amount, 0, maxSlurp);
    }

    public int GetCurrentSlurp()
    {
        return currentSlurp;
    }

    public void setcurrentSlurp(int amount)
    {
        currentSlurp = Mathf.Clamp(amount, 0, maxSlurp);
    }

    //player death
    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        UI_PlayerHealth uiPlayerHealth = FindObjectOfType<UI_PlayerHealth>();
        uiPlayerHealth.RefreshPlayerHealth();
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
