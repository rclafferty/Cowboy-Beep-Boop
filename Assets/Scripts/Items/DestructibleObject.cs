using UnityEngine;

public class DestructibleObject : MonoBehaviour, ICombat
{
    [SerializeField] HealthComponent health;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        health = GetComponent<HealthComponent>();
        health.OnDeath.AddListener(Die);
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Bullet bulletComp = collision.GetComponent<Bullet>();
        if (bulletComp == null)
            return;

        if (bulletComp.instigator.GetTeam() == GetTeam())
            return;

        health.TakeDamage(1);
        Debug.Log($"Player {gameObject.name} took {1} damage -- HP: {health.GetCurrentHealth()}");
    }

    public void Die()
    {
        Debug.Log($"{gameObject.name} is destroyed!");
        Destroy(gameObject);
    }

    public int GetTeam()
    {
        return -1; // Neutral team
    }

    public void Heal(int amount)
    {
        return; // Destructible objects cannot be healed
    }

    public void TakeDamage(int amount)
    {
        health.TakeDamage(amount);
    }
}
