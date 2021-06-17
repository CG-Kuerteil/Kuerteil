using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hintswitch : MonoBehaviour
{
    public GameObject light;
    public GameObject text;
    bool trigger = false;

    private void Start()
    {
        light.SetActive(false);
        text.SetActive(false);
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player" && Input.GetKeyDown(KeyCode.L) && !trigger)
        {
            light.SetActive(true);
            text.SetActive(true);
            trigger = true;
        }
    }
}
