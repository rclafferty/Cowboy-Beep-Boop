using Unity.VisualScripting;
using UnityEngine;

public class Robber : MonoBehaviour
{
    [SerializeField] Player player;
    [SerializeField] GameObject bullet;

    [SerializeField] float shootDistance = 3f;
    [SerializeField] HealthComponent health;
    float shootCooldown = 0.5f;
    float currentShootCooldown = 0f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = FindAnyObjectByType<Player>();
        health = GetComponent<HealthComponent>();
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

        transform.position += (player.transform.position - transform.position).normalized * Time.deltaTime;
    }

    void Shoot()
    {
        if (player == null)
            return;

        if (bullet == null)
            return;

        Vector3 directionToPlayer = (player.transform.position - transform.position).normalized;
        GameObject newBullet = Instantiate(bullet, transform.position, Quaternion.Euler(directionToPlayer.x, 0, directionToPlayer.y));
        Bullet bulletComponent = newBullet.GetComponent<Bullet>();
        bulletComponent.objectsToIgnore.Add(gameObject);
        newBullet.transform.up = directionToPlayer;
    }

    public int GetTeam()
    {
        return 1; // 1 for enemy
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
