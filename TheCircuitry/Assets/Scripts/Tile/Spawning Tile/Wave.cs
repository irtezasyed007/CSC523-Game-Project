using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Wave : MonoBehaviour
{
    public static Wave wave;
    public GameObject spawnTileGameObject;
    public GameObject waveScaleGameObject;

    public int baseNumberOfEnemiesToSpawn;
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
    private bool rewardedOnWaveComplete = false;

    // Use this for initialization
    void Start()
    {
        if(wave == null)
        {
            wave = this;
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
            if (!rewardedOnWaveComplete)
            {
                GameManager.Manager.addToGold(Level1Scene.level1Scene.getRandomValue(baseGoldIncrementOnWaveComplete));
                GameManager.Manager.addToScore(Level1Scene.level1Scene.getRandomValue(baseScoreIncrementOnWaveComplete));
                rewardedOnWaveComplete = true;
            } 

            if (isReadyForNextWave)
            { 
                GameManager.Manager.wave++;
                scaleToNextWave();
                spawnTile.startEnemySpawning();
                isReadyForNextWave = false;
                rewardedOnWaveComplete = false;
            }

            else
            {
                //Prompt user for input for when they are ready for the next wave to start
                GameObject.Find("BeginNextWave").GetComponentInChildren<Button>(true).gameObject.SetActive(true);
                spawnTile.stopEnemySpawning();
            }
        }
    }

    public bool isWaveFinished()
    {
        return spawnTile.allEnemiesSpawnedForWave() && Enemy.instantiedEnemies.Count == 0;
    }

    public void startNextWave()
    {
        isReadyForNextWave = true;
    }

    public void beginGame()
    {
        spawnTile.startEnemySpawning();
    }

    public bool IsReadyForWave { get { return isReadyForNextWave; } set { isReadyForNextWave = value; } }

    private void scaleToNextWave()
    {
        baseNumberOfEnemiesToSpawn += waveScale.NextNumEnemiesSpawn;
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
        spawnTile.MaxEnemies = baseNumberOfEnemiesToSpawn;

        //Makes it so harder enemies spawn every 7 waves
        if (GameManager.Manager.wave == 7)
        {
            spawnTile.minYieldTime = 2;
            spawnTile.maxYieldTime = 4;
            spawnTile.enemyTier = 1;
        }
        if (GameManager.Manager.wave == 14)
        {
            spawnTile.minYieldTime = 2;
            spawnTile.maxYieldTime = 3;
            spawnTile.enemyTier = 2;
        }
        if (GameManager.Manager.wave == 21)
        {
            spawnTile.minYieldTime = 1;
            spawnTile.maxYieldTime = 2;
            spawnTile.enemyTier = 3;
        }
    }

    private void OnDestroy()
    {
        Level1Scene.level1Scene.instantiedLevel1GameObjects.Remove(gameObject);
    }
}
