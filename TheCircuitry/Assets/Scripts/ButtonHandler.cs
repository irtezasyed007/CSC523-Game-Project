using Assets.Scripts;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ButtonHandler : MonoBehaviour {

    public static event EventHandler GetAudioToggle = delegate { };

    public void SetActive(GameObject go, bool isActive)
    {
        go.SetActive(isActive);
    }

	public void LoadScene(string sceneName)
    {
        //they wish to load a scene other than level1
        if (sceneName != "level1")
        {
            GameManager.Manager.setIsActiveForLevelGameObjects(false);
        }

        SceneManager.LoadScene(sceneName);
    }

    public void setGameManagerTipShown(bool val)
    {
        GameManager.Manager.tipShown = val;
    }

    public void stopMusic()
    {
        GameManager.Manager.musicEnabled = !GameManager.Manager.musicEnabled;

        GetAudioToggle(this, new EventArgs());
    }

    public void startNextWave()
    {
        Wave.wave.startNextWave();
    }

    public void beginGame()
    {
        Wave.wave.beginGame();
    }

    public void playRandomCoinSound()
    {
        string num = UnityEngine.Random.Range(0, 22).ToString("D2");
        GameObject go = Instantiate(Resources.Load<GameObject>("Sounds/Coins_Several/Coins_Several_" + num));
        AudioSource audioSource = go.GetComponent<AudioSource>();
        audioSource.volume = 1.0f;
        AudioSource.PlayClipAtPoint(audioSource.clip, transform.position);
        Destroy(go);
    }

    public void startPlaneRun(int amt)
    {
        if(amt > GameManager.Manager.gold)
        {
            GameObject go = Resources.Load<GameObject>("Prefabs/Text GameObjects/NotEnoughGoldText");
            go = Instantiate(go, gameObject.transform.position, Quaternion.identity, gameObject.transform);
            go.GetComponent<TextFadeOut>().FadeOut();
        }

        else
        {
            GameManager.Manager.doGoldTransaction(amt);
            GameObject plane = Resources.Load<GameObject>("Prefabs/Planes/GreenPlane");

            Vector2 pos1 = new Vector2(1229, 199);
            Vector2 pos2 = new Vector2(1204, 420);
            Vector2 pos3 = new Vector2(1229, 632);

            Instantiate(plane, pos1, Quaternion.identity);
            Instantiate(plane, pos2, Quaternion.identity);
            Instantiate(plane, pos3, Quaternion.identity);

            transform.parent.parent.gameObject.SetActive(false);
            playRandomCoinSound();
        }
    }

    public void startTankRun(int amt)
    {
        if (amt > GameManager.Manager.gold)
        {
            GameObject go = Resources.Load<GameObject>("Prefabs/Text GameObjects/NotEnoughGoldText");
            go = Instantiate(go, gameObject.transform.position, Quaternion.identity, gameObject.transform);
            go.GetComponent<TextFadeOut>().FadeOut();
        }

        else
        {
            GameManager.Manager.doGoldTransaction(amt);
            GameObject tank = Resources.Load<GameObject>("Prefabs/Tanks/GreenTank");

            //Starting points for tank
            Vector2 pos1 = new Vector2(718, 948);
            Vector2 pos2 = new Vector2(499, 948);
            Vector2 pos3 = new Vector2(274, 948);

            //Instantiating and endpoints for tank (where it should stop)
            GameObject tank1 = Instantiate(tank, pos1, tank.transform.rotation);
            tank1.GetComponent<Tank>().endPoint = new Vector2(718, 785);

            GameObject tank2 = Instantiate(tank, pos2, tank.transform.rotation);
            tank2.GetComponent<Tank>().endPoint = new Vector2(499, 785);

            GameObject tank3 = Instantiate(tank, pos3, tank.transform.rotation);
            tank3.GetComponent<Tank>().endPoint = new Vector2(274, 785);

            transform.parent.parent.gameObject.SetActive(false);
            playRandomCoinSound();
        }
    }

    private IEnumerator wait()
    {
        yield return new WaitForSeconds(1);
    }
}
