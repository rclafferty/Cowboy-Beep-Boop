using UnityEngine;
using UnityEngine.Events;

public class Engine : MonoBehaviour
{
    [SerializeField] public UnityEvent OnEngineRepaired;

    [SerializeField] int totalPartsRequired = 5;
    int currentParts = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddPart(int numParts)
    {
        if (numParts <= 0)
            return;
        Debug.Log($"Added {numParts} engine part(s)!");
        currentParts += numParts;

        if (currentParts >= totalPartsRequired)
            OnEngineRepaired?.Invoke();
    }
}
