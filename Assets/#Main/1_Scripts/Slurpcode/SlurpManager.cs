using UnityEngine;
using System;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class SlurpManager : MonoBehaviour
{
    [Header("player Reference")]
    [SerializeField] PlayerActions playerActions;
    [SerializeField] PlayerContainer playerContainer;

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

    void Start()
    {
        playerContainer = FindFirstObjectByType<PlayerContainer>();
        if(playerContainer == null)
        {
            Debug.LogError("SlurpManager could not find PlayerContainer in the scene!", this);
        }
    }

    /// <summary>
    /// Add slurp to the silo (player collects slurp)
    /// </summary>
    /// <param name="amount"></param>
    /// <returns>The surplus amount that couldn't be added due to capacity</returns>
    public int AddSlurp(int amount)
    {
        if (amount <= 0) return 0;

        int availableSpace = maxHealth - currentHealth;
        int actualAdded = Mathf.Min(amount, availableSpace);
        
        currentHealth += actualAdded;
        
        OnBaseHealthChanged?.Invoke(currentHealth);
        
        int surplus = amount - actualAdded;
        return surplus;
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
    public void DepostietSlurp(InputAction.CallbackContext context)
    {
        Debug.Log("Depositing Slurp");
        int slurpplus = AddSlurp(playerContainer.GetCurrentSlurp());
        playerContainer.setcurrentSlurp(slurpplus);
        
    }
    void OnTriggerEnter(Collider other)
    {
        Debug.Log("SLurp trigger entered by " + other.name);
        if(other.CompareTag("Player")){
            if(playerActions == null)
            {
               playerActions = other.GetComponent<PlayerActions>(); 
               playerActions.possiableInteractEvent.Invoke();
            }
            if(playerActions != null){
                playerActions.pirrorityInteraction = true;
                Debug.Log("Player in range to interact with SLurp");
                playerActions.Interact.action.started += DepostietSlurp;
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        Debug.Log("SLurp trigger exited by " + other.name);
        if(other.CompareTag("Player")){
            if(playerActions != null){
                playerActions.pirrorityInteraction = false;
                Debug.Log("Player out of range to interact with SLurp");
                playerActions.Interact.action.started -= DepostietSlurp;
                playerActions.unPossiableInteractEvent.Invoke();
                playerActions = null;
            }
        }
    }


    public int GetCurrentHealth() => currentHealth;
    public int GetMaxHealth() => maxHealth;
    public bool IsAlive() => currentHealth > 0; 
}
