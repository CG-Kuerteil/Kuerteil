using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using Random = UnityEngine.Random;

public class LabyrinthCreator : MonoBehaviour
{
    private Vector3[] _portalPositionen;

    public Vector3[] PortalPositionen
    {
        get
        {
            return _portalPositionen;
        }
        set
        {
            _portalPositionen = value;
        }
    }

    public void AddPortalPosition(Vector3[] position)
    {
        _portalPositionen = position;
    }

    [Header("Deko")]
    [SerializeField]
    private int _WallDekoAnzahl;
    [SerializeField]
    private Transform[] _WallDekoListe;
    private int _DekoRatio;
    [SerializeField]
    private int _DekoRatioMax;
    [SerializeField]
    private Transform[] _DekoListe;

    [Space]
    [Header("Portale")]
    [SerializeField]
    private GameObject[] _Portale;

    [Space]
    [Header("General Settings")]
    public int dimension = 21;
    public int _Iterations = 20;
    public int gangWahrscheinlichkeit = 5;
    private static string filepath = "CSVData.csv";
    public Transform gangSmall;
    public Transform hub1;
    public Transform hub2;
    public Transform hub3;
    public Transform hub4;

    public Transform minispiel_portal;
    [SerializeField]
    
    public int anzahlMinispiele;
    public Transform _Light;
    public int anzahlLights = 20;

    [Space]
    [Header("Gegner")]
    public GameObject _EnemyOne;
    public int _AnzahlGegner;
    
    private int[,] mainFeld;

    private int mitte;

    private float offsetLengthNormal = 4f;

    public int[,] MainField
    {
        get
        {
            return mainFeld;
        }
        set
        {
            mainFeld = value;
        }
    }

    [Space]
    [Header("Door Settings")]
    public Transform Door;


    private void Awake()
    {
        if (_portalPositionen == null)
        {
            _portalPositionen = new Vector3[3];
        }
    }

    /// <summary>
    /// Spawns Wall dekos on WallSocket obj
    /// Deletes all Wall Socket objs in the Scene in the end
    /// </summary>
    public void InitializeGeneration()
    {
        _DekoRatio = _DekoRatioMax;
        if (mainFeld == null)
        {
            mainFeld = new int[dimension, dimension];
            mitte = dimension / 2;

            for (int i = 1; i < dimension; i++)
            {
                for (int j = 1; j < dimension; j++)
                {
                    mainFeld[i, j] = 0;
                    if (i == mitte && j == mitte)
                    {
                        mainFeld[i, j] = 1;
                    }
                }
            }
            InitSnake();
        }
        else
        {
            //Wenn die Labyrinth Matrix manuell gesetzt wurde
            mitte = mainFeld.GetLength(0) / 2;
        }
        PrintCSV(mainFeld);
        PrintArray(mainFeld);
        SpawnElements();
        SpawnPortals();
        SpawnLights();
        SpawnWallDeko();
        SpawnDoor();

        //Add PortalPositions to Saving queue
        GameControl.Instance.SavePortalPositionen(_portalPositionen);
    }

    private void SpawnDoor()
    {
        GameObject[] sockelList = GameObject.FindGameObjectsWithTag("WandSockel");
        int r = Random.Range(0, sockelList.Length);

        Vector3 tmp = new Vector3(sockelList[r].transform.position.x, 0, sockelList[r].transform.position.z);
        Instantiate(Door, tmp, sockelList[r].transform.rotation);
        Destroy(sockelList[r]);
    }

    public void SpawnWallDeko()
    {
        GameObject[] sockelList = GameObject.FindGameObjectsWithTag("WandSockel");
        int count = _WallDekoAnzahl;
        int rand;
        int sockel;

        while (count > 0)
        {
            sockel = Random.Range(0, sockelList.Length);
            rand = Random.Range(0, _WallDekoListe.Length);
            Vector3 tmp = new Vector3(sockelList[sockel].transform.position.x, 0, sockelList[sockel].transform.position.z);
            Instantiate(_WallDekoListe[rand], tmp, sockelList[sockel].transform.rotation);
            count--;
            Destroy(sockelList[sockel]);
        }
    }

