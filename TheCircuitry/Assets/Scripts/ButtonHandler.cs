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
        double amt = towerToUpgrade.getTier() * 1000;

        if (!GameManager.Manager.hasEnoughToUpgrade(amt))
        {
            GameManager.level1Scene.doTextFadeOut(1);
        }

        else if (towerToUpgrade.canUpgrade())
        {
            GameManager.level1Scene.doTextFadeOut(2);

            towerToUpgrade.upgradeTower();
            GameManager.Manager.addToGold((int)amt * -1);
            GameManager.level1Scene.upgradeButton.gameObject.SetActive(false);
        }
    }

    public void stopMusic()
    {
        GameManager.Manager.musicEnabled = !GameManager.Manager.musicEnabled;
    }

    private IEnumerator wait()
    {
        yield return new WaitForSeconds(1);
    }
}
