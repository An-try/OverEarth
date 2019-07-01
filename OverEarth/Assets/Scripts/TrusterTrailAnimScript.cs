using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrusterTrailAnimScript : MonoBehaviour
{
    public Transform ThusterTrail;
    public Transform PlayerShip;
    void Update()
    {
        Transform thusterTrail = Instantiate(ThusterTrail);
        thusterTrail.transform.position = transform.position;
        thusterTrail.rotation = PlayerShip.rotation;
        Destroy(thusterTrail.gameObject, 0.3f); // удалить объект следа через 0.3 секунды
    }
}