    public void SpawnEnemies()
    {
        int n = _AnzahlGegner;
        int i, j;
        while (n > 0)
        {
            i = Random.Range(0, (dimension - 1));
            j = Random.Range(0, (dimension - 1));
            if (mainFeld[i, j] == 1)
            {
                Instantiate(_EnemyOne, new Vector3((i * offsetLengthNormal) - ((dimension * offsetLengthNormal) / 2), 2, (j * offsetLengthNormal) - ((dimension * offsetLengthNormal) / 2)), Quaternion.identity);
            }
            else
            {
                continue;
            }
            n--;
        }
    }

    public void SpawnDeko(int i, int j, float r)
    {
        int rand = Random.Range(0, _DekoListe.Length);

        Instantiate(_DekoListe[rand], new Vector3((i * offsetLengthNormal) - ((dimension * offsetLengthNormal) / 2), 0, (j * offsetLengthNormal) - ((dimension * offsetLengthNormal) / 2)), Quaternion.Euler(new Vector3(0, r, 0)));
    }

    private void SpawnLights()
    {
        int s = anzahlLights;
        int i, j;
        while (s > 0)
        {
            i = Random.Range(0, (dimension - 1));
            j = Random.Range(0, (dimension - 1));
            if (mainFeld[i, j] == 1)
            {
                Instantiate(_Light, new Vector3((i * offsetLengthNormal) - ((dimension * offsetLengthNormal) / 2), 2, (j * offsetLengthNormal) - ((dimension * offsetLengthNormal) / 2)), Quaternion.identity);
            }
            else
            {
                continue;
            }
            s--;
        }
    }

    private void SpawnPortals()
    {
        if (_portalPositionen == null)
        {
            _portalPositionen = new Vector3[3];
        }

        GameObject[] sockelList = GameObject.FindGameObjectsWithTag("WandSockel");

        if (_portalPositionen[0] != Vector3.zero)
        {
            for (int j = 0; j < _portalPositionen.Length; j++)
            {
                Vector3 tmp = new Vector3(_portalPositionen[j].x, 0, _portalPositionen[j].z);

                for (int k = 0; k < sockelList.Length; k++)
                {
                    if (_portalPositionen[j] == sockelList[k].transform.position)
                    {
                        Instantiate(_Portale[j], tmp, sockelList[k].transform.rotation);
                    }
                }
            }
            GameObject[] sockelListe = GameObject.FindGameObjectsWithTag("WandSockel");
            for (int k = 0; k < sockelListe.Length; k++)
            {
                for (int l = 0; l < _portalPositionen.Length; l++)
                {
                    if (_portalPositionen[l].x == sockelListe[k].transform.position.x && _portalPositionen[l].z == sockelListe[k].transform.position.z)
                    {
                        Destroy(sockelListe[k]);
                    }

                }
            }
            return;
        }
        //TODO: speichere Sockelpositionen beim generieren und vor dem löschen des jeweiligen sockels.
        int s = 0;

        sockelList = GameObject.FindGameObjectsWithTag("WandSockel");

        int sockel;

        while (s < _Portale.Length)
        {
            sockel = Random.Range(0, sockelList.Length);

            _portalPositionen[s] = (sockelList[sockel].transform.position);
            Vector3 tmp = new Vector3(sockelList[sockel].transform.position.x, 0, sockelList[sockel].transform.position.z);

            Instantiate(_Portale[s], tmp, sockelList[sockel].transform.rotation);
            Destroy(sockelList[sockel]);
            s++;
        }
    }

