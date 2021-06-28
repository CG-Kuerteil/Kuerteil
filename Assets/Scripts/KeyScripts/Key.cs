using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : Collectable
{
    
    public KeyType _keyType;
    protected override void OnCollect()
    {
        GameControl.Instance.addKey(_keyType);
        Destroy(gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
