using System.Collections;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : TrackableObject, ICombat
{
    [SerializeField] Animator animationController;

    [SerializeField] Vector3 movement;
    [SerializeField] float speed = 2.0f;
    [SerializeField] HealthComponent health;
    [SerializeField] GameObject bullet;
    [SerializeField] GameObject dynamite;
    [SerializeField] float bulletCooldown = 0.1f;
    [SerializeField] float dynamiteCooldown = 5.0f;

    [SerializeField] bool disableControls = false;
    [SerializeField] bool disableShootAnyDirection = false;

    [SerializeField] GameObject dynamitePreview;
    bool showingDynamitePreview = false;
    bool isBulletCooldown = false;
    bool isDynamiteCooldown = false;
    bool allPartsGathered = false;

    int enginePartsGathered = 0;
    bool interactingWithEngine = false;
    Engine currentEngine = null;

    bool isDead = false;

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

        if (animationController == null)
            animationController = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += new Vector3(movement.x, movement.y) * Time.deltaTime;

        if (showingDynamitePreview && !isDynamiteCooldown)
        {
            Vector2 screenPos = Mouse.current.position.ReadValue();
            Vector3 worldPos = Camera.main.ScreenToWorldPoint(new Vector3(screenPos.x, screenPos.y, 10f));
            worldPos.z = transform.position.z;

            Vector3 directionToShoot = (worldPos - transform.position).normalized;
            dynamitePreview.transform.up = directionToShoot;

            const float fuseTime = 2;
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
        //Debug.Log($"Player {gameObject.name} moving: {movement}");

        //if (movement == Vector3.zero)
        //{
        //    return;
        //}

        animationController.SetFloat("Walk-X", movement.x);
        animationController.SetFloat("Walk-Y", movement.y);
    }

    public void OnShoot(InputValue value)
    {
        if (disableControls)
        {
            return;
        }

        if (bullet == null)
            return;

        if (isBulletCooldown)
            return;

        GameObject g = SpawnProjectile(bullet);
        if (g == null)
            return;

        Bullet bulletComponent = g.GetComponent<Bullet>();
        bulletComponent.objectsToIgnore.Add(gameObject);
        bulletComponent.instigator = this;

        animationController.SetTrigger("Shooting");
        StartCoroutine(StartBulletCooldown());
    }

    public void OnInteract(InputValue value)
    {
        if (!interactingWithEngine)
            return;

        if (currentEngine == null)
            return;

        //if (!allPartsGathered)
        //    return;

        currentEngine.AddPart(enginePartsGathered);
    }

    public void OnAbilityShoot(InputValue value)
    {
        if (disableControls)
        {
            return;
        }

        if (isDynamiteCooldown)
        {
            return;
        }

        Debug.Log($"Player {gameObject.name} shooting...");

        if (dynamite == null)
            return;

        GameObject g = SpawnProjectile(dynamite);
        if (g == null)
            return;

        Dynamite dynamiteComponent = g.GetComponent<Dynamite>();
        dynamiteComponent.IsDud = false;

        showingDynamitePreview = false;
        dynamitePreview.SetActive(false);
        StartCoroutine(StartDynamiteCooldown());
    }

    GameObject SpawnProjectile(GameObject prefab)
    {
        if (prefab == null)
            return null;

        GameObject newProjectile = null;
        if (disableShootAnyDirection)
        {
            Vector3 directionToShoot = movement == Vector3.zero ? Vector3.down : movement.normalized;
            newProjectile = Instantiate(prefab, transform.position, Quaternion.identity);
            newProjectile.transform.up = directionToShoot;
        }
        else
        {
            Vector2 screenPos = Mouse.current.position.ReadValue();
            Vector3 worldPos = Camera.main.ScreenToWorldPoint(new Vector3(screenPos.x, screenPos.y, 10f));
            worldPos.z = transform.position.z;

            Vector3 directionToShoot = (worldPos - transform.position).normalized;
            newProjectile = Instantiate(prefab, transform.position, Quaternion.identity);
            newProjectile.transform.up = directionToShoot;
        }
        return newProjectile;
    }

    public void OnAbilityShootPrep(InputValue value)
    {
        if (disableControls)
        {
            return;
        }

        if (isDynamiteCooldown)
        {
            return;
        }

        Debug.Log($"Player {gameObject.name} aiming dynamite...");

        if (dynamite == null)
            return;

        showingDynamitePreview = true;
        dynamitePreview.SetActive(true);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        bool isBullet = HandleBulletTriggerEvent(collision);
        if (isBullet)
            return;

        bool isEnginePart = HandleEnginePartTriggerEvent(collision);
        if (isEnginePart)
        {
            allPartsGathered = CheckIfAllEnginePartsGathered();
        }

        Engine engine = collision.GetComponent<Engine>();
        if (engine != null)
        {
            currentEngine = engine;
            interactingWithEngine = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        Engine engine = collision.GetComponent<Engine>();
        if (engine != null)
        {
            interactingWithEngine = false;
        }
    }

    bool HandleBulletTriggerEvent(Collider2D collision)
    {
        Bullet bulletComp = collision.GetComponent<Bullet>();
        if (bulletComp == null)
            return false;

        if (bulletComp.instigator.GetTeam() == GetTeam())
            return false;

        health.TakeDamage(1);
        //Debug.Log($"Player {gameObject.name} took {1} damage -- HP: {health.GetCurrentHealth()}");

        return true;
    }

    bool HandleEnginePartTriggerEvent(Collider2D collision)
    {
        EnginePart enginePartComp = collision.GetComponent<EnginePart>();
        if (enginePartComp == null)
            return false;

        enginePartsGathered++;
        Debug.Log($"Player {gameObject.name} picked up 1 engine part.");
        Destroy(collision.gameObject);

        return true;
    }

    bool CheckIfAllEnginePartsGathered()
    {
        return enginePartsGathered >= 5;
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
        if (isDead)
            return;

        isDead = true;
        disableControls = true;
        movement = Vector3.zero;
        Debug.Log($"Player {gameObject.name} is dead!");
        animationController.SetTrigger("Die");
    }

    IEnumerator StartBulletCooldown()
    {
        isBulletCooldown = true;
        yield return new WaitForSeconds(bulletCooldown);
        isBulletCooldown = false;
    }

    IEnumerator StartDynamiteCooldown()
    {
        isDynamiteCooldown = true;
        yield return new WaitForSeconds(dynamiteCooldown);
        isDynamiteCooldown = false;
    }
}
