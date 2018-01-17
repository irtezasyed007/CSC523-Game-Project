using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.EventSystems;

public class TurretBuilder : MonoBehaviour
{
    public const int MAX_INSTANCES = 15;
    public static List<GameObject> instantiatedTiles = new List<GameObject>();
    public static int totalTiles = 0;
    public GameObject purchaseTurretPanel;

    // Use this for initialization
    void Start()
    {
        if(totalTiles < MAX_INSTANCES)
        {
            instantiatedTiles.Add(gameObject);
            totalTiles++;
            Level1Scene.level1Scene.instantiedLevel1GameObjects.Add(gameObject);
            DontDestroyOnLoad(gameObject);
        }

        else
        {
            Destroy(gameObject);
        }
    }

    void Update()
    {

    }

    public void loadPurchaseTurretPanel()
    {
        Text[] textArray = purchaseTurretPanel.GetComponentsInChildren<Text>(true);

        foreach(Text text in textArray)
        {
            if(text.name == "RocketPriceText")
            {
                text.text = RocketTower.price.ToString();
            }

            else if(text.name == "BulletPriceText")
            {
                text.text = BulletTower.price.ToString();
            }
        }

        Button[] buttonArray = purchaseTurretPanel.GetComponentsInChildren<Button>(true);

        foreach(Button btn in buttonArray)
        {
            if(btn.gameObject.name == "RocketPurchaseButton")
            {
                btn.GetComponent<TurretButtonHandler>().spawnTurretHere = gameObject;
            }

            else if(btn.gameObject.name == "BulletPurchaseButton")
            {
                btn.GetComponent<TurretButtonHandler>().spawnTurretHere = gameObject;
            }
        }

        purchaseTurretPanel.SetActive(true);
    }

    private void OnDestroy()
    {
        instantiatedTiles.Remove(gameObject);
        Level1Scene.level1Scene.instantiedLevel1GameObjects.Remove(gameObject);
    }
}
