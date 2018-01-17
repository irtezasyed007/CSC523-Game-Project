using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomCoinSound : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void playRandomCoinSound()
    {
        string num = Random.Range(0, 22).ToString("D2");
        Debug.Log(num);
        GameObject go = Resources.Load<GameObject>("Coins Sfx/Mp3/Coins_Several/Coins_Several_" + num);
        go.GetComponent<AudioSource>().Play();
    }
}
