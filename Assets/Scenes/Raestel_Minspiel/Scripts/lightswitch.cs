using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lightswitch : MonoBehaviour
{

    public GameObject light;
    bool trigger = false;

    [SerializeField]
    private Animator controller;

    private void Start()
    {
        light.SetActive(false);
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.tag == "Player" && Input.GetKeyDown(KeyCode.L) && !trigger)
        {
            light.SetActive(true);
            trigger = true;
            controller.SetBool("click", true);
        }
    }

}
