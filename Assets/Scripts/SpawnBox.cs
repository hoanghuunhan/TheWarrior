using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnBox : MonoBehaviour
{
    public Transform[] spawnPointsGround;
    public Transform[] spawnPointsSky;
    public GameObject[] enemyPrefabs;
    public GameObject[] enemyFlyPrefabs;
    public GameObject smokeEffect;
    public int limitEnemyGround = 5;
    public int limitEnemyFly = 3;
    public float spawnTimeEnemyGround = 2f;
    public float spawnTimeEnemyFly = 3f;
    bool isTrigger = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !isTrigger)
        {
            if (smokeEffect && spawnPointsGround.Length > 0 && spawnPointsSky.Length > 0 && enemyPrefabs.Length > 0)
            {
                StartCoroutine(SpawnEnemy());
                StartCoroutine(SpawnEnemyFly());
                isTrigger = true;
            }
        }
    }

    IEnumerator SpawnEnemy()
    {
        int posIdx = 0;
        for (int i = 0; i < limitEnemyGround; i++)
        {
            int randEnemy = Random.Range(0, enemyPrefabs.Length);

            Instantiate(smokeEffect, spawnPointsGround[posIdx].position, Quaternion.identity);
            Instantiate(enemyPrefabs[randEnemy], spawnPointsGround[posIdx].position, Quaternion.identity);

            SoundManager.Ins.PlaySound(SoundManager.Ins.appearSound);

            posIdx++;
            yield return new WaitForSeconds(spawnTimeEnemyGround);
        }
    }
    IEnumerator SpawnEnemyFly()
    {
        yield return new WaitForSeconds(1f);

        int posIdx = 0;
        for (int i = 0; i < limitEnemyFly; i++)
        {
            int randEnemy = Random.Range(0, enemyFlyPrefabs.Length);

            Instantiate(smokeEffect, spawnPointsSky[posIdx].position, Quaternion.identity);
            Instantiate(enemyFlyPrefabs[randEnemy], spawnPointsSky[posIdx].position, Quaternion.identity);

            SoundManager.Ins.PlaySound(SoundManager.Ins.appearSound);

            posIdx++;
            yield return new WaitForSeconds(spawnTimeEnemyFly);
        }
    }
}
