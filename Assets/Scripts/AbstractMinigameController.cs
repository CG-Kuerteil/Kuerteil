using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// A Singleton class which manages each minigame Scenes.
/// </summary>
public abstract class AbstractMinigameController<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T _instance;

    /// <summary>
    /// The public Isntance of this class.
    /// </summary>
    public static T Instance
    {
        get
        {
            return _instance;
        }
        protected set
        {
            _instance = value;
        }
    }

    [SerializeField]
    protected GameObject _TeleporterPref;
    
    [SerializeField]
    protected Mesh _TeleporterMeshForDebug;

    [SerializeField]
    protected GameObject _keyObject;

    [SerializeField]
    protected Mesh _keyMeshForDebug;

    private int _NumberOfTries;

    public int NumberOfTries
    {
        get
        {
            return _NumberOfTries;
        }
        set
        {
            _NumberOfTries = value;
        }
    }

    [SerializeField]
    private Vector3 _PortalSpawnLocation;

    private int _MinigameSceneIndex;

    public int MinigameSceneIndex
    {
        get
        {
            return _MinigameSceneIndex;
        }
        set
        {
            _MinigameSceneIndex = value;
        }
    }

    /// <summary>
    /// Called when the Challenge of the Minigame is completed
    /// Calls the Hook() method for additional funcionality, if needed
    /// </summary>
    public void ChallengeCompleted()
    {
        MinigameWon();
        Hook();
    }

    /// <summary>
    /// Called by ChallengeCompleted. For extra funcionality. Is opitonal.
    /// </summary>
    protected virtual void Hook() { }

    /// <summary>
    /// Isntantiates a TeleporterPrefab. Is calles When the Minigame Challenge is completed
    /// </summary>
    void MinigameWon()
    {
        var s = Instantiate(_TeleporterPref, _PortalSpawnLocation, Quaternion.identity);
        s.GetComponentInChildren<PortalScript>()._SceneToTravelTo = 1;
    }

    /// <summary>
    /// Is called when the Palyer died.
    /// Calls GameControl.GameOver() if _NumberOfTries is less than zero
    /// </summary>
    public void Died()
    {
        _NumberOfTries--;
        if (_NumberOfTries < 0)
        {
            GameControl.Instance.GameOver();
        }
        else
        {
            GameControl.Instance.SceneWechseln(_MinigameSceneIndex);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireMesh(_TeleporterMeshForDebug, _PortalSpawnLocation);

        Gizmos.color = Color.green;
        Gizmos.DrawWireMesh(_keyMeshForDebug, _PortalSpawnLocation);
    }
}
