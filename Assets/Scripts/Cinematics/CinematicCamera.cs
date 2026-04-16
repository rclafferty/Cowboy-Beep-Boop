using System.Collections;
using Unity.Cinemachine;
using UnityEngine;

public class CinematicCamera : MonoBehaviour
{
    Vector3 lerpPos = Vector3.zero;
    Vector3 prevLerpPos = Vector3.zero;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SetLensSize(float lens)
    {
        CinemachineCamera cam = GetComponent<CinemachineCamera>();
        if (cam == null)
        {
            return;
        }

        cam.Lens.OrthographicSize = lens;
    }

    public void SetLerpX(float x) => lerpPos.x = x;
    public void SetLerpY(float y) => lerpPos.y = y;
    public void SetLerpZ(float z) => lerpPos.z = z;

    public void StartLerp(float duration = 2f)
    {
        LerpToPosition(lerpPos, duration);
    }

    public void StartLerpReturn(float duration = 2f)
    {
        LerpToPosition(prevLerpPos, duration);
    }

    public void LerpToPosition(Vector3 end, float duration = 2f)
    {
        prevLerpPos = transform.position;
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
