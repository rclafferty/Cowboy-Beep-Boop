using System.Collections;
using UnityEngine;

public class Dynamite : MonoBehaviour
{
    [SerializeField] private float movementSpeed = 2f;
    [SerializeField] private float explosionRadius = 1f;
    [SerializeField] private float explosionFuseSec = 3f;
    [SerializeField] private int explosionDamage = 50;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(ExplodeAfterDelay());
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += transform.up * movementSpeed * Time.deltaTime;
    }

    IEnumerator ExplodeAfterDelay()
    {
        yield return new WaitForSeconds(explosionFuseSec);
        Explode();
    }

    private void Explode()
    {
        // Implement explosion logic here
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, explosionRadius);
        foreach (var hitCollider in hitColliders)
        {
            ICombat combat = hitCollider.GetComponent<ICombat>();
            if (combat != null)
            {
                combat.TakeDamage(explosionDamage); // Example damage value
            }
        }
    
        Destroy(gameObject); // Destroy the dynamite after exploding
    }
}
