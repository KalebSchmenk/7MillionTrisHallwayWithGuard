using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAIController : MonoBehaviour
{

    private enum AIState
    { 
        Roam,
        Search,
        Chase
    }

    private AIState _AIState;
    private NavMeshAgent _navMeshAgent;

    [SerializeField]
    [Range(0.0f, 250.0f)] private float _walkRadius;

    [SerializeField]
    [Range(1.0f, 15.0f)] private float _waitTime;

    [SerializeField]
    [Range(0.0f, 75.0f)] private float _proximityRange;

    [SerializeField]
    private float _proxCheckWaitTime;

    [SerializeField] private float _navMeshAgentSprintSpeed = 7.5f;
    [SerializeField] private float _navMeshAgentJogSpeed = 5.0f;

    private float _navMeshSpeed;

    private bool _canSeePlayer;
    private bool _IAmWaiting = false;

    private Vector3 _lastPosition;
    private Vector3 _lastKnownLocation;

    private Renderer _enemyColor;

    private GameObject _player;


    // Start is called before the first frame update
    void Start()
    {
        _AIState = AIState.Roam;
        _enemyColor = GetComponent<Renderer>();
        _navMeshAgent = GetComponent<NavMeshAgent>();

        _player = GameObject.FindGameObjectWithTag("Player");

        _navMeshSpeed = _navMeshAgent.speed;

        StartCoroutine(ProximityCheck());
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log("I am waiting is: " + _IAmWaiting);
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

                if (_canSeePlayer == false) 
                { 
                    _AIState = AIState.Search; 
                    _lastKnownLocation = _player.transform.position; 
                }
                break;

            // State changed handled in LookRight()
            case AIState.Search:
                _enemyColor.material.color = Color.magenta;
                Search();

                if (_canSeePlayer == true) _AIState = AIState.Chase;

                if (_navMeshAgent.remainingDistance <= _navMeshAgent.stoppingDistance && _canSeePlayer == false)
                {
                    _AIState = AIState.Roam;
                }
                break;
        }
    }

    // Wanders to a random point
    private void Wander()
    {
        if (_IAmWaiting == false && _navMeshAgent.remainingDistance <= _navMeshAgent.stoppingDistance)
        {
            _navMeshAgent.speed = _navMeshSpeed;
            _IAmWaiting = true;
            StartCoroutine(WaitTimer());
        }
    }

    // Finds a random point on the nav mesh to roam to
    private Vector3 RandomNavMeshLocation()
    {
        // Loop may be performance heavy but ensures
        // that the position is on the nav mesh
        while (true)
        {
            Vector3 targetPosition = Vector3.zero;
            Vector3 randomPosition = Random.insideUnitSphere * _walkRadius;
            randomPosition += transform.position;

            NavMeshHit hit;
            if (NavMesh.SamplePosition(randomPosition, out hit, _walkRadius, 1) && hit.position != _lastPosition)
            {
                Debug.Log("Got a new target position");
                targetPosition = hit.position;

                _lastPosition = targetPosition;
                
                Debug.Log("Target Position: " + targetPosition);
                return targetPosition;

            }
        }
    }

    private void Chase()
    {
        _navMeshAgent.SetDestination(_player.transform.position);
        _navMeshAgent.speed = _navMeshAgentSprintSpeed;
    }

    private void Search()
    {
        _navMeshAgent.SetDestination(_lastKnownLocation);
        _navMeshAgent.speed = _navMeshAgentJogSpeed;
    }

    // FIXME!! Fix up logic
    private IEnumerator WaitTimer()
    {
        _IAmWaiting = true;

        yield return new WaitForSeconds(_waitTime);

        if (_IAmWaiting == true) _IAmWaiting = false;

        // State Roam is the only caller of this function. This is why setting a destination here is acceptable
        _navMeshAgent.SetDestination(RandomNavMeshLocation());
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
