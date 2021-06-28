using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPointScript : MonoBehaviour
{
    public Transform CheckPointLocation;

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "Player")
        {
            GameObject.FindObjectOfType<CheckPointController>().ChangeActiveCheckPoint(CheckPointLocation);
        }
    }
}
