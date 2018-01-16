using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.EventSystems;

public class TurretBuilder : MonoBehaviour
{

    public GameObject purchaseTurretPanel;

    // Use this for initialization
    void Start()
    {

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
}
