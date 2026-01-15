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
    [Header("Interaction key")]
    public GameObject interactKeyUI;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerContainer = FindFirstObjectByType<PlayerContainer>();
        slurpManager = FindFirstObjectByType<SlurpManager>();
        weapon = FindFirstObjectByType<PlayerActions>();
        interactKeyUI = weapon.interactKeyUI;
        refreshAll();
        weapon.possiableInteractEvent.AddListener(ActivateInteractKey);
        weapon.unPossiableInteractEvent.AddListener(DeactivateInteractKey);
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

    void ActivateInteractKey()
    {
        interactKeyUI.SetActive(true);
    }

    void DeactivateInteractKey()
    {
        interactKeyUI.SetActive(false);
    }

}
