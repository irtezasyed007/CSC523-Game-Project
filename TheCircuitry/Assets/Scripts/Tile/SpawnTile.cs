using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnTile : MonoBehaviour {

    public int minYieldTime;
    public int maxYieldTime;

    private float x;
    private float y;

    private bool canSpawn = false;
    private bool readyToSpawn = true;
    private System.Random rand;

	// Use this for initialization
	void Start () {
        //Point of collider in space
        Vector2 gameObjectPos = (Vector2)gameObject.transform.position;
        rand = new System.Random();

        this.x = gameObjectPos.x;
        this.y = gameObjectPos.y;

        if(GameManager.Manager.tipShown) canSpawn = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (canSpawn)
        {
            canSpawn = false;
            Instantiate(Resources.Load<GameObject>("Prefabs/enemy1"));

            int wait = rand.Next(maxYieldTime) + minYieldTime;

            StartCoroutine(waitUntilNextSpawn(wait));
        }
    }

    private IEnumerator waitUntilNextSpawn(int time)
    {
        yield return new WaitForSeconds((float) time);
        canSpawn = true;
    }

    public void enabledSpawn()
    {
        GameManager.Manager.tipShown = true;
        canSpawn = true;
    }
}
