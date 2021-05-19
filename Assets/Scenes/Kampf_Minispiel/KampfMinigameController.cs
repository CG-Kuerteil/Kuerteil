using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KampfMinigameController : AbstractMinigameController<KampfMinigameController>
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
}
