using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Weapon : MonoBehaviour {

    public double damage;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public virtual void FireWeapon(Vector3 rotation, Vector2 position, Vector2 velocity)
    {
        GameObject go = (GameObject)Instantiate(this.gameObject, position, Quaternion.identity);
        go.GetComponent<Rigidbody2D>().velocity = velocity;
        go.transform.eulerAngles = new Vector3(
                                        rotation.x,
                                        rotation.y,
                                        rotation.z
                                        );
    }

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject go = collision.gameObject;

        if(go.tag == "Enemy")
        {
            Destroy(this.gameObject);
        }

        if(go.tag == "OutOfBounds")
        {
            Destroy(this.gameObject);
        }
    }

    public double getDamage()
    {
        return this.damage;
    }
}
