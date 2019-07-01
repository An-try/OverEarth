using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetInfoCamera : MonoBehaviour
{
    public GameObject PlayerShip;

    public Vector3 Offset = Vector3.zero;

    void Start()
    {
        Offset = new Vector3(Offset.x, Offset.y, -30f);
    }

    void Update()
    {
        if (Manager.instance.CurrentSelectedTarget != null)
        {
            transform.SetParent(Manager.instance.CurrentSelectedTarget.transform);
            transform.position = Manager.instance.CurrentSelectedTarget.transform.position + Offset;
        }
    }
}
