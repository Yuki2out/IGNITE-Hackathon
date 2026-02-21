using UnityEngine;

public class RoomStateManager : MonoBehaviour
{
    public Transform[] spawnPositions;
    public float[] environmentStrength;
    public float[] sunStrength;

    public Transform player;
    public Light sun;

    public int spawnPointIndex;

    void Spawn(int spawnPointIndex)
    {
        spawnPointIndex = Mathf.Min(spawnPositions.Length - 1, spawnPointIndex);

        player.transform.position = spawnPositions[spawnPointIndex].position;
        RenderSettings.ambientIntensity = environmentStrength[spawnPointIndex];
        sun.intensity = sunStrength[spawnPointIndex];
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Spawn(PlayerPrefs.GetInt("StageReached", 0));
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("t"))
        {
            spawnPointIndex++;

            if (spawnPointIndex >= spawnPositions.Length)
                spawnPointIndex = 0;

            Spawn(spawnPointIndex);
            PlayerPrefs.SetInt("StageReached", spawnPointIndex);
        }
    }
}
