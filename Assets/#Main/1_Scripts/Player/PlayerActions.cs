using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class PlayerActions : MonoBehaviour
{
    public GameObject Weapon;
    public Transform handContainer;
    public InputActionReference Fire,Interact,SwapNext,SwapPrev;
    bool canFire = true;
    bool carryinObject = false;
    public bool pirrorityInteraction = false;

    [SerializeField]
    private float placeDistance = 2f;
    [SerializeField]
    bool isFiring = false;
    public ProjectileSpawner ActiveProjectileSpawner; //ActiveProjectileSpawner = Weapon.GetComponentInChildren<ProjectileSpawner>();
    public int activeWeaponSpot;
    public List<GameObject> Inventory;
    public UnityEvent possiableInteractEvent;
    public UnityEvent unPossiableInteractEvent;
    [SerializeField]
    PlacementController placementController;
    public GameObject interactKeyUI;

    void OnEnable()
    {
        foreach(Transform g in gameObject.GetComponentsInChildren<Transform>())
        {
            if(g.GetComponent<ProjectileSpawner>()){
                Inventory.Add(g.gameObject);
                g.gameObject.SetActive(false);
            }
        }
        if(Inventory[0])
        {
            ActiveProjectileSpawner = Inventory[0].GetComponent<ProjectileSpawner>();
            Inventory[0].SetActive(true);
        }

        Fire.action.started += ctx => isFiring = true;
        Fire.action.canceled += ctx => isFiring = false;


        Interact.action.started += OnInteract;
        SwapNext.action.performed += OnSwapNext;
        SwapPrev.action.performed += OnSwapPrev;
    }

    void OnDisable()
    {
        if (Fire != null)
        {
            Fire.action.started -= OnFireStarted;
            Fire.action.canceled -= OnFireCanceled;
        }

        if (SwapNext != null)
            SwapNext.action.performed -= OnSwapNext;
        if (SwapPrev != null)
            SwapPrev.action.performed -= OnSwapPrev;
        Interact.action.started -= OnInteract;
    }

    void Update()
    {
        if (placementController != null && placementController.IsPlacing)
            return;

        if (isFiring)
        {
            OnFire();
        }
    }


    private void OnFire()
    {
        if(canFire){
            if(ActiveProjectileSpawner){
                ActiveProjectileSpawner.Fire(); //player should always have a spatula, but if you want to, you can add a fallback here - Be angry at Casper for this
                StartCoroutine(Reload(ActiveProjectileSpawner.cooldown));
            }
        }
    }

    IEnumerator Reload(float cooldownTime)
    {
        canFire = false;
        float cooldown = cooldownTime;
        while(cooldown > 0)
        {
            cooldown-=Time.deltaTime;
            yield return null;
        }

        canFire = true;
        yield return null;
    }

    private void OnInteract(InputAction.CallbackContext context)
    {
        Debug.Log("Interact action triggered");
        if (!pirrorityInteraction && carryinObject)
        {
            dropObject();
        }
            
    }

    void OnFireStarted(InputAction.CallbackContext ctx)
    {
        if (placementController != null && placementController.IsPlacing)
        {
            placementController.TryConfirmPlacement();
            return;
        }

        isFiring = true;
    }

    void OnFireCanceled(InputAction.CallbackContext ctx)
    {
        isFiring = false;
    }

    public void carryObject(GameObject obj)
    {
        obj.transform.SetParent(handContainer);
        obj.transform.localPosition = Vector3.zero;
        obj.transform.localRotation = Quaternion.identity;
        carryinObject = true;

        PickUp pu = obj.GetComponent<PickUp>();
        if (pu != null)
            pu.DisablePickup();
        
    }
    public bool isCarryingObject()
    {
        return carryinObject;
    }
    public GameObject removeCarriedObject()
    {
        carryinObject = false;
        return handContainer.GetChild(0).gameObject;
    }
    public void dropObject()
    {
        GameObject carryedObject = handContainer.GetChild(0).gameObject;
        carryedObject.transform.SetParent(null);
        carryinObject = false;
        carryedObject.transform.position = transform.position + transform.forward * placeDistance;
        PickUp pu = carryedObject.GetComponent<PickUp>();
        if (pu != null)
            pu.EnablePickup();
    }
    

#region Swapping Weapons
    void SwapWeapon(int direction)
    {
        if (Inventory == null || Inventory.Count == 0)
            return;

        // Disable current weapon
        Inventory[activeWeaponSpot].SetActive(false);

        // Move index
        activeWeaponSpot += direction;

        // Wrap index safely
        if (activeWeaponSpot >= Inventory.Count)
            activeWeaponSpot = 0;
        else if (activeWeaponSpot < 0)
            activeWeaponSpot = Inventory.Count - 1;

        // Enable new weapon
        GameObject newWeapon = Inventory[activeWeaponSpot];
        newWeapon.SetActive(true);

        // Update spawner
        ActiveProjectileSpawner = newWeapon.GetComponent<ProjectileSpawner>();
        UI_PlayerStats uiPlayerStats = FindObjectOfType<UI_PlayerStats>();
        uiPlayerStats.RefreshAmmoText();
    }

    void OnSwapNext(InputAction.CallbackContext ctx)
    {
        if (placementController != null && placementController.IsPlacing)
            return;
        SwapWeapon(1);
    }

    void OnSwapPrev(InputAction.CallbackContext ctx)
    {
        if (placementController != null && placementController.IsPlacing)
            return;
        SwapWeapon(-1);
    }

    public void BeginPlacement(GameObject prefab, Material validMaterial, Material invalidMaterial)
    {
        if (placementController == null)
            return;

        isFiring = false;
        placementController.BeginPlacement(prefab, validMaterial, invalidMaterial);
    }

    public void CancelPlacement()
    {
        if (placementController == null)
            return;

        isFiring = false;
        placementController.CancelPlacement();
    }

    //Function that activates and deactivate interact prompt

 #endregion
}
