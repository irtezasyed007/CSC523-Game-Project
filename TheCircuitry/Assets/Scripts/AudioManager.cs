using UnityEngine;
using System.Collections;

public class AudioManager : MonoBehaviour
{

    public static AudioManager audioManager;

    private GameObject audioSource;
    private AudioSource music;

    // Use this for initialization
    void Awake()
    {
        if(audioManager == null)
        {
            audioManager = this;
            DontDestroyOnLoad(gameObject);
        }

        else
        {
            Destroy(gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(music != null) checkIsMusicEnabled();
    }

    private void checkIsMusicEnabled()
    {
        if (GameManager.Manager.musicEnabled)
        {
            if (!music.isPlaying) music.Play();
        }

        else if (!GameManager.Manager.musicEnabled)
        {
            if (music.isPlaying) music.Stop();
        }
    }

    public void setMusic(string path)
    {
        if (audioSource != null)
        {
            Destroy(audioSource);
        }

        this.audioSource = Instantiate(Resources.Load<GameObject>(path), transform);
        this.music = this.audioSource.GetComponent<AudioSource>();
    }

    public GameObject getMusic()
    {
        return this.audioSource;
    }
}
