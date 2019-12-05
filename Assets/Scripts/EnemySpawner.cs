using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject Enemy;
    public GameObject[] SpawnPosition;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(EnemySpawn());   
    }
    public IEnumerator EnemySpawn()
    {
        Instantiate(Enemy, SpawnPosition[Random.Range(0, SpawnPosition.Length)].transform.position,Quaternion.identity);
        yield return new WaitForSeconds(60);
        StartCoroutine(EnemySpawn());
    }
}
