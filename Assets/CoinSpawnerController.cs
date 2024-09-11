using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinSpawnerController : MonoBehaviour
{
    public GameObject coinPrefab; 
    public int coinAmount = 10;   
    public Vector2 spawnArea;    

    void Start()
    {
        SpawnCoins();
    }

    void SpawnCoins()
    {
        for (int i = 0; i < coinAmount; i++)
        {
            Vector2 spawnPosition = new Vector2(
                Random.Range(-spawnArea.x, spawnArea.x),
                Random.Range(-spawnArea.y, spawnArea.y)
            );
            Instantiate(coinPrefab, spawnPosition, Quaternion.identity);
        }
    }
}
