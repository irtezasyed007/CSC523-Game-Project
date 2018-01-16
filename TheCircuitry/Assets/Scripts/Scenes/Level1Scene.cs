using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Level1Scene : MonoBehaviour
{

    public Button upgradeButton;
    private GameObject previousClickedTower; //The previous clicked tower
    private Tower towerToUpgrade; //The tower the player clicked on to upgrade

    void Start()
    {
        if (GameManager.level1Scene == null)
        {
            GameManager.level1Scene = this;
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
            GameManager.Manager.setIsActiveForEnemiesAndTowers(true);

            if (GameManager.Manager.tipShown) GameObject.Find("StartPanel").SetActive(false);

            GameObject waveManager = GameObject.Find("waveManager");
            if (GameManager.Manager.waveManager == null) GameManager.Manager.waveManager = waveManager.GetComponent<Wave>();
            else waveManager = GameManager.Manager.waveManager.gameObject;
            

            this.upgradeButton = GameObject.Find("UpgradeTurretPanel").GetComponentInChildren<Button>();
            if(upgradeButton != null)
            {
                this.upgradeButton.gameObject.SetActive(false);
            }
        }
    }

    private void Update()
    {
        updateMusicButton();

        GameManager.Manager.loadAndRenderStats();

        //Player clicks on broken circuit
        if (Input.GetMouseButtonDown(0))
        {
            //If they click on a broken tower/turrent then load the "circuitBuilderScene"
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D hit = Physics2D.GetRayIntersection(ray, Mathf.Infinity);
            if (hit.collider != null && hit.collider.gameObject != null && 
                hit.collider.gameObject.name.Contains("Turret"))
            {
                if (hit.collider.gameObject.GetComponent<TowerManager>().getActiveTower().isBroken)
                {
                    GameManager.Manager.loadAndPrepScene("circuitBuilderScene");
                    CircuitBuilder.instance = hit.collider.gameObject.GetComponent<TowerManager>().getActiveTower();
                    GameManager.Manager.setIsActiveForEnemiesAndTowers(false);
                }
                
            }
        }

        //Player right-clicks on a tower they wish to upgrade
        else if (Input.GetMouseButtonDown(1))
        {
            //If they click on a broken tower/turrent then load the "circuitBuilderScene"
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D hit = Physics2D.GetRayIntersection(ray, Mathf.Infinity);
            if (hit.collider != null && hit.collider.gameObject != null && 
                hit.collider.gameObject.name.Contains("Turrets"))
            {
                GameObject clickedTower = hit.collider.gameObject.GetComponent<TowerManager>().getActiveTower().gameObject;

                //If they right-click on the same turret, clear the upgrade prompt
                if (upgradeButton.IsActive() && previousClickedTower == clickedTower)
                {
                    upgradeButton.gameObject.SetActive(false);
                }

                //If they right-click on a different turret, update the upgrade prompt
                else
                {
                    previousClickedTower = clickedTower;

                    Tower tower = clickedTower.GetComponent<Tower>();
                    this.towerToUpgrade = tower;
                    string towerType = tower.getTowerType();
                    
                    if(tower.getTier() < 3)
                    {
                        int cost = tower.getTier() * 1000;
                        Text[] upgradeTexts = upgradeButton.GetComponentsInChildren<Text>();
                        upgradeTexts[0].text = "Upgrade " + towerType + " Tower";
                        upgradeTexts[1].text = cost.ToString();
                        upgradeButton.gameObject.SetActive(true);
                    }

                    //Max tier has been reached
                    else
                    {
                        doMaxTierReachedText((Vector2) Input.mousePosition);
                    }
                }
            }
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

    private void doMaxTierReachedText(Vector2 position)
    {
        GameObject go = Resources.Load<GameObject>("Prefabs/Text GameObjects/MaxTierReachedText");
        go = Instantiate(go, GameObject.Find("Canvas").transform);
        go.transform.position = new Vector2(position.x, position.y);
    }

    public Tower getTowerToUpgrade()
    {
        return this.towerToUpgrade;
    }
}
