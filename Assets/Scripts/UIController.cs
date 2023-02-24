using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIController : MonoBehaviour
{
    [SerializeField] GameObject _notSeen;
    [SerializeField] GameObject _searching;
    [SerializeField] GameObject _chasing;

    GameObject[] _enemyUnits;
    bool _isRoaming = true;
    bool _isSearching = false;
    bool _isChasing = false;
    public bool chaseMusic;
    public bool normalMusic;
   

    private void Update()
    {
        FindEnemies();

        // Roam State As Int = 1
        // Search State and Look State As Int = 2
        // Chase State and CaughtPlayer State As Int = 3
        // Return of 0 means state was not any of the listed
        foreach (GameObject unit in _enemyUnits) 
        {
            EnemyAIController unitAI = unit.GetComponent<EnemyAIController>();

            int tempInt = unitAI.GetState();

            if (tempInt == 1)
            {
                _isRoaming = true;
            }
            else if (tempInt == 2) 
            {
                _isSearching = true;
            }
            else if (tempInt == 3) 
            {
                _isChasing = true;
            }
            else
            {
                continue;
            }
        }

        if (_isChasing == true)
        {
            Chasing();
        }
        else if (_isSearching == true)
        {
            Searching();
        }
        else if (_isRoaming == true)
        {
            NotSeen();
        }
        else
        {
            NotSeen();
        }

        ResetBools();
    }
    public void NotSeen()
    {
        _searching.SetActive(false);
        _chasing.SetActive(false);
        _notSeen.SetActive(true);
        chaseMusic = false;
        normalMusic = true;
        
    }

    public void Searching()
    {
        _chasing.SetActive(false);
        _notSeen.SetActive(false);
        _searching.SetActive(true);
        chaseMusic = false;
        normalMusic = true;
    }

    public void Chasing()
    {
        
        _searching.SetActive(false);
        _notSeen.SetActive(false);
        _chasing.SetActive(true);
        chaseMusic = true;
        normalMusic = false;

    }

    private void FindEnemies()
    {
        _enemyUnits = GameObject.FindGameObjectsWithTag("Enemy");
    }

    private void ResetBools()
    {
        _isChasing = false;
        _isSearching = false;
        _isRoaming = false;
    }
}
