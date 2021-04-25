using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using Random = UnityEngine.Random;

public class ArraySpawner : MonoBehaviour
{
    public Transform gang;
    public Transform hub;
    public Transform miniSpiel;

    public int dimension = 21;
    public int anzahlMinispiele;
    private int[,] mainFeld;
    public int iterations = 20;

    public int gangWahrscheinlichkeit = 8;
    private int mitte;
    public string filepath;
    private float offsetLength = 8f;


    // Start is called before the first frame update
    void Start()
    {
        filepath = "CSVData.csv";
        mainFeld = new int[dimension, dimension];
        mitte = dimension / 2;
        for (int i = 0; i < dimension; i++)
        {
            for (int j = 0; j < dimension; j++)
            {
                mainFeld[i,j] = 0;
                if (i == mitte && j == mitte)
                {
                    mainFeld[i, j] = 1;
                }
            }
        }
        for (int i = 0; i < iterations; i++)
        {
            RegenArray();
            //PrintArray(mainFeld);
        }

        
        PrintCSV(mainFeld);
        //SpawnElements();

    }

    private void RegenArray()
    {
        for (int i = 1; i < dimension - 1; i++)
        {
            for (int j = 1; j < dimension - 1; j++)
            {
                if (mainFeld[i, j] == 1)
                {
                    if (mainFeld[i + 1, j] == 0 && mainFeld[i, j + 1] == 0 && mainFeld[i - 1, j] == 0 && mainFeld[i, j - 1] == 0)
                    {
                        //Anfang
                        mainFeld[i + 1, j] = 1;
                        mainFeld[i, j + 1] = 1;
                        mainFeld[i - 1, j] = 1;
                        mainFeld[i, j - 1] = 1;
                        PrintArray(mainFeld);
                    }
                    
                }
                /*if (mainFeld[i + 1, j] == 1 && mainFeld[i, j + 1] == 0 && mainFeld[i - 1, j] == 0 && mainFeld[i, j - 1] == 0)
                {
                    //Richtung oben
                    mainFeld[i, j] = 1;

                }*/

                //+-----+-----+-----+-----+-----+[Gang richtung Oben]+-----+-----+-----+-----+-----+\\
                //Gang richtung oben
                if (mainFeld[i, j] == 1 &&mainFeld[i + 1, j] == 1 && mainFeld[i, j + 1] == 0 && mainFeld[i - 1, j] == 0 && mainFeld[i, j - 1] == 0)
                {
                    int rand = Random.Range(1, 11);
                    if (rand >= 1 && rand <= 8)
                    {
                        //Debug.Log("Richtung oben mit rand= " + rand);
                        //Richtung oben
                        mainFeld[i - 1, j] = 1;
                        Debug.Log("richtung Oben: Schritt nach vorne: " + (i-1) + "," + (j));
                        //return;
                        
                    }
                    if (rand >= 9 && rand <= 10)
                    {
                        //Debug.Log("Richtung oben mit links oder recht mit rand= " + rand);
                        //(1x1) - Hub schon platziert (weil mainFeld[i, j] == 1)
                        rand = Random.Range(1, 4);
                        if (rand == 1)
                        {
                            //nur links
                            mainFeld[i, j - 1] = 1;
                            Debug.Log("richtung Oben: Schritt nach Links: " + (i) + "," + (j-1));
                        }
                        else if (rand == 2)
                        {
                            //nur rechts
                            mainFeld[i, j + 1] = 1;
                            Debug.Log("richtung Oben: Schritt nach Rechts: " + (i) + "," + (j + 1));
                        }
                        else if (rand == 3)
                        {
                            //links und rechts
                            mainFeld[i, j + 1] = 1;
                            mainFeld[i, j - 1] = 1;
                            Debug.Log("richtung Oben: Schritt nach Rechts: " + (i) + "," + (j + 1)+ "\t und Schritt nach Links: " + (i) + "," + (j - 1));
                        }
                        //return;
                    }
                    PrintArray(mainFeld);
                }

                //+-----+-----+-----+-----+-----+[Gang richtung unten]+-----+-----+-----+-----+-----+\\
                //Gang richtung Unten
                if (mainFeld[i, j] == 1 && mainFeld[i + 1, j] == 0 && mainFeld[i, j + 1] == 0 && mainFeld[i - 1, j] == 1 && mainFeld[i, j - 1] == 0)
                {
                    int rand = Random.Range(1, 11);
                    if (rand >= 1 && rand <= 8)
                    {
                        //Richtung oben
                        mainFeld[i + 1, j] = 1;
                        Debug.Log("richtung unten: Schritt nach vorne: " + (i + 1) + "," + (j));
                        //return;

                    }
                    if (rand >= 9 && rand <= 10)
                    {
                        //Debug.Log("Richtung: unten mit links oder rechts");
                        //(1x1) - Hub schon platziert (weil mainFeld[i, j] == 1)
                        rand = Random.Range(1, 4);
                        if (rand == 1)
                        {
                            //nur links
                            mainFeld[i, j - 1] = 1;
                            Debug.Log("richtung unten: Schritt nach links: " + (i) + "," + (j - 1));
                        }
                        else if (rand == 2)
                        {
                            //nur rechts
                            mainFeld[i, j + 1] = 1;
                            Debug.Log("richtung unten: Schritt nach rechts: " + (i) + "," + (j + 1));
                        }
                        else if (rand == 3)
                        {
                            //links und rechts
                            mainFeld[i, j + 1] = 1;
                            mainFeld[i, j - 1] = 1;
                            Debug.Log("richtung unten: Schritt nach links: " + (i) + "," + (j + 1) + "\t und Schritt nach rechts: " + (i) + "," + (j - 1));
                        }
                        //return;
                    }
                    PrintArray(mainFeld);
                }

                //+-----+-----+-----+-----+-----+[Gang richtung Rechts]+-----+-----+-----+-----+-----+\\
                if (mainFeld[i, j] == 1 && mainFeld[i + 1, j] == 0 && mainFeld[i, j + 1] == 0 && mainFeld[i - 1, j] == 0 && mainFeld[i, j - 1] == 1)
                {
                    int rand = Random.Range(1, 11);
                    if (rand >= 1 && rand <= 8)
                    {
                        //Debug.Log("Richtung rechts mit rand= " + rand);
                        //Richtung oben
                        mainFeld[i, j + 1] = 1;
                        Debug.Log("richtung Rechts: Schritt nach vorne: " + (i) + "," + (j-1));
                        //return;

                    }
                    if (rand >= 9 && rand <= 10)
                    {
                        //Debug.Log("Richtung: rechts mit oben oder unten mit rand= " + rand);
                        //(1x1) - Hub schon platziert (weil mainFeld[i, j] == 1)
                        rand = Random.Range(1, 4);
                        if (rand == 1)
                        {
                            //nur oben
                            mainFeld[i - 1, j] = 1;
                            Debug.Log("richtung Rechts: Schritt nach links: " + (i - 1) + "," + (j));
                        }
                        else if (rand == 2)
                        {
                            //nur unten
                            mainFeld[i + 1, j] = 1;
                            Debug.Log("richtung Rechts: Schritt nach Rechts: " + (i + 1) + "," + (j));
                        }
                        else if (rand == 3)
                        {
                            //oebn und unten
                            mainFeld[i - 1, j] = 1;
                            mainFeld[i + 1, j] = 1;
                            Debug.Log("richtung Rechts: Schritt nach links: " + (i - 1) + "," + (j) + "\t und Schritt nach rechts: " + (i + 1) + "," + (j));
                        }
                        //return;
                    }
                    PrintArray(mainFeld);
                }


                //+-----+-----+-----+-----+-----+[Gang richtung Links]+-----+-----+-----+-----+-----+\\
                if (mainFeld[i, j] == 1 && mainFeld[i + 1, j] == 0 && mainFeld[i, j + 1] == 1 && mainFeld[i - 1, j] == 0 && mainFeld[i, j - 1] == 0)
                {
                    int rand = Random.Range(1, 11);
                    if (rand >= 1 && rand <= 8)
                    {
                        //Debug.Log("Richtung rechts mit rand= " + rand);
                        //Richtung oben
                        mainFeld[i, j - 1] = 1;
                        Debug.Log("richtung Links: Schritt nach vorne: " + (i) + "," + (j - 1));
                        //return;

                    }
                    if (rand >= 9 && rand <= 10)
                    {
                        //Debug.Log("Richtung: rechts mit oben oder unten mit rand= " + rand);
                        //(1x1) - Hub schon platziert (weil mainFeld[i, j] == 1)
                        rand = Random.Range(1, 4);
                        if (rand == 1)
                        {
                            //nur oben
                            mainFeld[i + 1, j] = 1;
                            Debug.Log("richtung Links: Schritt nach rechts: " + (i + 1) + "," + (j));
                        }
                        else if (rand == 2)
                        {
                            //nur unten
                            mainFeld[i - 1, j] = 1;
                            Debug.Log("richtung Links: Schritt nach links: " + (i - 1) + "," + (j));
                        }
                        else if (rand == 3)
                        {
                            //oebn und unten
                            mainFeld[i - 1, j] = 1;
                            mainFeld[i + 1, j] = 1;
                            Debug.Log("richtung Links: Schritt nach rechts: " + (i + 1) + "," + (j) + "\t und Schritt nach links: " + (i - 1) + "," + (j));
                        }
                        //return;
                    }
                    PrintArray(mainFeld);
                }
            }
        }
    }

    private void SpawnElements()
    {
        for (int i = 0; i < dimension; i++)
        {
            for (int j = 0; j < dimension; j++)
            {
                if (mainFeld[i, j] == 1)
                {
                    if (mainFeld[i + 1, j] == 1 && mainFeld[i, j + 1] == 1 && mainFeld[i - 1, j] == 1 && mainFeld[i, j - 1] == 1)
                    {
                        //Anfang
                        if (mainFeld[i + 1, j] == 0 && mainFeld[i, j + 1] == 0 && mainFeld[i - 1, j] == 0 && mainFeld[i, j - 1] == 0 )
                        {
                            //Anfang
                            SpawnRandomObject(i, j, Quaternion.identity);
                        }
                    }
                }
    
            }
        }
    }


    private void SpawnRandomObject(int x, int z, Quaternion rotate)
    {
        int random = UnityEngine.Random.Range(0, 10);
        if (random >= 0 && random < 8)
        {
            Instantiate(gang, new Vector3(x*offsetLength, 0, z*offsetLength), rotate); 
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
