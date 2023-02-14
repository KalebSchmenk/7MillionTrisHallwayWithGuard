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
        LookAround,
        Chase,
        CaughtPlayer
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
    [SerializeField] private float _lookAroundSpeed = 35.0f;


    private float _navMeshSpeed;

    private bool _canSeePlayer;
    private bool _IAmWaiting = false;

    private bool _lookingRight = false;
    private bool _lookingLeft = false;
    private bool _lookingBackCenter = false;
    private bool _doneLooking = false;

    private Vector3 _lastPosition;
    private Vector3 _lastKnownLocation;
    private Vector3 _rotationBeforeLookAround;
    private Vector3 _rotateToRight;
    private Vector3 _rotateToLeft;

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

                if (Vector3.Distance(transform.position, _player.transform.position) <= _navMeshAgent.stoppingDistance + 1.5f)
                {
                    _AIState= AIState.CaughtPlayer;
                }

                if (_canSeePlayer == false) 
                { 
                    _AIState = AIState.Search; 
                    _lastKnownLocation = _player.transform.position; 
                }
                break;

            case AIState.Search:
                _enemyColor.material.color = Color.magenta;
                Search();

                if (_canSeePlayer == true) _AIState = AIState.Chase;

                if (_navMeshAgent.remainingDistance <= _navMeshAgent.stoppingDistance && _canSeePlayer == false)
                {
                    // Look around calcs done here because it should only be done once
                    _rotationBeforeLookAround = transform.rotation.eulerAngles;
                    _rotateToRight = new Vector3(_rotationBeforeLookAround.x, _rotationBeforeLookAround.y + 65.0f, _rotationBeforeLookAround.z);
                    _rotateToLeft = new Vector3(_rotationBeforeLookAround.x, _rotationBeforeLookAround.y - 65.0f, _rotationBeforeLookAround.z);

                    _lookingRight = true;
                    _AIState = AIState.LookAround;
                }
                break;

            case AIState.LookAround:
                _enemyColor.material.color = Color.cyan;
                LookAround();


                if (_canSeePlayer == true) _AIState = AIState.Chase;

                if (_doneLooking == true && _canSeePlayer == false)
                {
                    _doneLooking= false;
                    _AIState = AIState.Roam;
                }
                break;

            case AIState.CaughtPlayer:
                _enemyColor.material.color = Color.blue;
                Debug.Log("Game over! Player caught");
                CatchPlayer();

                // Player looks toward Guard
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

    // Guard Chases Player
    private void Chase()
    {
        _navMeshAgent.SetDestination(_player.transform.position);
        _navMeshAgent.speed = _navMeshAgentSprintSpeed;
    }

    // Guard Searches Players Last Known Position
    private void Search()
    {
        _navMeshAgent.SetDestination(_lastKnownLocation);
        _navMeshAgent.speed = _navMeshAgentJogSpeed;
    }

    // Guard Looks Around For Player At Last Known Position
    private void LookAround()
    {
        var step = _lookAroundSpeed * Time.deltaTime;
        if (_lookingRight == true)
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(_rotateToRight), step);
            if (transform.rotation == Quaternion.Euler(_rotateToRight))
            {
                _lookingRight = false;
                _lookingLeft = true;
            }
        }

        if (_lookingLeft == true) 
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(_rotateToLeft), step);
            if (transform.rotation == Quaternion.Euler(_rotateToLeft))
            {
                _lookingLeft = false;
                _lookingBackCenter = true;
            }
        }

        if (_lookingBackCenter == true)
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(_rotationBeforeLookAround), step);
            if (transform.rotation == Quaternion.Euler(_rotationBeforeLookAround))
            {
                _lookingBackCenter = false;
                _doneLooking = true;
            }
        } 
    }

    // Guard Has Caught The Player
    private void CatchPlayer()
    {
        _navMeshAgent.speed = 0;
        _navMeshAgent.SetDestination(transform.position);
        //FIXME!!! REPLACE WITH PLAYER SCRIPT NAME
        var playerScript = _player.GetComponent<TestingPlayerController>();
        if (playerScript != null) playerScript._playerCaptured = true;
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

    // Guard Shoots Raycast At Max Range _proximityRange Towards Player To See If Player Is Close Enough To Be Chased
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
        else
        {
            _canSeePlayer = false;
        }

        yield return new WaitForSeconds(_proxCheckWaitTime);
        
        StartCoroutine(ProximityCheck());
    }
}
