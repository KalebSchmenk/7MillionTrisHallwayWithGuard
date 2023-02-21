using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnerController : MonoBehaviour
{
    [SerializeField] private float _spawnIn = 2.5f;
    [SerializeField] private GameObject _enemy;

    void Start()
    {
        StartCoroutine(SpawnEnemyAfter());
    }

    private IEnumerator SpawnEnemyAfter()
    {
        yield return new WaitForSeconds(_spawnIn);

        Instantiate(_enemy, this.transform.position, Quaternion.identity);
    }

 
}
