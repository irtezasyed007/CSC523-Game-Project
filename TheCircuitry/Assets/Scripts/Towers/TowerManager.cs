using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TowerManager : MonoBehaviour
{
    public static List<TowerManager> activeTowers = new List<TowerManager>();
    public const int ALLOWED_TOWERS = 2;

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
            DontDestroyOnLoad(this.gameObject);
            initTowers();

            activeTowers.Add(this);
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
}
