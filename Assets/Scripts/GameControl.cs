using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using SerializableTypes;
using UnityEngine.SceneManagement;

public class GameControl : MonoBehaviour
{
    [SerializeField]
    public Vector3 PlayerSpawnPosition = new Vector3(-2, 1, -2);

    private static GameControl _instance;

    public static GameControl Instance
    {
        get
        {
            return _instance;
        }
    }


    public GameObject playerPref;
    private LabyrinthCreator arraySpawner;

    public GameObject player;
    public AudioFiles audio;

    public float _FireDamage = 1f;
    public float _HP_Pickup = 40f;

    public GameObject _Minigame_3_Controller;
    public GameObject _Minigame_2_Controller;
    public GameObject _Minigame_4_Controller;

    public bool _MenuState = false;

    public int _NumberOfTries = 3;

    public bool _GameOver { get; private set; }

    #region keyManagement

    private List<KeyType> _keyList = new List<KeyType>();

    public List<KeyType> KeyList
    {
        get
        {
            return _keyList;
        }
    }

    #endregion

    // Start is called before the first frame update
    void Awake()
    {
        //Check if Isntance is Singleton
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
            Debug.Log("DESTROYED GO!!!");
        }
        else
        {
            _instance = this;
        }

        DontDestroyOnLoad(Instance);

        Cursor.lockState = CursorLockMode.Locked;

