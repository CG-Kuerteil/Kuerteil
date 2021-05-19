using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinigameControlOld : MonoBehaviour
{
    public GameObject _TeleporterPref;
    //public GameObject _EnemyPref;
    private GameObject _Player;

    public int _NumberOfTries;

    public static MinigameControlOld instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        if (instance != this)
        {
            Debug.Log("Error! instance != this from MinigameControl");
            Destroy(this.gameObject);
            return;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        _NumberOfTries = GameControl.instance._NumberOfTries;
        _Player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void EnemyKilled()
    {
        MinigameWon();
    }

    public void MinigameWon()
    {
        Instantiate(_TeleporterPref, new Vector3(4, 0, 4), Quaternion.identity);
    }

    public void Died()
    {
        _NumberOfTries--;
        if (_NumberOfTries < 0)
        {
            GameControl.instance.GameOver();
        }
        else
        {
            GameControl.instance.SceneWechseln(3);
        }
    }
}
