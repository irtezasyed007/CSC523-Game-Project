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
        if (sceneName != "level1" || sceneName != "welcome")
        {
            GameManager.Manager.setIsActiveForEnemiesAndTowers(false);
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
