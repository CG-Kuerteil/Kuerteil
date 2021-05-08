using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class EnemyController : MonoBehaviour
{
    private AudioFiles audioFiles;
    private AudioSource audioSource;

    private GameObject[] _AvailablePaths;
    public float _LookRadius = 10f;
    public Transform _PositionMarker;
    GameObject target;
    NavMeshAgent agent;
    private SphereCollider col;
    bool _ShootRayCast = false;
    private Vector3 _LastKnownPosition;
    private Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        Physics2D.queriesStartInColliders = false;
        animator = GetComponent<Animator>();
        target = GameObject.FindGameObjectWithTag("Player");
        agent = GetComponent<NavMeshAgent>();
        col = GetComponent<SphereCollider>();

        #region AudioSetp
        audioSource = GetComponent<AudioSource>();
        #endregion
    }

    // Update is called once per frame
    void Update()
    {
        if (agent.isActiveAndEnabled)
        {
            if (!audioSource.isPlaying)
            {
                audioSource.PlayOneShot(GameControl.instance.audio.PickRandom());
            }
        }
        if (_ShootRayCast == true)
        {
            RaycastHit hit;
            Vector3 direction = target.transform.position - transform.position;
            if (Physics.Raycast(transform.position, direction.normalized, out hit, col.radius))
            {
                if (hit.collider.gameObject.tag == "Player")
                {
                    Debug.Log("player in sight!!!");
                    float distance = Vector3.Distance(target.transform.position, transform.position);
                    agent.isStopped = false;
                    agent.SetDestination(target.transform.position);
                    animator.Play("Walking");
                    if (distance <= agent.stoppingDistance)
                    {
                        //TODO: Attack the target
                        //Face the target
                        FaceTarget();
                    }
                }
                else
                {
                    if (agent.transform.position == _LastKnownPosition)
                    {
                        animator.Play("EvilSlayer");
                    }
                    //Debug.Log("raycast not hit, gameObject: " + hit.collider.GetInstanceID());

                }
            }
            else
            {
                float dist = agent.remainingDistance;
                if (dist != Mathf.Infinity && agent.remainingDistance <= 1.9f)
                {
                    //Arrived.
                    GenerateNewPosition();
                }
                else
                {
                    //Debug.Log("Remaining Distance: " + agent.remainingDistance);
                }
                //Debug.Log("raycast not hit :(");
            }
        }
        else
        {
            float dist = agent.remainingDistance; 
            if (dist != Mathf.Infinity && agent.remainingDistance <= 1.9f) 
            {
                //Arrived.
                GenerateNewPosition();
            }
            else
            {
                //Debug.Log("Remaining Distance: " + agent.remainingDistance);
            }
        }
    }

    private void GenerateNewPosition()
    {
        _AvailablePaths = GameObject.FindGameObjectsWithTag("EnemyPathPoint");
        int _NextPoint = Random.Range(0, _AvailablePaths.Length);
        Vector3 nextPos = _AvailablePaths[_NextPoint].transform.position;
        agent.SetDestination(nextPos);
        
        Instantiate(_PositionMarker, nextPos, Quaternion.identity);
        Debug.Log("new position generated...");
    }

    private void FaceTarget()
    {
        Vector3 direction = (target.transform.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _LookRadius);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            _ShootRayCast = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            _ShootRayCast = false;
        }
    }

    /*void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _LookRadius);
        /*Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, agent.stoppingDistance);

    }*/
}
