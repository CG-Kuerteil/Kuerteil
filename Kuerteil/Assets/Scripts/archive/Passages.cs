using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Passages : MonoBehaviour
{
    protected Dictionary<bool[,], List<Vector2>> orga;

    public Dictionary<bool[,], List<Vector2>> GetPassages()
    {
        return this.orga;
    }

    public Dictionary<bool[,], List<Vector2>> GetOrga()
    {
        return this.orga;
    }
    protected abstract void InitOrga();
}
