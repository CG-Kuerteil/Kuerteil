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
    private static bool alreadyPlaerSpawned = false;
    public static GameControl instance;
    public GameObject playerPref;
    private ArraySpawner arraySpawner;
    public GameObject player;
    public AudioFiles audio;

    //false = menu closed vv
    public bool _MenuState = false;

    // Start is called before the first frame update
    void Awake()
    {
        if (instance == null)
        {
            instance = this;

        }
        else if (instance != this)
        {
            Destroy(instance);
        }

        DontDestroyOnLoad(instance);

        Cursor.lockState = CursorLockMode.Locked;
        arraySpawner = gameObject.GetComponent<ArraySpawner>();

        arraySpawner.InitializeGeneration();

        

        if (alreadyPlaerSpawned == false)
        {
            player = Instantiate(playerPref, new Vector3(-2f, 0f, -2f), Quaternion.identity);
            Debug.Log("Player Spawned!");
            alreadyPlaerSpawned = true;
        }
        //playerObj = GameObject.FindGameObjectWithTag("Player");
        DontDestroyOnLoad(player);
        player.GetComponent<FirstPersonController>().canLook = true;
        audio = GetComponent<AudioFiles>();
    }

    public void SceneWechseln(int index)
    {
        if (SceneManager.GetActiveScene().buildIndex == 1)
        {
            Save();
        }
        SceneManager.LoadScene(index);
        //Scene loaded...
        if (SceneManager.GetActiveScene().buildIndex == 1)
        {
            Load();
        }
    }

    private void Start()
    {
        //Create NavMesh
        List<GameObject> tempList = new List<GameObject>();
        tempList.AddRange(GameObject.FindGameObjectsWithTag("Ground"));
        GetComponent<NavMeshGenerator>().SetNavMeshElements(tempList);
        GetComponent<NavMeshGenerator>().BuildNavMesh();

        arraySpawner.SpawnEnemies();
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void Save()
    {

        BinaryFormatter bf = new BinaryFormatter();
        FileStream fs = File.Open(Application.persistentDataPath + "/gameData.dat", FileMode.OpenOrCreate);

        GameData gameData = new GameData();
        gameData.health = 100f;

        gameData.palyerPosition = player.transform.position;

        gameData.palyerRotation = player.transform.rotation;
        
        gameData.mainFeld = arraySpawner.GetArraySpawner().GetMainFeld();
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
            Debug.Log("LOADED...");
            fs.Close();

            //DestroyObjects("Player");
            DestroyObjects("lab");
            player = GameObject.FindGameObjectWithTag("Player");
            arraySpawner.SetMainFeld(gameData.mainFeld);
            arraySpawner.InitializeGeneration();

            //Create NavMesh
            List<GameObject> tempList = new List<GameObject>();
            tempList.AddRange(GameObject.FindGameObjectsWithTag("Ground"));
            GetComponent<NavMeshGenerator>().SetNavMeshElements(tempList);
            GetComponent<NavMeshGenerator>().BuildNavMesh();

            player.transform.position = gameData.palyerPosition;
            player.transform.position = new Vector3(player.transform.position.x, 1.2f, player.transform.position.z);
            player.transform.rotation = gameData.palyerRotation;
            //playerObj = Instantiate(player, gameData.palyerPosition, gameData.palyerRotation);
            Debug.Log(gameData.palyerPosition);
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
}

[Serializable]
class GameData
{
    public float health;

    public int[,] mainFeld;

    public SVector3 palyerPosition = new SVector3();

    public SQuaternion palyerRotation = new SQuaternion();
}

