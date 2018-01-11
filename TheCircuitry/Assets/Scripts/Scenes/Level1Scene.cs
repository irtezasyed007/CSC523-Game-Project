using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Level1Scene : MonoBehaviour
{

    private void Start()
    {
        
    }

    void OnEnable()
        {
        //Tell our 'OnLevelFinishedLoading' function to start listening for a scene change as soon as this script is enabled.
        SceneManager.sceneLoaded += OnLevelFinishedLoading;
    }

    void OnDisable()
    {
        //Tell our 'OnLevelFinishedLoading' function to stop listening for a scene change as soon as this script is disabled. Remember to always have an unsubscription for every delegate you subscribe to!
        SceneManager.sceneLoaded -= OnLevelFinishedLoading;
    }

    void OnLevelFinishedLoading(Scene scene, LoadSceneMode mode)
    {
        if(scene.name == "level1")
        {
            GameManager.Manager.setIsActiveForEnemiesAndTowers(true);
        }
    }

    private void Update()
    {
        updateMusicButton();
    }

    private void updateMusicButton()
    {
        GameObject musicButton = GameObject.Find("MusicToggleButton");

        if (musicButton != null)
        {
            Text[] musicState = GameObject.Find("MusicToggleButton").GetComponentsInChildren<Text>();

            if (GameManager.Manager.musicEnabled)
            {
                musicState[0].text = "Music";
                musicState[1].text = "On";
            }
            else
            {
                musicState[0].text = "Music";
                musicState[1].text = "Off";
            }

        }
    }
}