    /// <summary>
    /// Main Algorithm ti contruct the 2D Matrix represeting the Labyrinth
    /// </summary>
    private void InitSnake()
    {
        Vector3Int direction = Vector3Int.left;
        Vector3Int newDirection = Vector3Int.zero;
        int iterations = _Iterations;
        int i = mitte;
        int j = mitte;
        bool alreadyTurned = true;
        
        while (iterations > 0)
        {
            int rand = Random.Range(0, 11);

            //\\+-----+-----+-----+[Randbedingungen]+-----+-----+-----+//\\
            #region linke kante
            if (j == 0 && i > 0 && i < dimension-1 && direction == Vector3Int.back)
            {
                rand = Random.Range(0, 2);
                if (rand == 0)
                {
                    direction = CalculateAngle(direction, -90f);
                }
                else
                {
                    direction = CalculateAngle(direction, 90f);
                }
                alreadyTurned = true;
            }
            #endregion linke kante

            #region obere kante
            if (i == 0 && j > 0 && j < dimension - 1 && direction == Vector3Int.left)
            {
                rand = Random.Range(0, 2);
                if (rand == 0)
                {
                    direction = CalculateAngle(direction, -90f);
                }
                else
                {
                    direction = CalculateAngle(direction, 90f);
                }
                alreadyTurned = true;
            }
            #endregion obere kante

            #region rechte kante
            if (j == dimension-1 && i > 0 && i < dimension - 1 && direction == Vector3Int.forward)
            {
                rand = Random.Range(0, 2);
                if (rand == 0)
                {
                    direction = CalculateAngle(direction, -90f);
                }
                else
                {
                    direction = CalculateAngle(direction, 90f);
                }
                alreadyTurned = true;
            }
            #endregion rechte kante

            #region untere kante
            if (i == dimension - 1 && j > 0 && j < dimension - 1 && direction == Vector3Int.right)
            {
                rand = Random.Range(0, 2);
                if (rand == 0)
                {
                    direction = CalculateAngle(direction, -90f);
                }
                else
                {
                    direction = CalculateAngle(direction, 90f);
                }
                alreadyTurned = true;
            }
            #endregion untere kante

            #region obere linke ecke
            if (i == 0 && j == 0 && (direction == Vector3Int.left || direction == Vector3Int.back))
            {
                if (direction == Vector3Int.left)
                {
                    direction = new Vector3Int(0, 0, 1);
                }
                else if (direction == Vector3Int.back)
                {
                    direction = new Vector3Int(1, 0, 0);
                }
                alreadyTurned = true;
            }
            #endregion obere linke ecke

            #region untere linke ecke
            if (i == dimension-1 && j == 0 && (direction == Vector3Int.right || direction == Vector3Int.back))
            {
                if (direction == Vector3Int.right)
                {
                    direction = new Vector3Int(0, 0, 1);
                }
                else if (direction == Vector3Int.back)
                {
                    direction = new Vector3Int(-1, 0, 0);
                }
                alreadyTurned = true;
            }
            #endregion untere linke ecke

            #region untere rechte ecke
            if (i == dimension-1 && j == dimension-1 && (direction == Vector3Int.right || direction == Vector3Int.forward))
            {
                if (direction == Vector3Int.right)
                {
                    direction = new Vector3Int(0, 0, -1);
                }
                else if (direction == Vector3Int.forward)
                {
                    direction = new Vector3Int(-1, 0, 0);
                }
                alreadyTurned = true;
            }
            #endregion untere rechte ecke

            #region obere rechte ecke
            if (i == 0 && j == dimension - 1 && (direction == Vector3Int.left || direction == Vector3Int.forward))
            {
                if (direction == Vector3Int.left)
                {
                    direction = new Vector3Int(0, 0, -1);
                }
                else if (direction == Vector3Int.forward)
                {
                    direction = new Vector3Int(1, 0, 0);
                }
                alreadyTurned = true;
            }
            #endregion obere rechte ecke
            //\\+-----+-----+-----+[Randbedingungen]+-----+-----+-----+//\\

            if (rand >= 0 && rand < gangWahrscheinlichkeit)
            {
                mainFeld[i, j] = 1;

                Vector3Int temp = new Vector3Int(i, 0, j);
                temp = temp + direction;

                i = temp.x;
                j = temp.z;

                //PrintArray(mainFeld);
                alreadyTurned = false;
                iterations--;
            }
            else
            {
                if (alreadyTurned == false)
                {
                    rand = Random.Range(0, 2);
                    if (rand == 0)
                    {
                        //right
                        direction = CalculateAngle(direction, -90f);
                    }
                    else
                    {
                        //left
                        direction = CalculateAngle(direction, 90f);
                    }
                    alreadyTurned = true;
                }
            }
        }
    }

