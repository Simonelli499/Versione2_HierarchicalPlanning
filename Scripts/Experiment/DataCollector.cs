using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using System.Globalization;

public class DataCollector : MonoBehaviour
{
    public static DataCollector Instance;

    public Transform player;

    public PlayerInfo playerInfo;
    public TrialInfo trialInfo;
    public FinalResults finalResults;

    List<string> events;


    bool timeIsRunning = true; // Se per qualche motivo serve stoppare il tempo, mettere false
    float time; // Tempo dall'inizio del livello

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
        }

        Instance = this;

        // Inizializzo la lista degli eventi
        events = new List<string>();

        // Inizializzo il tempo a zero
        time = 0;

        // non metto la parte di OnDestroyOnLoad() perché non voglio che ogni volta mi perda i dati e mi vada a riacchiappare dati che sono uguali per ogni trial (es. nome, ecc…)
    }

    private void Update()
    {
        if (timeIsRunning)
        {
            time += Time.deltaTime; // Aumento del tempo intercorso dall'ultimo fotogramma
        }
    }

    public void AddPlayerInfo(string username, int age, string nationality, string gender, string gamer)
    {
        playerInfo = new PlayerInfo()
        {
            username = username,
            age = age, // è uguale a scrivere playerInfo.age = age;
            nationality = nationality,
            gender = gender,
            gamer = gamer
        };
    }


    public void CreateFinalResults(int score, string code)
    {
        finalResults = new FinalResults()
        {
            score = score,
            code = code
        };
    }


    // Il tipo di evento dipende dall'oggetto che chiama questa funzione esternamente (es. StartLevel, Movement, ec..)
    // mentre posizione del player e tempo vengono richiesti direttamente dal DataCollector
    public void AddEvent(string eventType)
    {
        string timeString = time.ToString("F3", CultureInfo.InvariantCulture); //F3 vuol dire che è un float di cui salvare solo 3 cifre decimali, cultureinvariant vuol dire che mette sempre il punto come separatore decimale
        string positionString = Mathf.FloorToInt(player.position.x) + ";" +  Mathf.FloorToInt(player.position.y + 0.61f);
        string eventNew = timeString + ";" + positionString + ";" + eventType;
        events.Add(eventNew);
    }

   public void AddFinalEvent(string eventType)
    {
        string timeString = time.ToString("F3", CultureInfo.InvariantCulture); //F3 vuol dire che è un float di cui salvare solo 3 cifre decimali, cultureinvariant vuol dire che mette sempre il punto come separatore decimale
        string eventNew = timeString + ";" + eventType;
        events.Add(eventNew);
    }

    public List<LandmarkInfo> CreateLandmarkInfo()
    {
        Landmark[] allLandmarks = GameObject.FindObjectsOfType<Landmark>();
        List<LandmarkInfo> landmarkInfos = new List<LandmarkInfo>();

        for (int i = 0; i < allLandmarks.Length; i++)
        {

            Landmark land = allLandmarks[i];

            LandmarkInfo info = new LandmarkInfo()
            {
                x = land.x,
                y = land.y,
                color = land.color,
                shape = land.shape,
                isGoal = land.isGoalReal,
                goalIndex = land.positionInLegend                    
            };
            landmarkInfos.Add(info);
        
        }
        Debug.Log(landmarkInfos.Count);

        return landmarkInfos;
    }

    public void CreateTrialInfo(int mapIdentificationNumber, int mapPresentationOrder)
    {

        List<LandmarkInfo> landmarkInfo = CreateLandmarkInfo();

        trialInfo = new TrialInfo()
        {
            mapIdentificationNumber = mapIdentificationNumber,
            mapPresentationOrder = mapPresentationOrder,
            events = events,
            landmarks = landmarkInfo
        };
    }


}

[Serializable]
public struct PlayerInfo
{
    public string username;
    public int age;
    public string nationality; 
    public string gender;
    public string gamer;
}

[Serializable]
public struct TrialInfo
{
    public int mapIdentificationNumber; //numero associato ad ogni mappa
    public int mapPresentationOrder; 
    public List<string> events;
    public List<LandmarkInfo> landmarks;
}


[Serializable]
public struct LandmarkInfo
{
    public int x;
    public int y;
    public int color;
    public int shape;
    public bool isGoal;
    public int goalIndex;
}

[Serializable]
public struct FinalResults
{
    public int score; //numero associato ad ogni mappa
    public string code; 
}
