using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Wave : MonoBehaviour
{
    public SpawnTile spawnTile; //The tile that is in charge of spawning enemies
    public WaveScale waveScale;

    public int numberOfEnemiesToSpawn;
    public int baseScoreIncrementOnKill;
    public int baseScoreIncrementOnCircuitComplete;
    public int baseScoreDecrementOnCircuitFailed;
    public int baseGoldIncrementOnKill;
    public int baseGoldIncrementOnCircuitComplete;

    //The tile that is currently active (Changes with waves)
    private GameObject currentSpawnTile;

    // Use this for initialization
    void Start()
    {
        currentSpawnTile = Instantiate(spawnTile.gameObject, transform);
        spawnTile.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            GameManager.Manager.wave++;

            Destroy(currentSpawnTile.gameObject);
            currentSpawnTile = Instantiate(spawnTile.gameObject, transform);
            currentSpawnTile.SetActive(true);
            scaleToNextWave();
        }
    }

    private bool isWaveFinished()
    {
        return spawnTile.EnemiesSpawned == spawnTile.MaxEnemies && Enemy.instantiedEnemies.Count == 0;
    }

    private void scaleToNextWave()
    {
        numberOfEnemiesToSpawn += waveScale.NextNumEnemiesSpawn;
        baseScoreIncrementOnKill += waveScale.NextScoreOnKill;
        baseScoreIncrementOnCircuitComplete += waveScale.NextScoreOnCircuitComplete;
        baseScoreDecrementOnCircuitFailed += waveScale.NextScoreOnCircuitFailed;
        baseGoldIncrementOnKill += waveScale.NextGoldOnKill;
        baseGoldIncrementOnCircuitComplete += waveScale.NextGoldOnCircuitComplete;

        scaleSpawnTileToWave();
    }

    private void scaleSpawnTileToWave()
    {
        SpawnTile tmp = currentSpawnTile.GetComponent<SpawnTile>();

        tmp.maxEnemies = numberOfEnemiesToSpawn;
    }
}
