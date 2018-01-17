using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Level1Scene : MonoBehaviour
{
    public static Level1Scene level1Scene;

    public List<GameObject> instantiedLevel1GameObjects = new List<GameObject>();

    private GameObject previousClickedTower; //The previous clicked tower
    private Tower towerToUpgrade; //The tower the player clicked on to upgrade

    void Awake()
    {
        if (level1Scene == null)
        {
            level1Scene = this;
            DontDestroyOnLoad(gameObject);
        }

        else Destroy(gameObject);
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
            GameManager.Manager.setIsActiveForLevelGameObjects(true);

            if (GameManager.Manager.tipShown) GameObject.Find("StartPanel").SetActive(false);
        }
    }

    private void Update()
    {
        if(SceneManager.GetActiveScene().name == "level1")
        {
            updateMusicButton();
            loadAndRenderStats();
        }
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

    //Updates the health and score Text to the Game's recorded values
    private void loadAndRenderStats()
    {
        refreshHealthText();
        refreshScoreText();
        refreshGoldText();
        refreshWaveText();
    }

    private void refreshHealthText()
    {
        Text healthText = GameObject.Find("HealthPanel").GetComponentInChildren<Text>();
        healthText.text = "Health: " + GameManager.Manager.health;
    }

    private void refreshScoreText()
    {
        Text scoreText = GameObject.Find("ScorePanel").GetComponentInChildren<Text>();
        scoreText.text = "Score: " + GameManager.Manager.score;
    }

    private void refreshGoldText()
    {
        Text goldText = GameObject.Find("GoldText").GetComponentInChildren<Text>();
        goldText.text = GameManager.Manager.gold.ToString();
    }

    private void refreshWaveText()
    {
        Text text = GameObject.Find("WaveText").GetComponentInChildren<Text>();
        text.text = "Wave: " + GameManager.Manager.wave;
    }

    private void OnDestroy()
    {
        foreach (GameObject go in instantiedLevel1GameObjects) Destroy(go);
    }
}
