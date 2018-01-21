using UnityEngine;
using System.Collections;

public class Rocket : Weapon
{

    public float explosionRadius = 50.0f;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public override void FireWeapon(Vector3 rotation, Vector2 position, Vector2 velocity)
    {
        base.FireWeapon(rotation, position, velocity);
    }

    public override void FireWeapon(Vector2 position, Vector2 velocity)
    {
        base.FireWeapon(position, velocity);
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {

        GameObject go = collision.gameObject;

        if (go.tag == "Enemy")
        {
            Enemy enemy = go.GetComponent<Enemy>();
            enemy.playDamageEffect();
            ExplosionDamage(enemy.gameObject.transform.position, explosionRadius);
        }

        if (go.tag == "OutOfBounds")
        {
            Destroy(this.gameObject);
        }
    }

    private void ExplosionDamage(Vector3 center, float radius)
    {
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(center, radius);
        int i = 0;
        int enemyCount = 0;
        Debug.Log(hitColliders.Length);
        while (i < hitColliders.Length)
        {
            GameObject go = hitColliders[i].gameObject;

            if(go.tag == "Enemy")
            {
                enemyCount++;
                Enemy enemy = go.GetComponent<Enemy>();
                enemy.applyDamage(getDamage());
            }

            i++;
        }

        Destroy(this.gameObject);
        Debug.Log("Enemies in Radius: " + enemyCount);
    }
}
