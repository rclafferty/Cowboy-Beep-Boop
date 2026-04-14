using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour, ICombat
{
    [SerializeField] Vector3 movement;
    [SerializeField] float speed = 2.0f;
    [SerializeField] HealthComponent health;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        health = GetComponent<HealthComponent>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += new Vector3(movement.x, movement.y) * Time.deltaTime;
    }

    public void OnMove(InputValue value)
    {
        movement = value.Get<Vector2>() * speed;
        Debug.Log($"Player {gameObject.name} moving: {movement}");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Bullet>() != null)
        {
            health.TakeDamage(1);
            Debug.Log($"Player {gameObject.name} took {1} damage -- HP: {health.GetCurrentHealth()}");
        }
    }

    public int GetTeam()
    {
        return 0; // 0 for player
    }

    public void Heal(int amount)
    {
        health.Heal(amount);
    }

    public void TakeDamage(int amount)
    {
        health.TakeDamage(amount);
    }
}
