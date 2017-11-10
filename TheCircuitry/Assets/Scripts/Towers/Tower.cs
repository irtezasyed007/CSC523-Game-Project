using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour {

    public GameObject projectile;
    public Vector2 velocity;
    public Vector2 offset = new Vector2(0.4f, 0.1f);

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
        if(Input.GetKeyDown(KeyCode.Space))
        {
            GameObject go = (GameObject) Instantiate(projectile, (Vector2)transform.position + offset * transform.localScale.y, Quaternion.identity);
            go.GetComponent<Rigidbody2D>().velocity = new Vector2(velocity.x, velocity.y);
        }

        if (Input.GetMouseButtonDown(0))
        {
            var mousePos = Input.mousePosition;
            var gameobjectPos = gameObject.transform.position;
        }
	}
}
