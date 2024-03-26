using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using TMPro;


[RequireComponent(typeof(MapLoader))] //map manager per esistere ha bisogno di map loader, serve per assicurarmi che la componente map loader ci sia!

public class MapManager : MonoBehaviour
{
    MapLoader mapLoader;

    [SerializeField] GameObject trianglePrefab;
    [SerializeField] GameObject squarePrefab;
    [SerializeField] GameObject circlePrefab;
    [SerializeField] GameObject rhombusPrefab;

    [SerializeField] Color pink;
    [SerializeField] Color green;
    [SerializeField] Color yellow;
    [SerializeField] Color lightBlue;

    [SerializeField] List<Color> colors = new List<Color>(); 
    [SerializeField] List<GameObject> shapes = new List<GameObject>(); 
    [SerializeField] List<int> colors_random_order = new List<int>(); 
    [SerializeField] List<int> shapes_random_order = new List<int>(); 

    [SerializeField] List<Transform> legendTransforms; //voglio una lista che contenga la componente transform dei squares figli di ABCDgriglia

    [SerializeField] AudioManager audioManager;
    [SerializeField] ExperimentManager experimentManager;
  

    public GameObject Griglia;
    public GameObject Player;

    public GameObject ABCDgriglia;

    public GameObject Message_StartPlaying;

    public GameObject ToppaGriglia;

    public GameObject ToppaABCDgrigla;


    public int goalIndex;

    public bool firstGoalTaken = false;

    private bool levelStarted = false;
    
    public System.Random rng = new System.Random(Guid.NewGuid().GetHashCode()); //use level number as seed instead, and therefore modify Guid.NewGuid().GetHashCode() by allocating there the level number


    void Start()
    {
        mapLoader = GetComponent<MapLoader>();


        RandomizeMapOrder();
        AddColorsToColorsListAndRandomize();
        CreateMap();
    }

    void RandomizeMapOrder()
    {
        List<MapLoader.Map> reorderedMaps = new List<MapLoader.Map>();
        
        List<int> randomMapsOrder = ExperimentManager.Instance.randomMapsOrder;
        
        for (int i = 0; i < randomMapsOrder.Count; i++)
        {
            reorderedMaps.Add(mapLoader.maps[randomMapsOrder[i]]);
        }
        mapLoader.maps = reorderedMaps;
    }


    /*void Update() 
    {
        if (Input.GetKeyDown(KeyCode.Return)) //&& variabile booleana
        {
            if (levelStarted == false)
            {
                StartPlaying();
                DataCollector.Instance.AddEvent("LevelStarted");
                levelStarted = true;
            }
   
        }
    }*/

    void StartPlaying()
    {
        SpriteRenderer[] GridspriteRenderers = Griglia.GetComponentsInChildren<SpriteRenderer>();
        foreach (SpriteRenderer spriteRenderer in GridspriteRenderers)
        {
            spriteRenderer.enabled = true;
        }

        Player.GetComponentInChildren<SpriteRenderer>().enabled = true;
        
    }

    void AddColorsToColorsListAndRandomize(bool isRandomized=true)
    {
        colors = new List<Color>{pink, green, yellow, lightBlue};
        shapes = new List<GameObject>{trianglePrefab, squarePrefab, circlePrefab, rhombusPrefab};

        colors_random_order = new List<int>{0,1,2,3};
        shapes_random_order = new List<int>{0,1,2,3};

        if (isRandomized)
        {
            colors_random_order = colors_random_order.OrderBy(a => rng.Next()).ToList();
            shapes_random_order = shapes_random_order.OrderBy(a => rng.Next()).ToList();
        }
    }

    void CreateMap()
    {
        int currentMapIndex = ExperimentManager.instance.currentMapIndex;
        int numberOfGoalstoCollect = mapLoader.maps[currentMapIndex].numberOfGoals;

        MapLoader.Map map = mapLoader.maps[currentMapIndex];
        string [,] mapMatrix = map.GetMapMatrix();

        for (int x = 0; x < mapLoader.gridSize; x++)
        {
            for (int y = 0; y < mapLoader.gridSize; y++)
            {
                if (mapMatrix [x,y] == "") //se è diverso da vuoto ("")
                    continue;
                
                Color color = Color.white;
                string firstCharacter = mapMatrix [x,y][0].ToString(); //0 intende che la B è il primo carattere da vedere

                int colors_random_order_chosen = -1;

                if (firstCharacter == "B")
                {
                    colors_random_order_chosen = colors_random_order[0];
                }
                else if (firstCharacter == "G") 
                {
                    colors_random_order_chosen = colors_random_order[1];
                }
                else if (firstCharacter == "Y") 
                {
                    colors_random_order_chosen = colors_random_order[2];
                }
                else if (firstCharacter == "R") 
                {
                    colors_random_order_chosen = colors_random_order[3];
                }

                color = colors[colors_random_order_chosen];

                GameObject prefab = null;
                string secondCharacter = mapMatrix [x,y][1].ToString(); //1 intende che la B è il primo carattere da vedere

                int shapes_random_order_chosen = -1;

                if (secondCharacter == "T") 
                {
                    shapes_random_order_chosen = shapes_random_order[0];
                }
                else if (secondCharacter == "S") 
                {
                    shapes_random_order_chosen = shapes_random_order[1];
                }
                else if (secondCharacter == "C") 
                {
                    shapes_random_order_chosen = shapes_random_order[2];
                }
                 else if (secondCharacter == "R") 
                {
                    shapes_random_order_chosen = shapes_random_order[3]; 
                }
                prefab = shapes[shapes_random_order_chosen];

                prefab.GetComponent<Landmark>().ChangeColor(color);
                
                Landmark landmarkOnGrid = Instantiate(prefab, new Vector2(y,(mapLoader.gridSize - 1.6f) -x), prefab.transform.rotation).GetComponent<Landmark>(); //prefab.transform.rotation mi mantiene la rotazione dell'oggetto così come è su unity
                landmarkOnGrid.audioManager = audioManager;
                landmarkOnGrid.mapManager = this; 
                landmarkOnGrid.color = colors_random_order_chosen;
                landmarkOnGrid.shape = shapes_random_order_chosen;
                landmarkOnGrid.x = y;
                landmarkOnGrid.y = Mathf.FloorToInt((mapLoader.gridSize - 1.6f) -x) + 1;

                if (map.goals.Contains(mapMatrix[x,y]))
                {
                    landmarkOnGrid.isGoalReal = true;
                    goalIndex = map.goals.IndexOf(mapMatrix[x,y]); //Gli dico prendimi i goals nella mappa che si trovano nella matrice
                    landmarkOnGrid.SetGoalStatus(); //SetGoalStatus l'ho definito come public in Landmark.cs 
                } 
                
            }
        }   
    }
}




    
