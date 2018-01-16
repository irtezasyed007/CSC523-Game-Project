using UnityEngine;
using System.Collections;

public class PurchaseTurretButton : MonoBehaviour
{
    public GameObject spawnTurretHere;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void purchaseAndBuildTurret(string type)
    {
        type = type.ToLower();

        if (type == "rocket")
        {
            GameObject rocketTurret = Resources.Load<GameObject>("Prefabs/Turrets/rocketTurrets");
            int price = RocketTower.price;

            if (GameManager.Manager.hasEnoughGold(price))
            {
                GameManager.Manager.doGoldTransaction(price);
                Instantiate(rocketTurret, spawnTurretHere.transform.position, Quaternion.identity);
                Destroy(spawnTurretHere);
                transform.parent.parent.gameObject.SetActive(false);
            }

            //Not enough gold
            else
            {
                GameObject notEnoughGold = Resources.Load<GameObject>("Prefabs/Text GameObjects/NotEnoughGoldText");
                notEnoughGold = Instantiate(notEnoughGold, transform.parent);
                notEnoughGold.transform.position = new Vector2(transform.position.x, transform.position.y);
                notEnoughGold.GetComponent<TextFadeOut>().FadeOut();
            }
        }

        else if (type == "bullet")
        {
            GameObject bulletTurret = Resources.Load<GameObject>("Prefabs/Turrets/bulletTurrets");
            int price = BulletTower.price;

            if (GameManager.Manager.hasEnoughGold(price))
            {
                GameManager.Manager.doGoldTransaction(price);
                Instantiate(bulletTurret, spawnTurretHere.transform.position, Quaternion.identity);
                Destroy(spawnTurretHere);
                transform.parent.parent.gameObject.SetActive(false);
            }

            //Not enough gold
            else
            {
                GameObject notEnoughGold = Resources.Load<GameObject>("Prefabs/Text GameObjects/NotEnoughGoldText");
                notEnoughGold = Instantiate(notEnoughGold, transform.parent);
                notEnoughGold.transform.position = new Vector2(transform.position.x, transform.position.y);
                notEnoughGold.GetComponent<TextFadeOut>().FadeOut();
            }
        }
    }
}
