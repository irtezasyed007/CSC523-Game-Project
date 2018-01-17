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

    public int NextNumEnemiesSpawn { get { return nextNumberOfEnemiesToSpawn; } set { nextNumberOfEnemiesToSpawn = value; } }
    public int NextScoreOnKill { get { return nextScoreIncrementOnKill; } set { nextScoreIncrementOnKill = value; } }
    public int NextScoreOnCircuitComplete { get {return nextScoreIncrementOnCircuitComplete; } set {nextScoreIncrementOnCircuitComplete = value; } }
    public int NextScoreOnCircuitFailed { get { return nextScoreDecrementOnCircuitFailed; } set {nextScoreDecrementOnCircuitFailed = value; } }
    public int NextScoreOnWaveComplete { get {return nextScoreIncrementOnWaveComplete; } set {nextScoreIncrementOnWaveComplete = value; } }
    public int NextGoldOnKill { get {return nextGoldIncrementOnKill; } set {nextGoldIncrementOnKill = value; } }
    public int NextGoldOnCircuitComplete { get {return nextGoldIncrementOnCircuitComplete; } set {nextGoldIncrementOnCircuitComplete = value; } }
    public int NextGoldOnWaveComplete { get {return nextGoldIncrementOnWaveComplete; } set {nextGoldIncrementOnWaveComplete = value; } }
}
