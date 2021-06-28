using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KeyUIManager : MonoBehaviour
{
    public CanvasRenderer RedPanel;
    public CanvasRenderer GreenPanel;
    public CanvasRenderer BluePanel;

    // Start is called before the first frame update
    void Start()
    {
        RedPanel.SetAlpha(0f);
        GreenPanel.SetAlpha(0f);
        BluePanel.SetAlpha(0f);
    }

    public void KeyPickedUp(KeyType type)
    {
        switch (type)
        {
            case KeyType.RedKey:
                RedPanel.SetAlpha(1f);
                break;
            case KeyType.BlueKey:
                BluePanel.SetAlpha(1f);
                break;
            case KeyType.GreenKey:
                GreenPanel.SetAlpha(1f);
                break;
            default:
                break;
        }
    }
}
