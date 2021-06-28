using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpMinigameController : AbstractMinigameController<JumpMinigameController>
{ 
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
