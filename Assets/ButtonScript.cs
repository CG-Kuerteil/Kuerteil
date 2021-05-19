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
        SaveBtn.onClick.AddListener(GameControl.instance.Save);
        LoadBtn.onClick.AddListener(GameControl.instance.Load);
        ExitBtn.onClick.AddListener(GameControl.instance.Exit);
    }
}
