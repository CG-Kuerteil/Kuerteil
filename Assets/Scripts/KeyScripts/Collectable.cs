using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Collectable : MonoBehaviour
{
    [SerializeField]
    protected string PlayerTag = "Player";
    /// <summary>
    /// Called when collided with "Player" tag GO
    /// </summary>
    protected abstract void OnCollect();

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.transform.tag == PlayerTag)
        {
            OnCollect();
        }
    }
}
