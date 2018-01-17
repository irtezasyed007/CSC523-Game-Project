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
            GameManager.Manager.setIsActiveForLevelGameObjects(false);
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

    public void playRandomCoinSound()
    {
        string num = Random.Range(0, 22).ToString("D2");
        GameObject go = Instantiate(Resources.Load<GameObject>("Sounds/Coins_Several/Coins_Several_" + num));
        AudioSource audioSource = go.GetComponent<AudioSource>();
        AudioSource.PlayClipAtPoint(audioSource.clip, transform.position);
        Destroy(go);
    }

    private IEnumerator wait()
    {
        yield return new WaitForSeconds(1);
    }
}
