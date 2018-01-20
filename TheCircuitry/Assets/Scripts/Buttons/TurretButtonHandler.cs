using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TurretButtonHandler : ButtonHandler
{
    public GameObject spawnTurretHere;
    public GameObject clickedTurret;

    // Use this for initialization
    void Start()
    {
        refreshRepairButton();
        refreshUpgradeButton();
    }

    // Update is called once per frame
    void Update()
    {
        refreshRepairButton();
        refreshUpgradeButton();
    }

    private void refreshRepairButton()
    {
        if(gameObject.name == "RepairTurretButton")
        {
            Button btn = gameObject.GetComponent<Button>();
            Tower tower = clickedTurret.GetComponent<TowerManager>().getActiveTower();

            foreach (Text text in btn.gameObject.GetComponentsInChildren<Text>(true))
            {
                if (text.gameObject.name == "OperationalText")
                {
                    //They are currently still fighting enemies
                    if (!Wave.wave.isWaveFinished())
                    {
                        if (tower.isBroken)
                        {
                            btn.interactable = true;
                            text.text = "Nonoperational";
                            text.color = new Color(255.0f / 255.0f, 0.0f / 255.0f, 0.0f / 255.0f, 255.0f / 255.0f);
                        }

                        else
                        {
                            btn.interactable = false;
                            text.text = "Operational";
                            text.color = new Color(32.0f / 255.0f, 150.0f / 255.0f, 52.0f / 255.0f, 255.0f / 255.0f);
                        }
                    }

                    //The wave of enemies is over
                    else
                    {
                        btn.interactable = false;
                        text.text = "Start Next Round!";
                        text.color = new Color(255.0f / 255.0f, 0.0f / 255.0f, 0.0f / 255.0f, 255.0f / 255.0f);
                    }

                }
            }
        }
    }

    private void refreshUpgradeButton()
    {
        if(gameObject.name == "UpgradeTurretButton")
        {
            Button btn = gameObject.GetComponent<Button>();
            Tower tower = clickedTurret.GetComponent<TowerManager>().getActiveTower();
            int price = tower.getTier() * 1000;

            if (tower.isBroken) price += 300;

            foreach (RectTransform rect in btn.GetComponentsInChildren<RectTransform>(true))
            {
                if (rect.gameObject.name == "TurretPrice")
                {
                    Text[] text = rect.gameObject.GetComponentsInChildren<Text>(true);

                    //Max tier reached
                    if(tower.towerTier == 3)
                    {
                        btn.interactable = false;
                        text[0].gameObject.transform.parent.gameObject.SetActive(false);
                        text[1].gameObject.SetActive(true);
                    }

                    //Elgible to upgrade
                    else
                    {
                        btn.interactable = true;
                        text[0].text = price.ToString();
                        text[0].gameObject.transform.parent.gameObject.SetActive(true);
                        text[1].gameObject.SetActive(false);
                    }
                    
                }
            }
        }
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
                base.playRandomCoinSound();
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
                base.playRandomCoinSound();
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

    public void upgradeClickedTurret()
    {
        Tower towerToUpgrade = clickedTurret.GetComponent<TowerManager>().getActiveTower();
        int amt = towerToUpgrade.getTier() * 1000;

        if (!GameManager.Manager.hasEnoughGold(amt))
        {
            doNotEnoughGoldText(transform.position);
        }

        else if (towerToUpgrade.canUpgrade())
        {
            doUpgradeSuccessfulText(transform.position);
            towerToUpgrade.upgradeTower();
            GameManager.Manager.addToGold((int)amt * -1);
            transform.parent.gameObject.SetActive(false);
        }
    }

    private void doNotEnoughGoldText(Vector2 position)
    {
        GameObject go = Resources.Load<GameObject>("Prefabs/Text GameObjects/NotEnoughGoldText");
        go = Instantiate(go, GameObject.Find("Level1Canvas").transform);
        go.transform.position = new Vector2(position.x, position.y);
        go.GetComponent<TextFadeOut>().FadeOut();
    }

    private void doUpgradeSuccessfulText(Vector2 position)
    {
        GameObject go = Resources.Load<GameObject>("Prefabs/Text GameObjects/UpgradeSuccessfulText");
        go = Instantiate(go, GameObject.Find("Level1Canvas").transform);
        go.transform.position = new Vector2(position.x, position.y);
        go.GetComponent<TextFadeOut>().FadeOut();
    }

    public void repairClickedTurret()
    {
        GameManager.Manager.loadAndPrepScene("circuitBuilderScene");
        CircuitBuilder.instance = clickedTurret.GetComponent<TowerManager>().getActiveTower();
    }
}
