using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireDamagerScript : MonoBehaviour
{
    private float _FireDamage;
    // Start is called before the first frame update
    void Start()
    {
        _FireDamage = GameControl.instance._FireDamage;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player")
        {
            other.GetComponentInChildren<PlayerHealthController>().TakeDamage(_FireDamage);
        }
    }
}
