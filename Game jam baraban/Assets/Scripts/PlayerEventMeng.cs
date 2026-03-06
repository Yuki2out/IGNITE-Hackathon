using System.Collections.Generic;
using UnityEngine;

public class PlayerEventMeng : MonoBehaviour
{
    public int knockCounter = 0;
    public int requiredKnocksAmount = 3;

    public GameObject cardboardHousePrefab; 
    public GameObject housesHolder;
    public GameObject firstHouse;
    public GameObject abandonedHouse;
    
    private bool allHousesEnabled;
    private bool abandonedHouseActive;

    private List<Transform> noticedHouses = new();
    private List<Transform> allHouses = new();
    private List<Transform> scheduledForRemoval = new();

    void Start()
    {
        this.allHouses = new();

        this.allHousesEnabled = false;
        this.abandonedHouseActive = false;

        for (int i = 0; i < housesHolder.transform.childCount; i++)
        {
            allHouses.Add(housesHolder.transform.GetChild(i));
        }

        allHouses.Add(firstHouse.transform);
    }

    void Update()
    {
        if (knockCounter < requiredKnocksAmount) return;

        if (!allHousesEnabled)
        {
            EnableAllChildren();
        }
        else if (allHouses.Count != 0)
        {
            SwitchHousesWithCardboard();
            abandonedHouse.SetActive(false);
        }
        else if(!abandonedHouseActive && allHousesEnabled)
        {
            Vector3 toHouseVector = Vector3.Normalize(abandonedHouse.transform.position - this.transform.position);

            if (Vector3.Dot(toHouseVector, this.transform.forward) < 0f)
            {
                abandonedHouseActive = true;
                abandonedHouse.SetActive(true);
            }
        }
    }

    void EnableAllChildren()
    {
        foreach (Transform child in housesHolder.transform)
        {    
            child.gameObject.SetActive(true);
        }
        
        this.allHousesEnabled = true;
    }

    void SwitchHousesWithCardboard()
    {
        scheduledForRemoval.Clear();

        foreach (Transform house in allHouses)
        {
            if (Vector3.Dot(Vector3.Normalize(house.transform.position - this.transform.position), this.transform.forward) >= 0f)
            {
                noticedHouses.Add(house);
                continue;
            }

            if (!noticedHouses.Contains(house)) continue;
            if (Vector3.Distance(house.transform.position, this.gameObject.transform.position) < 15f) continue;

            GameObject cardboard = Instantiate(cardboardHousePrefab, house.position, house.rotation);
            cardboard.transform.parent = housesHolder.transform;

            house.gameObject.SetActive(false);

            scheduledForRemoval.Add(house);
        }
        
        foreach (Transform house in scheduledForRemoval)
        {
            allHouses.Remove(house);
            noticedHouses.Remove(house);

            Destroy(house.gameObject);
        }

        scheduledForRemoval.Clear();
    }
}