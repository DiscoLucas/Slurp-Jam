using TMPro;
using UnityEngine;

public class UI_PlayerHealth : MonoBehaviour
{
    public PlayerContainer playerContainer;
    public TextMeshProUGUI text;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        RefreshPlayerHealth();

    }

    // Update is called once per frame
    void Update()
    {
        RefreshPlayerHealth();
    }

    public void RefreshPlayerHealth()
    {
        text.text = "Health: " + playerContainer.GetCurrentHealth().ToString() + " / " + playerContainer.Max;
    }
}
