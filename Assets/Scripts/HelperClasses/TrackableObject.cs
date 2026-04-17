using UnityEngine;
using UnityEngine.Events;

public class TrackableObject : MonoBehaviour
{
    public UnityEvent OnObjectDestroyed;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnDestroy()
    {
        OnObjectDestroyed?.Invoke();
    }
}
