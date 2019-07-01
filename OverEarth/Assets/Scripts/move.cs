using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class move : MonoBehaviour
{
    private float startTime;
    private float journeyLength;

    public Transform target;
    Transform starterMark;

    void Start()
    {
        starterMark = transform;
        startTime = Time.time;
        journeyLength = Vector3.Distance(starterMark.position, target.position);
    }

    void FixedUpdate()
    {
        float distCovered = (Time.time - startTime);
        float fracJourney = distCovered / journeyLength;
        transform.position = Vector3.Lerp(starterMark.position, target.position, fracJourney * 0.1f);
        transform.rotation = Quaternion.Lerp(starterMark.rotation, target.rotation, Time.time * 0.0001f);
    }
}
