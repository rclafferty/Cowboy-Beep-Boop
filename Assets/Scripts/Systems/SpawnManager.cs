using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] List<GameObject> spawners;
    [SerializeField] List<TrackableObject> prefabsToTrack;

    [SerializeField] UnityEvent OnAllEntitiesDestroyed;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddSpawnedEntity(TrackableObject spawnedEntity)
    {
        if (prefabsToTrack.Contains(spawnedEntity))
        {
            return;
        }

        prefabsToTrack.Add(spawnedEntity);
        spawnedEntity.OnObjectDestroyed.AddListener(OnEntityDied);
        SpawnedEntities++;
    }

    public void OnEntityDied()
    {
        SpawnedEntities--;
        Debug.Log($"An entity died! Remaining: {SpawnedEntities}");

        int numRemaining = 0;
        foreach (TrackableObject t in prefabsToTrack)
        {
            if (t == null)
                continue;

            HealthComponent health = t.GetComponent<HealthComponent>();
            if (health == null)
                continue;

            if (!health.IsAlive)
                continue;

            numRemaining++;
        }

        if (numRemaining == 0)
        {
            OnAllEntitiesDestroyed?.Invoke();
        }
    }

    public static SpawnManager Instance { get; private set; }

    public static int SpawnedEntities { get; private set; }
}
