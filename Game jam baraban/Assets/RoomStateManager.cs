using UnityEngine;

public class RoomStateManager : MonoBehaviour
{
    public Transform[] spawnPositions;
    public float[] environmentStrength;
    public float[] sunStrength;

    public Transform player;
    public Light sun;

    public int spawnpointUsed;

    void Spawn(int spawnpointIndex)
    {
        player.transform.position = spawnPositions[spawnpointIndex].position;
        RenderSettings.ambientIntensity = environmentStrength[spawnpointIndex];
        sun.intensity = sunStrength[spawnpointIndex];
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Spawn(PlayerPrefs.GetInt("StageReached", 0));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
