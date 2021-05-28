using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hintswitchTable : MonoBehaviour
{
    public GameObject light;
    public GameObject light2;
    public GameObject text;
    bool trigger = false;

    private void Start()
    {
        light.SetActive(false);
        light2.SetActive(false);
        text.SetActive(false);
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player" && Input.GetKeyDown(KeyCode.L) && !trigger)
        {
            light.SetActive(true);
            light2.SetActive(true);
            text.SetActive(true);
            trigger = true;
        }
    }
}
