using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] private GameObject prefabToSpawn;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GameObject g = Instantiate(prefabToSpawn, transform.position, Quaternion.identity);
        TrackableObject trackable = g.GetComponent<TrackableObject>();
        if (trackable != null )
        {
            SpawnManager.Instance.AddSpawnedEntity(trackable);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
