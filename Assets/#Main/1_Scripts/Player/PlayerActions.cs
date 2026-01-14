using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
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
    

    void OnEnable()
    {
        /*foreach(Transform g in gameObject.GetComponentsInChildren<Transform>())
        {
            Inventory.Add(g.gameObject);
        }
        ActiveProjectileSpawner = Inventory[0].GetComponent<ProjectileSpawner>(); //Very risky, and assumes that we have projectile spawners.*/

        Fire.action.started += ctx => isFiring = true;
        Fire.action.canceled += ctx => isFiring = false;

        SwapNext.action.performed += OnSwapNext;
        SwapPrev.action.performed += OnSwapPrev;
    }

    void OnDisable()
    {
        Fire.action.started -= ctx => isFiring = true;
        Fire.action.canceled -= ctx => isFiring = false;
    }

    void Update()
    {
        if (isFiring)
        {
            Debug.Log("Fire!");
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

    private void InteractAction(InputAction.CallbackContext context)
    {
        Debug.Log("Interact action triggered");
        if (!pirrorityInteraction && carryinObject)
        {
            dropObject();
        }
            
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
    }

    void OnSwapNext(InputAction.CallbackContext ctx)
    {
        SwapWeapon(1);
    }

    void OnSwapPrev(InputAction.CallbackContext ctx)
    {
        SwapWeapon(-1);
    }
 #endregion
}
