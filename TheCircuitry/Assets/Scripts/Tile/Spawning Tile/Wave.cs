using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Wave : MonoBehaviour
{
    private static Wave instance;
    public GameObject spawnTileGameObject;
    public GameObject waveScaleGameObject;

    public int numberOfEnemiesToSpawn;
    public int baseScoreIncrementOnKill;
    public int baseScoreIncrementOnCircuitComplete;
    public int baseScoreDecrementOnCircuitFailed;
    public int baseScoreIncrementOnWaveComplete;
    public int baseGoldIncrementOnKill;
    public int baseGoldIncrementOnCircuitComplete;
    public int baseGoldIncrementOnWaveComplete;

    //The tile that is currently active (Changes with waves)
    private bool isReadyForNextWave = false;
    //The tile that is in charge of spawning enemies
    private SpawnTile spawnTile;
    //In charge of scaling the waves as the player progresses
    private WaveScale waveScale;

    // Use this for initialization
    void Start()
    {
        if(instance == null)
        {
            DontDestroyOnLoad(gameObject);
            Level1Scene.level1Scene.instantiedLevel1GameObjects.Add(gameObject);
            spawnTile = spawnTileGameObject.GetComponent<SpawnTile>();
            waveScale = waveScaleGameObject.GetComponent<WaveScale>();
        }

        else
        {
            Destroy(gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (isWaveFinished())
        {

            if (isReadyForNextWave)
            {
                GameObject.Find("BeginNextWave").GetComponentInChildren<Button>(true).gameObject.SetActive(false);
                GameManager.Manager.wave++;
                scaleToNextWave();
                spawnTile.startEnemySpawning();
                isReadyForNextWave = false;
            }

            else
            {
                //Prompt user for input for when they are ready for the next wave to start
                GameObject.Find("BeginNextWave").GetComponentInChildren<Button>(true).gameObject.SetActive(true);
                spawnTile.stopEnemySpawning();
            }
        }
    }

    private bool isWaveFinished()
    {
        return spawnTile.EnemiesSpawned >= spawnTile.MaxEnemies && Enemy.instantiedEnemies.Count == 0;
    }

    public void startNextWave()
    {
        isReadyForNextWave = true;
    }

    private void scaleToNextWave()
    {
        numberOfEnemiesToSpawn += waveScale.NextNumEnemiesSpawn;
        baseScoreIncrementOnKill += waveScale.NextScoreOnKill;
        baseScoreIncrementOnCircuitComplete += waveScale.NextScoreOnCircuitComplete;
        baseScoreDecrementOnCircuitFailed += waveScale.NextScoreOnCircuitFailed;
        baseScoreIncrementOnWaveComplete += waveScale.nextScoreIncrementOnWaveComplete;
        baseGoldIncrementOnKill += waveScale.NextGoldOnKill;
        baseGoldIncrementOnCircuitComplete += waveScale.NextGoldOnCircuitComplete;
        baseGoldIncrementOnWaveComplete += waveScale.NextGoldOnCircuitComplete;

        scaleSpawnTileToWave();
    }

    private void scaleSpawnTileToWave()
    {
        spawnTile.MaxEnemies = numberOfEnemiesToSpawn;
    }

    private void OnDestroy()
    {
        Level1Scene.level1Scene.instantiedLevel1GameObjects.Remove(gameObject);
    }
}
