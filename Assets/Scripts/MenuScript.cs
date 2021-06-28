using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuScript : MonoBehaviour
{
    public GameObject _MenuView;
    private bool menuOn;

    private void Start()
    {
        if (_MenuView == null)
        {
            _MenuView = GameObject.FindGameObjectWithTag("MenuView");
        }
    }
    private void Update()
    {
        if (SceneManager.GetActiveScene().buildIndex != 0 && GameControl.Instance._GameOver == false)
        {
            if (Input.GetKeyUp(KeyCode.Tab) && _MenuView.GetComponent<Animator>().GetBool("menuOpen") == false)
            {
                _MenuView.GetComponent<Animator>().Play("MenuOpeningState");
                _MenuView.GetComponent<Animator>().SetBool("menuOpen", true);
            }
            else if (Input.GetKeyUp(KeyCode.Tab) && _MenuView.GetComponent<Animator>().GetBool("menuOpen") == true)
            {
                _MenuView.GetComponent<Animator>().Play("MenuClosingState");
                _MenuView.GetComponent<Animator>().SetBool("menuOpen", false);
            }
            if (GameControl.Instance._MenuState == true)
            {
                GameControl.Instance.player.gameObject.GetComponent<FirstPersonController>().canLook = false;
                Cursor.lockState = CursorLockMode.None;
                Time.timeScale = 0f;
            }
            else if (GameControl.Instance._MenuState == false)
            {
                Cursor.lockState = CursorLockMode.Locked;
                Time.timeScale = 1f;
                GameControl.Instance.player.gameObject.GetComponent<FirstPersonController>().canLook = true;
            }
            //Debug.Log("Menu state " + GameControl.instance._MenuState);
        }
    }

    /*private void OnGUI()
    {
        if (SceneManager.GetActiveScene().buildIndex == 1)
        { 
            if (GUI.Button(new Rect(10, 100, 100, 40), "Save"))
            {
                GameControl.instance.Save();
                Cursor.lockState = CursorLockMode.Locked;
            }
            if (GUI.Button(new Rect(10, 160, 100, 40), "Load"))
            {
                GameControl.instance.Load();
                Cursor.lockState = CursorLockMode.Locked;
            }
            if (GUI.Button(new Rect(10, 220, 100, 40), "Exit"))
            {
                SceneManager.LoadScene(0);
            }
        }
    }*/
}
