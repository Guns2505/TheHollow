using UnityEngine;
using UnityEngine.InputSystem;

public class Flashlight : MonoBehaviour
{
    public Light spotLight;
    public float maxBattery = 100f;
    public float drainPerSecond = 1.1f;
    public float baseIntensity = 6f;

    float battery;
    bool isOn = true;

    public float BatteryPercent => battery / maxBattery;
    public bool IsOn => isOn;

    void Awake()
    {
        battery = maxBattery;
    }

    void Update()
    {
        if (GameManager.PlayerInputEnabled && Keyboard.current != null && Keyboard.current.fKey.wasPressedThisFrame && battery > 0f)
            isOn = !isOn;

        if (isOn)
        {
            battery -= drainPerSecond * Time.deltaTime;
            if (battery <= 0f)
            {
                battery = 0f;
                isOn = false;
            }
        }

        spotLight.enabled = isOn;

        if (isOn)
        {
            float weakness = Mathf.Clamp01(battery / 25f);
            float flicker = battery < 25f ? Mathf.PerlinNoise(Time.time * 14f, 0f) : 1f;
            spotLight.intensity = baseIntensity * Mathf.Lerp(0.25f, 1f, weakness) * flicker;
        }
    }

    public void AddBattery(float amount)
    {
        battery = Mathf.Min(maxBattery, battery + amount);
    }
}
