using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerActions : MonoBehaviour
{
    public GameObject Weapon;
    public InputActionReference Fire;
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
        Weapon.GetComponentInChildren<ProjectileSpawner>().Fire(); //player should always have a spatula, but if you want to, you can add a fallback here - Be angry at Casper for this
    }
}
