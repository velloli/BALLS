using UnityEngine;
using UnityEngine.UIElements;

public class SetCollidingSurfaceNormal : MonoBehaviour
{
    private Material ballMaterial;

    private Vector3 GIZMO_TERRAIN_NRM;
    void Start()
    {
        // Get the material of the ball
        Renderer ballRenderer = GetComponent<Renderer>();
        if (ballRenderer != null)
        {
            ballMaterial = ballRenderer.material;
        }
        else
        {
            Debug.LogError("Ball does not have a Renderer component.");
        }
    }

    void OnCollisionStay(Collision collision)
    {
        // Check if the collided object has a TerrainCollider component
        TerrainCollider terrainCollider = collision.collider.GetComponent<TerrainCollider>();
        if (terrainCollider != null && ballMaterial != null)
        {
            // Get the point of contact
            ContactPoint contact = collision.contacts[0];

            // Use a raycast to determine the normal of the terrain surface at the collision point
            
                // Get the normal of the terrain surface
                Vector3 surfaceNormal = contact.normal;
                GIZMO_TERRAIN_NRM = surfaceNormal.normalized;
                // Set the normal to the shader property on the ball material
                ballMaterial.SetVector("_CollidingSurfaceNormal", surfaceNormal);

            

        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawLine(transform.position, transform.position + GIZMO_TERRAIN_NRM);


    }
}
