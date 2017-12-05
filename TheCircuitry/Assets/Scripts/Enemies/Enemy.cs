using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
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
        if (health <= 0.0) Destroy(this);
	}

    public void OnTriggerEnter2D(Collider2D collision)
    {
        string tag = collision.gameObject.tag;

        if(tag == "Rocket")
        {
            health -= 20.0;
        }

        if(tag == "EndCollider")
        {
            Text currHealth = GameObject.Find("healthText").GetComponent<Text>();
            string[] tokens = currHealth.text.Split(':');
            int tmpHealth = System.Convert.ToInt32(tokens[1]) - 1;
            Destroy(this);

            if(tmpHealth < 0)
            {
                GameOver.staticPanel.SetActive(true);
            }

            else
            {
                currHealth.text = "Health: " + tmpHealth;
            }
            
        }
    }

    private void OnDestroy()
    {
        instantiedEnemies.Remove(this);
        Destroy(this.gameObject);
    }
}
