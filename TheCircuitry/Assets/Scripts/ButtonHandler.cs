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

        if(sceneName == "level1")
        {
            foreach(GameObject go in Enemy.enemyGameObject)
            {
                if (go == null) Enemy.enemyGameObject.Remove(go);
                else go.SetActive(true);
            }

            foreach (GameObject go in Tower.towerGameObjects)
            {
                if (go == null) Tower.towerGameObjects.Remove(go);
                else go.SetActive(true);
            }
        }

        else if(sceneName == "welcome")
        {
            Tower.towerID = 1;
            GameObject[] objects = GameManager.FindObjectsOfType<GameObject>();
            foreach(GameObject o in objects)
            {
                if(o.name != "Main Camera" || o.name != "Defense Line")
                    Destroy(o.gameObject);
            }

            Tower.towerGameObjects.Clear();
            Tower.towerObjects.Clear();
            Enemy.instantiedEnemies.Clear();
            Enemy.enemyGameObject.Clear();
            GameManager.score = 0;
            GameManager.health = 100;
            GameManager.tipShown = false;
        }

        //they wish to load a scene other than level1
        else if (sceneName != "level1")
        {
            foreach (GameObject go in Enemy.enemyGameObject)
            {
                go.SetActive(false);
            }

            foreach(GameObject go in Tower.towerGameObjects)
            {
                go.SetActive(false);
            }
        }
    }

    private IEnumerator wait()
    {
        yield return new WaitForSeconds(1);
    }
}
