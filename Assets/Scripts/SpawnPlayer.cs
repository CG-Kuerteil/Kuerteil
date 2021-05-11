using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPlayer : MonoBehaviour
{
    public Transform _Player;

    public Vector3 _SpawnPosition;
    // Start is called before the first frame update
    void Start()
    {
        _SpawnPosition = GetComponent<Transform>().position;

        if (GameObject.FindGameObjectWithTag("Player") == null)
        {
            Instantiate(_Player, _SpawnPosition, Quaternion.identity);
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
