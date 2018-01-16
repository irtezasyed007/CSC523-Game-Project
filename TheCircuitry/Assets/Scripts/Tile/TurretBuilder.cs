using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TurretBuilder : MonoBehaviour
{

    public GameObject purchaseTurretPanel;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D hit = Physics2D.GetRayIntersection(ray, Mathf.Infinity);
            if(hit.collider != null && hit.collider.gameObject != null)
            {
                string colliderName = hit.collider.name;

                if (colliderName.Contains("turretConstructor"))
                {
                    loadPurchaseTurretPanel();
                }
            }
        }
    }

    private void loadPurchaseTurretPanel()
    {
        Text[] textArray = purchaseTurretPanel.GetComponentsInChildren<Text>(true);

        foreach(Text text in textArray)
        {
            if(text.gameObject.name == "RocketTurretPrice")
            {
                text.text = RocketTower.price.ToString();
            }

            else if(text.gameObject.name == "BulletTurretPrice")
            {
                text.text = BulletTower.price.ToString();
            }
        }

        Button[] buttonArray = purchaseTurretPanel.GetComponentsInChildren<Button>(true);

        foreach(Button btn in buttonArray)
        {
            Debug.Log("Name: " + btn.gameObject.name);
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
