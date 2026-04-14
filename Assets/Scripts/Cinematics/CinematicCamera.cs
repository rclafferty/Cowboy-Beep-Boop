using System.Collections;
using UnityEngine;

public class CinematicCamera : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LerpToPosition(Vector3 end, float duration)
    {
        StartCoroutine(LerpCameraPosition(transform.position, end, duration));
    }

    IEnumerator LerpCameraPosition(Vector3 start, Vector3 end, float duration)
    {
        for (float elapsedTime = 0; elapsedTime < duration; elapsedTime += Time.deltaTime)
        {
            transform.position = Vector3.Lerp(start, end, elapsedTime / duration);
            yield return null;
        }
        transform.position = end; // Ensure the final position is set
    }
}
