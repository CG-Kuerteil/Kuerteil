using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using Random = UnityEngine.Random;

public class ArraySpawner : MonoBehaviour
{
    private static ArraySpawner arraySpawner;

    public ArraySpawner GetArraySpawner()
    {
        return arraySpawner;
    }


    public Transform gangSmall;
    public Transform hub1;
    public Transform hub2;
    public Transform hub3;
    public Transform hub4;

    public Transform minispiel_portal;
    //public Transform miniSpiel;



    public int dimension = 21;
    public int anzahlMinispiele;
    private int[,] mainFeld;
    public int iterations = 20;

    public int gangWahrscheinlichkeit = 5;
    private int mitte;
    public string filepath;

    //Laenge vom 1x2 Gang
    private float offsetLengthLong = 20f;
    //Laenge von 1x1 Elementen
    private float offsetLengthNormal = 4f;

    private void Awake()
    {
        if (arraySpawner == null)
        {
            arraySpawner = this;
        }
        else if(arraySpawner != this)
        {
            Destroy(arraySpawner);
        }
    }
    public int[,] GetMainFeld()
    {
        return this.mainFeld;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }
    public void InitializeGeneration()
    {
        filepath = "CSVData.csv";
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
            mitte = mainFeld.Length / 2;
        }
        SpawnPortals();
        PrintCSV(mainFeld);
        PrintArray(mainFeld);
        SpawnElements();
    }

    private void SpawnPortals()
    {
        int i, j;
        while (anzahlMinispiele > 0)
        {
            i = Random.Range(0, (dimension - 1));
            j = Random.Range(0, (dimension - 1));
            if (mainFeld[i, j] == 1)
            {
                Instantiate(minispiel_portal, new Vector3((i * offsetLengthNormal) - ((dimension * offsetLengthNormal) / 2), 0, (j * offsetLengthNormal) - ((dimension * offsetLengthNormal) / 2)), Quaternion.identity);
            }
            else
            {
                continue;
            }
            anzahlMinispiele--;
        }
    }
    internal void SetMainFeld(int[,] mainFeld)
    {
        this.mainFeld = mainFeld;
    }

    private void InitSnake()
    {
        Vector3Int direction = Vector3Int.left;
        Vector3Int newDirection = Vector3Int.zero;

        int i = mitte;
        int j = mitte;
        bool alreadyTurned = true;
        
        while (iterations > 0)
        {
            int rand = Random.Range(0, 11);

            //\\+-----+-----+-----+[Randbedingungen]+-----+-----+-----+//\\
            //linke kante
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
            //obere kante
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
            //rechte kante
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
            //untere kante
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
            //obere linke ecke
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
            //untere linke ecke
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
            //untere rechte ecke
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
            //obere rechte ecke
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

    Vector3Int CalculateAngle(Vector3Int oldAngle, float angle)
    {
        float angleInRad = angle * Mathf.Deg2Rad;
        int xTemp = (int)((oldAngle.x * Mathf.Cos(angleInRad)) - (oldAngle.z * Mathf.Sin(angleInRad)));
        int zTemp = (int)((oldAngle.x * Mathf.Sin(angleInRad)) + (oldAngle.z * Mathf.Cos(angleInRad)));
        Vector3Int rotation = new Vector3Int(xTemp, 0, zTemp);

        return Vector3Int.FloorToInt(rotation);
    }

    private void SpawnElements()
    {
        for (int i = 0; i < dimension; i++)
        {
            for (int j = 0; j < dimension; j++)
            {
                if (i == dimension -1 || i == 0 || j == dimension - 1 || j == 0)
                {
                    //links
                    if (mainFeld[i, j] == 1 && j == 0 && i > 0 && i < dimension-1)
                    {
                        if (mainFeld[i + 1, j] == 1 && mainFeld[i, j + 1] == 1 && mainFeld[i - 1, j] == 1)
                        {
                            Instantiate(hub3, new Vector3((i * offsetLengthNormal) - ((dimension * offsetLengthNormal) / 2), 0, (j * offsetLengthNormal) - ((dimension * offsetLengthNormal) / 2)), Quaternion.Euler(new Vector3(0f, 0f, 0f)));
                        }
                        else if (mainFeld[i + 1, j] == 1 && mainFeld[i, j + 1] == 1 && mainFeld[i - 1, j] == 0)
                        {
                            Instantiate(hub2, new Vector3((i * offsetLengthNormal) - ((dimension * offsetLengthNormal) / 2), 0, (j * offsetLengthNormal) - ((dimension * offsetLengthNormal) / 2)), Quaternion.Euler(new Vector3(0f, 90f, 0f)));
                        }
                        else if (mainFeld[i + 1, j] == 0 && mainFeld[i, j + 1] == 1 && mainFeld[i - 1, j] == 1)
                        {
                            Instantiate(hub2, new Vector3((i * offsetLengthNormal) - ((dimension * offsetLengthNormal) / 2), 0, (j * offsetLengthNormal) - ((dimension * offsetLengthNormal) / 2)), Quaternion.Euler(new Vector3(0f, 0f, 0f)));
                        }
                        else if (mainFeld[i + 1, j] == 1 && mainFeld[i, j + 1] == 0 && mainFeld[i - 1, j] == 1)
                        {
                            Instantiate(gangSmall, new Vector3((i * offsetLengthNormal) - ((dimension * offsetLengthNormal) / 2), 0, (j * offsetLengthNormal) - ((dimension * offsetLengthNormal) / 2)), Quaternion.identity);
                        }
                        else if (mainFeld[i -1, j] == 0 && mainFeld[i, j + 1] == 0 && mainFeld[i + 1, j] == 1)
                        {
                            Instantiate(hub1, new Vector3((i * offsetLengthNormal) - ((dimension * offsetLengthNormal) / 2), 0, (j * offsetLengthNormal) - ((dimension * offsetLengthNormal) / 2)), Quaternion.Euler(new Vector3(0f, 180f, 0f)));
                        }
                        else if (mainFeld[i - 1, j] == 1 && mainFeld[i, j + 1] == 0 && mainFeld[i + 1, j] == 0)
                        {
                            Instantiate(hub1, new Vector3((i * offsetLengthNormal) - ((dimension * offsetLengthNormal) / 2), 0, (j * offsetLengthNormal) - ((dimension * offsetLengthNormal) / 2)), Quaternion.identity);
                        }
                    }
                    //rechts
                    else if (mainFeld[i, j] == 1 && j == dimension-1 && i > 0 && i < dimension - 1)
                    {
                        if (mainFeld[i + 1, j] == 1 && mainFeld[i, j - 1] == 1 && mainFeld[i - 1, j] == 1)
                        {
                            Instantiate(hub3, new Vector3((i * offsetLengthNormal) - ((dimension * offsetLengthNormal) / 2), 0, (j * offsetLengthNormal) - ((dimension * offsetLengthNormal) / 2)), Quaternion.Euler(new Vector3(0f, 180f, 0f)));
                        }
                        else if (mainFeld[i + 1, j] == 1 && mainFeld[i, j - 1] == 1 && mainFeld[i - 1, j] == 0)
                        {
                            Instantiate(hub2, new Vector3((i * offsetLengthNormal) - ((dimension * offsetLengthNormal) / 2), 0, (j * offsetLengthNormal) - ((dimension * offsetLengthNormal) / 2)), Quaternion.Euler(new Vector3(0f, 180f, 0f)));
                        }
                        else if (mainFeld[i + 1, j] == 0 && mainFeld[i, j - 1] == 1 && mainFeld[i - 1, j] == 1)
                        {
                            Instantiate(hub2, new Vector3((i * offsetLengthNormal) - ((dimension * offsetLengthNormal) / 2), 0, (j * offsetLengthNormal) - ((dimension * offsetLengthNormal) / 2)), Quaternion.Euler(new Vector3(0f, -90f, 0f)));
                        }
                        else if (mainFeld[i + 1, j] == 1 && mainFeld[i, j - 1] == 0 && mainFeld[i - 1, j] == 1)
                        {
                            Instantiate(gangSmall, new Vector3((i * offsetLengthNormal) - ((dimension * offsetLengthNormal) / 2), 0, (j * offsetLengthNormal) - ((dimension * offsetLengthNormal) / 2)), Quaternion.identity);
                        }
                        else if (mainFeld[i - 1, j] == 0 && mainFeld[i, j - 1] == 0 && mainFeld[i + 1, j] == 1)
                        {
                            Instantiate(hub1, new Vector3((i * offsetLengthNormal) - ((dimension * offsetLengthNormal) / 2), 0, (j * offsetLengthNormal) - ((dimension * offsetLengthNormal) / 2)), Quaternion.Euler(new Vector3(0f, 180f, 0f)));
                        }
                        else if (mainFeld[i - 1, j] == 1 && mainFeld[i, j - 1] == 0 && mainFeld[i + 1, j] == 0)
                        {
                            Instantiate(hub1, new Vector3((i * offsetLengthNormal) - ((dimension * offsetLengthNormal) / 2), 0, (j * offsetLengthNormal) - ((dimension * offsetLengthNormal) / 2)), Quaternion.identity);
                        }
                    }
                    //oben
                    else if (mainFeld[i, j] == 1 && i == 0 && j > 0 && j < dimension - 1)
                    {
                        if (mainFeld[i + 1, j] == 1 && mainFeld[i, j - 1] == 1 && mainFeld[i, j + 1] == 1)
                        {
                            Instantiate(hub3, new Vector3((i * offsetLengthNormal) - ((dimension * offsetLengthNormal) / 2), 0, (j * offsetLengthNormal) - ((dimension * offsetLengthNormal) / 2)), Quaternion.Euler(new Vector3(0f, 90f, 0f)));
                        }
                        else if (mainFeld[i + 1, j] == 1 && mainFeld[i, j - 1] == 1 && mainFeld[i, j + 1] == 0)
                        {
                            Instantiate(hub2, new Vector3((i * offsetLengthNormal) - ((dimension * offsetLengthNormal) / 2), 0, (j * offsetLengthNormal) - ((dimension * offsetLengthNormal) / 2)), Quaternion.Euler(new Vector3(0f, 180f, 0f)));
                        }
                        else if (mainFeld[i + 1, j] == 1 && mainFeld[i, j - 1] == 0 && mainFeld[i, j + 1] == 1)
                        {
                            Instantiate(hub2, new Vector3((i * offsetLengthNormal) - ((dimension * offsetLengthNormal) / 2), 0, (j * offsetLengthNormal) - ((dimension * offsetLengthNormal) / 2)), Quaternion.Euler(new Vector3(0f, 90f, 0f)));
                        }
                        else if (mainFeld[i + 1, j] == 0 && mainFeld[i, j + 1] == 1 && mainFeld[i, j + 1] == 1)
                        {
                            Instantiate(gangSmall, new Vector3((i * offsetLengthNormal) - ((dimension * offsetLengthNormal) / 2), 0, (j * offsetLengthNormal) - ((dimension * offsetLengthNormal) / 2)), Quaternion.Euler(new Vector3(0f, 90f, 0f)));
                        }
                        else if (mainFeld[i, j + 1] == 1 && mainFeld[i, j - 1] == 0 && mainFeld[i + 1, j] == 0)
                        {
                            Instantiate(hub1, new Vector3((i * offsetLengthNormal) - ((dimension * offsetLengthNormal) / 2), 0, (j * offsetLengthNormal) - ((dimension * offsetLengthNormal) / 2)), Quaternion.Euler(new Vector3(0f, 90f, 0f)));
                        }
                        else if (mainFeld[i, j + 1] == 0 && mainFeld[i, j - 1] == 1 && mainFeld[i + 1, j] == 0)
                        {
                            Instantiate(hub1, new Vector3((i * offsetLengthNormal) - ((dimension * offsetLengthNormal) / 2), 0, (j * offsetLengthNormal) - ((dimension * offsetLengthNormal) / 2)), Quaternion.Euler(new Vector3(0f, -90f, 0f)));
                        }
                    }
                    //uten
                    else if (mainFeld[i, j] == 1 && i == dimension-1 && j > 0 && j < dimension - 1)
                    {
                        if (mainFeld[i - 1, j] == 1 && mainFeld[i, j - 1] == 1 && mainFeld[i, j + 1] == 1)
                        {
                            Instantiate(hub3, new Vector3((i * offsetLengthNormal) - ((dimension * offsetLengthNormal) / 2), 0, (j * offsetLengthNormal) - ((dimension * offsetLengthNormal) / 2)), Quaternion.Euler(new Vector3(0f, -90f, 0f)));
                        }
                        else if (mainFeld[i - 1, j] == 1 && mainFeld[i, j - 1] == 1 && mainFeld[i, j + 1] == 0)
                        {
                            Instantiate(hub2, new Vector3((i * offsetLengthNormal) - ((dimension * offsetLengthNormal) / 2), 0, (j * offsetLengthNormal) - ((dimension * offsetLengthNormal) / 2)), Quaternion.Euler(new Vector3(0f, -90f, 0f)));
                        }
                        else if (mainFeld[i - 1, j] == 1 && mainFeld[i, j - 1] == 0 && mainFeld[i, j + 1] == 1)
                        {
                            Instantiate(hub2, new Vector3((i * offsetLengthNormal) - ((dimension * offsetLengthNormal) / 2), 0, (j * offsetLengthNormal) - ((dimension * offsetLengthNormal) / 2)), Quaternion.Euler(new Vector3(0f, 0f, 0f)));
                        }
                        else if (mainFeld[i - 1, j] == 0 && mainFeld[i, j + 1] == 1 && mainFeld[i, j + 1] == 1)
                        {
                            Instantiate(gangSmall, new Vector3((i * offsetLengthNormal) - ((dimension * offsetLengthNormal) / 2), 0, (j * offsetLengthNormal) - ((dimension * offsetLengthNormal) / 2)), Quaternion.Euler(new Vector3(0f, 90f, 0f)));
                        }
                        //TODO:
                        else if (mainFeld[i, j + 1] == 1 && mainFeld[i, j - 1] == 0 && mainFeld[i - 1, j] == 0)
                        {
                            Instantiate(hub1, new Vector3((i * offsetLengthNormal) - ((dimension * offsetLengthNormal) / 2), 0, (j * offsetLengthNormal) - ((dimension * offsetLengthNormal) / 2)), Quaternion.Euler(new Vector3(0f, 90f, 0f)));
                        }
                        else if (mainFeld[i, j + 1] == 0 && mainFeld[i, j - 1] == 1 && mainFeld[i - 1, j] == 0)
                        {
                            Instantiate(hub1, new Vector3((i * offsetLengthNormal) - ((dimension * offsetLengthNormal) / 2), 0, (j * offsetLengthNormal) - ((dimension * offsetLengthNormal) / 2)), Quaternion.Euler(new Vector3(0f, -90f, 0f)));
                        }
                    }
                    //Ecke, links oben
                    if (mainFeld[i, j] == 1 && i == 0 && j == 0)
                    {
                        if (mainFeld[i, j+1] == 1 && mainFeld[i+1, j] == 1)
                        {
                            Instantiate(hub2, new Vector3((i * offsetLengthNormal) - ((dimension * offsetLengthNormal) / 2), 0, (j * offsetLengthNormal) - ((dimension * offsetLengthNormal) / 2)), Quaternion.Euler(new Vector3(0f, 90f, 0f)));
                        }
                    }
                    //Ecke, links unten
                    else if (mainFeld[i, j] == 1 && i == dimension-1 && j == 0)
                    {
                        if (mainFeld[i-1, j] == 1 && mainFeld[i, j+1] == 1)
                        {
                            Instantiate(hub2, new Vector3((i * offsetLengthNormal) - ((dimension * offsetLengthNormal) / 2), 0, (j * offsetLengthNormal) - ((dimension * offsetLengthNormal) / 2)), Quaternion.Euler(new Vector3(0f, 0f, 0f)));
                        }
                    }
                    //Ecke, rechts oben
                    else if (mainFeld[i, j] == 1 && i == 0 && j == dimension-1)
                    {
                        if (mainFeld[i + 1, j] == 1 && mainFeld[i, j - 1] == 1)
                        {
                            Instantiate(hub2, new Vector3((i * offsetLengthNormal) - ((dimension * offsetLengthNormal) / 2), 0, (j * offsetLengthNormal) - ((dimension * offsetLengthNormal) / 2)), Quaternion.Euler(new Vector3(0f, 180f, 0f)));
                        }
                    }
                    //Ecke, rechts unten
                    else if (mainFeld[i, j] == 1 && i == dimension-1 && j == dimension - 1)
                    {
                        if (mainFeld[i - 1, j] == 1 && mainFeld[i, j - 1] == 1)
                        {
                            Instantiate(hub2, new Vector3((i * offsetLengthNormal) - ((dimension * offsetLengthNormal) / 2), 0, (j * offsetLengthNormal) - ((dimension * offsetLengthNormal) / 2)), Quaternion.Euler(new Vector3(0f, -90f, 0f)));
                        }
                    }
                    continue;
                }

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
                        Instantiate(hub2, new Vector3((i * offsetLengthNormal) - ((dimension * offsetLengthNormal) / 2), 0, (j * offsetLengthNormal) - ((dimension * offsetLengthNormal) / 2)), Quaternion.Euler(new Vector3(0f, 90f, 0f)));
                    }
                    else if (mainFeld[i + 1, j] == 1 && mainFeld[i, j + 1] == 0 && mainFeld[i - 1, j] == 0 && mainFeld[i, j - 1] == 1)
                    {
                        Instantiate(hub2, new Vector3((i * offsetLengthNormal) - ((dimension * offsetLengthNormal) / 2), 0, (j * offsetLengthNormal) - ((dimension * offsetLengthNormal) / 2)), Quaternion.Euler(new Vector3(0f, 180f, 0f)));
                    }
                    else if (mainFeld[i + 1, j] == 0 && mainFeld[i, j + 1] == 1 && mainFeld[i - 1, j] == 1 && mainFeld[i, j - 1] == 0)
                    {
                        //richtig
                        Instantiate(hub2, new Vector3((i * offsetLengthNormal) - ((dimension * offsetLengthNormal) / 2), 0, (j * offsetLengthNormal) - ((dimension * offsetLengthNormal) / 2)), Quaternion.Euler(new Vector3(0f, 0f, 0f)));
                    }
                    else if (mainFeld[i + 1, j] == 0 && mainFeld[i, j + 1] == 0 && mainFeld[i - 1, j] == 1 && mainFeld[i, j - 1] == 1)
                    {
                        Instantiate(hub2, new Vector3((i * offsetLengthNormal) - ((dimension * offsetLengthNormal) / 2), 0, (j * offsetLengthNormal) - ((dimension * offsetLengthNormal) / 2)), Quaternion.Euler(new Vector3(0f, -90f, 0f)));
                    }
                }

                //gang spawnen
                if (mainFeld[i, j] == 1)
                {
                    if (mainFeld[i + 1, j] == 1 && mainFeld[i, j + 1] == 0 && mainFeld[i - 1, j] == 1 && mainFeld[i, j - 1] == 0)
                    {
                        Instantiate(gangSmall, new Vector3((i * offsetLengthNormal) - ((dimension * offsetLengthNormal) / 2), 0, (j * offsetLengthNormal) - ((dimension * offsetLengthNormal) / 2)), Quaternion.identity);
                    }
                    else if (mainFeld[i + 1, j] == 0 && mainFeld[i, j + 1] == 1 && mainFeld[i - 1, j] == 0 && mainFeld[i, j - 1] == 1)
                    {
                        Instantiate(gangSmall, new Vector3((i * offsetLengthNormal) - ((dimension * offsetLengthNormal) / 2), 0, (j * offsetLengthNormal) - ((dimension * offsetLengthNormal) / 2)), Quaternion.Euler(new Vector3(0f, 90f, 0f)));
                    }
                }


                //3er ecke spawnen
                if (mainFeld[i, j] == 1)
                {
                    if (mainFeld[i + 1, j] == 1 && mainFeld[i, j + 1] == 1 && mainFeld[i - 1, j] == 0 && mainFeld[i, j - 1] == 1)
                    {
                        Instantiate(hub3, new Vector3((i * offsetLengthNormal) - ((dimension * offsetLengthNormal) / 2), 0, (j * offsetLengthNormal) - ((dimension * offsetLengthNormal) / 2)), Quaternion.Euler(new Vector3(0f, 90f, 0f)));
                    }
                    else if (mainFeld[i + 1, j] == 1 && mainFeld[i, j + 1] == 0 && mainFeld[i - 1, j] == 1 && mainFeld[i, j - 1] == 1)
                    {
                        Instantiate(hub3, new Vector3((i * offsetLengthNormal) - ((dimension * offsetLengthNormal) / 2), 0, (j * offsetLengthNormal) - ((dimension * offsetLengthNormal) / 2)), Quaternion.Euler(new Vector3(0f, 180f, 0f)));
                    }
                    else if (mainFeld[i + 1, j] == 0 && mainFeld[i, j + 1] == 1 && mainFeld[i - 1, j] == 1 && mainFeld[i, j - 1] == 1)
                    {
                        //richtig
                        Instantiate(hub3, new Vector3((i * offsetLengthNormal) - ((dimension * offsetLengthNormal) / 2), 0, (j * offsetLengthNormal) - ((dimension * offsetLengthNormal) / 2)), Quaternion.Euler(new Vector3(0f, -90f, 0f)));
                    }
                    else if (mainFeld[i + 1, j] == 1 && mainFeld[i, j + 1] == 1 && mainFeld[i - 1, j] == 1 && mainFeld[i, j - 1] == 0)
                    {
                        Instantiate(hub3, new Vector3((i * offsetLengthNormal) - ((dimension * offsetLengthNormal) / 2), 0, (j * offsetLengthNormal) - ((dimension * offsetLengthNormal) / 2)), Quaternion.Euler(new Vector3(0f, 0f, 0f)));
                    }
                }

                //4er Ecke(=4hub) einfuegen
                if (mainFeld[i, j] == 1)
                {
                    if (mainFeld[i + 1, j] == 1 && mainFeld[i, j + 1] == 1 && mainFeld[i - 1, j] == 1 && mainFeld[i, j - 1] == 1)
                    {
                        Instantiate(hub4, new Vector3((i * offsetLengthNormal) - ((dimension * offsetLengthNormal) / 2), 0, (j * offsetLengthNormal) - ((dimension * offsetLengthNormal) / 2)), Quaternion.identity);
                    }
                }
            }
        }
    }

    private void PrintArray(int[,] m)
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
        Debug.Log(sb.ToString());
    }

    private void PrintCSV(int[,] m)
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
            //writer.Write(sb.ToString());
            writer.WriteLine(sb.ToString());
            //Debug.Log(sb.ToString());
            sb = new StringBuilder();
        }
        writer.Close();
    }
}