    Vector3Int GenerateNewDirection(Vector3Int currentDirection, int chance)
    {
        Vector3Int newDirection = Vector3Int.zero;
        int rand = Random.Range(0, 11);

        if (rand >= 0 && rand < chance)
        {
            newDirection = currentDirection;
        }
        else
        {
            rand = Random.Range(0, 2);
            if (rand == 0)
            {
                //right
                newDirection = CalculateAngle(currentDirection, -90f);
            }
            else
            {
                //left
                newDirection = CalculateAngle(currentDirection, 90f);
            }
        }
        return newDirection;
    }

    /// <summary>
    /// Rotates "oldAngle" "angle"s degress around Y-Axis
    /// </summary>
    /// <param name="oldAngle"></param>
    /// <param name="angle"></param>
    /// <returns></returns>
    Vector3Int CalculateAngle(Vector3Int oldAngle, float angle)
    {
        float angleInRad = angle * Mathf.Deg2Rad;
        int xTemp = (int)((oldAngle.x * Mathf.Cos(angleInRad)) - (oldAngle.z * Mathf.Sin(angleInRad)));
        int zTemp = (int)((oldAngle.x * Mathf.Sin(angleInRad)) + (oldAngle.z * Mathf.Cos(angleInRad)));
        Vector3Int rotation = new Vector3Int(xTemp, 0, zTemp);

        return Vector3Int.FloorToInt(rotation);
    }

    /// <summary>
    /// Spawnes the Transform t in given Y-Rotation at point (i,0,j). Where the position is relative to the Filedmatrix
    /// </summary>
    /// <param name="t"></param>
    /// <param name="i"></param>
    /// <param name="j"></param>
    /// <param name="r"></param>
    private void SpawnElement(Transform t, int i, int j, float r)
    {
        Instantiate(t, new Vector3((i * offsetLengthNormal) - ((dimension * offsetLengthNormal) / 2), 0, (j * offsetLengthNormal) - ((dimension * offsetLengthNormal) / 2)), Quaternion.Euler(new Vector3(0, r, 0)));
        _DekoRatio--;
        if (_DekoRatio < 0)
        {
            _DekoRatio = _DekoRatioMax;
            SpawnDeko(i, j, r);
        }
    }

