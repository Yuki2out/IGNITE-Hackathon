using System;
using UnityEngine;

public class FootstepManager : MonoBehaviour
{
    public AudioSource[] footstepSounds;

    public bool playing;

    private float counter = 0f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    public void Play()
    {
        playing = true;
    }

    public void Stop()
    {
        foreach (AudioSource sound in footstepSounds)
        {
            sound.Stop();
        }

        playing = false;
        counter = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (!playing) return;

        counter += Time.deltaTime;
        if (counter < 0.5f) return;

        int index = UnityEngine.Random.Range(0, footstepSounds.Length);

        footstepSounds[index].pitch = UnityEngine.Random.Range(0.7f, 1.3f);
        footstepSounds[index].Play();

        counter = 0f;
    }
}
