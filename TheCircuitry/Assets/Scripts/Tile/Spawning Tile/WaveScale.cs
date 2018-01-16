using UnityEngine;
using System.Collections;

public class WaveScale : MonoBehaviour
{

    public int nextNumberOfEnemiesToSpawn;
    public int nextScoreIncrementOnKill;
    public int nextScoreIncrementOnCircuitComplete;
    public int nextScoreDecrementOnCircuitFailed;
    public int nextScoreIncrementOnWaveComplete;
    public int nextGoldIncrementOnKill;
    public int nextGoldIncrementOnCircuitComplete;
    public int nextGoldIncrementOnWaveComplete;

    public int NextNumEnemiesSpawn { get; set; }
    public int NextScoreOnKill { get; set; }
    public int NextScoreOnCircuitComplete { get; set; }
    public int NextScoreOnCircuitFailed { get; set; }
    public int NextScoreOnWaveComplete { get; set; }
    public int NextGoldOnKill { get; set; }
    public int NextGoldOnCircuitComplete { get; set; }
    public int NextGoldOnWaveComplete { get; set; }
}
