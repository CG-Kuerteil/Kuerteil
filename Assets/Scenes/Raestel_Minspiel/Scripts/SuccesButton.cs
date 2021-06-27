using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuccesButton : AbstractButton
{
    public GameObject Light;
    public GameObject Text;

    bool trigger = false;

    [SerializeField]
    private Animator controller;

    public override void PushButton()
    {
        Light.SetActive(true);
        Text.SetActive(true);
        trigger = true;
        controller.SetBool("click", true);
    }

    // Start is called before the first frame update
    void Start()
    {
        Light.SetActive(false);
        Text.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
