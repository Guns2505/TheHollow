using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class ChaseEffect : MonoBehaviour
{
    public Volume volume;
    public MonsterAI monster;
    public float maxDistance = 25f;

    Vignette vignette;
    ColorAdjustments colorAdjustments;

    void Start()
    {
        volume.profile.TryGet(out vignette);
        volume.profile.TryGet(out colorAdjustments);
    }

    void Update()
    {
        if (monster == null || vignette == null) return;

        float closeness = 1f - Mathf.Clamp01(monster.DistanceToPlayer / maxDistance);
        float pulse = monster.IsChasing ? Mathf.Abs(Mathf.Sin(Time.time * 5f)) * 0.12f : 0f;

        vignette.intensity.value = 0.3f + closeness * 0.3f + pulse;
        vignette.color.value = Color.Lerp(Color.black, new Color(0.35f, 0f, 0f), closeness);

        if (colorAdjustments != null)
            colorAdjustments.saturation.value = Mathf.Lerp(-20f, -65f, closeness);
    }
}
