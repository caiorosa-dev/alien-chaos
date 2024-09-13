using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class WaveEnemy
{
    [Range(0, 500)]
    public int minSpawnCount;
    [Range(0, 500)]
    public int maxSpawnCount;

    public GameObject enemyPrefab;
}

[System.Serializable]
public class Wave
{
    public int maxEnemyCount;
    public int killAmountToWin;
    public int duration;

    public float timeBetweenRounds;

    public List<WaveEnemy> enemies;
}

public class WaveManager : Singleton<WaveManager>
{
    public Transform[] spawnPoints;
    public List<Wave> waves;

    [SerializeField] private int currentWaveIndex = 0;
    [SerializeField] private int currentSpawnedEnemies = 0;
    [SerializeField] private int currentKilledEnemies = 0;

    public delegate void OnWaveFail();
    public OnWaveFail onWaveFail;

    public delegate void OnWaveSuccess();
    public OnWaveSuccess onWaveSuccess;

    public delegate void OnWaveProgress(float percentage);
    public OnWaveProgress onWaveProgress;

    private float waveStartTime;
    [SerializeField] private bool isWaveActive = false;
    [SerializeField] private GameObject WaveStatusBar;

    private Coroutine spawnEnemiesCoroutine;

    private void Update()
    {
        //WaveStatusBar.GetComponent<Image>().fillAmount = ;
        if (isWaveActive)
        {
            float currentWaveDuration = Time.time - waveStartTime;

            if (currentWaveDuration > waves[currentWaveIndex].duration)
            {
                EndWave(false);
                return;
            }

            if (currentKilledEnemies >= waves[currentWaveIndex].killAmountToWin)
            {
                EndWave(true);
                return;
            }
        }
    }

    public void TriggerNextWaveStart()
    {
        currentKilledEnemies = 0;
        if (currentWaveIndex + 1 > waves.Count)
            return;

        Wave currentWave = waves[currentWaveIndex];

        isWaveActive = true;
        waveStartTime = Time.time;

        spawnEnemiesCoroutine = StartCoroutine(SpawnEnemies(currentWave));
    }

    private IEnumerator SpawnEnemies(Wave wave)
    {
        while (isWaveActive)
        {
            if (currentSpawnedEnemies >= wave.maxEnemyCount) yield return null;

            foreach (WaveEnemy enemyType in wave.enemies)
            {
                int spawnCount = Random.Range(enemyType.minSpawnCount, enemyType.maxSpawnCount + 1);

                for (int i = 0; i < spawnCount; i++)
                {
                    if (currentSpawnedEnemies >= wave.maxEnemyCount)
                        yield break;

                    Transform spawnPoint = getRandomSpawnPoint();

                    if (spawnPoint != null && enemyType.enemyPrefab != null)
                    {
                        Instantiate(enemyType.enemyPrefab, spawnPoint.position, Quaternion.identity);
                        currentSpawnedEnemies++;
                    }
                }

            }

            yield return new WaitForSeconds(wave.timeBetweenRounds);
        }
    }

    private void EndWave(bool isSuccess)
    {
        isWaveActive = false;
        CameraShake.Instance.ShakeCamera(1f, 3f);

        foreach (GameObject enemy in GameObject.FindGameObjectsWithTag("Enemy"))
        {
            EnemyDeathSystem deathSystem = enemy.GetComponent<EnemyDeathSystem>();
            if (deathSystem != null)
            {
                deathSystem.TriggerDeath();
            }
        }

        currentSpawnedEnemies = 0;

        if (isSuccess)
        {
            currentWaveIndex++;
            onWaveSuccess?.Invoke();
        }
        else
        {
            onWaveFail?.Invoke();
        }
    }

    public int getRemainingTimeToEndWave()
    {
        return waves[currentWaveIndex].duration - (int) (Time.time - waveStartTime);
    }

    public Transform getRandomSpawnPoint()
    {
        return spawnPoints[Random.Range(0, spawnPoints.Length)];
    }

    public void OnEnemyDespawn()
    {
        currentSpawnedEnemies--;
        currentKilledEnemies++;

        float completePercentage = (float)currentKilledEnemies / waves[currentWaveIndex].killAmountToWin;
        onWaveProgress?.Invoke(completePercentage);
    }
}
