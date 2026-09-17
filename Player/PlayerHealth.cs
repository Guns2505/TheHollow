using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public float maxHealth = 100f;
    public float regenDelay = 9f;
    public float regenPerSecond = 5f;

    float health;
    float timeSinceDamage;

    public float HealthPercent => health / maxHealth;
    public bool IsDead => health <= 0f;

    void Awake()
    {
        health = maxHealth;
    }

    void Update()
    {
        if (IsDead) return;

        timeSinceDamage += Time.deltaTime;
        if (timeSinceDamage > regenDelay && health < maxHealth)
            health = Mathf.Min(maxHealth, health + regenPerSecond * Time.deltaTime);
    }

    public void TakeDamage(float amount)
    {
        if (IsDead) return;

        health -= amount;
        timeSinceDamage = 0f;
        GameManager.Instance.hud.FlashDamage();

        if (health <= 0f)
        {
            health = 0f;
            GameManager.Instance.Lose();
        }
    }
}
