using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Weapon : MonoBehaviour {

    public double damage;
    protected bool isTriggered = false;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public virtual void FireWeapon(Vector2 position, Vector2 velocity)
    {
        GameObject go = Instantiate(this.gameObject, position, Quaternion.identity);
        go.GetComponent<Rigidbody2D>().velocity = velocity;
    }

    public virtual void FireWeapon(Vector3 rotation, Vector2 position, Vector2 velocity)
    {
        GameObject go = Instantiate(this.gameObject, position, Quaternion.identity);
        go.GetComponent<Rigidbody2D>().velocity = velocity;
        go.transform.eulerAngles = new Vector3(
                                        rotation.x,
                                        rotation.y,
                                        rotation.z
                                        );
    }

    public virtual void FireWeapon(Quaternion rotation, Vector2 position, Vector2 velocity)
    {
        GameObject go = Instantiate(this.gameObject, position, rotation);
        go.GetComponent<Rigidbody2D>().velocity = velocity;
    }

    public virtual void FireWeapon(GameObject enemy, Vector2 position, Vector2 velocity)
    {
        GameObject go = Instantiate(this.gameObject, position, Quaternion.identity);
        go.GetComponent<Rigidbody2D>().velocity = velocity;
        faceEnemy(enemy, go);
    }

    private void faceEnemy(GameObject enemy, GameObject objectToRotate)
    {
        Vector3 pos = enemy.gameObject.transform.position;
        Vector3 dir = (objectToRotate.transform.position - pos);
        objectToRotate.transform.right = dir;
        objectToRotate.transform.eulerAngles = new Vector3(
                                                objectToRotate.transform.eulerAngles.x,
                                                objectToRotate.transform.eulerAngles.y,
                                                objectToRotate.transform.eulerAngles.z + 90
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
