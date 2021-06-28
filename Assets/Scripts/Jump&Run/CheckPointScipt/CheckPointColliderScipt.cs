using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPointColliderScipt : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "Player")
        {
            GameObject.FindObjectOfType<CheckPointController>().EnteredCollider();
        }
    }
}
