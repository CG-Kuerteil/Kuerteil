using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPlayer : MonoBehaviour
{
    public Transform _Player;

    public Vector3 _SpawnPosition;
    // Start is called before the first frame update
    void Start()
    {
        //_SpawnPosition = GetComponent<Transform>().position;

        if (GameObject.FindGameObjectWithTag("Player") == null)
        {
            Instantiate(_Player, _SpawnPosition, Quaternion.identity);
        }
        else
        {
            GameObject.FindGameObjectWithTag("Player").transform.position = _SpawnPosition;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawWireCube(_SpawnPosition + Vector3.up, new Vector3(1, 2, 1));
    }
}
