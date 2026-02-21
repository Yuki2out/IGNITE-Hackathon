using UnityEngine;

public class PlayerEventMeng : MonoBehaviour
{
    public int myCounter = 0;
    public int timesToKnock = 3;

    public float switchTimer = 5f; 
    public GameObject cardboardHousePrefab; 
    public GameObject housesHolder;
    
    private bool allHousesEnabled = false;
    private bool housesSwitched = false;

    void Update()
    {
        if (myCounter >= timesToKnock && !allHousesEnabled)
        {
            EnableAllChildren();
        }

        if (allHousesEnabled && !housesSwitched)
        {
            if (switchTimer > 0)
            {
                switchTimer -= Time.deltaTime;
            }
            else
            {
                SwitchHousesWithCardboard();
            }
        }
    }

    void EnableAllChildren()
    {
        foreach (Transform child in housesHolder.transform)
        {
            child.gameObject.SetActive(true);
        }
        allHousesEnabled = true;
    }

    void SwitchHousesWithCardboard()
    {
        Debug.Log("Timer up! Switching houses... saving the farmhouse.");
        housesSwitched = true;

        // Create a list of children to iterate through safely
        Transform[] allHouses = new Transform[housesHolder.transform.childCount];
        for (int i = 0; i < housesHolder.transform.childCount; i++)
        {
            allHouses[i] = housesHolder.transform.GetChild(i);
        }

        foreach (Transform house in allHouses)
        {
            // --- THE EXCEPTION CHECK ---
            // If the name matches exactly, we skip the switch logic
            if (house.name == "farmhouse_obj")
            {
                Debug.Log("Found the farmhouse! Keeping it real.");
                continue; // Skip the rest of the code for this specific loop
            }

            // Spawn cardboard version
            GameObject cardboard = Instantiate(cardboardHousePrefab, house.position, house.rotation);
            cardboard.transform.parent = housesHolder.transform;

            // Delete original
            Destroy(house.gameObject);
        }

        Debug.Log("Switch complete! Only the farmhouse remains.");
    }
}