    private void SpawnElements()
    {
        for (int i = 0; i < dimension; i++)
        {
            for (int j = 0; j < dimension; j++)
            {
                #region rand
                if (i == dimension -1 || i == 0 || j == dimension - 1 || j == 0)
                {
                    #region links
                    //links
                    if (mainFeld[i, j] == 1 && j == 0 && i > 0 && i < dimension-1)
                    {
                        if (mainFeld[i + 1, j] == 1 && mainFeld[i, j + 1] == 1 && mainFeld[i - 1, j] == 1)
                        {
                            SpawnElement(hub3, i, j, 0);
                        }
                        else if (mainFeld[i + 1, j] == 1 && mainFeld[i, j + 1] == 1 && mainFeld[i - 1, j] == 0)
                        {
                            SpawnElement(hub2, i, j, 90f);
                        }
                        else if (mainFeld[i + 1, j] == 0 && mainFeld[i, j + 1] == 1 && mainFeld[i - 1, j] == 1)
                        {
                            SpawnElement(hub2, i, j, 0);
                        }
                        else if (mainFeld[i + 1, j] == 1 && mainFeld[i, j + 1] == 0 && mainFeld[i - 1, j] == 1)
                        {
                            SpawnElement(gangSmall, i, j, 0);
                        }
                        else if (mainFeld[i -1, j] == 0 && mainFeld[i, j + 1] == 0 && mainFeld[i + 1, j] == 1)
                        {
                            SpawnElement(hub1, i, j, 180);
                        }
                        else if (mainFeld[i - 1, j] == 1 && mainFeld[i, j + 1] == 0 && mainFeld[i + 1, j] == 0)
                        {
                            SpawnElement(hub1, i, j, 0);
                        }
                    }
                    #endregion

                    #region rechts
                    //rechts
                    else if (mainFeld[i, j] == 1 && j == dimension-1 && i > 0 && i < dimension - 1)
                    {
                        if (mainFeld[i + 1, j] == 1 && mainFeld[i, j - 1] == 1 && mainFeld[i - 1, j] == 1)
                        {
                            SpawnElement(hub3, i, j, 180);
                        }
                        else if (mainFeld[i + 1, j] == 1 && mainFeld[i, j - 1] == 1 && mainFeld[i - 1, j] == 0)
                        {
                            SpawnElement(hub2, i, j, 180);
                        }
                        else if (mainFeld[i + 1, j] == 0 && mainFeld[i, j - 1] == 1 && mainFeld[i - 1, j] == 1)
                        {
                            SpawnElement(hub2, i, j, -90);
                        }
                        else if (mainFeld[i + 1, j] == 1 && mainFeld[i, j - 1] == 0 && mainFeld[i - 1, j] == 1)
                        {
                            SpawnElement(gangSmall, i, j, 0);
                        }
                        else if (mainFeld[i - 1, j] == 0 && mainFeld[i, j - 1] == 0 && mainFeld[i + 1, j] == 1)
                        {
                            SpawnElement(hub1, i, j, 180);
                        }
                        else if (mainFeld[i - 1, j] == 1 && mainFeld[i, j - 1] == 0 && mainFeld[i + 1, j] == 0)
                        {
                            SpawnElement(hub1, i, j, 0);
                        }
                    }
                    #endregion

                    #region oben
                    //oben
                    else if (mainFeld[i, j] == 1 && i == 0 && j > 0 && j < dimension - 1)
                    {
                        if (mainFeld[i + 1, j] == 1 && mainFeld[i, j - 1] == 1 && mainFeld[i, j + 1] == 1)
                        {
                            SpawnElement(hub3, i, j, 90);
                        }
                        else if (mainFeld[i + 1, j] == 1 && mainFeld[i, j - 1] == 1 && mainFeld[i, j + 1] == 0)
                        {
                            SpawnElement(hub2, i, j, 180);
                        }
                        else if (mainFeld[i + 1, j] == 1 && mainFeld[i, j - 1] == 0 && mainFeld[i, j + 1] == 1)
                        {
                            SpawnElement(hub2, i, j, 90);
                        }
                        else if (mainFeld[i + 1, j] == 0 && mainFeld[i, j - 1] == 1 && mainFeld[i, j + 1] == 1)
                        {
                            SpawnElement(gangSmall, i, j, 90);
                        }
                        else if (mainFeld[i, j + 1] == 1 && mainFeld[i, j - 1] == 0 && mainFeld[i + 1, j] == 0)
                        {
                            SpawnElement(hub1, i, j, 90);
                        }
                        else if (mainFeld[i, j + 1] == 0 && mainFeld[i, j - 1] == 1 && mainFeld[i + 1, j] == 0)
                        {
                            SpawnElement(hub1, i, j, -90);
                        }
                    }
                    #endregion

                    #region unten
                    //uten
                    else if (mainFeld[i, j] == 1 && i == dimension-1 && j > 0 && j < dimension - 1)
                    {
                        if (mainFeld[i - 1, j] == 1 && mainFeld[i, j - 1] == 1 && mainFeld[i, j + 1] == 1)
                        {
                            SpawnElement(hub3, i, j, -90);
                        }
                        else if (mainFeld[i - 1, j] == 1 && mainFeld[i, j - 1] == 1 && mainFeld[i, j + 1] == 0)
                        {
                            SpawnElement(hub2, i, j,-90);
                        }
                        else if (mainFeld[i - 1, j] == 1 && mainFeld[i, j - 1] == 0 && mainFeld[i, j + 1] == 1)
                        {
                            SpawnElement(hub2, i, j, 0);
                        }
                        else if (mainFeld[i - 1, j] == 0 && mainFeld[i, j - 1] == 1 && mainFeld[i, j + 1] == 1)
                        {
                            SpawnElement(gangSmall, i, j, 90);
                        }
                        else if (mainFeld[i, j + 1] == 1 && mainFeld[i, j - 1] == 0 && mainFeld[i - 1, j] == 0)
                        {
                            SpawnElement(hub1, i, j, 90);
                        }
                        else if (mainFeld[i, j + 1] == 0 && mainFeld[i, j - 1] == 1 && mainFeld[i - 1, j] == 0)
                        {
                            SpawnElement(hub1, i, j, -90);
                        }
                    }
                    #endregion

                    #region ecken
                    //Ecke, links oben
                    if (mainFeld[i, j] == 1 && i == 0 && j == 0)
                    {
                        if (mainFeld[i, j+1] == 1 && mainFeld[i+1, j] == 1)
                        {
                            SpawnElement(hub2, i, j, 90);
                        }
                    }
                    //Ecke, links unten
                    else if (mainFeld[i, j] == 1 && i == dimension-1 && j == 0)
                    {
                        if (mainFeld[i-1, j] == 1 && mainFeld[i, j+1] == 1)
                        {
                            SpawnElement(hub2, i, j, 0);
                        }
                    }
                    //Ecke, rechts oben
                    else if (mainFeld[i, j] == 1 && i == 0 && j == dimension-1)
                    {
                        if (mainFeld[i + 1, j] == 1 && mainFeld[i, j - 1] == 1)
                        {
                            SpawnElement(hub2, i, j, 180);
                        }
                    }
                    //Ecke, rechts unten
                    else if (mainFeld[i, j] == 1 && i == dimension-1 && j == dimension - 1)
                    {
                        if (mainFeld[i - 1, j] == 1 && mainFeld[i, j - 1] == 1)
                        {
                            SpawnElement(hub2, i, j, -90);
                        }
                    }
                    #endregion

                    continue;
                }
                #endregion

                #region 1er ecke
                //1er ecke spawnen
                if (mainFeld[i, j] == 1)
                {
                    if (mainFeld[i + 1, j] == 0 && mainFeld[i, j + 1] == 0 && mainFeld[i - 1, j] == 0 && mainFeld[i, j - 1] == 1)
                    {
                        Instantiate(hub1, new Vector3((i*offsetLengthNormal) - ((dimension * offsetLengthNormal) / 2), 0, (j*offsetLengthNormal) - ((dimension * offsetLengthNormal) / 2)), Quaternion.Euler(new Vector3(0f, -90f, 0f)));
                    }
                    else if (mainFeld[i + 1, j] == 0 && mainFeld[i, j + 1] == 1 && mainFeld[i - 1, j] == 0 && mainFeld[i, j - 1] == 0)
                    {
                        Instantiate(hub1, new Vector3((i * offsetLengthNormal) - ((dimension * offsetLengthNormal) / 2), 0, (j * offsetLengthNormal) - ((dimension * offsetLengthNormal) / 2)), Quaternion.Euler(new Vector3(0f, 90f, 0f)));
                    }
                    else if (mainFeld[i + 1, j] == 1 && mainFeld[i, j + 1] == 0 && mainFeld[i - 1, j] == 0 && mainFeld[i, j - 1] == 0)
                    {
                        Instantiate(hub1, new Vector3((i * offsetLengthNormal) - ((dimension * offsetLengthNormal) / 2), 0, (j * offsetLengthNormal) - ((dimension * offsetLengthNormal) / 2)), Quaternion.Euler(new Vector3(0f, 180f, 0f)));
                    }
                    else if (mainFeld[i + 1, j] == 0 && mainFeld[i, j + 1] == 0 && mainFeld[i - 1, j] == 1 && mainFeld[i, j - 1] == 0)
                    {
                        Instantiate(hub1, new Vector3((i * offsetLengthNormal) - ((dimension * offsetLengthNormal) / 2), 0, (j * offsetLengthNormal) - ((dimension * offsetLengthNormal) / 2)), Quaternion.identity);
                    }
                }

                //2er Ecke spawnen
                if (mainFeld[i, j] == 1)
                {
                    if (mainFeld[i + 1, j] == 1 && mainFeld[i, j + 1] == 1 && mainFeld[i - 1, j] == 0 && mainFeld[i, j - 1] == 0)
                    {
                        SpawnElement(hub2, i, j, 90);
                    }
                    else if (mainFeld[i + 1, j] == 1 && mainFeld[i, j + 1] == 0 && mainFeld[i - 1, j] == 0 && mainFeld[i, j - 1] == 1)
                    {
                        SpawnElement(hub2, i, j, 180);
                    }
                    else if (mainFeld[i + 1, j] == 0 && mainFeld[i, j + 1] == 1 && mainFeld[i - 1, j] == 1 && mainFeld[i, j - 1] == 0)
                    {
                        //richtig
                        SpawnElement(hub2, i, j, 0);
                    }
                    else if (mainFeld[i + 1, j] == 0 && mainFeld[i, j + 1] == 0 && mainFeld[i - 1, j] == 1 && mainFeld[i, j - 1] == 1)
                    {
                        SpawnElement(hub2, i, j, -90);
                    }
                }
                #endregion

                #region gang
                //gang spawnen
                if (mainFeld[i, j] == 1)
                {
                    if (mainFeld[i + 1, j] == 1 && mainFeld[i, j + 1] == 0 && mainFeld[i - 1, j] == 1 && mainFeld[i, j - 1] == 0)
                    {
                        SpawnElement(gangSmall, i, j, 0);
                    }
                    else if (mainFeld[i + 1, j] == 0 && mainFeld[i, j + 1] == 1 && mainFeld[i - 1, j] == 0 && mainFeld[i, j - 1] == 1)
                    {
                        SpawnElement(gangSmall, i, j, 90);
                    }
                }
                #endregion

                #region 3er ecke
                //3er ecke spawnen
                if (mainFeld[i, j] == 1)
                {
                    if (mainFeld[i + 1, j] == 1 && mainFeld[i, j + 1] == 1 && mainFeld[i - 1, j] == 0 && mainFeld[i, j - 1] == 1)
                    {
                        SpawnElement(hub3, i, j, 90);
                    }
                    else if (mainFeld[i + 1, j] == 1 && mainFeld[i, j + 1] == 0 && mainFeld[i - 1, j] == 1 && mainFeld[i, j - 1] == 1)
                    {
                        SpawnElement(hub3, i, j, 180);
                    }
                    else if (mainFeld[i + 1, j] == 0 && mainFeld[i, j + 1] == 1 && mainFeld[i - 1, j] == 1 && mainFeld[i, j - 1] == 1)
                    {
                        //richtig
                        SpawnElement(hub3, i, j, -90);
                    }
                    else if (mainFeld[i + 1, j] == 1 && mainFeld[i, j + 1] == 1 && mainFeld[i - 1, j] == 1 && mainFeld[i, j - 1] == 0)
                    {
                        SpawnElement(hub3, i, j, 0);
                    }
                }
                #endregion

                #region 4er Ecke
                //4er Ecke(=4hub) einfuegen
                if (mainFeld[i, j] == 1)
                {
                    if (mainFeld[i + 1, j] == 1 && mainFeld[i, j + 1] == 1 && mainFeld[i - 1, j] == 1 && mainFeld[i, j - 1] == 1)
                    {
                        SpawnElement(hub4, i, j, 0);
                    }
                }
                #endregion
            }
        }
    }

    public static void PrintArray(int[,] m)
    {
        StringBuilder sb = new StringBuilder();
        for (int i = 0; i < m.GetLength(1); i++)
        {
            for (int j = 0; j < m.GetLength(0); j++)
            {
                sb.Append(m[i, j]);
                sb.Append(' ');
            }
            sb.AppendLine();
        }
        //Debug.Log(sb.ToString());
    }

    private static void PrintCSV(int[,] m)
    {
        StreamWriter writer = new StreamWriter(filepath);

        StringBuilder sb = new StringBuilder();
        writer.WriteLine("sep=,");
        for (int i = 0; i < m.GetLength(1); i++)
        {
            for (int j = 0; j < m.GetLength(0); j++)
            {
                if (j != 0)
                {
                    sb.Append(",");
                }
                sb.Append(Convert.ToString(m[i, j]));
            }
            writer.WriteLine(sb.ToString());
            sb = new StringBuilder();
        }
        writer.Close();
    }
}
