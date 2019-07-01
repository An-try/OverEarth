using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExecutorBuilding : MonoBehaviour
{
    public GameObject Portal;

    public Mesh MainHullMesh;

    private List<Vector3> Directions = new List<Vector3>();

    private Vector3[] oldVerticles;
    private Vector3[] verticles;

    void Awake()
    {
        Directions.Add(Vector3.up);
        Directions.Add(-Vector3.up);
        Directions.Add(Vector3.right);
        Directions.Add(-Vector3.right);
        Directions.Add(Vector3.forward);
        Directions.Add(-Vector3.forward);
    }

    void Start()
    {
        MainHullMesh = GetComponent<MeshFilter>().mesh;

        oldVerticles = MainHullMesh.vertices;
        verticles = MainHullMesh.vertices;
    }

    void FixedUpdate()
    {
        if (Input.GetKeyDown(KeyCode.J))
        {
            Deformate();
        }

        if (Input.GetKey(KeyCode.H))
        {
            Formate();
        }
    }

    void Deformate()
    {
        for (int i = 0; i < MainHullMesh.vertexCount; i++)
        {
            verticles[i] += Directions[Random.Range(0, Directions.Count)] * 0.1f;
            RecalculateMesh();
        }
    }

    void Formate()
    {
        for (int i = 0; i < MainHullMesh.vertexCount; i++)
        {
            Vector3 direction = verticles[i] - oldVerticles[i];
            verticles[i] -= direction * 0.01f;
            RecalculateMesh();
        }
    }

    void RecalculateMesh()
    {
        MainHullMesh.vertices = verticles;
        //MainHullMesh.RecalculateNormals();
        //MainHullMesh.RecalculateBounds();
        //if (GetComponent<MeshCollider>() != null)
        //{
        //    GetComponent<MeshCollider>().sharedMesh = MainHullMesh;
        //}
    }
}