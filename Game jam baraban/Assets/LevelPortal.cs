using UnityEngine;
using UnityEngine.SceneManagement; // Required for scene switching

public class LevelPortal : MonoBehaviour
{
    [Header("Settings")]
    public string sceneToLoad; // Type the name of your scene here
    public int stageIndex;     // What level number is this? (e.g., 1, 2, 3)

    private bool isPlayerInside = false;

    void Update()
    {
        // 1. Check if player is in trigger and presses E
        if (isPlayerInside && Input.GetKeyDown(KeyCode.E))
        {
            SaveProgress();
            LoadNextScene();
        }
    }

    void SaveProgress()
    {
        // 2. Get the current saved stage (default to 0 if it doesn't exist)
        int currentSavedStage = PlayerPrefs.GetInt("StageReached", 0);

        // 3. Only update if the new stage is higher than what we already reached
        if (stageIndex > currentSavedStage)
        {
            PlayerPrefs.SetInt("StageReached", stageIndex);
            PlayerPrefs.Save(); // Forces the save to disk
            Debug.Log("New Stage Reached Saved: " + stageIndex);
        }
    }

    void LoadNextScene()
    {
        if (!string.IsNullOrEmpty(sceneToLoad))
        {
            SceneManager.LoadScene(sceneToLoad);
        }
        else
        {
            Debug.LogError("No Scene Name entered in the Inspector!");
        }
    }

    // --- Trigger Logic ---

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInside = true;
            Debug.Log("At portal. Press E to travel to " + sceneToLoad);
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