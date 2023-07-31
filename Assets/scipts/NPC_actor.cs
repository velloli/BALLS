using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC_actor : MonoBehaviour
{
    public string[] dialogueLines; // Array to hold the NPC's dialogue lines

    public Transform playerTransform; // Reference to the player's transform
    public Transform npcTransform; // Reference to the NPC's transform

    public Camera mainCamera; // Reference to the main camera

    public float cameraLookSpeed = 5f; // Speed at which the camera looks at the midpoint
    public float minFOV = 40f; // Minimum field of view to ensure both objects are in frame

    private Vector3 initialCameraPosition;
    private Quaternion initialCameraRotation;
    private float initialFOV;

    private void Start()
    {
        initialCameraPosition = mainCamera.transform.position;
        initialCameraRotation = mainCamera.transform.rotation;
        initialFOV = mainCamera.fieldOfView;
    }

    public void InitiateInteraction()
    {
        // You can implement various interactions here, such as showing dialogue or performing actions
        // For this example, let's simply print the NPC's dialogue lines to the console.
        Debug.Log("Initiated interaction with " + gameObject.name);

        foreach (string line in dialogueLines)
        {
            //Debug.Log(line);
        }

        // Calculate the midpoint between the player and the NPC
        Vector3 midpoint = (playerTransform.position + npcTransform.position) * 0.5f;

        // Calculate the direction from the midpoint to the player
        Vector3 directionToPlayer = playerTransform.position - midpoint;

        // Calculate the distance required for both the player and NPC to be in the camera's view
        float requiredDistance = directionToPlayer.magnitude / Mathf.Tan(mainCamera.fieldOfView * 0.5f * Mathf.Deg2Rad);

        // Calculate the new position of the camera
        Vector3 cameraPosition = midpoint - directionToPlayer.normalized * requiredDistance;

        GameManager.Instance.player.GetComponent<Schmovement>().stopMovement();
        // Smoothly move the camera to the new position and look at the midpoint
        mainCamera.transform.position = Vector3.Lerp(mainCamera.transform.position, cameraPosition, Time.deltaTime * cameraLookSpeed);
        mainCamera.GetComponent<CamerMovement>().holdUp = true;
        mainCamera.transform.LookAt(midpoint);

        // Adjust the camera's field of view to ensure both objects are in frame
       mainCamera.fieldOfView = Mathf.Max(minFOV, requiredDistance * Mathf.Tan(mainCamera.fieldOfView * 0.5f * Mathf.Deg2Rad));
    }

    public void ResetCamera()
    {
        // Reset the camera to its initial position, rotation, and field of view
        GameManager.Instance.player.GetComponent<Schmovement>().startMovement();
        mainCamera.GetComponent<CamerMovement>().holdUp = false;
        mainCamera.transform.position = initialCameraPosition;
        mainCamera.transform.rotation = initialCameraRotation;
        mainCamera.fieldOfView = initialFOV;
    }
}
