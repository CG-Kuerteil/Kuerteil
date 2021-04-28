using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuScript : MonoBehaviour
{
    private void OnGUI()
    {
        if (GUI.Button(new Rect(10, 100, 100, 30), "Save"))
        {
            GameControl.control.Save();
            Cursor.lockState = CursorLockMode.Locked;
        }
        if (GUI.Button(new Rect(10, 140, 100, 30), "Load"))
        {
            GameControl.control.Load();
            Cursor.lockState = CursorLockMode.Locked;
        }
    }
}
