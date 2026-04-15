using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour, ICombat
{
    [SerializeField] Vector3 movement;
    [SerializeField] float speed = 2.0f;
    [SerializeField] HealthComponent health;
    [SerializeField] GameObject bullet;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        health = GetComponent<HealthComponent>();
        health.OnDeath.AddListener(Die);
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

    public void OnShoot(InputValue value)
    {
        //if (!value.Get<bool>())
        //    return;

        Debug.Log($"Player {gameObject.name} shooting...");

        if (bullet == null)
            return;

        Vector2 screenPos = Mouse.current.position.ReadValue();
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(new Vector3(screenPos.x, screenPos.y, 10f));

        Vector3 directionToShoot = (worldPos - transform.position).normalized;
        GameObject newBullet = Instantiate(bullet, transform.position, Quaternion.Euler(directionToShoot.x, 0, directionToShoot.y));
        Bullet bulletComponent = newBullet.GetComponent<Bullet>();
        bulletComponent.objectsToIgnore.Add(gameObject);
        bulletComponent.instigator = this;
        newBullet.transform.up = directionToShoot;
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

    public void Die()
    {
        Debug.Log($"Player {gameObject.name} is dead!");
    }
}
