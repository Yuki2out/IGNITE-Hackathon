using UnityEngine;

public class KnockDoor : MonoBehaviour
{
    private bool isPlayerInside = false;
    public bool needToPressE = false;
    private PlayerEventMeng playerScript;

    void Update()
    {
        // 1. Check for Input
        if (!(isPlayerInside && (Input.GetKeyDown(KeyCode.E) || !needToPressE))) return;
        if (playerScript == null) return;

        // Increase the counter on the player
        playerScript.knockCounter++;
        Debug.Log("Knock! Total knocks: " + playerScript.knockCounter);
        
        // Trigger the house spawning logic
        EnableNearestDisabledHouse();

        // 2. Destroy the trigger so this door can't be used again
        Debug.Log("Door trigger used and destroyed.");
        gameObject.SetActive(false);
        // Destroy(gameObject);
    }

    void EnableNearestDisabledHouse()
    {
        Transform pTrans = playerScript.transform;
        GameObject closestHiddenHouse = null;
        float minDistance = Mathf.Infinity;

        // Loop through ONLY the direct children (the houses themselves)
        foreach (Transform house in playerScript.housesHolder.transform)
        {
            // 1. Check if the house is actually disabled
            if (house.gameObject.activeSelf) continue;

            float dist = Vector3.Distance(pTrans.position, house.position);
            
            if (dist < minDistance)
            {
                minDistance = dist;
                closestHiddenHouse = house.gameObject;
            }
        }

        // 2. Enable the winner
        if (closestHiddenHouse != null)
        {
            closestHiddenHouse.SetActive(true);
            Debug.Log("SUCCESS! Enabled: " + closestHiddenHouse.name);
        }
        else
        {
            Debug.LogWarning("FAILED: No disabled child found inside " + playerScript.housesHolder.name);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerScript = other.GetComponent<PlayerEventMeng>();
            if (playerScript != null) 
            {
                isPlayerInside = true;
                Debug.Log("In range of door. Press E.");
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInside = false;
        }
    }
}