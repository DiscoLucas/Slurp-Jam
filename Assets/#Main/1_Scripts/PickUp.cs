using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class PickUp : MonoBehaviour
{
    [SerializeField]
    protected PickUpType pickUpType;

    [SerializeField]
    UnityEvent onPickUp;

    [SerializeField]
    bool moveToPlayer = true;

    [SerializeField]
    bool pressToPickUp = false;

    [SerializeField]
    float pickUpRange = 5f;

    [SerializeField]
    float moveSpeed = 5f;

    [SerializeField]
    int amountToGive = 1;

    private PlayerActions playerAction;
    private PlayerContainer playerContainer;
    private bool isMovingToPlayer = false;
    private bool playerInRange = false;

    [SerializeField]
    private bool destoryOnPickup = true;

    private bool isPickupActive = true;

    void Start()
    {
        if (playerAction == null)
        {
            playerAction = FindFirstObjectByType<PlayerActions>();
            playerContainer = playerAction.GetComponent<PlayerContainer>();
            if (playerAction == null)
            {
                Debug.LogError("PickUp script could not find PlayerActions in the scene!", this);
            }
            if(playerContainer == null)
            {
                Debug.LogError("PickUp script could not find PlayerContainer in the scene!", this);
            }
        }

    }
    bool unSubcribedInteraction = false;
    void Update()
    {
        if (!isPickupActive || playerAction == null)
            return;

        
        if(moveToPlayer && isMovingToPlayer){
            Vector3 direction = (playerAction.gameObject.transform.position- transform.position).normalized;
            transform.position += direction * moveSpeed * Time.deltaTime;
        }

        float distance = Vector3.Distance(transform.position, playerAction.gameObject.transform.position);
        if (pressToPickUp){
            if(distance <= pickUpRange){
                playerAction.pirrorityInteraction = true;
                playerAction.Interact.action.started += clickPickup; 
                unSubcribedInteraction = true;
            }else if(unSubcribedInteraction){
                playerAction.pirrorityInteraction = false;
                playerAction.Interact.action.started -= clickPickup;
                unSubcribedInteraction = false;
            }

        } else if (!pressToPickUp && distance < pickUpRange)
        {
            CollectPickup();
        }
          
    }

    void OnTriggerEnter(Collider other)
    {
        if (!isPickupActive)
            return;
            
        if (other.CompareTag("Player"))
        {
            if (!pressToPickUp)
            {
                isMovingToPlayer = true;
            }
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (!isPickupActive)
            return;
            
        if (collision.gameObject.CompareTag("Player"))
        {
            if (!pressToPickUp)
            {
                CollectPickup();
            }
        }
    }

    private void CollectPickup()
    {

        onPickUp?.Invoke();
        switch (pickUpType)
        {
            case PickUpType.Slurp:
                playerContainer.changeBaseHealth(amountToGive);
                Debug.Log("Collected Slurp!");
                break;
            case PickUpType.Scrap:
                playerContainer.changeScrap(amountToGive);
                Debug.Log("Collected Scrap!");
                break;
            case PickUpType.Carryable:
                playerAction.carryObject(gameObject);
                break;
            case PickUpType.Other:
                onPickUp?.Invoke();
                break;
        }
        if(destoryOnPickup)
            Destroy(gameObject);
    }

    public void clickPickup(InputAction.CallbackContext context)
    {
        Debug.Log("Click pickup triggered");
        CollectPickup();
    }

    void OnDestroy()
    {
        playerAction.pirrorityInteraction = false;
        playerAction.Interact.action.started -= clickPickup;       
    }

    /// <summary>
    /// Enable the pickup functionality
    /// </summary>
    public void EnablePickup()
    {
        isPickupActive = true;
    }

    /// <summary>
    /// Disable the pickup functionality
    /// </summary>
    public void DisablePickup()
    {
        isPickupActive = false;
        isMovingToPlayer = false;
        
        if (playerAction != null && pressToPickUp)
        {
            playerAction.pirrorityInteraction = false;
            playerAction.Interact.action.started -= clickPickup;
        }
    }

    /// <summary>
    /// Check if the pickup is currently active
    /// </summary>
    public bool IsPickupActive()
    {
        return isPickupActive;
    }
}

[Serializable]
public enum PickUpType
{
    Slurp,
    Scrap,
    Carryable,
    Other
}
