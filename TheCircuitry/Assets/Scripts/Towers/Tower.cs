using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour {

    public GameObject projectile;
    public Vector2 velocity;
    public Vector2 offset = new Vector2(0.4f, 0.1f);
    public GameObject brokenTowerParticles;
    private bool canFire = true;
    private bool isBroken = true;

	// Use this for initialization
	void Start () {
		
	}

    // Update is called once per frame
    void Update()
    {
        if (isBroken)
        {
            brokenTowerParticles.GetComponent<ParticleSystem>().Play();
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            GameObject go = (GameObject)Instantiate(projectile, (Vector2)transform.position + offset * transform.localScale.y, Quaternion.identity);
            go.GetComponent<Rigidbody2D>().velocity = new Vector2(velocity.x, velocity.y);
        }


        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            facePoint(ray.origin);
        }

        List<Enemy> nearEnemies = getNearEntities(1);

        if(nearEnemies.Count != 0)
        {
            if (canFire)
            {
                faceEnemy(nearEnemies[0]);
                GameObject go = (GameObject)Instantiate(projectile, (Vector2)transform.position + offset * transform.localScale.y, Quaternion.identity);
                go.GetComponent<Rigidbody2D>().velocity = new Vector2(velocity.x, velocity.y);
                StartCoroutine(WeaponCooldown());
            }
        }
    }

    private IEnumerator WeaponCooldown()
    {
        canFire = false;
        yield return new WaitForSeconds(2);
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
        //We only want to change z, not x or y
        Vector3 dir = enemy.transform.position - transform.position;
        Quaternion lookRot = Quaternion.LookRotation(dir);
        lookRot.x = 0; lookRot.y = 0;
        lookRot.z *= -1.64f;
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRot, Mathf.Clamp01(3.0f * Time.maximumDeltaTime));
    }

    private void facePoint(Vector3 origin)
    {
        origin.z = 0;
        Vector3 dir = transform.position - origin;
        Quaternion lookRot = Quaternion.LookRotation(dir);
        lookRot.x = 0; lookRot.y = 0;
        lookRot.z *= -0.64f;
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRot, Mathf.Clamp01(3.0f * Time.maximumDeltaTime));
    }
}
