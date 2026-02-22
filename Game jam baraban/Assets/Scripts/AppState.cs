using UnityEngine;

public class AppState : MonoBehaviour
{
    public static AppState Instance;
    public bool hasShownMenu = false;

    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
}