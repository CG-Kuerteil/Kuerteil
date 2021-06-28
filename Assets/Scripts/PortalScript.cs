using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalScript : MonoBehaviour
{
    public int _SceneToTravelTo = 1;

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.tag == "Player")
        {
            Debug.Log("Loading Scene: " + _SceneToTravelTo);
            GameControl.Instance.SceneWechseln(_SceneToTravelTo);
            Debug.Log("Minigame done! new Scene: "+_SceneToTravelTo);
        }
    }
}
