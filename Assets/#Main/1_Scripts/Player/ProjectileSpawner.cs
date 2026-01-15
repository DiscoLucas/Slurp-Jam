using System;
using UnityEngine;

public class ProjectileSpawner : MonoBehaviour
{
    public GameObject projectile;
    public bool usesAmmo; //this is only actual for the spatula, might just remove it.
    public int ammo;
    public float cooldown;

    public void Fire()
    {
        UI_PlayerHealth uiPlayerHealth = FindObjectOfType<UI_PlayerHealth>();
        uiPlayerHealth.RefreshAmmoText();
        if (ammo>0){
            GameObject.Instantiate<GameObject>(projectile,transform.position,transform.rotation);
            if (usesAmmo)
            {
                ammo--;
            }
        }
    }
}
