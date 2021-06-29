using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CapsuleCollider))]
public class EnemyAttackHandler : MonoBehaviour
{
    [SerializeField]
    private EnemyController _enemyController;
    // Start is called before the first frame update
    void Start()
    {
        //_enemyController = gameObject.gameObject.GetComponent<EnemyController>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "Player")
        {
            _enemyController.Attack(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.transform.tag == "Player")
        {
            _enemyController.Attack(false);
        }
    }
}
