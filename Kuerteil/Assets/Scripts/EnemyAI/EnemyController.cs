using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class EnemyController : MonoBehaviour
{
    public float _LookRadius = 10f;

    GameObject target;
    NavMeshAgent agent;
    private SphereCollider col;
    bool _ShootRayCast = false;

    private Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        Physics2D.queriesStartInColliders = false;
        animator = GetComponent<Animator>();
        target = GameObject.FindGameObjectWithTag("Player");
        agent = GetComponent<NavMeshAgent>();
        col = GetComponent<SphereCollider>();
    }

    // Update is called once per frame
    void Update()
    {
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
                    if (agent.remainingDistance == 0)
                    {
                        animator.Play("EvilSlayer");
                    }
                    Debug.Log("raycast not hit, gameObject: " + hit.collider.GetInstanceID());

                }
            }
            else
            {
                if (agent.remainingDistance == 0)
                {
                    animator.Play("EvilSlayer");
                }
                Debug.Log("raycast not hit :(");
            }
        }
        else
        {
            if (agent.remainingDistance == 0)
            {
                animator.Play("EvilSlayer");
            }
        }
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
