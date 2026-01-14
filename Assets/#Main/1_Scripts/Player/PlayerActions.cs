using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerActions : MonoBehaviour
{
    public GameObject Weapon;
    public InputActionReference Fire;
    bool canFire = true;
    void OnEnable()
    {
        Fire.action.started += OnFire;
    }

    void OnDisable()
    {
        Fire.action.started -= OnFire;
    }

    private void OnFire(InputAction.CallbackContext context)
    {
        if(canFire){
            ProjectileSpawner ActiveProjectileSpawner = Weapon.GetComponentInChildren<ProjectileSpawner>();
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
            Debug.Log("reloading");
            yield return null;
        }

        Debug.Log("Can fire!");
        canFire = true;
        yield return null;
    }
}
