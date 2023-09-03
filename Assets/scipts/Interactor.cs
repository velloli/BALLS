using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactor : MonoBehaviour
{
    public float interactionRadius = 3f; // The radius within which the player can interact with NPCs
    public bool interactionInProgress = false;

    private NPC_actor npcInteraction;
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E)) // Change KeyCode.E to the key you want to use for interaction
        {
            if (!interactionInProgress)
            {
                TryInteractWithNPC();
            }
            else
            {
                StopInteractWithNPC();
            }
        }
    }

    private void StopInteractWithNPC()
    {
        npcInteraction.ResetCamera();
        interactionInProgress = false;

    }
    private void TryInteractWithNPC()
    {
        // Detect NPCs within the interaction radius
        Collider[] colliders = Physics.OverlapSphere(transform.position, interactionRadius);

        foreach (Collider collider in colliders)
        {
            // Check if the object is an NPC (you can tag NPCs with "NPC" tag or use a specific layer)
            if (collider.CompareTag("NPC"))
            {
                // Get the NPC script component and initiate the interaction
                npcInteraction = collider.GetComponent<NPC_actor>();
                if (npcInteraction != null)
                {
                    npcInteraction.InitiateInteraction();
                    interactionInProgress = true;

                }
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        // Visualize the interaction radius in the scene view
        Gizmos.color = Color.yellow;
        //Gizmos.DrawWireSphere(transform.position, interactionRadius);
    }
}
