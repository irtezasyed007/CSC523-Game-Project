﻿    using Assets.Scripts;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class Tower : MonoBehaviour {

    protected static bool loaded = false;
    protected const int MAX_TOWERS = 6;
    protected static List<int> instantiatedTowers = new List<int>(); //A list of loaded towers with their IDs
    
    public static List<Tower> towerObjects = new List<Tower>();
    public static List<GameObject> towerGameObjects = new List<GameObject>();
    public static int towerID = 1;

    public GameObject projectile;
    public int velocity;
    public Vector2 offset = new Vector2(0.4f, 0.1f);
    public GameObject brokenTowerParticles;
    public int towerTier;
    internal bool isBroken = true;
    protected bool canFire = true;
    protected Weapon weapon;
    protected Vector2 dir;
    protected int id;
    protected int noWeaponFireCount = 0;
    protected int noTowerCountdownCount = 0;
    protected bool startCountdown = false;

    // Use this for initialization
    protected void Start () {
        init();
    }

    protected void init()
    {
        this.id = towerID;
        towerID++;

        velocity *= -1; //So tower isn't shooting backwards

        DontDestroyOnLoad(this.gameObject);
        instantiatedTowers.Add(this.id);
        towerObjects.Add(this);
        towerGameObjects.Add(this.gameObject);
        this.weapon = projectile.GetComponent<Weapon>();

        //Deletes tower if they all have already been instantiated
        if (id > MAX_TOWERS)
        {
            Destroy(this.gameObject);
            Destroy(this);
            this.gameObject.SetActive(false);
            towerObjects.Remove(this);
            towerGameObjects.Remove(this.gameObject);
            instantiatedTowers.Remove(this.id);
        }
    }

    void Awake()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //DEBUG FEATURE TO FIX ALL TOWERS IN A LEVEL
        //if (Input.GetMouseButtonDown(1))
        //{
        //    Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        //    Vector2 dir = (transform.position - pos);
        //    this.transform.right = dir;
        //    this.transform.eulerAngles = new Vector3(
        //                                this.transform.eulerAngles.x,
        //                                this.transform.eulerAngles.y,
        //                                this.transform.eulerAngles.z + 90
        //                                );

        //    this.dir = dir.normalized;
        //    this.isBroken = !this.isBroken;
        //}

        if (isBroken)
        {
            brokenTowerParticles.GetComponent<ParticleSystem>().Play();
        }

        else
        {
            //If it didnt start the countdown yet, start it.
            if (!startCountdown)
            {
                StartCoroutine(TowerBreak());               
            }

            brokenTowerParticles.GetComponent<ParticleSystem>().Stop();

            //DEBUG FEATURE FOR SHOOTING
            //if (Input.GetKeyDown(KeyCode.Space))
            //{
            //    GameObject go = (GameObject)Instantiate(projectile, (Vector2)transform.position + offset * transform.localScale.y, Quaternion.identity);
            //    go.GetComponent<Rigidbody2D>().velocity = this.dir * velocity;
            //    go.transform.eulerAngles = new Vector3(
            //                                    this.transform.eulerAngles.x,
            //                                    this.transform.eulerAngles.y,
            //                                    this.transform.eulerAngles.z
            //                                    );
            //}

            List<Enemy> nearEnemies = getNearEntities(60);

            if (nearEnemies.Count != 0)
            {
                if (canFire)
                {
                    int enemyIndex = Random.Range(0, nearEnemies.Count - 1);
                    faceEnemy(nearEnemies[enemyIndex], this.gameObject);

                    GameObject go = (GameObject)Instantiate(projectile, (Vector2)transform.position + offset * transform.localScale.y, Quaternion.identity);
                    go.GetComponent<Rigidbody2D>().velocity = this.dir * velocity;
                    go.transform.eulerAngles = new Vector3(
                                                    this.transform.eulerAngles.x,
                                                    this.transform.eulerAngles.y,
                                                    this.transform.eulerAngles.z
                                                    );

                    StartCoroutine(WeaponCooldown());
                    noWeaponFireCount = 0;
                }
                noWeaponFireCount++;
                noTowerCountdownCount++;

                if (noWeaponFireCount > 75)
                {
                    canFire = true;
                    noWeaponFireCount = 0;
                }

                if(noTowerCountdownCount > 1000)
                {
                    startCountdown = false;
                    noTowerCountdownCount = 0;
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

    protected IEnumerator TowerBreak()
    {
        noTowerCountdownCount = 0;
        startCountdown = true;
        yield return new WaitForSeconds(20);
        isBroken = true;
        startCountdown = false;
    }

    protected List<Enemy> getNearEntities(float range)
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
            if (isNear(towerPosition, enemyPosition, range))
            {
                nearEnemies.Add(enemy);
            }
        }

        return nearEnemies;
    }

    protected bool isNear(Vector2 pos1, Vector2 pos2, float range)
    {
        float lengthDiff = Mathf.Abs(pos1.magnitude - pos2.magnitude);

        if (lengthDiff <= range) return true;
        else return false;
    }

    protected void faceEnemy(Enemy enemy, GameObject objectToRotate)
    {
        Vector3 pos = enemy.gameObject.transform.position;
        Vector2 dir = (objectToRotate.transform.position - pos);
        objectToRotate.transform.right = dir;
        objectToRotate.transform.eulerAngles = new Vector3(
                                                objectToRotate.transform.eulerAngles.x,
                                                objectToRotate.transform.eulerAngles.y,
                                                objectToRotate.transform.eulerAngles.z + 90
                                                );

        this.dir = dir.normalized;
    }

    protected int getID()
    {
        return this.id;
    }

}