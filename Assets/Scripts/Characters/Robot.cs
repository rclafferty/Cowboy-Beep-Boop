using UnityEngine;

public class Robot : MonoBehaviour
{
    [SerializeField] HealthComponent health;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        health = GetComponent<HealthComponent>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
