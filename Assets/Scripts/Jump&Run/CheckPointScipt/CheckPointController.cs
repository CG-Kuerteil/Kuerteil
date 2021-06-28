using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPointController : MonoBehaviour
{
    public Transform Player;

    /// <summary>
    /// Called if palyer fell down into the lava collider
    /// </summary>
    /// <param name="id"></param>
    public void EnteredCollider()
    {
        Player.transform.position = _checkpoint.position;
    }
    // Start is called before the first frame update
    void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player").transform;
            
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private Transform _checkpoint;

    /// <summary>
    /// Called if a new Platform is entered
    /// </summary>
    /// <param name="p"></param>
   public void ChangeActiveCheckPoint(Transform p)
    {
        _checkpoint = p;
    }
}
