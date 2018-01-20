using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Tank : MonoBehaviour
{
    private static List<GameObject> targets = new List<GameObject>();
    private const int timeToLive = 300; //In seconds
    private static int tankCount = 0;

    public Weapon weapon;
    public int velocity;
    public int movementSpeed;
    public Vector2 endPoint;

    private GameObject muzzleFlash;
    private Vector2 direction = Vector2.right;
    private GameObject tankHead;
    private Vector3 weaponDir;
    private GameObject target;
    private int ttlCnt = 0;
    private bool duplicateInstance = false;

    private void Start()
    {
        if(tankCount < 3)
        {
            tankCount++;
            Level1Scene.level1Scene.instantiedLevel1GameObjects.Add(gameObject);
            DontDestroyOnLoad(gameObject);
        }

        else
        {
            duplicateInstance = true;
            Destroy(gameObject);
        }
    }
    // Use this for initialization
    void OnEnable()
    {
        StartCoroutine(Count());
        StartCoroutine(FireWeapon());

        foreach (Transform go in GetComponentsInChildren<Transform>())
        {
            if(go.name == "tankHead")
            {
                tankHead = go.gameObject;
                break;
            }
        }

        muzzleFlash = tankHead.GetComponentInChildren<ParticleSystem>().gameObject;
    }

    // Update is called once per frame
    void Update()
    {

        if (Vector2.Distance(transform.position, endPoint) <= 1) movementSpeed = 0;

        else
        {
            Vector2 val = direction * movementSpeed * Time.deltaTime;
            transform.Translate(val);
        }
    }

    private IEnumerator FireWeapon()
    {
        while (true)
        {
            
            if(movementSpeed == 0 && ttlCnt < timeToLive)
            {
                //If the tank doesn't have a target
                if (target == null)
                {
                    target = getNearestEnemy();

                    if (target == null || targets.Contains(target))
                    {
                        target = null;
                        yield return null;
                    }

                    else
                    {
                        targets.Add(target);
                    }
                }

                //The tank does have a target
                else
                {
                    faceEnemy(target, tankHead);
                    Vector2 newVelocity = this.weaponDir * velocity;

                    weapon.FireWeapon(target, muzzleFlash.transform.position, newVelocity * -1);
                    StartCoroutine(MuzzleFlash());
                    yield return new WaitForSeconds(7);
                }
                
            }

            else yield return null;
        }
    }

    private IEnumerator MuzzleFlash()
    {
        muzzleFlash.GetComponent<ParticleSystem>().Play();
        yield return new WaitForSeconds(1);
        muzzleFlash.GetComponent<ParticleSystem>().Stop();
    }

    private IEnumerator Count()
    {
        while (true)
        {
            if (!Wave.wave.isWaveFinished())
            {
                if (ttlCnt < timeToLive) ttlCnt++;

                else GetComponent<Rigidbody2D>().velocity = new Vector2(0, 50);
            }

            yield return new WaitForSeconds(1);     
        }

    }

    private GameObject getNearestEnemy()
    {
        GameObject closestEnemy = null;
        float distance = float.MaxValue;

        foreach (GameObject enemy in Enemy.instantiedEnemies)
        {
            float tmp = Vector2.Distance(enemy.transform.position, transform.position);

            if(tmp < distance)
            {
                distance = tmp;
                closestEnemy = enemy;
            }
        }

        return closestEnemy;
    }

    private void faceEnemy(GameObject enemy, GameObject objectToRotate)
    {
        Vector3 pos = enemy.gameObject.transform.position;
        Vector3 dir = (objectToRotate.transform.position - pos);
        objectToRotate.transform.right = dir;
        objectToRotate.transform.eulerAngles = new Vector3(
                                                objectToRotate.transform.eulerAngles.x,
                                                objectToRotate.transform.eulerAngles.y,
                                                objectToRotate.transform.eulerAngles.z + 180
                                                );

        this.weaponDir = dir.normalized;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        string tag = collision.tag;

        if(tag == "OutOfBounds")
        {
            Destroy(gameObject);
        }
    }

    private void OnDestroy()
    {
        if (!duplicateInstance)
        {
            tankCount = 0;
            targets.Clear();
            Level1Scene.level1Scene.instantiedLevel1GameObjects.Remove(gameObject);
        }
    }
}
