using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnTile : MonoBehaviour {

    public GameObject objectToSpawn;

    public int clusterMin;
    public int clusterMax;
    public int minYieldTime;
    public int maxYieldTime;

    private float x;
    private float y;

    private bool canSpawn = true;
    private bool readyToSpawn = true;
    private System.Random rand;
	// Use this for initialization
	void Start () {
        //Point of collider in space
        Vector2 gameObjectPos = (Vector2)gameObject.transform.position;
        rand = new System.Random();

        this.x = gameObjectPos.x;
        this.y = gameObjectPos.y;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 spawnPosition = new Vector3(x, y, 0.0f);
        Quaternion q = new Quaternion(0, 0, 0, this.objectToSpawn.transform.rotation.w);
    }

    private IEnumerator waitUntilNextSpawn(int time)
    {
        yield return new WaitForSeconds((float) time);
        canSpawn = true;
    }
    
    private IEnumerator spawnGameObject(Vector3 position, Quaternion q)
    {
        readyToSpawn = false;
        yield return new WaitForSeconds(1f);
        Instantiate(objectToSpawn, position, q);
        readyToSpawn = true;
    }
}
