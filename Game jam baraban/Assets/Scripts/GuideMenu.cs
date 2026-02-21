using UnityEngine;

public class GuideMenu : MonoBehaviour
{
    public GameObject guidePanel;
    public PlayerController player;

    void Start()
    {
        player.cameraLocked = true;

        if(AppState.Instance.hasShownMenu)
        {
            Disable();
        }
        else
        {
            AppState.Instance.hasShownMenu = true;
        }
    }

    void Disable()
    {
        guidePanel.SetActive(false);
        player.cameraLocked = false;
    }

    void Update()
    {
        if (Input.anyKeyDown)
            Disable();
    }
}