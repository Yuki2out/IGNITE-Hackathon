using UnityEngine;

public class RoomStateManager : MonoBehaviour
{
    public Transform[] spawnPositions;
    public float[] environmentStrength;
    public float[] sunStrength;
    public float[] audioVolumes;
    public float[] audioPitches;

    public Transform player;
    public Light sun;

    public int spawnPointIndex;
    public AudioSource backgroundMusic;

    private T TryReadArr<T>(T[] arr, int index, T defaultValue)
    {
        if (index < 0 || index >= arr.Length) return defaultValue;
        return arr[index];
    }

    void Spawn(int spawnPointIndex)
    {
        spawnPointIndex = Mathf.Min(spawnPositions.Length - 1, spawnPointIndex);

        player.transform.position = spawnPositions[spawnPointIndex].position;

        RenderSettings.ambientIntensity = TryReadArr(environmentStrength, spawnPointIndex, 0.85f);
        sun.intensity = TryReadArr(sunStrength, spawnPointIndex, 0.85f);

        backgroundMusic.pitch = TryReadArr(audioPitches, spawnPointIndex, 1);
        backgroundMusic.volume = TryReadArr(audioVolumes, spawnPointIndex, 0.5f);
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
