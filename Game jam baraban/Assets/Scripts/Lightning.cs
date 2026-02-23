using UnityEngine;

public class Lightning : MonoBehaviour
{
    private float time = 0;
    public float speed;

    private float lightingAccumulator;
    private int drawResult = 0;

    private Camera self;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        this.self = GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime * speed;

        float thunderProbability = Mathf.Pow(Mathf.PerlinNoise1D(time), 20) * 100f;
        float thunderProbability2 = Mathf.Pow(Mathf.PerlinNoise1D(time - 0.3f * speed), 20) * 100f;

        this.lightingAccumulator -= Time.deltaTime * 2f;
        this.lightingAccumulator = Mathf.Max(0, this.lightingAccumulator);

        if (thunderProbability > 0.7)
        {
            lightingAccumulator = 1;
        }
        
        if (thunderProbability2 > 0.9 && drawResult == 1)
        {
            lightingAccumulator += 0.1f;
        }

        if (lightingAccumulator <= 0)
        {
            drawResult = Random.Range(0, 2);
        }

        self.backgroundColor = Color.Lerp(
            new Color(0.25f, 0.25f, 0.25f),
            Color.white,
            Mathf.Pow(lightingAccumulator, 3)
        );
    }
}
