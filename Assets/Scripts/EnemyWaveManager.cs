using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class EnemyWaveManager : MonoBehaviour
{
    public static EnemyWaveManager Instance { get; private set; }
    public event EventHandler onEnemyWaveChanged;
    private enum State
    {
        WaitingToSpawnNextWave,
        SpawningWave,
    }
    [SerializeField] private List<Transform> spawnPositionTransformList;
    [SerializeField] private Transform nextWaveSpawnPositionTransform;
    private State state;
    private int waveNumber;
    private float nextWaveSpawnTimer;
    private float nextEnemySpwanTimer;
    private int remainingEnemySpawnAmount;
    private Vector3 spawnPosition;
    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        waveNumber = 0;
        spawnPosition = spawnPositionTransformList[UnityEngine.Random.Range(0, spawnPositionTransformList.Count)].position;
        nextWaveSpawnPositionTransform.position = spawnPosition;
        state = State.WaitingToSpawnNextWave;
        nextWaveSpawnTimer = 10f;
    }
    private void Update()
    {
        switch (state)
        {
            case State.WaitingToSpawnNextWave:
                {
                    nextWaveSpawnTimer -= Time.deltaTime;
                    if (nextWaveSpawnTimer < 0f)
                    {
                        SpawnWave();
                    }
                    break;
                }
            case State.SpawningWave:
                {
                    if (remainingEnemySpawnAmount > 0)
                    {
                        nextEnemySpwanTimer -= Time.deltaTime;
                        if (nextEnemySpwanTimer < 0f)
                        {
                            nextEnemySpwanTimer = UnityEngine.Random.Range(0f, 0.2f);
                            Enemy.Create(spawnPosition + UtilsClass.GetRandomDir() * UnityEngine.Random.Range(0f, 10f));
                            remainingEnemySpawnAmount--;
                            if (remainingEnemySpawnAmount <= 0f)
                            {
                                state = State.WaitingToSpawnNextWave;
                                spawnPosition = spawnPositionTransformList[UnityEngine.Random.Range(0, spawnPositionTransformList.Count)].position;
                                nextWaveSpawnPositionTransform.position = spawnPosition;
                                nextWaveSpawnTimer = 15f;
                            }
                        }
                    }
                    break;
                }
        }



    }
    private void SpawnWave()
    {
        remainingEnemySpawnAmount = 3 + 3 * waveNumber;
        state = State.SpawningWave;
        waveNumber++;
        onEnemyWaveChanged?.Invoke(this, EventArgs.Empty);
    }

    public int GetWaveNumber()
    {
        return waveNumber;
    }
    public float GetNextWaveSpawnTimer()
    {
        return nextWaveSpawnTimer;
    }
    public Vector3 GetSpawnPosition()
    {
        return spawnPosition;
    }
}
