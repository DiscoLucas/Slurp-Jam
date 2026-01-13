using System;
using UnityEngine;

public class ProjectileSpawner : MonoBehaviour
{
    public GameObject projectile;
    public bool usesAmmo; //this is only actual for the spatula, might just remove it.
    public int ammo;

    public void Fire()
    {
        if(ammo>0){
            GameObject.Instantiate<GameObject>(projectile);
            if (usesAmmo)
            {
                ammo--;
            }
        }
    }
}
