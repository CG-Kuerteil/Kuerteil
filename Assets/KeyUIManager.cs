using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KeyUIManager : MonoBehaviour
{
    public GameObject RedPanel;
    public GameObject GreenPanel;
    public GameObject BluePanel;

    // Start is called before the first frame update
    void Start()
    {
        RedPanel.SetActive(false);
        GreenPanel.SetActive(false);
        BluePanel.SetActive(false);
    }

    public void KeyPickedUp(KeyType type)
    {
        switch (type)
        {
            case KeyType.RedKey:
                RedPanel.SetActive(true);
                break;
            case KeyType.BlueKey:
                BluePanel.SetActive(true);
                break;
            case KeyType.GreenKey:
                GreenPanel.SetActive(true);
                break;
            default:
                break;
        }
    }
}
