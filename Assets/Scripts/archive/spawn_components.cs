using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spawn_components : MonoBehaviour
{
    public Vector3 origin;
    public GameObject[] floor;
    public int steps = 10;

    private Dictionary<bool[,], List<Vector2>> tempOrga;
    // Start is called before the first frame update
    void Start()
    {
        GameObject floors = Instantiate(floor[0], new Vector3(0f, 0f, 0f), Quaternion.identity);
        PassagesSpawnerField script = floors.GetComponent<PassagesSpawnerField>();
        tempOrga = script.GetOrga();


        float x = origin.x;
        float y = origin.y;
        float z = origin.z;

        for (int i = 0; i < steps; i++)
        {/*
            foreach (bool[,] item in tempOrga.Keys)
            {
                if (item)
                {

                }
            }
            for (int j = 0; j < tempOrga.Values; j++)
            {

            }
            i++;*/
        }
    }

    private void SpawnObject()
    {

    }

    private void MovePointer(Vector3 direction, float distance)
    {
        transform.position += direction * distance;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
