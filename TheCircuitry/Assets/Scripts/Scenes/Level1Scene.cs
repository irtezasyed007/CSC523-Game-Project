using System;
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
            ButtonHandler.GetAudioToggle += new EventHandler(OnAudioToggle);
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
            AudioManager.audioManager.setMusic("Sounds/Music/Polka Train");
            GameManager.Manager.setIsActiveForLevelGameObjects(true);

            if (!Wave.wave.isStarted())
            {
                GameObject.Find("BeginNextWave").GetComponentsInChildren<Button>(true)[1].gameObject.SetActive(true);
            }
            if (GameManager.Manager.tipShown) GameObject.Find("StartPanel").SetActive(false);
        }

        else
        {
            GameManager.Manager.setIsActiveForLevelGameObjects(false);
        }
    }

    private void Update()
    {
        if(SceneManager.GetActiveScene().name == "level1")
        {
            loadAndRenderStats();
            refreshMusicButtonText();
        }
    }

    private void OnAudioToggle(object sender, EventArgs e)
    {
        refreshMusicButtonText();
    }

    private void refreshMusicButtonText()
    {
        if (SceneManager.GetActiveScene().name == "level1")
        {
            if (GameManager.Manager.musicEnabled)
            {
                Text text = GameObject.Find("MusicToggleButton").GetComponentsInChildren<Text>()[1];
                text.text = "On";
            }

            else
            {
                Text text = GameObject.Find("MusicToggleButton").GetComponentsInChildren<Text>()[1];
                text.text = "Off";
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

    public int getRandomValue(int value)
    {
        return UnityEngine.Random.Range(value / 2, value);
    }

    private void OnDestroy()
    {
        if(this == level1Scene)
        {
            TurretBuilder.totalTiles = 0;
            TurretBuilder.instantiatedTiles.Clear();
            foreach (GameObject go in instantiedLevel1GameObjects) Destroy(go);
        }
    }

}
