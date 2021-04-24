using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

public class ArraySpawner : MonoBehaviour
{
    public int dimension = 21;
    public int anzahlMinispiele;
    private int[,] mainFeld;
    public int iterations = 10;

    public int gangWahrscheinlichkeit = 8;
    private int mitte;
    public string filepath;


    // Start is called before the first frame update
    void Start()
    {
        //filepath = GetPath();
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
        }

        PrintArray(mainFeld);
        PrintCSV(mainFeld);

    }

    private void RegenArray()
    {
        for (int i = 1; i < dimension-1; i++)
        {
            for (int j = 1; j < dimension-1; j++)
            {
                if (mainFeld[i,j] == 1)
                {
                    if (mainFeld[i + 1, j] == 0 && mainFeld[i, j + 1] == 0 && mainFeld[i - 1, j] == 0 && mainFeld[i, j - 1] == 0 )
                    {
                        //Anfang
                        mainFeld[i + 1, j] = 1;
                        mainFeld[i, j + 1] = 1;
                        mainFeld[i - 1, j] = 1;
                        mainFeld[i, j - 1] = 1;
                    }
                }
                if (mainFeld[i + 1, j] == 1 && mainFeld[i, j + 1] == 0 && mainFeld[i - 1, j] == 0 && mainFeld[i, j - 1] == 0 )
                {
                    //Richtung oben
                    mainFeld[i, j] = 1;
                    //SpawnRandomObject(i, j);
                }
                /*if (mainFeld[i + 1, j] == 0 && mainFeld[i, j + 1] == 1 && mainFeld[i - 1, j] == 0 && mainFeld[i, j - 1] == 0)
                {
                    mainFeld[i, j] = 1;
                }
                
                if (mainFeld[i + 1, j] == 0 && mainFeld[i, j + 1] == 0 && mainFeld[i - 1, j] == 1 && mainFeld[i, j - 1] == 0)
                {
                    mainFeld[i, j] = 1;
                }
                if (mainFeld[i + 1, j] == 0 && mainFeld[i, j + 1] == 0 && mainFeld[i - 1, j] == 0 && mainFeld[i, j - 1] == 1)
                {
                    mainFeld[i, j] = 1;
                }*/
            }
        }
    }

    private void SpawnRandomObject(int x, int y)
    {
        int random = UnityEngine.Random.Range(0, 10);

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
                sb.Append(Convert.ToString(m[j, i]));
            }
            //writer.Write(sb.ToString());
            writer.WriteLine(sb.ToString());
            //Debug.Log(sb.ToString());
            sb = new StringBuilder();
        }
        writer.Close();
    }

    private string GetPath()
    {
#if UNITY_EDITOR
        return Application.dataPath + "/Data/" + "Saved_Inventory.csv";
        //"Participant " + "   " + DateTime.Now.ToString("dd-MM-yy   hh-mm-ss") + ".csv";
#elif UNITY_ANDROID
        return Application.persistentDataPath+"Saved_Inventory.csv";
#elif UNITY_IPHONE
        return Application.persistentDataPath+"/"+"Saved_Inventory.csv";
#else
        return Application.dataPath +"/"+"Saved_Inventory.csv";
#endif
    }
}
