using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class WelcomeScene : MonoBehaviour
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
        if (scene.name == "welcome")
        {
            GameManager.Manager.resetGame();
            AudioManager.audioManager.setMusic("Sounds/Music/Mishief Stroll");
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
