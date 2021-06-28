using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonScript : MonoBehaviour
{
    [SerializeField]
    private Button SaveBtn;
    [SerializeField]
    private Button LoadBtn;
    [SerializeField]
    private Button ExitBtn;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        /*SaveBtn.onClick.AddListener(Save);
        LoadBtn.onClick.AddListener(Load);
        ExitBtn.onClick.AddListener(Exit);*/
    }

    public void Exit()
    {
        GameControl.Instance.Exit();
    }

    public void Load()
    {
        GameControl.Instance.Load();
    }

    public void Save()
    {
        GameControl.Instance.Save();
    }

    /*private void Load()
    {
        Debug.Log("Laod button presssed...");
        LoadBtn.onClick.RemoveAllListeners();
        GameControl.instance.Load();
    }*/
}
