using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreviousSceneLoader : MonoBehaviour {

    string previousSceneName;
    private void Awake()
    {
        previousSceneName = GameManager.Manager.activeScene;
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void LoadPreviousScene()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(previousSceneName);
    }
}
