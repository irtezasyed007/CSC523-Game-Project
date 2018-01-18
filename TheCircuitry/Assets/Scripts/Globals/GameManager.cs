using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

    public static GameManager Manager;

    public bool tipShown = false;
    public int score = 0;
    public int wave = 1;
    public int gold = 150;
    public double health = 100;
    public bool musicEnabled = true;

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

        if (activeScene == "circuitBuilderScene" || activeScene == "circuitBuilderTutorial")
        {
            loadAndPrepScene(activeScene);
        }
    }
	
	// Update is called once per frame
	void Update () {
        activeScene = SceneManager.GetActiveScene().name;

        if (activeScene == "circuitBuilderScene" || activeScene == "circuitBuilderTutorial")
        {
            if(circuitBuilder == null)
            {
                loadAndPrepScene("circuitBuilderTutorial");
            }
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

    internal void loadAndPrepScene(string sceneName)
    {
        if(sceneName == "circuitBuilderScene" || sceneName == "circuitBuilderTutorial")
        {
            if(circuitBuilder == null)
            {
                circuitBuilder = gameObject.AddComponent<CircuitBuilder>();
                //circuitBuilder.enabled = true;
            }

            else
            {
                circuitBuilder.enabled = true;
            }

            SceneManager.LoadScene(sceneName);
        }
        
    }

    public void decrementHealth()
    {
        health -= 1;
    }

    public void addToScore(int amt)
    {
        score += amt;
    }

    public void addToGold(int amount)
    {
        this.gold += amount;
    }

    public void incrementWave()
    {
        wave += 1;
    }

    public void setIsActiveForLevelGameObjects(bool active)
    {

        if(Level1Scene.level1Scene != null)
        {
            foreach (GameObject go in Level1Scene.level1Scene.instantiedLevel1GameObjects)
            {
                go.SetActive(active);
            }
        }


    }

    public void resetGame()
    {
        if(Level1Scene.level1Scene != null)
        {
            Destroy(Level1Scene.level1Scene.gameObject);

            score = 0;
            wave = 1;
            health = 100;
            tipShown = false;
            gold = 150;
        }
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
