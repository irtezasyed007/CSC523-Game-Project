using UnityEngine;
using System.Collections;

public class Bullet : Weapon
{

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

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject go = collision.gameObject;

        if (go.tag == "Enemy")
        {
            Destroy(this.gameObject);
            Enemy enemy = go.GetComponent<Enemy>();
            enemy.applyDamage(getDamage());
        }

        if (go.tag == "OutOfBounds")
        {
            Destroy(this.gameObject);
        }
    }
}
