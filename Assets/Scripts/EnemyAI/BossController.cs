using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BossController : MonoBehaviour
{
    private NavMeshAgent agent;
    public Animator _Animator;
    private bool _InRange = false;

    private Transform _Target;
    public float _AttackDistance = 2f;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        _Target = GameControl.instance.player.transform;
        _Animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
        if (Vector3.Distance(transform.position, _Target.position) <= _AttackDistance)
        {
            _Animator.Play("Attack");
        }
        else
        {
            if (_InRange == true)
            {
                agent.SetDestination(_Target.position);
                _Animator.Play("Walking");
            }
            else
            {
                if (agent.remainingDistance <= agent.stoppingDistance)
                {
                    _Animator.Play("Idle");
                }
            }
        }
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.tag == "Player")
        {
            _InRange = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            _InRange = false;
        }
    }
}
