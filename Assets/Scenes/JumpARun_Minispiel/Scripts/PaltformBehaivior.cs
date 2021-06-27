using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PaltformBehaivior : MonoBehaviour
{
    [SerializeField][Range(0, 1)] float speed = 1f;
    [SerializeField] [Range(0, 100)] float range = 1f;

    private Vector3 StartzPosition;

    private void Start()
    {
        StartzPosition = transform.position;
    }
    void Update()
    {
        loop();
    }

    void loop()
    {
        float yPos = Mathf.PingPong(Time.time * speed, 1) * range + StartzPosition.x;
        transform.position = new Vector3(yPos, transform.position.y, transform.position.z);
    }

    private void OnDrawGizmosSelected()
    {
        /*Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(new Vector3(StartPosition.x + Range, StartPosition.y, StartPosition.z), Vector3.one);
        Gizmos.DrawWireCube(new Vector3(StartPosition.x - Range, StartPosition.y, StartPosition.z), Vector3.one);*/
    }
}
