using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {
    public static GameManager Manager;

    public static bool tipShown = false;
    public static int score = 0;
    public static double health = 100;

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

        
        else if (SceneManager.GetActiveScene().name == "level1")
        {

            loadAndRenderStats();

            if (Input.GetMouseButtonDown(0))
            {
                //If they click on a broken tower/turrent then load the "circuitBuilderScene"
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit2D hit = Physics2D.GetRayIntersection(ray, Mathf.Infinity);
                if (hit.collider != null && hit.collider.name.Contains("Turret")
                    && hit.collider.gameObject.GetComponent<Tower>().isBroken)
                {
                    loadAndPrepScene("circuitBuilderScene");
                    CircuitBuilder.instance = hit.collider.gameObject.GetComponent<Tower>();
                    setIsActiveForEnemiesAndTowers(false);
                }
            }
        }

    }

    //Plays for all scenes except for "welcome"
     private void startAudioIfAllowed()
    {
        GameObject[] go = GameObject.FindGameObjectsWithTag("Music");
        AudioSource audio = null;

        foreach (GameObject g in go)
        {
            if(g.name == "Defense Line")
            {
                audio = g.GetComponent<AudioSource>();
                break;
            }
        }

        if(audio == null)
        {
            Debug.Log("[ERROR] Cannot find audio: Defense Line");
            Debug.Log("[ERROR] Caused In: GameManager startAudioIfAllowed()");
            return;
        }

        if(SceneManager.GetActiveScene().name == "welcome")
        {
            audio.Stop();
        }

        else if(!audio.isPlaying && !audio.isActiveAndEnabled)
        {
            audio.Play();
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
    public static void loadAndRenderStats()
    {
        updateHealth();
        updateScore();
    }

    public static void decrementHealth()
    {
        health -= 1;
    }
    
    public static void updateHealth()
    {
        Text healthText = GameObject.Find("HealthPanel").GetComponentInChildren<Text>();
        healthText.text = "Health: " + health;
    }

    public static void incrementScore()
    {
        score += 1;
    }

    public static void appendToScore(int val)
    {
        score += val;
    }

    public static void updateScore()
    {
        Text scoreText = GameObject.Find("ScorePanel").GetComponentInChildren<Text>();
        scoreText.text = "Score: " + score;
    }

    ///<summary>
    /// Loads or Unloads all instantiated towers/enemies
    ///</summary>
    public static void setIsActiveForEnemiesAndTowers(bool active)
    {

        foreach (GameObject go in Enemy.enemyGameObject)
        {
            go.SetActive(active);
        }

        foreach (GameObject go in Tower.towerGameObjects)
        {
            go.SetActive(active);
        }

    }
}
