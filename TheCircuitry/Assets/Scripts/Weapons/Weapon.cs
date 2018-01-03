using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour {

    public ParticleSystem ps;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject go = collision.gameObject;

        if(go.tag == "Enemy")
        {
            Destroy(this.gameObject);
        }

        if(go.tag == "OutOfBounds")
        {
            Destroy(this.gameObject);
        }
    }
}
