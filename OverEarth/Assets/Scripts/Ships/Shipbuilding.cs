using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class for shipbuilding visualization.
/// </summary>
public class Shipbuilding : MonoBehaviour
{
    public Mesh MainHullMesh;

    // Directions of the vertices of the mesh in which they will move, visualizing the construction of the ship
    private List<Vector3> Directions = new List<Vector3>();

    private Vector3[] defaultVertices;
    private Vector3[] currentVertices;

    private void Awake() // Awake is called when the script instance is being loaded
    {
        // Add random directions to the directions list
        Directions.Add(Vector3.up);
        Directions.Add(Vector3.down);
        Directions.Add(Vector3.right);
        Directions.Add(Vector3.left);
        Directions.Add(Vector3.forward);
        Directions.Add(Vector3.back);
    }

    private void Start() // Start is called on the frame when a script is enabled just before any of the Update methods are called the first time
    {
        MainHullMesh = GetComponent<MeshFilter>().mesh; // Get the mesh of this game object

        // Set current mesh vertices as default vertices and current vertices
        defaultVertices = MainHullMesh.vertices;
        currentVertices = MainHullMesh.vertices;
    }

    private void FixedUpdate() // FixedUpdate is called at a fixed framerate frequency
    {
        // User can deformate the ship by pressing the J key
        if (Input.GetKeyDown(KeyCode.J))
        {
            Deformate();
        }

        // User can formate the ship by holding the H key
        if (Input.GetKey(KeyCode.H))
        {
            Formate();
        }
    }

    private void Deformate()
    {
        for (int i = 0; i < MainHullMesh.vertexCount; i++) // Pass all vertices in mesh
        {
            currentVertices[i] += Directions[Random.Range(0, Directions.Count)] * 0.1f; // Change each vertex position in random direction
            RecalculateMesh();
        }
    }

    private void Formate()
    {
        for (int i = 0; i < MainHullMesh.vertexCount; i++) // Pass all vertices in mesh
        {
            Vector3 direction = currentVertices[i] - defaultVertices[i]; // Define direction in which each vertex will move
            currentVertices[i] -= direction * 0.01f; // Apply new direction
            RecalculateMesh(); // Apply new vertex positions to the mesh
        }
    }

    private void RecalculateMesh()
    {
        MainHullMesh.vertices = currentVertices; // Apply new vertex positions to the mesh
    }
}
