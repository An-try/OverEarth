using UnityEngine;

/// <summary>
/// Class that deformate this object when hit by any other object
/// </summary>
public class ObjectDeformation : MonoBehaviour
{
    private Mesh mesh; // This object mesh component

    public float minVelocity = 5f; // Minimum velocity at which the object will be deformed
    public float deformationRadius = 0.5f;
    public float multiply = 0.04f;

    private void Start() // Start is called on the frame when a script is enabled just before any of the Update methods are called the first time
    {
        mesh = GetComponent<MeshFilter>().mesh; // Get mesh component on this game object
    }

    private void OnCollisionEnter(Collision collision) // Called when this collider/rigidbody has begun touching another rigidbody/collider
    {
        // If relative velocity between two objects is higher than minimum velocity for deformation
        if (collision.relativeVelocity.magnitude > minVelocity)
        {
            bool isDeformated = false;
            Vector3[] vertices = mesh.vertices; // Get all mesh vecticle positions

            for (int i = 0; i < mesh.vertexCount; i++) // Pass all vertices in the mesh
            {
                for (int j = 0; j < collision.contacts.Length; j++) // Pass all contact points
                {
                    // Get local coordinates of the contact point in relation to this object
                    Vector3 point = transform.InverseTransformPoint(collision.contacts[j].point);
                    // Get local velocity vector of these objects that hit each other
                    Vector3 velocity = transform.InverseTransformVector(collision.relativeVelocity);
                    float distance = Vector3.Distance(point, vertices[i]); // Get the distance between contact point and each vertex

                    if (distance < deformationRadius) // If the distance is less than deformation radius
                    {
                        // Set and apply the deformation
                        Vector3 deformation = velocity * (deformationRadius - distance) * multiply;
                        vertices[i] += deformation;
                        isDeformated = true;
                    }
                }
            }

            if (isDeformated)
            {
                mesh.vertices = vertices; // Apply new vectices
                mesh.RecalculateNormals(); // Recalculate the normals of the mesh from the triangles and vertices
                mesh.RecalculateBounds(); // Recalculate the bounding volume of the mesh from the vertices
                if (GetComponent<MeshCollider>()) // If this object has mesh collider
                {
                    GetComponent<MeshCollider>().sharedMesh = mesh; // Set mesh to the mesh collider
                }
            }
        }
    }
}
