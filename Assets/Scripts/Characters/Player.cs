using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : TrackableObject, ICombat
{
    [SerializeField] Vector3 movement;
    [SerializeField] float speed = 2.0f;
    [SerializeField] HealthComponent health;
    [SerializeField] GameObject bullet;
    [SerializeField] GameObject dynamite;

    [SerializeField] bool disableControls = false;

    [SerializeField] GameObject dynamitePreview;
    bool showingDynamitePreview = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        health = GetComponent<HealthComponent>();
        health.OnDeath.AddListener(Die);

        if (dynamite != null)
        {
            dynamitePreview = Instantiate(dynamite);
            dynamitePreview.SetActive(false);
            Dynamite dynamiteComponent = dynamitePreview.GetComponent<Dynamite>();
            if (dynamiteComponent != null)
                dynamiteComponent.IsDud = true;

            SpriteRenderer renderer = dynamitePreview.GetComponent<SpriteRenderer>();
            if (renderer != null)
            {
                Color c = renderer.color;
                c.a = 0.4f;
                renderer.color = c;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += new Vector3(movement.x, movement.y) * Time.deltaTime;

        if (showingDynamitePreview)
        {
            Vector2 screenPos = Mouse.current.position.ReadValue();
            Vector3 worldPos = Camera.main.ScreenToWorldPoint(new Vector3(screenPos.x, screenPos.y, 10f));

            Vector3 directionToShoot = (worldPos - transform.position).normalized;
            dynamitePreview.transform.up = directionToShoot;

            const float fuseTime = 3;
            const float movementSpeed = 2;

            dynamitePreview.transform.position = transform.position + (directionToShoot * movementSpeed * fuseTime);
        }
    }

    public void OnMove(InputValue value)
    {
        if (disableControls)
        {
            return;
        }

        movement = value.Get<Vector2>() * speed;
        Debug.Log($"Player {gameObject.name} moving: {movement}");
    }

    public void OnShoot(InputValue value)
    {
        if (disableControls)
        {
            return;
        }

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

    public void OnAbilityShoot(InputValue value)
    {
        if (disableControls)
        {
            return;
        }

        Debug.Log($"Player {gameObject.name} shooting...");

        if (dynamite == null)
            return;

        Vector2 screenPos = Mouse.current.position.ReadValue();
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(new Vector3(screenPos.x, screenPos.y, 10f));

        Vector3 directionToShoot = (worldPos - transform.position).normalized;
        GameObject newBullet = Instantiate(dynamite, transform.position, Quaternion.Euler(directionToShoot.x, 0, directionToShoot.y));
        Dynamite bulletComponent = newBullet.GetComponent<Dynamite>();
        bulletComponent.IsDud = false;
        newBullet.transform.up = directionToShoot;

        showingDynamitePreview = false;
        dynamitePreview.SetActive(false);
    }

    public void OnAbilityShootPrep(InputValue value)
    {
        if (disableControls)
        {
            return;
        }

        Debug.Log($"Player {gameObject.name} aiming dynamite...");

        if (dynamite == null)
            return;

        //Vector2 screenPos = Mouse.current.position.ReadValue();
        //Vector3 worldPos = Camera.main.ScreenToWorldPoint(new Vector3(screenPos.x, screenPos.y, 10f));

        //Vector3 directionToShoot = (worldPos - transform.position).normalized;
        //GameObject newBullet = Instantiate(dynamite, transform.position, Quaternion.Euler(directionToShoot.x, 0, directionToShoot.y));
        //Dynamite bulletComponent = newBullet.GetComponent<Dynamite>();
        //if (bulletComponent == null)
        //    return;

        //bulletComponent.IsDud = true;
        //newBullet.transform.up = directionToShoot;

        //SpriteRenderer renderer = newBullet.GetComponent<SpriteRenderer>();
        //if (renderer == null)
        //{
        //    // TODO: ERROR
        //    return;
        //}

        //Color c = renderer.color;
        //c.a = 0.4f;
        //renderer.color = c;

        showingDynamitePreview = true;
        dynamitePreview.SetActive(true);
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
