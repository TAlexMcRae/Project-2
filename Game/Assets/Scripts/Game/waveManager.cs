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

    #region Variables
    public static waveManager instance;

    public Wave[] waves;
    public Transform[] spawnPoints;

    private Wave currWave;
    public int currWaveNum;
    private bool canSpawn;
    #endregion

    void Start()
    {

        if (PlayerPref.hardMode)
        {

            for (int rnr = 0; rnr < waves.Length; rnr++)
            {

                waves[rnr].enemyNum *= 2;
            }
        }

        instance = this;
        canSpawn = true;
        currWaveNum = 0;
    }

    // Update is called once per frame
    void Update()
    {
        currWave = waves[currWaveNum];
        waveSpawner();

        if (GameManager.instance.enemiesToKill <= 0 && GameManager.instance.capturedAll)
        {

            if (!canSpawn && currWaveNum <= waves.Length - 1)
            {

                currWaveNum++;
                canSpawn = true;

                for (int rnr = 0; rnr < GameManager.instance.captureZones.Length; rnr++)
                {

                    GameObject temp = GameManager.instance.captureZones[rnr];

                    temp.GetComponent<zoneCollider>().temp = null;
                    temp.GetComponent<zoneCollider>().captured = false;
                    temp.GetComponent<zoneCollider>().ColorChange();
                }

                GameManager.instance.captureCount = GameManager.instance.captureZones.Length;
                GameManager.instance.capturedAll = false;
                GameManager.instance.UpdateUI();
            }
        }
    }
    public void waveSpawner()
    {

        if (canSpawn)
        {

            GameObject enemy = currWave.enemyType[Random.Range(0, currWave.enemyType.Length)];
            Transform spawnPos = spawnPoints[Random.Range(0, spawnPoints.Length)];

            Instantiate(enemy, spawnPos.position, Quaternion.identity);
            currWave.enemyNum--;

            if (currWave.enemyNum <= 0)
            {
                canSpawn = false;
            }
        }
    }
}