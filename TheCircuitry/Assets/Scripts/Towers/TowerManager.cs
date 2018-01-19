using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class TowerManager : MonoBehaviour
{
    public static List<GameObject> activeTowers = new List<GameObject>();
    public const int ALLOWED_TOWERS = 12;

    private GameObject activeTower;
    private GameObject[] allTowers;

    // Use this for initialization
    void Start()
    {
        if(ALLOWED_TOWERS <= activeTowers.Count)
        {
            Destroy(this.gameObject);
        }

        else
        {
            initTowers();
            DontDestroyOnLoad(gameObject);
            activeTowers.Add(gameObject);
            Level1Scene.level1Scene.instantiedLevel1GameObjects.Add(gameObject);
        }
    }

    private void initTowers()
    {
        int index = 0;
        allTowers = new GameObject[4];
        foreach (Tower tower in GetComponentsInChildren<Tower>(true))
        {
            if (tower.gameObject.name.Contains("tier"))
            {
                allTowers[index] = tower.gameObject;
                index++;
            }
        }

        activeTower = allTowers[0]; //The First Tower (tier1)
    }

    public void openTurretOptionsPanel()
    {
        GameObject canvas = GameObject.Find("Canvas");

        foreach(RectTransform go in canvas.GetComponentsInChildren<RectTransform>(true))
        {
            //Activating the "TowerOptionsPanel" so the player can upgrade/repair their turrets
            if(go.gameObject.name == "TowerOptionsPanel")
            {
                initButtons(go.gameObject);
                go.gameObject.SetActive(true);
                break;
            }
        }
    }

    private void initButtons(GameObject panelGameObject)
    {
        //Assigning the instance of the tower-to-upgrade to the buttons
        foreach (Button btn in panelGameObject.GetComponentsInChildren<Button>(true))
        {
            TurretButtonHandler btnHandler = btn.GetComponent<TurretButtonHandler>();
            if (btnHandler != null) btnHandler.clickedTurret = gameObject;
        }
    }

    public void activateNextTower(int index)
    {
        activeTower.SetActive(false);
        activeTower = allTowers[index];
        activeTower.SetActive(true);
    }

    public Tower getActiveTower()
    {
        return this.activeTower.GetComponent<Tower>();
    }

    private void OnDestroy()
    {
        activeTowers.Remove(gameObject);
    }
}
