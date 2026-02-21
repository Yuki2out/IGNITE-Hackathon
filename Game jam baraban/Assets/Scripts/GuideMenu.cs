using UnityEngine;

public class GuideMenu : MonoBehaviour
{
    public GameObject guidePanel;

    void Update()
    {
        if (Input.anyKeyDown)
        {
            guidePanel.SetActive(false);
        }
    }
}