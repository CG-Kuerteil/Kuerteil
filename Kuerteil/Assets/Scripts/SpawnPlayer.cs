using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPlayer : MonoBehaviour
{
    public Transform _Player;
    // Start is called before the first frame update
    void Start()
    {
        if (GameObject.FindGameObjectWithTag("Player") == null)
        {
            Instantiate(_Player, Vector3.zero, Quaternion.identity);
        }
        else
        {
            GameObject.FindGameObjectWithTag("Player").transform.position = transform.position;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
