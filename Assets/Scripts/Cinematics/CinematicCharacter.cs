using System.Collections;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public class CinematicCharacter : MonoBehaviour
{
    [SerializeField] bool cycleIdle = false;
    [SerializeField] Vector3 cycleHalfDirection = Vector3.up;
    [SerializeField] float halfCycleDuration = 1f;

    Coroutine movementCoroutine = null;

    Vector3 lerpPos = Vector3.zero;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCycle();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SetLerpX(float x) => lerpPos.x = x;
    public void SetLerpY(float y) => lerpPos.y = y;
    public void SetLerpZ(float z) => lerpPos.z = z;

    public void StartLerp(float duration = 2f)
    {
        LerpTo(lerpPos, duration);
    }

    public void StartCycle()
    {
        if (movementCoroutine != null)
            StopCoroutine(movementCoroutine);

        movementCoroutine = StartCoroutine(CycleInPlace());
    }

    IEnumerator CycleInPlace()
    {
        Vector3 origin = transform.position;
        Vector3 part1 = transform.position + cycleHalfDirection;
        Vector3 part2 = transform.position - cycleHalfDirection;

        while (true)
        {
            // Origin to part 1
            yield return CycleInPlace(origin, part1);

            // Part 1 to origin
            yield return CycleInPlace(part1, origin);

            // Origin to part 2
            yield return CycleInPlace(origin, part2);

            // Part 2 to origin
            yield return CycleInPlace(part2, origin);
        }
    }

    IEnumerator CycleInPlace(Vector3 startLocation, Vector3 endLocation)
    {
        for (float time = 0; time < halfCycleDuration; time += Time.deltaTime)
        {
            transform.position = Vector3.Lerp(startLocation, endLocation, time / halfCycleDuration);
            yield return new WaitForEndOfFrame();
        }
    }

    public void LerpTo(Vector3 pos, float duration)
    {
        if (movementCoroutine != null)
            StopCoroutine(movementCoroutine);

        movementCoroutine = StartCoroutine(LerpToLocation(pos, duration));
    }

    IEnumerator LerpToLocation(Vector3 newLocation, float duration)
    {
        Vector3 startLocation = transform.position;
        for (float time = 0; time < duration; time += Time.deltaTime)
        {
            transform.position = Vector3.Lerp(startLocation, newLocation, time / duration);
            yield return new WaitForEndOfFrame();
        }

        if (cycleIdle)
        {
            StartCycle();
        }
    }
}
