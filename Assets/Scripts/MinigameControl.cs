using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinigameControl : MonoBehaviour
{
    public GameObject _TeleporterPref;
    //public GameObject _EnemyPref;
    public GameObject _Player;

    public static MinigameControl instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        if (instance != this)
        {
            Destroy(instance);
            Debug.Log("Error! instance != this from MinigameControl");
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        _Player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void EnemyKilled()
    {
        Instantiate(_TeleporterPref, Vector3.zero, Quaternion.identity);
    }
}
