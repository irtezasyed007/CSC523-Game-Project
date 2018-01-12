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
            Tower.towerID = 1;
            GameManager.Manager.setIsActiveForEnemiesAndTowers(false);

            Tower.towerGameObjects.Clear();
            Tower.towerObjects.Clear();
            Enemy.instantiedEnemies.Clear();
            Enemy.enemyGameObject.Clear();
            GameManager.Manager.score = 0;
            GameManager.Manager.health = 100;
            GameManager.Manager.tipShown = false;
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
