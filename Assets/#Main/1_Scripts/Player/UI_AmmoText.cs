using TMPro;
using UnityEngine;

public class UI_AmmoText : MonoBehaviour
{

    public ProjectileSpawner projectileSpawner;
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
        if (projectileSpawner.usesAmmo)
        {
            text.text = "Ammo: " + projectileSpawner.ammo.ToString();
        }
        else
        {
            text.text = "Ammo: ∞";
        }
    }
}
