using UnityEngine;
using UnityEngine.UIElements;

public class SetCollidingSurfaceNormal : MonoBehaviour
{
    private Material ballMaterial;

    private Vector3 GIZMO_TERRAIN_NRM;
    //void OnCollisionStay(Collision collision)
    //{
    //    // Check if the collided object has a TerrainCollider component
    //    TerrainCollider terrainCollider = collision.collider.GetComponent<TerrainCollider>();
    //    if (terrainCollider != null && ballMaterial != null)
    //    {
    //        // Get the point of contact
    //        ContactPoint contact = collision.contacts[0];
    //        // Use a raycast to determine the normal of the terrain surface at the collision point    
    //            // Get the normal of the terrain surface
    //            Vector3 surfaceNormal = contact.normal;
    //            GIZMO_TERRAIN_NRM = surfaceNormal.normalized;
    //            // Set the normal to the shader property on the ball material
    //            ballMaterial.SetVector("_CollidingSurfaceNormal", surfaceNormal);       
    //    }
    //}

    private Terrain terrain;
    void Start()
    {
        // Assume the terrain is the only one in the scene
        terrain = Terrain.activeTerrain;
        if (terrain == null)
        {
            Debug.LogError("No active terrain found in the scene.");
        }

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

    void FixedUpdate()
    {
        if (terrain != null)
        {
            // Cast a ray from the player's position downward to find the point on the terrain
            Ray ray = new Ray(transform.position + Vector3.up * 2f, Vector3.down);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, LayerMask.GetMask("Terrain")))
            {
                // Get the point on the terrain below the player
                Vector3 terrainPoint = hit.point;

                // Get the normal of the surface at the hit point
                Vector3 surfaceNormal = hit.normal;
                GIZMO_TERRAIN_NRM = surfaceNormal;
                ballMaterial.SetVector("_CollidingSurfaceNormal", surfaceNormal);       

                // Do something with the terrain point and surface normal
                //Debug.Log("Terrain Point Below Player: " + terrainPoint);
                Debug.Log("Surface Normal: " + surfaceNormal.normalized);
            }
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawLine(transform.position, transform.position + GIZMO_TERRAIN_NRM);


    }
}
