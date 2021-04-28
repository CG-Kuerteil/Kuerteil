using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuScript : MonoBehaviour
{
    private void OnGUI()
    {
        if (GUI.Button(new Rect(10, 100, 100, 40), "Save"))
        {
            GameControl.control.Save();
            Cursor.lockState = CursorLockMode.Locked;
        }
        if (GUI.Button(new Rect(10, 160, 100, 40), "Load"))
        {
            GameControl.control.Load();
            Cursor.lockState = CursorLockMode.Locked;
        }
        if (GUI.Button(new Rect(10, 220, 100, 40), "Exit"))
        {
            SceneManager.LoadScene(0);
        }
    }
}
