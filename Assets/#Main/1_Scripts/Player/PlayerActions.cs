using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerActions : MonoBehaviour
{
    public GameObject Weapon;

    public Transform handContainer;
    public InputActionReference Fire,Interact;
    bool canFire = true;
    bool carryinObject = false;
    public bool pirrorityInteraction = false;

    [SerializeField]
    private float placeDistance = 2f;
    [SerializeField]
    public ProjectileSpawner ActiveProjectileSpawner; //ActiveProjectileSpawner = Weapon.GetComponentInChildren<ProjectileSpawner>();
    bool isFiring = false;

    void OnEnable()
    {
        Fire.action.started += ctx => isFiring = true;
        Fire.action.canceled += ctx => isFiring = false;
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

    //weapon switching function should be added here


}
