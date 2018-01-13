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
    public int gold = 0;
    public double health = 100;
    public bool musicEnabled = true;
    public bool musicPlaying = false;

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

        activeScene = SceneManager.GetActiveScene().name;

        if (activeScene == "level1")
        {
            if (tipShown)
            {
                if (GameObject.Find("StartPanel") != null) GameObject.Find("StartPanel").SetActive(false);
            }
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

        
        //else if (SceneManager.GetActiveScene().name == "level1")
        //{

        //    loadAndRenderStats();

        //    if (Input.GetMouseButtonDown(0))
        //    {
        //        //If they click on a broken tower/turrent then load the "circuitBuilderScene"
        //        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        //        RaycastHit2D hit = Physics2D.GetRayIntersection(ray, Mathf.Infinity);
        //        if (hit.collider != null && hit.collider.name.Contains("Turret")
        //            && hit.collider.gameObject.GetComponent<Tower>().isBroken)
        //        {
        //            loadAndPrepScene("circuitBuilderScene");
        //            CircuitBuilder.instance = hit.collider.gameObject.GetComponent<Tower>();
        //            setIsActiveForEnemiesAndTowers(false);
        //        }
        //    }
        //}

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
        updateHealth();
        updateScore();
        updateGold();
    }

    public void decrementHealth()
    {
        health -= 1;
    }
    
    public void updateHealth()
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

    public void updateScore()
    {
        Text scoreText = GameObject.Find("ScorePanel").GetComponentInChildren<Text>();
        scoreText.text = "Score: " + score;
    }

    public void addToGold(int amount)
    {
        this.gold += amount;
    }

    public void updateGold()
    {
        Text goldText = GameObject.Find("GoldText").GetComponentInChildren<Text>();
        goldText.text = this.gold.ToString();
    }

    public void setIsActiveForEnemiesAndTowers(bool active)
    {

        foreach (GameObject go in Enemy.enemyGameObject)
        {
            go.SetActive(active);
        }

        foreach (TowerManager towerManager in TowerManager.activeTowers)
        {
            towerManager.getActiveTower().gameObject.SetActive(active);
        }

    }

    public void resetGame()
    {
        setIsActiveForEnemiesAndTowers(false);

        TowerManager.activeTowers.Clear();
        TowerManager.instantiatedTowers = 0;

        Enemy.instantiedEnemies.Clear();
        Enemy.enemyGameObject.Clear();

        score = 0;
        health = 100;
        tipShown = false;
        gold = 0;
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
}
