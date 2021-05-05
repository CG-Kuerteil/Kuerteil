using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioFiles : MonoBehaviour
{
    public AudioClip[] _FootSteps_long;
    public AudioClip[] _FootSteps;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public AudioClip PickRandomLong()
    {
        int n = Random.Range(0, _FootSteps_long.Length);
        return _FootSteps_long[n];
    }
    public AudioClip PickRandom()
    {
        int n = Random.Range(0, _FootSteps.Length);
        return _FootSteps[n];
    }
}
