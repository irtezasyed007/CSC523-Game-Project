using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnTile : MonoBehaviour {

    public int minYieldTime;
    public int maxYieldTime;

    private float x;
    private float y;

    private GameObject[] enemies = new GameObject[4];
    private bool canSpawn = false;
    private bool readyToSpawn = true;

	// Use this for initialization
	void Start () {
        //Point of collider in space
        Vector2 gameObjectPos = (Vector2)gameObject.transform.position;

        this.x = gameObjectPos.x;
        this.y = gameObjectPos.y;

        if(GameManager.Manager.tipShown) canSpawn = true;

        enemies[0] = Resources.Load<GameObject>("Prefabs/Enemies/enemy1");
        enemies[1] = Resources.Load<GameObject>("Prefabs/Enemies/enemy2");
        enemies[2] = Resources.Load<GameObject>("Prefabs/Enemies/enemy3");
        enemies[3] = Resources.Load<GameObject>("Prefabs/Enemies/enemy4");
    }

    // Update is called once per frame
    void Update()
    {
        if (canSpawn)
        {
            canSpawn = false;
            
            int wait = Random.Range(minYieldTime, maxYieldTime);
            Instantiate(getRandomEnemy());
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

    private GameObject getRandomEnemy()
    {
        int index = Random.Range(0, 3);
        return enemies[index];
    }
}
