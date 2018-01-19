using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.EventSystems;

public class TurretBuilder : MonoBehaviour
{
    public const int MAX_INSTANCES = 12;
    public static List<GameObject> instantiatedTiles = new List<GameObject>();
    public static int totalTiles = 0;

    private void Start()
    {
        if (totalTiles != MAX_INSTANCES)
        {
            instantiatedTiles.Add(gameObject);
            Level1Scene.level1Scene.instantiedLevel1GameObjects.Add(gameObject);
            DontDestroyOnLoad(gameObject);

            totalTiles++;
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
        GameObject canvas = GameObject.Find("Canvas");
        GameObject purchaseTurretPanel = null;

        foreach(RectTransform rt in canvas.GetComponentsInChildren<RectTransform>(true))
        {
            if(rt.gameObject.name == "PurchaseTurretPanel")
            {
                purchaseTurretPanel = rt.gameObject;
                break;
            }
        }

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
