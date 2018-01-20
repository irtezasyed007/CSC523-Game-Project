using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnTile : MonoBehaviour {

    //Time (in seconds) to wait before next spawn
    public int minYieldTime;
    public int maxYieldTime;

    //The highest (hardest) tier of enemy that will spawn (0-3)
    public int enemyTier = 0;

    //Total number of enemies to spawn (-1 means infinite)
    public int maxEnemies = -1;

    //Coordinates to spawn enemies at
    private float x;
    private float y;

    private GameObject[] enemies = new GameObject[4];
    private bool canSpawn = false;
    private bool readyToSpawn = true;
    private bool updatedEnemiesWavesAliveFor = false;
    private int totalEnemiesSpawned = 0;
    private int minEnemyHealth = 1;
    private int maxEnemyHealth = 5;

    //Helps enemies scale appropriately this way newly introduced enemies don't immediately scale up
    private int tier0EnemiesWavesAliveFor = 0;
    private int tier1EnemiesWavesAliveFor = 0;
    private int tier2EnemiesWavesAliveFor = 0;
    private int tier3EnemiesWavesAliveFor = 0;

    // Use this for initialization
    void Start () {
        //Point of collider in space
        Vector2 gameObjectPos = (Vector2)gameObject.transform.position;

        this.x = gameObjectPos.x;
        this.y = gameObjectPos.y;

        //Easiest to Hardest
        enemies[0] = Resources.Load<GameObject>("Prefabs/Enemies/enemy1");
        enemies[1] = Resources.Load<GameObject>("Prefabs/Enemies/enemy2");
        enemies[2] = Resources.Load<GameObject>("Prefabs/Enemies/enemy3");
        enemies[3] = Resources.Load<GameObject>("Prefabs/Enemies/enemy4");
        
    }

    private void OnEnable()
    {
        StartCoroutine(SpawnEnemy());
    }

    // Update is called once per frame
    void Update()
    {
        if (allEnemiesSpawnedForWave()) updateEnemiesWavesAliveFor();
    }

    private IEnumerator SpawnEnemy()
    {
        while (true)
        {
            if (enemyCanSpawn())
            {
                int wait = Random.Range(minYieldTime, maxYieldTime);
                int enemiesLeftToSpawn = maxEnemies - totalEnemiesSpawned;
                int amtToSpawn = Random.Range(1, enemiesLeftToSpawn / 2);

                for(int i = 0; i < amtToSpawn; i++)
                {
                    spawnRandomEnemy();
                    totalEnemiesSpawned++;
                    yield return new WaitForSeconds(Random.Range(0.0f, 1.0f));
                }
                
                yield return new WaitForSeconds(wait);
            }

            else yield return null;
        }
    }

    private bool enemyCanSpawn()
    {
        return canSpawn && (totalEnemiesSpawned < maxEnemies || maxEnemies == -1);
    }

    public bool allEnemiesSpawnedForWave()
    {
        return totalEnemiesSpawned == maxEnemies;
    }

    private void spawnRandomEnemy()
    {
        int index = Random.Range(0, enemyTier+1);

        if(enemies[index] != null)
        {
            GameObject go = Instantiate(enemies[index], transform.position, Quaternion.identity);
            scaleEnemy(go.GetComponent<Enemy>(), index);
        }
    }

    private void scaleEnemy(Enemy enemy, int tier)
    {
        int scale = 0;

        if (tier == 0)
        {
            scale = tier0EnemiesWavesAliveFor;
        }
        else if (tier == 1)
        {
            scale = tier1EnemiesWavesAliveFor;
        } 
        else if (tier == 2)
        {
            scale = tier2EnemiesWavesAliveFor;
        }
        else if (tier == 3)
        {
            scale = tier3EnemiesWavesAliveFor;
        }

        enemy.maxHealth = enemy.maxHealth + (scale * .2);
    }

    private void updateEnemiesWavesAliveFor()
    {
        if (updatedEnemiesWavesAliveFor) return;

        if (enemyTier == 0)
        {
            tier0EnemiesWavesAliveFor++;
            updatedEnemiesWavesAliveFor = true;
        }
        else if (enemyTier == 1)
        {
            tier0EnemiesWavesAliveFor++;
            tier1EnemiesWavesAliveFor++;
            updatedEnemiesWavesAliveFor = true;
        }
        else if (enemyTier == 2)
        {
            tier0EnemiesWavesAliveFor++;
            tier1EnemiesWavesAliveFor++;
            tier2EnemiesWavesAliveFor++;
            updatedEnemiesWavesAliveFor = true;
        }
        else if (enemyTier == 3)
        {
            tier0EnemiesWavesAliveFor++;
            tier1EnemiesWavesAliveFor++;
            tier2EnemiesWavesAliveFor++;
            tier3EnemiesWavesAliveFor++;
            updatedEnemiesWavesAliveFor = true;
        }
    }

    public void stopEnemySpawning()
    {
        canSpawn = false;
    }

    //Resets the spawn tile
    public void startEnemySpawning()
    {
        totalEnemiesSpawned = 0;
        canSpawn = true;
        updatedEnemiesWavesAliveFor = false;
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
