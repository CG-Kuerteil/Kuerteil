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
    public static GameControl control;
    public Transform player;
    private ArraySpawner arraySpawner;
    private Transform playerObj;

    // Start is called before the first frame update
    void Awake()
    {
        if (control == null)
        {
            control = this;

        }
        else if (control != this)
        {
            Destroy(control);
        }

        DontDestroyOnLoad(control);

        Cursor.lockState = CursorLockMode.Locked;
        arraySpawner = gameObject.GetComponent<ArraySpawner>();

        arraySpawner.InitializeGeneration();

        

        if (alreadyPlaerSpawned == false)
        {
            playerObj = Instantiate(player, new Vector3(-2f, 0f, -2f), Quaternion.identity);
            Debug.Log("Player Spawned!");
            alreadyPlaerSpawned = true;
        }
        playerObj = GameObject.FindGameObjectWithTag("Player").transform;
        DontDestroyOnLoad(playerObj);
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
        if (SceneManager.GetActiveScene().buildIndex == 1)
        {
            if (Input.GetKeyUp(KeyCode.Tab) && playerObj.GetComponent<FirstPersonController>().canLook == true)
            {
                playerObj.GetComponent<FirstPersonController>().canLook = false;
                Cursor.lockState = CursorLockMode.None;
            }
            else if (Input.GetKeyUp(KeyCode.Tab) && playerObj.GetComponent<FirstPersonController>().canLook == false)
            {
                playerObj.GetComponent<FirstPersonController>().canLook = true;
                Cursor.lockState = CursorLockMode.Locked;
            }
        }
    }

    public void Save()
    {

        BinaryFormatter bf = new BinaryFormatter();
        FileStream fs = File.Open(Application.persistentDataPath + "/gameData.dat", FileMode.OpenOrCreate);

        GameData gameData = new GameData();
        gameData.health = 100f;

        gameData.palyerPosition = playerObj.transform.position;

        gameData.palyerRotation = playerObj.transform.rotation;
        
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
            playerObj = GameObject.FindGameObjectWithTag("Player").transform;
            arraySpawner.SetMainFeld(gameData.mainFeld);
            arraySpawner.InitializeGeneration();

            //Create NavMesh
            List<GameObject> tempList = new List<GameObject>();
            tempList.AddRange(GameObject.FindGameObjectsWithTag("Ground"));
            GetComponent<NavMeshGenerator>().SetNavMeshElements(tempList);
            GetComponent<NavMeshGenerator>().BuildNavMesh();

            playerObj.transform.position = gameData.palyerPosition;
            playerObj.transform.position = new Vector3(playerObj.transform.position.x, 1.2f, playerObj.transform.position.z);
            playerObj.transform.rotation = gameData.palyerRotation;
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

