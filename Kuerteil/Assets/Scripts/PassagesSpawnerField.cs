using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassagesSpawnerField : Passages
{
    protected override void InitOrga()
    {
        this.orga = new Dictionary<bool[,], List<Vector2>>();

        bool[,] n = new bool[1, 1];
        n[0, 0] = true;
        List<Vector2> directions = new List<Vector2>();
        directions.Add(new Vector2(1f, 0f));
        orga[n] = directions;
    }

    // Start is called before the first frame update
    void Start()
    {
        InitOrga(); 
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
