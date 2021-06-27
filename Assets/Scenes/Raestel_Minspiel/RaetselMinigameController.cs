using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaetselMinigameController : AbstractMinigameController<RaetselMinigameController>
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    protected override void Hook()
    {
        Debug.Log("Spawned RaetselMinigameController");
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
