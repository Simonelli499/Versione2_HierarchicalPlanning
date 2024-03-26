using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class MapLoader : MonoBehaviour
{
    [SerializeField] public int gridSize = 9;
    [SerializeField] public int numberOfTutorials = 0;
    [SerializeField] public List<Map> maps = new List<Map>();

 //   public System.Random rng = new System.Random(Guid.NewGuid().GetHashCode());
    public System.Random rng = new System.Random();

    /*
    public GameObject Goals3Legend;
    public GameObject Goals2Legend;
    */

    public void LoadMapFromCSV(string filePath)
    {
        maps = new List<Map>();
        List<Map> tutorialMaps = new List<Map>();
        int numberOfMapsCreated = 0;
        string[] lines = System.IO.File.ReadAllLines(filePath);

        int lineCount = 0;
        string mapLine = "";
        foreach (string line in lines)
        {
            mapLine += line.Replace(";", ",");
            
            if (lineCount == gridSize)
            {
                if (numberOfMapsCreated < numberOfTutorials){
                    tutorialMaps.Add(CreateMapArray(mapLine));
                }else{
                    maps.Add(CreateMapArray(mapLine));
                }
                numberOfMapsCreated += 1;
                mapLine = "";
                lineCount = 0;
            }else{
                mapLine += "\n";
                lineCount++;
            }
        }
    
        tutorialMaps = tutorialMaps.OrderBy(a => rng.Next()).ToList(); //QUI MISCHIA I TUTORIALS
        maps = maps.OrderBy(a => rng.Next()).ToList(); //QUI MISCHIA LE MAPPE

        foreach (Map map in maps)
        {
            tutorialMaps.Add(map);
        }
        
        maps = tutorialMaps;


    }


    public Map CreateMapArray(string mapString)
    {
        string[] lines = mapString.Split("\n"); 

        int mapNumber = -1, mapType = -1, numberOfGoals = -1;
        bool isStructured = false;
        List<string> goals = new List<string>();
        string[] mapArray = new string[gridSize*gridSize];
        for (int j = 0; j < lines.Length; j++)
        {
            string line = lines[j];
            if (j == 0) 
            {
                string[] cells = line.Split(',');

                mapNumber = int.Parse(cells[0]);
                mapType = int.Parse(cells[1]);
                numberOfGoals = int.Parse(cells[2]);
                isStructured = (cells[3] == "S");
                goals = new List<string>();
                for (int i = 4; i < cells.Length; i++)
                {
                    if (cells[i] != "") goals.Add(cells[i]);
                }
 
            }else{
                
                string[] cells = line.Split(',');
                for (int i = 0; i < cells.Length; i++)
                {
                    mapArray[(j-1)*gridSize + i] = cells[i];
                }
            }
        }
        return new Map(mapArray, mapNumber, mapType, numberOfGoals, isStructured, goals, gridSize);

    }

    public void PrintMapMatrix(int index)
    {
        string[,] mapArray = maps[index].GetMapMatrix();

        string mapString = "| ";
        for (int j = 0; j < mapArray.GetLength(0); j++)
        {
            for (int i = 0; i < mapArray.GetLength(1); i++)
            {
                if (mapArray[j, i] == "")
                { 
                    mapString += "__";
                }else{
                    mapString += mapArray[j, i];
                }
                mapString += " | ";
            }
            mapString += "\n| ";
        }
        Debug.Log(mapString);
    }


    [System.Serializable]
    public class Map
    {
        public string[] mapArray;
        public int mapNumber;
        public int mapType;
        public int numberOfGoals;
        public bool isStructured;
        public List<string> goals = new List<string>();
        public int gridSize;

        public Map(string[] mapArray, int mapNumber, int mapType, int numberOfGoals, bool isStructured, List<string> goals, int gridSize)
        {
            this.mapArray = mapArray;
            this.mapNumber = mapNumber;
            this.mapType = mapType;
            this.numberOfGoals = numberOfGoals;
            this.isStructured = isStructured;
            this.goals = goals;
            this.gridSize = gridSize;
        }

        public string[,] GetMapMatrix()
        {
            string [,] mapMatrix = new string[gridSize, gridSize];

            for (int j = 0; j < gridSize; j++)
            {
                for (int i = 0; i < gridSize; i++)
                {
                    mapMatrix[j, i] = mapArray[j*gridSize + i];
                }
            }

            return mapMatrix;
        }
    }
}

