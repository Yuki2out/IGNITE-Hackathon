using UnityEngine;
using UnityEngine.UI;

public class FadeManager : MonoBehaviour
{
    public RawImage screen;

    private float counter = 0f;
    private float multiplier = 1f;
    private bool counting = false;
    public bool finished = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Unfade();
    }

    // Update is called once per frame
    void Update()
    {
        if (!counting) return;

        counter += Time.deltaTime;

        float opacity = Mathf.Max(0f, Mathf.Min(1f, screen.color.a + counter * multiplier));
        screen.color = new Color(screen.color.r, screen.color.g, screen.color.b, opacity);

        if (counter >= 1f)
        {
            counting = false;
            finished = true;
            counter = 0f;
        }
    }

    public void Fade()
    {
        this.multiplier = 1f;
        this.counting = true;
        this.finished = false;
    }

    public void Unfade()
    {
        this.multiplier = -1f;
        this.counting = true;
        this.finished = false;
    }
}