        InitComponents();
    }

    private void InitComponents()
    {
        arraySpawner = gameObject.GetComponent<LabyrinthCreator>();

        if (GameObject.FindGameObjectWithTag("Player") == null)
        {
            player = Instantiate(playerPref, PlayerSpawnPosition, Quaternion.identity);
            //Debug.Log("Player Spawned!");
        }
        else
        {
            player = GameObject.FindGameObjectWithTag("Player");
            player.transform.position = PlayerSpawnPosition;
        }
        
        if (SceneManager.GetActiveScene().buildIndex == 1 && !_dontGenerate)
        {
            Debug.Log("----------------neu generiert...-----------------");
            arraySpawner.InitializeGeneration();
        }

        DontDestroyOnLoad(player);
        player.GetComponent<FirstPersonController>().canLook = true;
        audio = GetComponent<AudioFiles>();
    }

    private bool _dontGenerate = false;

    /// <summary>
    /// Wechselt die Scene in eine Andere
    /// </summary>
    /// <param name="index">Der Index der neuen Scene</param>
    public void SceneWechseln(int index)
    {
        if (SceneManager.GetActiveScene().buildIndex != 0 && index == 1)
        {
            _dontGenerate = true;
            SceneManager.LoadScene(index);
            GameControl.Instance.Load();
            
        }
        else if (SceneManager.GetActiveScene().buildIndex == 1 && index > 1)
        {
            GameControl.Instance.Save();
            SceneManager.LoadScene(index);
        }

        InitControllers(index);

        if (index == 1)
        {
            _GameOver = false;
        }
    }

    /// <summary>
    /// Beendet das Spiel
    /// </summary>
    public void GameOver()
    {
        Debug.Log("GameOver...");
        //Load try again...
        SceneManager.LoadScene(5);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        Destroy(GameObject.FindGameObjectWithTag("Player"));
        Destroy(Instance.gameObject);
        //Exit();
    }

    /// <summary>
    /// Beendet das Spiel
    /// </summary>
    public void Exit()
    {
        /*#if UNITY_EDITOR
                // Application.Quit() does not work in the editor so
                // UnityEditor.EditorApplication.isPlaying need to be set to false to end the game
                UnityEditor.EditorApplication.isPlaying = false;
        #else
                 Application.Quit();
        #endif*/
        SceneManager.LoadScene(0);
        _GameOver = true;
        if (SceneManager.GetActiveScene().buildIndex == 1)
        {
            //Save();
        }
        Cursor.lockState = CursorLockMode.None;

        Destroy(GameObject.FindGameObjectWithTag("Player"));
        Destroy(Instance.gameObject);
    }

    private void Start()
    {
        if (SceneManager.GetActiveScene().buildIndex == 1)
        {
            //Create NavMesh
            List<GameObject> tempList = new List<GameObject>();
            tempList.AddRange(GameObject.FindGameObjectsWithTag("Ground"));
            GetComponent<NavMeshGenerator>().SetNavMeshElements(tempList);
            GetComponent<NavMeshGenerator>().BuildNavMesh();

            arraySpawner.SpawnEnemies();
        }
        if (portalPositionen == null)
        {
            portalPositionen = new Vector3[3];
        }
    }

    private void InitControllers(int index)
    {
        if (index == 2)
        {
            var s = Instantiate(_Minigame_2_Controller, Vector3.zero, Quaternion.identity);
            s.GetComponent<JumpMinigameController>().MinigameSceneIndex = 2;
            s.GetComponent<JumpMinigameController>().NumberOfTries = _NumberOfTries;
            _NumberOfTries--;
        }
        if (index == 3)
        {
            var s = Instantiate(_Minigame_3_Controller, Vector3.zero, Quaternion.identity);
            s.GetComponent<KampfMinigameController>().MinigameSceneIndex = 3;
            s.GetComponent<KampfMinigameController>().NumberOfTries = _NumberOfTries;
            _NumberOfTries--;
        }
        if (index == 4)
        {
            var s = Instantiate(_Minigame_4_Controller, Vector3.zero, Quaternion.identity);
            s.GetComponent<RaetselMinigameController>().MinigameSceneIndex = 4;
            s.GetComponent<RaetselMinigameController>().NumberOfTries = _NumberOfTries;
            _NumberOfTries--;
        }
    }

    /// <summary>
    /// Called when Door has been openened and Game is Won
    /// </summary>
    public void GameWon()
    {
        SceneManager.LoadScene(5);
    }

    Vector3[] portalPositionen;

    /// <summary>
    /// Speichert den Spielstand ab
    /// </summary>
    public void Save()
    {

        BinaryFormatter bf = new BinaryFormatter();
        
        FileStream fs = File.Open(Application.persistentDataPath + "/gameData.dat", FileMode.OpenOrCreate);

        GameData gameData = new GameData();

        #region Saving Data

        gameData.portalPosition1 = portalPositionen[0];
        gameData.portalPosition2 = portalPositionen[1];
        gameData.portalPosition3 = portalPositionen[2];

        gameData.health = 100f;

        gameData.playerPosition = player.transform.position;

        gameData.playerRotation = player.transform.rotation;

        gameData.mainFeld = arraySpawner.MainField;

        foreach (var key in KeyList)
        {
            switch (key)
            {
                case KeyType.RedKey:
                    gameData.Schluessel[0] = true;
                    break;
                case KeyType.BlueKey:
                    gameData.Schluessel[1] = true;
                    break;
                case KeyType.GreenKey:
                    gameData.Schluessel[2] = true;
                    break;
                default:
                    break;
            }
        }
        #endregion

        bf.Serialize(fs, gameData);
        fs.Close();
    }

    /// <summary>
    /// Laedt den zuletzt ghespeicherten Spielstand
    /// </summary>
    public void Load()
    {
        if (File.Exists(Application.persistentDataPath + "/gameData.dat"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream fs = File.Open(Application.persistentDataPath + "/gameData.dat", FileMode.Open);

            GameData gameData = (GameData)bf.Deserialize(fs);
            fs.Close();

            DestroyObjects("lab");
            DestroyObjects("Ground");
            DestroyObjects("Deko");
            DestroyObjects("Enemy");

            player = GameObject.FindGameObjectWithTag("Player");
            arraySpawner.MainField = (int[,])gameData.mainFeld.Clone();

            if (portalPositionen == null)
            {
                portalPositionen = new Vector3[3];
            }
            portalPositionen[0] = gameData.portalPosition1;
            portalPositionen[1] = gameData.portalPosition2;
            portalPositionen[2] = gameData.portalPosition3;

            if (gameData.Schluessel[0] == true)
            {
                _keyList.Add(KeyType.RedKey);
            }
            if (gameData.Schluessel[1] == true)
            {
                _keyList.Add(KeyType.BlueKey);
            }
            if (gameData.Schluessel[2] == true)
            {
                _keyList.Add(KeyType.GreenKey);
            }

            arraySpawner.PortalPositionen = portalPositionen;

            arraySpawner.InitializeGeneration();

            player.transform.position = PlayerSpawnPosition;
            //player.transform.position = gameData.playerPosition;
            
            player.transform.rotation = gameData.playerRotation;
        }
        else
        {
            Debug.Log("FILE NOT FOUND...");
        }
    }

    private void DestroyObjects(string v)
    {
        GameObject[] labs = GameObject.FindGameObjectsWithTag(v);

        for (int i = 0; i < labs.Length; i++)
        {
            Destroy(labs[i]);
        }
    }

    /// <summary>
    /// Fuegt einen -aufgehobenen- Schluessel zum "Inventar" hinzu
    /// </summary>
    /// <param name="keyType"></param>
    public void addKey(KeyType keyType)
    {
        if (_keyList.Contains(keyType) == false)
        {
            _keyList.Add(keyType);
            Debug.Log("KeyCollected: "+keyType);

            GameObject.FindObjectOfType<KeyUIManager>().KeyPickedUp(keyType);

            //Todo: notify UI
        }
        else
        {
            Debug.Log("Key schon vorhanden...");
        }
    }

    /// <summary>
    /// Speichert die Positionen der Generierten Portale privat ab.
    /// </summary>
    /// <param name="positionen"></param>
    public void SavePortalPositionen(Vector3[] positionen)
    {
        if (portalPositionen == null)
        {
            portalPositionen = new Vector3[3];
        }
        portalPositionen = positionen;
    }

    /// <summary>
    /// Gibt das Array als String zurueck. Für Test Zwecke
    /// </summary>
    /// <param name="list"></param>
    /// <returns></returns>
    public static string PrintArray(Vector3[] list)
    {
        string tmp = "[";
        foreach (var item in list)
        {
            tmp += ", " + item.ToString();
        }
        return tmp + "]";
    }
}

/// <summary>
/// Eine GameData klasse wird intern benutzt, um alle relevanten Daten Konsistent im speicher zu halten.
/// </summary>
/// 
[Serializable]
class GameData
{
    public SVector3 portalPosition1 = new SVector3();
    public SVector3 portalPosition2 = new SVector3();
    public SVector3 portalPosition3 = new SVector3();

    /// <summary>
    /// [0] = redKey
    /// [1] = blueKey
    /// [2] = greeenKey
    /// </summary>
    public bool[] Schluessel = new bool[3];

    public float health;

    public int[,] mainFeld;

    public SVector3 playerPosition = new SVector3();
    
    public SQuaternion playerRotation = new SQuaternion();
}