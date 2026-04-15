using UnityEngine;
using UnityEngine.Events;

public class HealthComponent : MonoBehaviour
{
    [SerializeField] GameObject healthBar;
    [SerializeField] int currentHealth = 10;
    [SerializeField] int maxHealth = 10;

    [SerializeField] public UnityEvent OnDeath;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentHealth = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public int GetCurrentHealth()
    {
        return currentHealth;
    }

    public int GetMaxHealth()
    {
        return maxHealth;
    }

    public void Heal(int amount)
    {
        if (amount < 0)
            return;

        Debug.Log($"{gameObject.name} healed for {amount} health!");
        currentHealth = Mathf.Clamp(currentHealth + amount, 0, maxHealth);
        UpdateHealthbar();
    }

    public void TakeDamage(int amount)
    {
        if (amount < 0)
            return;

        Debug.Log($"{gameObject.name} took {amount} damage!");
        currentHealth = Mathf.Clamp(currentHealth - amount, 0, maxHealth);
        UpdateHealthbar();

        if (currentHealth <= 0)
        {
            OnDeath?.Invoke();
        }
    }

    void UpdateHealthbar()
    {
        Vector3 healthbarScale = healthBar.transform.localScale;
        healthbarScale.x = (float)currentHealth / maxHealth;
        healthBar.transform.localScale = healthbarScale;
    }
}
