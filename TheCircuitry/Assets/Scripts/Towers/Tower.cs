using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Tower : MonoBehaviour {

    protected static bool loaded = false;

    public GameObject projectile;
    public int velocity;
    public Vector2 offset = new Vector2(0.4f, 0.1f);
    public GameObject brokenTowerParticles;
    public float weaponFireRate;
    protected int fireRateCnt = 0;
    public int towerTimeToLive;
    protected int ttlCnt = 0;
    public int towerTier;
    internal bool isBroken = false;
    protected bool canFire = true;
    protected Weapon weapon;
    protected Vector2 dir;
    protected int noWeaponFireCount = 0;
    protected int noTowerCountdownCount = 0;
    protected int secondsWaited = 0;
    protected bool startCountdown = false;
    protected bool initialized = false;

    // Use this for initialization
    protected void Start () {

    }

    protected void init()
    {
        velocity *= -1; //So tower isn't shooting backwards

        this.weapon = projectile.GetComponent<Weapon>();

    }

    private void OnEnable()
    {
        StartCoroutine(TowerBreak());
    }

    // Update is called once per frame
    void Update()
    {
        if (isBroken)
        {
            brokenTowerParticles.GetComponent<ParticleSystem>().Play();
        }

        else
        {
            brokenTowerParticles.GetComponent<ParticleSystem>().Stop();           
        }
    }

    private IEnumerator WeaponCooldown()
    {
        yield return new WaitForSeconds(weaponFireRate);
    }

    //Only breaks the towers when they are active and a wave is going on
    protected IEnumerator TowerBreak()
    {
        while (true)
        {
            yield return new WaitForSeconds(1);

            if (!Wave.wave.isWaveFinished())
            {
                if (isBroken) ttlCnt = 0;

                if (ttlCnt >= towerTimeToLive && !isBroken)
                {
                    isBroken = true;
                }
                else if (!isBroken)
                {
                    ttlCnt++;
                }
            }
        }
    }

    protected List<Enemy> getNearEntities(float range)
    {
        Vector2 towerPosition = this.gameObject.transform.position;
        List<Enemy> nearEnemies = new List<Enemy>();

        //Loops through all spawned entities to find the nearest one
        //NOTE: Could potentially be a source for lag if to many enemies are spawned
        foreach(GameObject enemy in Enemy.instantiedEnemies)
        {
            Vector2 enemyPosition = enemy.transform.position;

            //If the enemy is within the tower's range capabilities
            if (isNear(towerPosition, enemyPosition, range))
            {
                nearEnemies.Add(enemy.GetComponent<Enemy>());
            }
        }

        return nearEnemies;
    }

    protected bool isNear(Vector2 pos1, Vector2 pos2, float range)
    {
        float lengthDiff = Vector2.Distance(pos1, pos2);

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

    public int getTier()
    {
        return this.towerTier;
    }

    public bool canUpgrade()
    {
        if (towerTier >= 3) return false;
        else return true;
    }

    public virtual string getTowerType() { return "Tower"; }

    public virtual bool upgradeTower() { return false; }

}
