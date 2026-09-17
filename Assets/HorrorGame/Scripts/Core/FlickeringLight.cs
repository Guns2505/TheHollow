using UnityEngine;

public class FlickeringLight : MonoBehaviour
{
    public Light targetLight;
    public float minIntensity = 0.4f;
    public float maxIntensity = 3.5f;
    public float speed = 5f;
    public float blackoutChance = 0.002f;
    public float blackoutDuration = 0.25f;

    float noiseOffset;
    float blackoutTimeLeft;

    void Awake()
    {
        noiseOffset = Random.value * 100f;
    }

    void Update()
    {
        if (blackoutTimeLeft > 0f)
        {
            blackoutTimeLeft -= Time.deltaTime;
            targetLight.intensity = 0f;
            return;
        }

        if (Random.value < blackoutChance)
        {
            blackoutTimeLeft = blackoutDuration;
            return;
        }

        float noise = Mathf.PerlinNoise(noiseOffset, Time.time * speed);
        targetLight.intensity = Mathf.Lerp(minIntensity, maxIntensity, noise);
    }
}
