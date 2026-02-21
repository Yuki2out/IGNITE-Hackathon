using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DoorsMngr : MonoBehaviour
{
    public GameObject Doors;       
    public GameObject trigger;     
    public float timer = 10f;      
    public float triggerOffset = 2f; 

    [Header("Vanishing Settings")]
    public float minFadeDuration = 0.1f; // Minimum time to disappear
    public float maxFadeDuration = 30f; // Maximum time to disappear

    private bool hasTriggered = false;

    void Update()
    {
        if (!hasTriggered)
        {
            if (timer > 0)
            {
                timer -= Time.deltaTime;
            }
            else
            {
                Debug.Log("<color=cyan>Timer Finished!</color> Selecting random door...");
                hasTriggered = true;
                SelectRandomObject();
            }
        }
    }

    void SelectRandomObject()
    {
        if (Doors == null) return;

        List<Transform> doorParents = new List<Transform>();
        foreach (Transform t in Doors.transform) {
            doorParents.Add(t);
        }

        if (doorParents.Count == 0) return;

        int randomIndex = Random.Range(0, doorParents.Count);
        Transform targetDoor = doorParents[randomIndex];
        Debug.Log("<color=green>Target Door Selected: </color>" + targetDoor.name);

        foreach (Transform doorParent in doorParents)
        {
            Renderer[] renderers = doorParent.GetComponentsInChildren<Renderer>();

            if (doorParent == targetDoor)
            {
                SpawnTrigger(targetDoor);
                foreach (Renderer r in renderers) {
                    // This uses a fixed 15 second duration for the red fade
                    StartCoroutine(FadeToColor(r, Color.red, 15f));
                }
            }
            else
            {
                // WE PICK ONE DURATION PER DOOR PARENT
                // This way the door and its frame disappear at the exact same rate
                float randomDuration = Random.Range(minFadeDuration, maxFadeDuration);
                
                foreach (Renderer r in renderers) {
                    StartCoroutine(FadeToTransparent(r, randomDuration));
                }
            }
        }
    }

    IEnumerator FadeToColor(Renderer rend, Color targetColor, float duration)
    {
        Material mat = rend.material; 
        Color startColor = mat.GetColor("_BaseColor");
        float elapsed = 0;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            mat.SetColor("_BaseColor", Color.Lerp(startColor, targetColor, elapsed / duration));
            yield return null;
        }
    }

    IEnumerator FadeToTransparent(Renderer rend, float duration)
    {
        Material mat = rend.material; 
        Color startColor = mat.GetColor("_BaseColor");
        float elapsed = 0;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float normalizedTime = elapsed / duration; // 0 at start, 1 at end
            
            // Calculate new alpha: starting at 1, ending at 0
            float newAlpha = Mathf.Lerp(1f, 0f, normalizedTime);
            
            mat.SetColor("_BaseColor", new Color(startColor.r, startColor.g, startColor.b, newAlpha));
            yield return null;
        }
        
        rend.gameObject.SetActive(false);
    }

    void SpawnTrigger(Transform target)
    {
        Vector3 spawnPos = target.position + (target.forward * triggerOffset);
        Instantiate(trigger, spawnPos, target.rotation);
    }
}