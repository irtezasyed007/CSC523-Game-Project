using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnTile : MonoBehaviour {

    //Time (in seconds) to wait before next spawn
    public int minYieldTime;
    public int maxYieldTime;

    //Total number of enemies to spawn (-1 means infinite)
    public int maxEnemies = -1;

    //Coordinates to spawn enemies at
    private float x;
    private float y;

    private GameObject[] enemies = new GameObject[4];
    private bool canSpawn = true;
    private bool readyToSpawn = true;
    private int totalEnemiesSpawned = 0;

	// Use this for initialization
	void Start () {
        //Point of collider in space
        Vector2 gameObjectPos = (Vector2)gameObject.transform.position;

        this.x = gameObjectPos.x;
        this.y = gameObjectPos.y;

        enemies[0] = Resources.Load<GameObject>("Prefabs/Enemies/enemy1");
        enemies[1] = Resources.Load<GameObject>("Prefabs/Enemies/enemy2");
        enemies[2] = Resources.Load<GameObject>("Prefabs/Enemies/enemy3");
        enemies[3] = Resources.Load<GameObject>("Prefabs/Enemies/enemy4");
    }

    // Update is called once per frame
    void Update()
    {
        if (enemyCanSpawn())
        {
            int wait = Random.Range(minYieldTime, maxYieldTime);
            Instantiate(getRandomEnemy(), transform.position, Quaternion.identity);
            totalEnemiesSpawned++;
            StartCoroutine(waitUntilNextSpawn(wait));
        }
    }

    private bool enemyCanSpawn()
    {
        return canSpawn && (totalEnemiesSpawned <= maxEnemies || maxEnemies == -1) && GameManager.Manager.tipShown;
    }

    private IEnumerator waitUntilNextSpawn(int time)
    {
        canSpawn = false;
        yield return new WaitForSeconds((float) time);
        canSpawn = true;
    }

    private GameObject getRandomEnemy()
    {
        int index = Random.Range(0, 3);
        return enemies[index];
    }

    public void activateEnemySpawning()
    {
        GameManager.Manager.tipShown = true;
        canSpawn = true;
    }

    public int EnemiesSpawned
    {
        get { return totalEnemiesSpawned; }
    }

    public int MaxEnemies
    {
        get { return maxEnemies; }
        set { maxEnemies = value; }
    }

}
