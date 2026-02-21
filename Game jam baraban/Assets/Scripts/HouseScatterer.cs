using UnityEngine;
using UnityEditor;

public class HouseScatterer : MonoBehaviour
{
    public GameObject housePrefab;
    public int numberOfHouses = 20;
    public float radius = 100f;
    public float houseRadius = 10f; 
    public LayerMask terrainLayer;
    public int houseLayerIndex = 6; // Set this to the ID of your "Houses" layer

    [ContextMenu("Scatter Houses Now")]
    public void Scatter()
    {
        int placedCount = 0;
        int attempts = 0;
        int maxAttempts = numberOfHouses * 10;

        while (placedCount < numberOfHouses && attempts < maxAttempts)
        {
            attempts++;
            Vector2 randomCircle = Random.insideUnitCircle * radius;
            Vector3 spawnPos = transform.position + new Vector3(randomCircle.x, 500f, randomCircle.y);

            if (Physics.Raycast(spawnPos, Vector3.down, out RaycastHit hit, 1000f, terrainLayer))
            {
                // Check for ANY collider within houseRadius, but ignore the terrain
                if (!IsSpotTaken(hit.point))
                {
                    GameObject newHouse = (GameObject)PrefabUtility.InstantiatePrefab(housePrefab);
                    
                    // FORCE the layer so the next check sees it
                    newHouse.layer = houseLayerIndex;
                    
                    newHouse.transform.position = hit.point;
                    
                    // Align to slope
                    Quaternion slopeRotation = Quaternion.FromToRotation(Vector3.up, hit.normal);
                    Quaternion randomFacing = Quaternion.Euler(0, Random.Range(0, 360), 0);
                    newHouse.transform.rotation = slopeRotation * randomFacing;
                    
                    newHouse.transform.parent = this.transform;
                    placedCount++;
                    
                    // CRITICAL: Tell Unity Physics to update so the next overlap check works
                    Physics.SyncTransforms(); 
                }
            }
        }
        Debug.Log($"Placed {placedCount} houses.");
    }

    bool IsSpotTaken(Vector3 targetPos)
    {
        // We look for anything on the House Layer
        int layerMask = 1 << houseLayerIndex;
        return Physics.CheckSphere(targetPos, houseRadius, layerMask);
    }
    [ContextMenu("Clear All Houses")]
    public void Clear()
    {
        while (transform.childCount > 0)
        {
            DestroyImmediate(transform.GetChild(0).gameObject);
        }
    }
}