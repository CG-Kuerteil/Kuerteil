using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class BossController : MonoBehaviour
{
    private NavMeshAgent agent;
    public Animator _Animator;
    private bool _InRange = false;

    private Transform _Target;
    public float _AttackDistance = 2f;

    public float _HP = 20;

    public Slider _HPSlider;
    public GameObject _DieVFX;

    // Start is called before the first frame update
    void Start()
    {
        _HPSlider.minValue = 0;
        _HPSlider.maxValue = _HP;
        _HPSlider.value = _HP;
        agent = GetComponent<NavMeshAgent>();
        _Target = GameControl.instance.player.transform;
        _Animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        _HPSlider.GetComponentInParent<Transform>().rotation = Quaternion.LookRotation(MinigameControl.instance._Player.transform.position, Vector3.up);
            
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

    public void TakeDamage(float d)
    {
        if (_HP - d <= 0)
        {
            Debug.Log("Life below or equal zero... Destroying agent");
            Destroy(gameObject);
            GameObject explosion = Instantiate(_DieVFX, transform.position, Quaternion.identity);
            Destroy(explosion, 4f);
            MinigameControl.instance.EnemyKilled();
        }
        else
        {
            _HP -= d;
            _HPSlider.value = _HP;
            Debug.Log("Enemy: Got " + d + " damage. HP is now: " + _HP);
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
