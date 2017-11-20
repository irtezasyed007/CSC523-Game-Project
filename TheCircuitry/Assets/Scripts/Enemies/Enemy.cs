using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {

    //All the current enemies that spawned
    public static List<Enemy> instantiedEnemies = new List<Enemy>();

    public Vector2 direction;
    public double health;
    public int movementSpeed;

	// Use this for initialization
	void Start () {
        direction = Vector2.right;
        instantiedEnemies.Add(this);
	}
	
	// Update is called once per frame
    //Implement enemy movement
	void Update () {
        Vector2 val = direction * movementSpeed * Time.deltaTime;
        transform.Translate(val);
	}

    private void OnDestroy()
    {
        instantiedEnemies.Remove(this);
        Destroy(this.gameObject);
    }
}
