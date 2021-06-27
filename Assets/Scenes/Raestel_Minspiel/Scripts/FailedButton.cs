using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FailedButton : AbstractButton
{
    public GameObject Light;

    bool trigger = false;

    [SerializeField]
    private Animator _controller;

    public override void PushButton()
    {
        Light.SetActive(true);
        trigger = true;
        _controller.SetBool("click", true);
    }

    // Start is called before the first frame update
    void Start()
    {
    Light.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
