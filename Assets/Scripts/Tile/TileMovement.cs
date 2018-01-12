using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileMovement : MonoBehaviour {

    public string tag;

	// Use this for initialization
	void Start () {
        tag = tag.ToLower();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnTriggerEnter2D(Collider2D collision)
    {

        Enemy enemy = null;
        foreach (Enemy e in Enemy.instantiedEnemies)
        {
            if (e.gameObject.Equals(collision.gameObject))
            {
                enemy = e;
                break;
            }
        }

        if (enemy != null)
        {
            //Manipulate the game object's velocity
            switch (tag)
            {
                case "up":
                    enemy.direction = Vector2.up;
                    break;
                case "down":
                    enemy.direction = Vector2.down;
                    break;
                case "left":
                    enemy.direction = Vector2.left;
                    break;
                case "right":
                    enemy.direction = Vector2.right;
                    break;
                default:
                    Debug.Log("[Warning] Could not translate enemies position!");
                    break;
            }
        }
    }

   
}
