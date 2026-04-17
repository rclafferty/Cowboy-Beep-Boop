using UnityEngine;
using UnityEngine.Events;

public class ObjectiveManager : MonoBehaviour
{
    [SerializeField] SpawnManager spawnManager;
    [SerializeField] bool killObjectiveActive = false;
    [SerializeField] bool destroyObjectiveActive = false;

    [SerializeField] bool killObjectiveComplete = false;
    [SerializeField] bool destroyObjectiveComplete = false;

    [SerializeField] UnityEvent OnAllObjectivesCompleted;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (!killObjectiveActive && !destroyObjectiveActive)
        {
            Debug.LogError("No objectives are active! Please activate at least one objective.");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void KillObjectiveComplete()
    {
        killObjectiveComplete = true;
        CheckObjectivesComplete();
    }

    public void DestroyObjectiveComplete()
    {
        destroyObjectiveComplete = true;
        CheckObjectivesComplete();
    }

    void CheckObjectivesComplete()
    {
        if (killObjectiveActive && !killObjectiveComplete)
            return;

        if (destroyObjectiveActive && !destroyObjectiveComplete)
            return;

        OnAllObjectivesCompleted?.Invoke();
    }
}
