using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField]
    private GameObject _enemyPrefab;
    [SerializeField]
    private GameObject _enemyContainer;
    [SerializeField]
    private GameObject[] powerups;

    private bool _stopSpawning =  false;
    
    public void StartSpawning()
    {
        StartCoroutine(SpawnEnemyRoutine()); // calling the enemy spawn routine method
        StartCoroutine(SpawnPowerupRoutine()); // calling the power up spawn routine method
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator SpawnEnemyRoutine()
    {
        yield return new WaitForSeconds(3f);
        while (_stopSpawning == false) 
        {
            Vector3 posToSpawn = new Vector3(Random.Range(-8f, 8f), 5.5f, 0); // This spawns enemies on the x axis randomly
            GameObject newEnemy = Instantiate(_enemyPrefab, posToSpawn, Quaternion.identity); // here we are just clearing up the hierarchy in unity an spawning enemies in the enemy container
            newEnemy.transform.parent = _enemyContainer.transform;
            yield return new WaitForSeconds(5.0f);
        }
    }
    IEnumerator SpawnPowerupRoutine()
    {
        yield return new WaitForSeconds(3f);
        while (_stopSpawning == false)
        {
            Vector3 posToSpawn = new Vector3(Random.Range(-8f, 8f), 5.5f, 0); // Spawns power ups in the x axis at random positions 
            int randomPowerup = Random.Range(0, 3);
            Instantiate(powerups[randomPowerup], posToSpawn, Quaternion.identity);
            yield return new WaitForSeconds(Random.Range(3, 8));
        }
    }

    public void OnPlayerDeath()
    {
        _stopSpawning = true;
    }
}
