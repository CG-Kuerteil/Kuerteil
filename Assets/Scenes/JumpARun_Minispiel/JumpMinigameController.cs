using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpMinigameController : AbstractMinigameController<JumpMinigameController>
{
    protected override void Hook()
    {
        Debug.Log("Spawned KampfMinigameController");
    }

    private void Awake()
    {
        if (Instance != this && Instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    private void Start()
    {
    }
}
