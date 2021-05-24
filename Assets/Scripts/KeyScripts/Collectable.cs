using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Collectable : MonoBehaviour
{
    /// <summary>
    /// Called when collided with "Player" tag GO
    /// </summary>
    protected abstract void OnCollect();

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.transform.tag == "Player")
        {
            OnCollect();
        }
    }
}
