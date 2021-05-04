using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioFiles : MonoBehaviour
{
    public AudioClip[] _FootSteps;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public AudioClip PickRandom()
    {
        int n = Random.Range(0, _FootSteps.Length);
        return _FootSteps[n];
    }
}
