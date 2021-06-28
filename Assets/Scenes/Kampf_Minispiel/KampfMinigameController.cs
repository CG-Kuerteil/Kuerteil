using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KampfMinigameController : AbstractMinigameController<KampfMinigameController>
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
