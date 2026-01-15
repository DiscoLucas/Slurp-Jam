using MoreMountains.TopDownEngine;
using TMPro;
using UnityEngine;

public class UI_PlayerStats : MonoBehaviour
{
    [Header("Player Health")]
    public PlayerContainer playerContainer;
    public TextMeshProUGUI hpText;
    [Header("Base Health")]
    public SlurpManager slurpManager;
    public TextMeshProUGUI baseHpText;
    [Header("Recources")]
    public TextMeshProUGUI slurpResource;
    public TextMeshProUGUI scrapResource;
    [Header("Ammo")]
    public PlayerActions weapon;
    public TextMeshProUGUI ammoText;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        refreshAll();
    }

    // Update is called once per frame
    void Update()
    {
        //RefreshPlayerHealth();
    }

    public void RefreshPlayerHealth()
    {
        hpText.text = "HP: " + playerContainer.GetCurrentHealth().ToString() + " / " + playerContainer.Max;
    }

    public void RefreshAmmoText()
    {
        if (weapon.ActiveProjectileSpawner.usesAmmo)
        {
            ammoText.text = "Ammo: " + weapon.ActiveProjectileSpawner.ammo.ToString();
        }
        else
        {
            ammoText.text = "Ammo: ∞";
        }
    }

    public void RefreshBaseHealth()
    {
        baseHpText.text = "Base Health: " + slurpManager.GetCurrentHealth().ToString() + " / " + slurpManager.GetMaxHealth().ToString();
    }

    public void RefreshScrapAmount()
    {
        scrapResource.text = "Scrap: " + playerContainer.scrapCount.ToString();
    }

    private void refreshAll()
    {
        RefreshPlayerHealth();
        RefreshAmmoText();
        RefreshBaseHealth();
        RefreshScrapAmount();
    }
}
