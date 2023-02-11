using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAIController : MonoBehaviour
{

    private enum AIState
    { 
        Roam,
        Chase
    }

    private AIState _AIState;
    private NavMeshAgent _navMeshAgent;

    [SerializeField]
    [Range(0, 250)] private float _walkRadius;

    [SerializeField]
    [Range(1f, 5f)] private float _waitTime;

    [SerializeField]
    [Range(0, 75)] private float _proximityRange;

    [SerializeField]
    private float _proxCheckWaitTime;

    private bool _canSeePlayer;
    private bool _IAmWaiting = false;

    private Vector3 _lastPosition;

    private Renderer _enemyColor;

    private GameObject _player;


    // Start is called before the first frame update
    void Start()
    {
        _AIState = AIState.Roam;
        _enemyColor = GetComponent<Renderer>();
        _navMeshAgent = GetComponent<NavMeshAgent>();

        _player = GameObject.FindGameObjectWithTag("Player");

        StartCoroutine(ProximityCheck());
    }

    // Update is called once per frame
    void Update()
    {
        switch (_AIState) 
        { 
            case AIState.Roam:
                _enemyColor.material.color = Color.yellow;
                Wander();

                if (_canSeePlayer == true) _AIState = AIState.Chase;
                break;

            case AIState.Chase:
                _enemyColor.material.color = Color.red;
                Chase();

                // FIXME! Replace with behaviour where the AI will check the last known
                // area
                if(_canSeePlayer == false) _AIState = AIState.Roam;
                break;
        }
    }

    // Wanders to a random point
    private void Wander()
    {
        if (_IAmWaiting == false && _navMeshAgent.remainingDistance <= _navMeshAgent.stoppingDistance)
        {
            _navMeshAgent.speed = 3.5f;

            _navMeshAgent.SetDestination(RandomNavMeshLocation());
            _IAmWaiting = true;
            StartCoroutine(RandomWaitTimer());
        }
    }

    // Finds a random point on the nav mesh to roam to
    private Vector3 RandomNavMeshLocation()
    {
        // Loop may be performance heavy but ensures
        // that the position is on the nav mesh
        while (true)
        {
            Vector3 finalPosition = Vector3.zero;
            Vector3 randomPosition = Random.insideUnitSphere * _walkRadius;
            randomPosition += transform.position;

            NavMeshHit hit;
            if (NavMesh.SamplePosition(randomPosition, out hit, _walkRadius, 1) && hit.position != _lastPosition)
            {
                Debug.Log("Got a new target position");
                finalPosition = hit.position;

                _lastPosition = finalPosition;
                
                Debug.Log("Final Position: " + finalPosition);
                return finalPosition;

            }
        }
    }

    private void Chase()
    {
        _navMeshAgent.SetDestination(_player.transform.position);
        _navMeshAgent.speed = 7.5f;
    }

    private IEnumerator RandomWaitTimer()
    {
        yield return new WaitForSeconds(_waitTime);

        if (_IAmWaiting == true) _IAmWaiting = false;
    }

    private IEnumerator ProximityCheck()
    {
        RaycastHit hit;

        if (Physics.Raycast(transform.position, _player.transform.position - transform.position, out hit, _proximityRange))
        {
            if (hit.transform.gameObject.CompareTag("Player"))
            {
                _canSeePlayer = true;
            }
            else
            {
                _canSeePlayer = false;
            }
        }

        yield return new WaitForSeconds(_proxCheckWaitTime);
        
        StartCoroutine(ProximityCheck());
    }
}
