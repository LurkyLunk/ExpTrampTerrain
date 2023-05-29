using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class TrampolineTerrain : MonoBehaviour
{
    public float bounceForce = 5f;
    public float deformationAmount = 0.1f;

    private Mesh mesh;
    private Vector3[] originalVertices;

    private void Start()
    {
        mesh = GetComponent<MeshFilter>().mesh;
        originalVertices = mesh.vertices;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // Get the contact point on the trampoline
            ContactPoint contact = collision.contacts[0];
            Vector3 contactPoint = contact.point;

            // Calculate the deformed vertices based on the contact point
            Vector3[] deformedVertices = new Vector3[originalVertices.Length];
            for (int i = 0; i < originalVertices.Length; i++)
            {
                Vector3 originalVertex = originalVertices[i];
                Vector3 deformedVertex = originalVertex;

                // Calculate the distance between the contact point and the current vertex
                float distance = Vector3.Distance(contactPoint, transform.TransformPoint(originalVertex));

                // Apply deformation based on the distance from the contact point
                if (distance < deformationAmount)
                {
                    float deformation = Mathf.Lerp(0f, deformationAmount, distance / deformationAmount);
                    deformedVertex.y += deformation;
                }

                deformedVertices[i] = deformedVertex;
            }

            // Update the mesh vertices with the deformed vertices
            mesh.vertices = deformedVertices;
            mesh.RecalculateNormals();

            // Apply a bounce force to the player
            Rigidbody playerRigidbody = collision.gameObject.GetComponent<Rigidbody>();
            playerRigidbody.AddForce(Vector3.up * bounceForce, ForceMode.Impulse);
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // Reset the mesh vertices to their original positions
            mesh.vertices = originalVertices;
            mesh.RecalculateNormals();
        }
    }
}
