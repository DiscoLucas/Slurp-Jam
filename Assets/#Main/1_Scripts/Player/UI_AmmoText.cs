using TMPro;
using UnityEngine;

public class UI_AmmoText : MonoBehaviour
{

    public ProjectileSpawner weapon;
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
        if (weapon.usesAmmo)
        {
            text.text = "Ammo: " + weapon.ammo.ToString();
        }
        else
        {
            text.text = "Ammo: ∞";
        }
    }
}
