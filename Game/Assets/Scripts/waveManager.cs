using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Wave
{
    public string waveName;
    public int enemyNum;
    public GameObject[] enemyType;
}

public class waveManager : MonoBehaviour
{
    public Wave[] waves;
    public Transform[] spawnPositions;

    private Wave currentWave;
    private int currentWaveNum;
    [SerializeField] public int waveTimer;

    public bool canSpawn = true;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        currentWave = waves[currentWaveNum];
        waveSpawner();
        GameObject[] enemyTotal = GameObject.FindGameObjectsWithTag("Enemy");
        if (enemyTotal.Length == 0 && !canSpawn && currentWaveNum + 1 != waves.Length)
        {
            currentWaveNum++;
            canSpawn = true;
        }
    }
    public void waveSpawner()
    {
        if (canSpawn)
        {
            GameObject enemy = currentWave.enemyType[Random.Range(0, currentWave.enemyType.Length)];
            Transform randomSpawnPos = spawnPositions[Random.Range(0, spawnPositions.Length)];
            Instantiate(enemy, randomSpawnPos.position, Quaternion.identity);
            currentWave.enemyNum--;
            if (currentWave.enemyNum == 0)
            {
                canSpawn = false;
            }
        }
    }
    
}
