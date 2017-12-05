using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour {

    public static List<GameObject> rockets = new List<GameObject>();

    public GameObject projectile;
    public int velocity;
    public Vector2 offset = new Vector2(0.4f, 0.1f);
    public GameObject brokenTowerParticles;
    private bool canFire = true;
    private bool isBroken = true;
    private Vector2 dir;

	// Use this for initialization
	void Start () {
        velocity *= -1;
	}

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 dir = (transform.position - pos);
            this.transform.right = dir;
            this.transform.eulerAngles = new Vector3(
                                        this.transform.eulerAngles.x,
                                        this.transform.eulerAngles.y,
                                        this.transform.eulerAngles.z + 90
                                        );

            this.dir = dir.normalized;
            this.isBroken = !this.isBroken;
        }

        if (isBroken)
        {
            brokenTowerParticles.GetComponent<ParticleSystem>().Play();
        }

        else
        {
            brokenTowerParticles.GetComponent<ParticleSystem>().Stop();

            if (Input.GetKeyDown(KeyCode.Space))
            {
                GameObject go = (GameObject)Instantiate(projectile, (Vector2)transform.position + offset * transform.localScale.y, Quaternion.identity);
                go.GetComponent<Rigidbody2D>().velocity = this.dir * velocity;
                go.transform.eulerAngles = new Vector3(
                                                this.transform.eulerAngles.x,
                                                this.transform.eulerAngles.y,
                                                this.transform.eulerAngles.z
                                                );
            }

            List<Enemy> nearEnemies = getNearEntities(1000);

            if (nearEnemies.Count != 0)
            {
                if (canFire)
                {
                    faceEnemy(nearEnemies[0]);

                    GameObject go = (GameObject)Instantiate(projectile, (Vector2)transform.position + offset * transform.localScale.y, Quaternion.identity);
                    go.GetComponent<Rigidbody2D>().velocity = this.dir * velocity;
                    go.transform.eulerAngles = new Vector3(
                                                    this.transform.eulerAngles.x,
                                                    this.transform.eulerAngles.y,
                                                    this.transform.eulerAngles.z
                                                    );
                    rockets.Add(go);
                    StartCoroutine(WeaponCooldown());
                }
            }
        }
    }

    private IEnumerator WeaponCooldown()
    {
        canFire = false;
        yield return new WaitForSeconds(1);
        canFire = true;
    }

    private List<Enemy> getNearEntities(float range)
    {
        Vector2 towerPosition = this.gameObject.transform.position;
        List<Enemy> nearEnemies = new List<Enemy>();

        //Loops through all spawned entities to find the nearest one
        //NOTE: Could potentially be a source for lag if to many enemies are spawned
        foreach(Enemy enemy in Enemy.instantiedEnemies)
        {
            Vector2 enemyPosition = enemy.gameObject.transform.position;

            float lengthDiff = Mathf.Abs(enemyPosition.magnitude - towerPosition.magnitude);

            //If the enemy is within the tower's range capabilities
            if (isNear(towerPosition, enemyPosition, range)) nearEnemies.Add(enemy);
        }

        return nearEnemies;
    }

    private bool isNear(Vector2 pos1, Vector2 pos2, float range)
    {
        float lengthDiff = Mathf.Abs(pos1.magnitude - pos2.magnitude);

        if (lengthDiff <= range) return true;
        else return false;
    }

    //Need code to set rotation of the turret to face the enemy
    private void faceEnemy(Enemy enemy)
    {
        Vector3 pos = enemy.gameObject.transform.position;
        Vector2 dir = (transform.position - pos);
        this.transform.right = dir;
        this.transform.eulerAngles = new Vector3(
                                    this.transform.eulerAngles.x,
                                    this.transform.eulerAngles.y,
                                    this.transform.eulerAngles.z + 90
                                    );

        this.dir = dir.normalized;
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("hit");
    }
}
