using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sun : MonoBehaviour
{
    public bool SpinSun = true;
    public bool SpinEarth = true;

    public float SpinSpeed = 1f;

    void Update()
    {
        if (SpinSun)
            transform.Rotate(Vector3.up, SpinSpeed / 10 * Time.deltaTime);
        if (SpinEarth)
            transform.GetChild(0).transform.Rotate(Vector3.up, SpinSpeed * Time.deltaTime);
    }
}
