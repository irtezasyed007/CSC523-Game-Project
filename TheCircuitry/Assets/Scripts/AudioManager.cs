using UnityEngine;
using System.Collections;

public class AudioManager : MonoBehaviour
{

    private AudioSource audioSource;

    // Use this for initialization
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        checkIsMusicEnabled();
    }

    // Update is called once per frame
    void Update()
    {
        checkIsMusicEnabled();
    }

    private void checkIsMusicEnabled()
    {
        if (GameManager.Manager.musicEnabled)
        {
            if (!audioSource.isPlaying) audioSource.Play();
        }

        else if (!GameManager.Manager.musicEnabled)
        {
            if (audioSource.isPlaying) audioSource.Stop();
        }
    }
}
