using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] public List<GameObject> objectsToIgnore;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += transform.up * Time.deltaTime * 5f;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (objectsToIgnore.Contains<GameObject>(collision.gameObject))
            return;

        Destroy(gameObject);
    }
}
