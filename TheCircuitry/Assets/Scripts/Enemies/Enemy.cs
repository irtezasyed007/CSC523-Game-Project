using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class Enemy : MonoBehaviour {

    //All the current enemies that spawned
    public static List<GameObject> instantiedEnemies = new List<GameObject>();

    //Gradually makes the enemies faster
    private static int movementSpeedAddition = 0;

    //A unit vector in the direction you want the enemy to move
    public Vector2 direction;
    public double maxHealth;
    public int movementSpeed;
    public int enemyTier = 1;
    public ParticleSystem damageEffect;

    private double currentHealth;
    private bool counting = false;
    private TextMesh healthTextMesh;

	// Use this for initialization
	void Start () {
        direction = Vector2.right;
        movementSpeed += movementSpeedAddition;
        instantiedEnemies.Add(gameObject);
        Level1Scene.level1Scene.instantiedLevel1GameObjects.Add(gameObject);
        currentHealth = maxHealth;

        MeshRenderer meshRenderer = GetComponentInChildren<MeshRenderer>();
        meshRenderer.sortingOrder = 100;
        TextMesh textMesh = GetComponentInChildren<TextMesh>();
        healthTextMesh = textMesh;
        textMesh.text = maxHealth.ToString("0");

        DontDestroyOnLoad(gameObject);
	}

    // Update is called once per frame
    void Update () {
        Vector2 val = direction * movementSpeed * Time.deltaTime;
        transform.Translate(val);
        StartCoroutine(StartCount());

        if (currentHealth <= 0.0)
        {
            killEnemy();
            Destroy(gameObject);
        }
	}

    private IEnumerator StartCount()
    {
        if (!counting)
        {
            counting = true;
            yield return new WaitForSeconds(15);
            movementSpeedAddition += 10;
            counting = false;
        }
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        string tag = collision.gameObject.tag;

        //Enemy reaches end of the path
        if(tag == "EndCollider")
        {
            Destroy(gameObject);

            GameManager.Manager.decrementHealth();

            if(GameManager.Manager.health == 0)
            {
                GameOver.staticPanel.SetActive(true);
                Text[] txt = GameObject.FindGameObjectWithTag("GameOverPanel").GetComponentsInChildren<Text>();
                txt[1].text = "Score: " + GameManager.Manager.score;
            }

        }
    }

    public void setDirection(Vector3 dir)
    {
        this.direction = dir;
    }

    public void applyDamage(double damage)
    {
        currentHealth -= damage;

        if (currentHealth < 1)
        {
            healthTextMesh.text = "0";
            killEnemy();
        }

        else
        {
            string health = currentHealth.ToString("0");
            healthTextMesh.text = health;
        }
    }

    private void killEnemy()
    {
        GameManager.Manager.addToScore(Level1Scene.level1Scene.getRandomValue(Wave.wave.baseScoreIncrementOnKill) * enemyTier);
        GameManager.Manager.addToGold(Level1Scene.level1Scene.getRandomValue(Wave.wave.baseGoldIncrementOnKill) * enemyTier);
        Destroy(gameObject);
    }

    public void playDamageEffect()
    {
        damageEffect.Play();
    }

    private void OnDestroy()
    {
        instantiedEnemies.Remove(gameObject);
        Level1Scene.level1Scene.instantiedLevel1GameObjects.Remove(gameObject);
    }
}
