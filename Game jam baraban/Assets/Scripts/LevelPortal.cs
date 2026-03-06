using UnityEngine;
using UnityEngine.SceneManagement; // Required for scene switching

public class LevelPortal : MonoBehaviour
{
    [Header("Settings")]
    public string sceneToLoad; // Type the name of your scene here
    public int stageIndex;     // What level number is this? (e.g., 1, 2, 3)

    public bool needToPressE = false;

    private bool isPlayerInside = false;

    private bool levelChangeScheduled = false;

    private FadeManager fadeManager;

    void Start()
    {
        this.fadeManager = Utils.TryGetComponent<FadeManager>("FadeManager");

        // if (this.fadeManager != null)
        // {
        //     this.fadeManager.Unfade();
        // }
    }

    void Update()
    {
        bool finished = (this.fadeManager == null || this.fadeManager.finished);

        if (levelChangeScheduled && finished)
        {
            LoadNextScene();
        }

        if (!isPlayerInside) return;

        bool pressedKey = Input.GetKeyDown(KeyCode.E) || !needToPressE;
        if (!pressedKey || levelChangeScheduled) return;

        // 1. Check if player is in trigger and presses E
        SaveProgress();

        GameObject.Find("DoorOpen").GetComponent<AudioSource>().Play();

        if (this.fadeManager != null)
        {
            fadeManager.Fade();
            this.fadeManager.finished = false; 
        }

        levelChangeScheduled = true;
    }

    void SaveProgress()
    {
        // 2. Get the current saved stage (default to 0 if it doesn't exist)
        int currentSavedStage = PlayerPrefs.GetInt("StageReached", 0);

        // 3. Only update if the new stage is higher than what we already reached
        if (stageIndex <= currentSavedStage) return;

        PlayerPrefs.SetInt("StageReached", stageIndex);
        PlayerPrefs.Save(); // Forces the save to disk

        Debug.Log("New Stage Reached Saved: " + stageIndex);
    }

    void LoadNextScene()
    {
        if (string.IsNullOrEmpty(sceneToLoad))
        {
            Debug.LogError("No Scene Name entered in the Inspector!");
            return;
        }

        SceneManager.LoadScene(sceneToLoad);
    }

    // --- Trigger Logic ---

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        isPlayerInside = true;
        Debug.Log("At portal. Press E to travel to " + sceneToLoad);
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        isPlayerInside = false;
    }
}