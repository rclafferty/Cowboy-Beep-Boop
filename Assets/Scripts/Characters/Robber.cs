using Unity.VisualScripting;
using UnityEngine;

public class Robber : TrackableObject, ICombat
{
    [SerializeField] Animator animationController;

    [SerializeField] Player player;
    [SerializeField] GameObject bullet;

    [SerializeField] float shootDistance = 3f;
    [SerializeField] HealthComponent health;
    float shootCooldown = 0.5f;
    float currentShootCooldown = 0f;

    bool isDead = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = FindAnyObjectByType<Player>();
        health = GetComponent<HealthComponent>();
        health.OnDeath.AddListener(Die);

        if (animationController == null)
            animationController = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(transform.position, player.transform.position) <= shootDistance)
        {
            if (currentShootCooldown < shootCooldown)
            {
                currentShootCooldown += Time.deltaTime;
            }
            else
            {
                currentShootCooldown = 0f;
                Shoot();
            }
        }
        else
        {
            MoveToPlayer();
        }
    }

    void MoveToPlayer()
    {
        if (player == null)
            return;

        if (!health.IsAlive)
            return;

        HealthComponent playerHealth = player.GetComponent<HealthComponent>();
        if (playerHealth == null)
            return;

        if (!playerHealth.IsAlive)
            return;

        Vector3 directionToPlayer = (player.transform.position - transform.position).normalized;
        Debug.Log($"Robber {gameObject.name} moving towards player at direction {directionToPlayer}");
        animationController.SetFloat("Walk-X", directionToPlayer.x);
        animationController.SetFloat("Walk-Y", directionToPlayer.y);
        transform.position += directionToPlayer * Time.deltaTime;
    }

    void Shoot()
    {
        if (player == null)
            return;

        if (!health.IsAlive)
            return;

        HealthComponent playerHealth = player.GetComponent<HealthComponent>();
        if (playerHealth == null)
            return;

        if (!playerHealth.IsAlive)
            return;

        if (bullet == null)
            return;

        animationController.SetTrigger("Shooting");

        Vector3 directionToPlayer = (player.transform.position - transform.position).normalized;
        GameObject newBullet = Instantiate(bullet, transform.position, Quaternion.Euler(directionToPlayer.x, 0, directionToPlayer.y));
        Bullet bulletComponent = newBullet.GetComponent<Bullet>();
        bulletComponent.objectsToIgnore.Add(gameObject);
        bulletComponent.instigator = this;
        newBullet.transform.up = directionToPlayer;
    }

    public int GetTeam()
    {
        return 1; // 1 for enemy
    }

    public void Heal(int amount)
    {
        if (!health.IsAlive)
            return;

        health.Heal(amount);
    }

    public void TakeDamage(int amount)
    {
        if (!health.IsAlive)
            return;

        health.TakeDamage(amount);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!health.IsAlive)
            return;

        Bullet bulletComp = collision.GetComponent<Bullet>();
        if (bulletComp == null)
            return;

        if (bulletComp.instigator.GetTeam() == GetTeam())
            return;

        health.TakeDamage(1);
        //Debug.Log($"Robber {gameObject.name} took {1} damage -- HP: {health.GetCurrentHealth()}");
    }

    public void Die()
    {
        if (isDead)
            return;

        isDead = true;
        animationController.SetTrigger("Die");

        //Destroy(gameObject);
    }
}
