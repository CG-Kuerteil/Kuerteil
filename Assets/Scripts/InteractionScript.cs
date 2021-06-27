using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionScript : MonoBehaviour
{
    /// <summary>
    /// von hier aus wird der Raycast geschossen
    /// </summary>
    public Transform Socket;

    [SerializeField]
    private float _maxDistance = 5f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            RaycastHit hit;

            if (Physics.Raycast(Socket.position, Socket.forward, out hit, _maxDistance))
            {
                if (hit.transform.tag == "Button")
                {
                    Debug.Log("Hit: " + hit.transform.tag);
                    hit.transform.gameObject.GetComponent<AbstractButton>().PushButton();
                }
                
            }
        }
    }
}
