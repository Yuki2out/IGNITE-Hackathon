<<<<<<< HEAD
using UnityEngine;

public class ExitTeleporter : MonoBehaviour
{
    // Drag your "TeleportTarget" object here in the Inspector
    public Transform destination; 
    public GameObject player;

    // This runs automatically when something leaves the trigger
    private void OnTriggerExit(Collider player)
    {
        // Check if the object leaving is actually the Player
        if (player)
        {
            // Teleport the player's position to the destination's position
            player.transform.position = destination.position;
            
            // Optional: Match the player's rotation to the destination's rotation
            player.transform.rotation = destination.rotation;

            Debug.Log("Player has been teleported!");
        }
    }
=======
using UnityEngine;

public class ExitTeleporter : MonoBehaviour
{
    // Drag your "TeleportTarget" object here in the Inspector
    public Transform destination; 
    public GameObject player;

    // This runs automatically when something leaves the trigger
    private void OnTriggerExit(Collider player)
    {
        // Check if the object leaving is actually the Player
        if (player)
        {
            // Teleport the player's position to the destination's position
            player.transform.position = destination.position;
            
            // Optional: Match the player's rotation to the destination's rotation
            player.transform.rotation = destination.rotation;

            Debug.Log("Player has been teleported!");
        }
    }
>>>>>>> main
}