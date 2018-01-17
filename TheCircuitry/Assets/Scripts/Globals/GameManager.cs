using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

    public static GameManager Manager;
    public static Level1Scene level1Scene;

    public bool tipShown = false;
    public int score = 0;
    public int wave = 1;
    public int gold = 150;
    public double health = 100;
    public bool musicEnabled = true;
    public bool musicPlaying = false;
    public Wave waveManager;

    internal CircuitBuilder circuitBuilder;
    internal string activeScene;
    internal struct TowerScene
    {

    }
    
    // Use this for initialization
    private void Awake()
    {
        if (Manager == null)
        {
            DontDestroyOnLoad(gameObject);
            Manager = this;
        }
        else if (Manager != null)
        {
            Destroy(gameObject);
        }
    }

    void Start () {
        activeScene = SceneManager.GetActiveScene().name;

        if (activeScene == "circuitBuilderScene")
        {
            loadAndPrepScene(activeScene);
        }
    }
	
	// Update is called once per frame
	void Update () {
        startAudioIfAllowed();
        activeScene = SceneManager.GetActiveScene().name;

        if (activeScene == "circuitBuilderScene")
        {

            if (Input.GetMouseButtonDown(1))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit2D hit = Physics2D.GetRayIntersection(ray, Mathf.Infinity, circuitBuilder.Mask);
                if (hit.collider != null && hit.transform.name.EndsWith("(Clone)"))
                {
                    if(circuitBuilder.SelectedGateCollider == hit.collider) // Right click on a Red Circle to delete a line
                    {
                        Destroy(circuitBuilder.RedCircle);
                        circuitBuilder.RedCircle = null;
                        circuitBuilder.RedCircleHolder = null;
                        circuitBuilder.SelectedGateCollider = null;
                        circuitBuilder.RemoveColliderAndItsPairsFromList(hit.collider);
                    }

                    else if(hit.collider.offset.x == 0)  // Right click on a gate's main body to delete it and any connected lines
                    {
                        circuitBuilder.RemoveGateFromList(hit.collider.gameObject);
                        if (circuitBuilder.RedCircleHolder == hit.collider.gameObject)
                        {
                            Destroy(circuitBuilder.RedCircle);
                            circuitBuilder.RedCircle = null;
                            circuitBuilder.RedCircleHolder = null;
                            circuitBuilder.SelectedGateCollider = null;
                        }

                        Destroy(hit.collider.gameObject);
                    }
                    
                }
            }

        }

    }

     private void startAudioIfAllowed()
    {
        string activeScene = SceneManager.GetActiveScene().name;
        GameObject[] go = GameObject.FindGameObjectsWithTag("Music");
        AudioSource audio = null;

        if (!musicEnabled)
        {
            if (musicPlaying)
            {
                foreach (GameObject g in go)
                {
                    g.GetComponent<AudioSource>().Stop();
                }

                musicPlaying = false;
            }         

            return;
        }

        if (activeScene == "welcome")
        {
            audio = searchAudioSourceByName(go, "8-bit-Arcade4");
            if (!audio.isPlaying)
            {
                audio.Play();
                musicPlaying = true;
            }
        }

        else
        {
            audio = searchAudioSourceByName(go, "Defense Line");
            if (!audio.isPlaying)
            {
                audio.Play();
                musicPlaying = true;
            }
            
        }
    }

    internal void loadAndPrepScene(string sceneName)
    {
        if(sceneName == "circuitBuilderScene")
        {
            if(circuitBuilder == null)
            {
                circuitBuilder = gameObject.AddComponent<CircuitBuilder>();
            }

            else
            {
                circuitBuilder.enabled = true;
            }

            SceneManager.LoadScene(sceneName);
        }
        
    }

    //Updates the health and score Text to the Game's recorded values
    public void loadAndRenderStats()
    {
        refreshHealthText();
        refreshScoreText();
        refreshGoldText();
        refreshWaveText();
    }

    public void decrementHealth()
    {
        health -= 1;
    }
    
    public void refreshHealthText()
    {
        Text healthText = GameObject.Find("HealthPanel").GetComponentInChildren<Text>();
        healthText.text = "Health: " + health;
    }

    public void incrementScore()
    {
        score += 1;
    }

    public void appendToScore(int val)
    {
        score += val;
    }

    public void refreshScoreText()
    {
        Text scoreText = GameObject.Find("ScorePanel").GetComponentInChildren<Text>();
        scoreText.text = "Score: " + score;
    }

    public void addToGold(int amount)
    {
        this.gold += amount;
    }

    public void refreshGoldText()
    {
        Text goldText = GameObject.Find("GoldText").GetComponentInChildren<Text>();
        goldText.text = this.gold.ToString();
    }

    public void incrementWave()
    {
        wave += 1;
    }

    public void refreshWaveText()
    {
        Text text = GameObject.Find("WaveText").GetComponentInChildren<Text>();
        text.text = "Wave: " + wave.ToString();
    }

    public void setIsActiveForLevelGameObjects(bool active)
    {

        foreach (GameObject go in Enemy.enemyGameObject)
        {
            go.SetActive(active);
        }

        foreach (TowerManager towerManager in TowerManager.activeTowers)
        {
            foreach(Tower tower in towerManager.gameObject.GetComponentsInChildren<Tower>(true))
            {
                tower.gameObject.SetActive(false);
            }

            towerManager.getActiveTower().gameObject.SetActive(active);
        }

        foreach (TurretBuilder turretTile in TurretBuilder.instantiatedTiles)
        {
            turretTile.gameObject.SetActive(active);
        }

    }

    public void resetGame()
    {
        foreach (Enemy enemy in Enemy.instantiedEnemies) Destroy(enemy.gameObject);
        foreach (TowerManager tower in TowerManager.activeTowers) Destroy(tower.gameObject);
        foreach (TurretBuilder turretTile in TurretBuilder.instantiatedTiles) Destroy(turretTile.gameObject);

        Enemy.instantiedEnemies.Clear();
        Enemy.enemyGameObject.Clear();

        TowerManager.activeTowers.Clear();

        TurretBuilder.instantiatedTiles.Clear();

        Destroy(waveManager.gameObject);

        score = 0;
        wave = 0;
        health = 100;
        tipShown = false;
        gold = 150;
    }

    public static AudioSource searchAudioSourceByName(GameObject[] audioSources, string audioToFind)
    {
        foreach(GameObject go in audioSources)
        {
            if (string.Equals(go.name, audioToFind, System.StringComparison.OrdinalIgnoreCase))
            {
                return go.GetComponent<AudioSource>();
            }
        }

        return null;
    }

    public bool hasEnoughGold(int amt)
    {
        if (amt > gold) return false;
        else return true;
    }

    public void doGoldTransaction(int amount)
    {
        gold -= amount;
    }
}
