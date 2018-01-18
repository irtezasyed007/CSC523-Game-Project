using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class Enemy : MonoBehaviour {

    //All the current enemies that spawned
    public static List<GameObject> instantiedEnemies = new List<GameObject>();

    private static int movementSpeedAddition = 0;

    public Vector2 direction;
    public double maxHealth;
    public int movementSpeed;
    public ParticleSystem damageEffect;

    private double currentHealth;
    private double appliedDamage;
    private double damageThreshold;
    private bool counting = false;
    private int selector = 4;
    private SpriteRenderer[] sprites; //The sprites used to display the enemies health (1, 2, 3, 4, 5)

	// Use this for initialization
	void Start () {
        direction = Vector2.right;
        movementSpeed += movementSpeedAddition;
        instantiedEnemies.Add(gameObject);
        Level1Scene.level1Scene.instantiedLevel1GameObjects.Add(gameObject);
        DontDestroyOnLoad(gameObject);
	}

    void Awake()
    {
        SpriteRenderer[] tmpSprites = GetComponentsInChildren<SpriteRenderer>();
        sprites = new SpriteRenderer[tmpSprites.Length - 1];

        //First sprite is always the enemy sprite and not its health...
        //...so we skip over it
        int index = -1;
        foreach(SpriteRenderer sr in tmpSprites)
        {
            if (index != -1)
            {
                sprites[index] = sr;
                if (index == 0) sr.gameObject.SetActive(true);
                else sr.gameObject.SetActive(false);             
            }
                

            index++;
        }

        this.damageThreshold = (this.maxHealth / 5);
        this.currentHealth = this.maxHealth;
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
        this.currentHealth -= damage;

        if (currentHealth <= 0.0 || damage >= currentHealth) killEnemy();

        else
        {
            this.appliedDamage += damage;

            if (this.appliedDamage >= this.damageThreshold)
            {
                this.appliedDamage -= this.damageThreshold;

                if (selector > 0)
                {
                    switch (selector)
                    {
                        case 1: //1 Sprite
                            this.GetComponentsInChildren<SpriteRenderer>()[1].gameObject.SetActive(false);
                            sprites[4].gameObject.SetActive(true);
                            break;
                        case 2: //2 Sprite
                            this.GetComponentsInChildren<SpriteRenderer>()[1].gameObject.SetActive(false);
                            sprites[3].gameObject.SetActive(true);
                            break;
                        case 3: //3 Sprite
                            this.GetComponentsInChildren<SpriteRenderer>()[1].gameObject.SetActive(false);
                            sprites[2].gameObject.SetActive(true);
                            break;
                        case 4: //4 Sprite
                            this.GetComponentsInChildren<SpriteRenderer>()[1].gameObject.SetActive(false);
                            sprites[1].gameObject.SetActive(true);
                            break;
                        default:
                            Debug.Log("Error");
                            break;
                    }

                    selector--;
                }
            }
        }

    }

    private void killEnemy()
    {
        GameManager.Manager.addToScore(Wave.wave.baseScoreIncrementOnKill);
        GameManager.Manager.addToGold(Wave.wave.baseGoldIncrementOnKill);
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
