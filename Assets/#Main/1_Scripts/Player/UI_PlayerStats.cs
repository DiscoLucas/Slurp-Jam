using MoreMountains.TopDownEngine;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Experimental.AssetDatabaseExperimental.AssetDatabaseCounters;

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
    public Image weaponIconUI;
    [Header("Interaction key")]
    public GameObject interactKeyUI;
    [Header("Wave Timer")]
    public TextMeshProUGUI prepTimeText;
    WaveManager waveManager;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerContainer = FindFirstObjectByType<PlayerContainer>();
        slurpManager = FindFirstObjectByType<SlurpManager>();
        weapon = FindFirstObjectByType<PlayerActions>();
        waveManager = FindFirstObjectByType<WaveManager>();
        waveManager.uiPlayerStats = this;
        interactKeyUI = weapon.interactKeyUI;
        interactKeyUI.SetActive(false);
        slurpManager.OnBaseDamageTaken.AddListener(refreshHealthAll);
        slurpManager.OnBaseHealthChanged.AddListener(refreshHealthAll);
        slurpManager.OnBaseDestroyed.AddListener(refreshAll);
        refreshAll();
        weapon.possiableInteractEvent.AddListener(ActivateInteractKey);
        weapon.unPossiableInteractEvent.AddListener(DeactivateInteractKey);
        waveManager.prepeaarWaveEvent.AddListener(preparWaveUI);
        waveManager.startWaveEvent.AddListener(startWaveUI);
        prepTimeText.gameObject.SetActive(false);
        
    }

    // Update is called once per frame
    void Update()
    {
        //RefreshPlayerHealth();
    }
    public void startWaveUI()
    {
        prepTimeText.gameObject.SetActive(false);
    }
    public void preparWaveUI(int counter) 
    {
        prepTimeText.gameObject.SetActive(true);
    }

    public void countdown(int counter) {
        prepTimeText.text = "Wave " + (waveManager.currentWave + 1) + " starting in " + counter + " seconds.";
    }

    public void RefreshPlayerHealth()
    {
        hpText.text = "HP: " + playerContainer.GetCurrentHealth().ToString() + " / " + playerContainer.Max;
    }

    public void RefreshAmmoText()
    {
        weaponIconUI.sprite = weapon.ActiveProjectileSpawner.weaponIcon;
        if (weapon.ActiveProjectileSpawner.usesAmmo)
        {
            ammoText.text = " " + weapon.ActiveProjectileSpawner.ammo.ToString();
        }
        else
        {
            ammoText.text = " ∞";
        }
    }

    public void RefreshBaseHealth()
    {
        baseHpText.text = "Base Health: " + slurpManager.GetCurrentHealth().ToString() + " / " + slurpManager.GetMaxHealth().ToString();
    }

    public void RefreshScrapAmount()
    {
        scrapResource.text = playerContainer.scrapCount.ToString();
    }

    public void RefreshSluptAmount()
    {
        slurpResource.text = playerContainer.currentSlurp.ToString();
    }
    private void refreshHealthAll(int n)
    {
        refreshAll();
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
        interactKeyUI?.SetActive(true);
    }

    void DeactivateInteractKey()
    {
        interactKeyUI?.SetActive(false);
    }

}
