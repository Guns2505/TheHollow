using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    public PlayerHealth playerHealth;
    public PlayerController playerController;
    public Flashlight flashlight;

    public Image healthFill;
    public Image dashFill;
    public Image batteryFill;
    public Image damageFlash;
    public Text keysText;
    public Text promptText;
    public Text messageText;

    public Color readyColor = new Color(0.35f, 0.8f, 1f);
    public Color chargingColor = new Color(0.4f, 0.4f, 0.45f);

    float flashAlpha;
    float messageTimeLeft;

    void Update()
    {
        healthFill.fillAmount = playerHealth.HealthPercent;
        batteryFill.fillAmount = flashlight.BatteryPercent;
        dashFill.fillAmount = playerController.DashCharge;
        dashFill.color = playerController.DashCharge >= 1f ? readyColor : chargingColor;

        if (flashAlpha > 0f) flashAlpha -= Time.deltaTime * 1.4f;
        damageFlash.color = new Color(0.55f, 0f, 0f, Mathf.Max(0f, flashAlpha));

        if (messageTimeLeft > 0f)
        {
            messageTimeLeft -= Time.deltaTime;
            if (messageTimeLeft <= 0f) messageText.text = "";
        }
    }

    public void UpdateKeys(int collected, int total)
    {
        keysText.text = "KEYS   " + collected + " / " + total;
    }

    public void ShowPrompt(string text)
    {
        promptText.text = text;
    }

    public void ShowMessage(string text)
    {
        messageText.text = text;
        messageTimeLeft = 3.5f;
    }

    public void FlashDamage()
    {
        flashAlpha = 0.75f;
    }
}
