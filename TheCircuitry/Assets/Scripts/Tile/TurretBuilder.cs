using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.EventSystems;

public class TurretBuilder : MonoBehaviour
{
    public const int MAX_INSTANCES = 15;
    public static List<TurretBuilder> instantiatedTiles = new List<TurretBuilder>();
    private static int currentNumberofInstances = 0;

    public GameObject purchaseTurretPanel;

    // Use this for initialization
    void Start()
    {
        if(currentNumberofInstances <= MAX_INSTANCES)
        {
            currentNumberofInstances++;
            instantiatedTiles.Add(this);
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
                btn.GetComponent<PurchaseTurretButton>().spawnTurretHere = gameObject;
            }

            else if(btn.gameObject.name == "BulletPurchaseButton")
            {
                btn.GetComponent<PurchaseTurretButton>().spawnTurretHere = gameObject;
            }
        }

        purchaseTurretPanel.SetActive(true);
    }

    private void OnDestroy()
    {
        instantiatedTiles.Remove(this);
    }
}
