using TMPro;
using UnityEngine;

public class UI_AmmoText : MonoBehaviour
{

    public PlayerActions weapon;
    public TextMeshProUGUI text;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        RefreshAmmoText();
    }

    // Update is called once per frame
    void Update()
    {
        RefreshAmmoText();
    }

    public void RefreshAmmoText()
    {
        if (weapon.ActiveProjectileSpawner.usesAmmo)
        {
            text.text = "Ammo: " + weapon.ActiveProjectileSpawner.ammo.ToString();
        }
        else
        {
            text.text = "Ammo: ∞";
        }
    }
}
