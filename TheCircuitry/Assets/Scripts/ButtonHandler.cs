using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ButtonHandler : MonoBehaviour {

    public void SetActive(GameObject go, bool isActive)
    {
        go.SetActive(isActive);
    }

	public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);

        //they wish to load a scene other than level1
        if (sceneName != "level1")
        {
            GameManager.Manager.setIsActiveForEnemiesAndTowers(false);
        }
    }

    public void upgradeTower()
    {
        Tower towerToUpgrade = GameManager.level1Scene.getTowerToUpgrade();
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
            GameManager.level1Scene.upgradeButton.gameObject.SetActive(false);
        }
    }

    public void setGameManagerTipShown(bool val)
    {
        GameManager.Manager.tipShown = val;
    }

    public void stopMusic()
    {
        GameManager.Manager.musicEnabled = !GameManager.Manager.musicEnabled;
    }

    private IEnumerator wait()
    {
        yield return new WaitForSeconds(1);
    }


    private void doNotEnoughGoldText(Vector2 position)
    {
        GameObject go = Resources.Load<GameObject>("Prefabs/Text GameObjects/NotEnoughGoldText");
        go = Instantiate(go, GameObject.Find("Canvas").transform);
        go.transform.position = new Vector2(position.x, position.y);
        go.GetComponent<TextFadeOut>().FadeOut();
    }

    private void doUpgradeSuccessfulText(Vector2 position)
    {
        GameObject go = Resources.Load<GameObject>("Prefabs/Text GameObjects/UpgradeSuccessfulText");
        go = Instantiate(go, GameObject.Find("Canvas").transform);
        go.transform.position = new Vector2(position.x, position.y);
        go.GetComponent<TextFadeOut>().FadeOut();
    }
}
