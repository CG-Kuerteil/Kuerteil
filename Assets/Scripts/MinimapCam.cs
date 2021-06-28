using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinimapCam : MonoBehaviour
{

    private Transform _Target;
    // Start is called before the first frame update
    void Start()
    {
        _Target = GameControl.Instance.player.transform;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(_Target.position.x, transform.position.y, _Target.position.z);
    }
}
