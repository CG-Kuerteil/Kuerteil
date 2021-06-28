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

    //Scene 3 - Kampf
    public float _FireDamage = 1f;
    public float _HP_Pickup = 40f;

    public GameObject _Minigame_3_Controller;
    public GameObject _Minigame_2_Controller;
    public GameObject _Minigame_4_Controller;

    //false = menu closed vv
    public bool _MenuState = false;

    public int _NumberOfTries = 3;

    public bool _GameOver { get; private set; }

    #region keyManagement

    private List<KeyType> _keyList = new List<KeyType>();

    #endregion

    // Start is called before the first frame update
    void Awake()
    {
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
        
        if (SceneManager.GetActiveScene().buildIndex == 1)
        {
            arraySpawner.InitializeGeneration();
        }

        DontDestroyOnLoad(player);
        player.GetComponent<FirstPersonController>().canLook = true;
        audio = GetComponent<AudioFiles>();
    }

    public void SceneWechseln(int index)
    {
        if (SceneManager.GetActiveScene().buildIndex != 0 && index == 1)
        {
            GameControl.Instance.Load();
            Debug.Log("Scene Loading...");
        }
        if (SceneManager.GetActiveScene().buildIndex > 1 && index == 1)
        {
            GameControl.Instance.Load();
            Debug.Log("Scene Loading...");
        }
        if (SceneManager.GetActiveScene().buildIndex == 1 && index > 1)
        {
            GameControl.Instance.Save();
            Debug.Log("Saving Scene...");
        }

        SceneManager.LoadScene(index);
        //Debug.Log("Scene Loading...");

        InitControllers(index);
        if (index == 1)
        {
            _GameOver = false;
            //Destroy(gameObject);
        }
    }

    public void GameOver()
    {
        Debug.Log("GameOver...");
        Exit();
    }
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
            Instantiate(_Minigame_2_Controller, Vector3.zero, Quaternion.identity);
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
            s.GetComponent<RaetselMinigameController>().MinigameSceneIndex = 3;
            s.GetComponent<RaetselMinigameController>().NumberOfTries = _NumberOfTries;
            _NumberOfTries--;
        }
    }

    Vector3[] portalPositionen;

    public void Save()
    {

        BinaryFormatter bf = new BinaryFormatter();
        
        FileStream fs = File.Open(Application.persistentDataPath + "/gameData.dat", FileMode.OpenOrCreate);

        GameData gameData = new GameData();

        #region Saving Data

        gameData.portalPosition1 = portalPositionen[0];
        gameData.portalPosition2 = portalPositionen[1];
        gameData.portalPosition3 = portalPositionen[2];

        Debug.Log(gameObject.GetInstanceID() + " GC:\tGespeicherte Position 1: " + gameData.portalPosition1);
        gameData.health = 100f;

        gameData.playerPosition = player.transform.position;

        gameData.playerRotation = player.transform.rotation;

        gameData.mainFeld = arraySpawner.MainField;
        #endregion

        //Debug.Log("Save: mainField length: " + gameData.mainFeld.GetLength(0));


        bf.Serialize(fs, gameData);
        fs.Close();

        Debug.Log("SAVED...");
    }

    public void Load()
    {
        if (File.Exists(Application.persistentDataPath + "/gameData.dat"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream fs = File.Open(Application.persistentDataPath + "/gameData.dat", FileMode.Open);

            GameData gameData = (GameData)bf.Deserialize(fs);
            fs.Close();

            //DestroyObjects("Player");
            DestroyObjects("lab");
            DestroyObjects("Ground");
            DestroyObjects("Deko");
            DestroyObjects("Enemy");

            player = GameObject.FindGameObjectWithTag("Player");
            arraySpawner.MainField = (int[,])gameData.mainFeld.Clone();
            //Debug.Log("Load: mainField length: " + gameData.mainFeld.GetLength(0));
            //Debug.Log("Laod->Creator: mainField length: " + arraySpawner.MainField.GetLength(0));
            //Debug.Log("Laod->Dimension: " + arraySpawner.dimension);

            if (portalPositionen == null)
            {
                portalPositionen = new Vector3[3];
            }
            portalPositionen[0] = gameData.portalPosition1;
            portalPositionen[1] = gameData.portalPosition2;
            portalPositionen[2] = gameData.portalPosition3;

            Debug.Log("GC:\tGeladene Positionen: " + PrintArray(portalPositionen));
            arraySpawner.PortalPositionen = portalPositionen;

            arraySpawner.InitializeGeneration();

            //Create NavMesh
            //GetComponent<NavMeshGenerator>().BuildNavRoot();
            //List<GameObject> tempList = new List<GameObject>();
            //tempList.AddRange(GameObject.FindGameObjectsWithTag("Ground"));
            //GetComponent<NavMeshGenerator>().SetNavMeshElements(tempList);
            //GetComponent<NavMeshGenerator>().BuildNavMesh();


            player.transform.position = gameData.playerPosition;
            
            player.transform.rotation = gameData.playerRotation;

            //playerObj = Instantiate(player, gameData.palyerPosition, gameData.palyerRotation);

            Debug.Log(gameData.playerPosition);
            Debug.Log("LOADED...");
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

    public void addKey(KeyType keyType)
    {
        if (_keyList.Contains(keyType) == false)
        {
            _keyList.Add(keyType);
            Debug.Log("KeyCollected: "+keyType);
            //Todo: notify UI
        }
        else
        {
            Debug.Log("Key schon vorhanden...");
        }
    }

    enum Objects
    {
        Deko1, Deko2, Deko3, Portal1, Portal2, Portal3
    }



    public void SavePortalPositionen(Vector3[] positionen)
    {
        Debug.Log("GC:\tList to Save: " + PrintArray(positionen));

        if (portalPositionen == null)
        {
            Debug.Log("GC:\tHave to create new List<>");
            portalPositionen = new Vector3[3];
        }
        portalPositionen = positionen;

        Debug.Log(gameObject.GetInstanceID() + " GC:\tList that was Saved: " + PrintArray(portalPositionen));
        StartCoroutine(Printer());
    }

    IEnumerator Printer()
    {
        Debug.Log("Coroutine: printing Array:: " + PrintArray(GameControl.Instance.portalPositionen));
        while (true)
        {
            yield return 2f;
            Debug.Log("Coroutine: printing Array: " + PrintArray(GameControl.Instance.portalPositionen));
        }
    }

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


[Serializable]
class GameData
{
    public SVector3 portalPosition1 = new SVector3();
    public SVector3 portalPosition2 = new SVector3();
    public SVector3 portalPosition3 = new SVector3();

    public float health;

    public int[,] mainFeld;

    public SVector3 playerPosition = new SVector3();
    

    public SQuaternion playerRotation = new SQuaternion();

    public string GetString()
    {
        return "Hello";
    }
}