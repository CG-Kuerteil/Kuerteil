using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hintswitch_animated : MonoBehaviour
{
    public GameObject light;
    public GameObject text;
    bool trigger = false;
    [SerializeField] private Animator controller;

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
            controller.SetBool("click", true);
        }
    }
}
