using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using Proyecto26;
using TMPro;


public class ExperimentManager : MonoBehaviour
{
    public static ExperimentManager instance;
    

    public static ExperimentManager Instance
    {
        get
        {
            return instance;
        }
    }

    private MapLoader mapLoader;

    public int mapsNumber = 0;
    public int currentMapIndex = 0;
    public int completed_trials = 0;

    private TMP_Text timerText;

    private GameObject interTrialToppa;

    private GameObject afterTutorialToppa;

    public bool isInterTrial = false;

    private string playerUsername;
    private string playerRandomSeed;

    // Server su cui salvare i dati - ora non lo sto chiamando per non occupare inutilmente il database di cross the river ma poi lo chiamerò
    private string databaseURL = "https://hierarchicalplanning-default-rtdb.europe-west1.firebasedatabase.app/";

    public int collectedGoalsNumber = 0;

    public int numberOfGoalstoCollect = 0;

    private GameObject legenda2;
    private GameObject legenda3;
    private GameObject legenda4;
    

    public List<int> randomMapsOrder = new List<int>();
    public System.Random rng = new System.Random();

    private void Awake()
    {
        CreateRandomOrder();

        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject); // Permette all'oggetto "ExperimentManager" di persistere tra le scene senza essere distrutto durante il caricamento della nuova scena
            // Per gestire il fatto che posso far cominciare il gioco a partire da qualsiasi scena (per testarlo) devo
            // metterci una toppa
            Scene currentScene = SceneManager.GetActiveScene();
            if (currentScene.name == "Experiment")
            {
                LevelStarted();
            }
        }

    }
    
    public void TimeEnded()
    {        
        Debug.Log("Time Ended");

        DataCollector.Instance.AddEvent("LevelEnded_Failed");
        LevelEnded();
    }
    


   public void CollectGoal()
    {
        collectedGoalsNumber++;

        if (collectedGoalsNumber == mapLoader.maps[currentMapIndex].numberOfGoals)
        {
            DataCollector.Instance.AddEvent("LevelEnded_Success");
            completed_trials += 1;
            LevelEnded();
        }
    }

    void CreateRandomOrder()
    {
        List<int> tutorialMaps = new List<int>();
        List<int> experimentMaps = new List<int>();

        for (int i = 0; i < 44; i++)
        {
            if (i <= 3) tutorialMaps.Add(i);
            else experimentMaps.Add(i);
        }

        tutorialMaps = tutorialMaps.OrderBy(a => rng.Next()).ToList(); //QUI MISCHIA I TUTORIALS
        experimentMaps = experimentMaps.OrderBy(a => rng.Next()).ToList(); //QUI MISCHIA LE MAPPE

        foreach (int ind in experimentMaps)
        {
            tutorialMaps.Add(ind);
        }
        
        randomMapsOrder = tutorialMaps;
    }

  

    public IEnumerator LoadLevel()
    {
        Debug.Log(isInterTrial);

        SceneManager.LoadScene("Experiment");
        yield return null;
        LevelStarted();      
        isInterTrial = false;

        Debug.Log(isInterTrial);

    }

    public void LevelStarted()
    {
        Debug.Log("Livello iniziato");
        mapLoader = FindObjectOfType<MapLoader>();
        
        mapsNumber = mapLoader.maps.Count;  
        //mapsNumber = 5;
        // VALERIA CANCELLA QUESTA RIGA DOPO LA PROVA
        // CANCELLALA O VERRAI UCCISA!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
       
        timerText = GameObject.Find("Timer").GetComponent<TMP_Text>();

        interTrialToppa = GameObject.Find("InterTrialToppa");
    
        interTrialToppa.SetActive(false);

        afterTutorialToppa = GameObject.Find("AfterTutorialToppa");
    
        afterTutorialToppa.SetActive(false);

        collectedGoalsNumber = 0;

        CheckNumberOfGoals();

    }

    public void CheckNumberOfGoals()
    {
        legenda4 = GameObject.Find("Legenda4");
        
        numberOfGoalstoCollect = mapLoader.maps[currentMapIndex].numberOfGoals;
    
    }

    public void LevelEnded()
    {
        Debug.Log("Level Ended");
        // Qui devo dire al Timer che deve smettere di visualizzare il testo
        timerText.enabled = false;


        // Se posso, carico il livello successivo (tecnicamente carico sempre lo stesso, ma currentMapIndex cambia e il mapManager caricherà una mappa diversa)
        if (currentMapIndex + 1 >= mapsNumber)
        {
            // Salvo sul server
            DataCollector.Instance.CreateTrialInfo(mapLoader.maps[currentMapIndex].mapNumber, currentMapIndex);
            SaveTrialInfo();

            Debug.Log("Tutte le mappe sono state completate.");
            SceneManager.LoadScene("FinalScene");

        }
        else if (currentMapIndex == 3) //voglio che mi metta la AfterTutorialToppa dopo i primi 4 trials tutorial
        {
            afterTutorialToppa.SetActive(true);
            isInterTrial = true;
            DataCollector.Instance.AddEvent("InterTrialStarted");
        }
        else
        {
            interTrialToppa.SetActive(true);
            isInterTrial = true;
            DataCollector.Instance.AddEvent("InterTrialStarted");
        }


    }

    private void Update()
    {

        if (isInterTrial)
        {
            if (Input.GetKey(KeyCode.Return))
            {
                Debug.Log("Premuto Invio");

                DataCollector.Instance.AddEvent("InterTrialFinished");
                GoNextLevel();
            }
        }
        

    }


    public void GoNextLevel()
    {
        Debug.Log("Go next level");

        // Salvo sul server
        DataCollector.Instance.CreateTrialInfo(mapLoader.maps[currentMapIndex].mapNumber, currentMapIndex);
        SaveTrialInfo();

        // Incremento l'indice della mappa corrente
        currentMapIndex++;

        StartCoroutine(LoadLevel());
    }

    public void SetPlayerUsername(string username)
    {
        playerUsername = username;
        playerRandomSeed = UnityEngine.Random.Range(0,10000).ToString("D5"); // Numero decimale con 5 caratteri (inserisce leading zeros se necessario, es.12 ->  00012)
    }
    public void SavePlayerInfo()
    {
        string jsonString = JsonUtility.ToJson(DataCollector.Instance.playerInfo, true); //true lo metto per il pretty print

        //DALLA RIGA SOTTO HO LEVATO databaseURL, poi lo metterò così (databaseURL + playerUsername + "_" + playerRandomSeed + ecc.....)
        RestClient.Put<PlayerInfo>(databaseURL + playerUsername + "_" + playerRandomSeed + "/Info.json", jsonString) ///Info indica come si chiama il gruppo di info
        .Then(res => { Debug.Log("Successo!"); })
        .Catch(err => Debug.LogError(err.Message));
    }

    public void SaveTrialInfo()
    {
        string jsonString = JsonUtility.ToJson(DataCollector.Instance.trialInfo, true);
        string trialOrder = DataCollector.Instance.trialInfo.mapPresentationOrder.ToString();

        //DALLA RIGA SOTTO HO LEVATO databaseURL, poi lo metterò così (databaseURL + playerUsername + "_" + playerRandomSeed + ecc.....)
        RestClient.Put<TrialInfo>(databaseURL + playerUsername + "_" + playerRandomSeed + "/Trials/Trial_" + trialOrder + ".json", jsonString) ///Info indica come si chiama il gruppo di info
        .Then(res => { Debug.Log("Successo!"); })
        .Catch(err => Debug.LogError(err.Message));   
    }

    public void SaveFinalInfo()
    {
        string jsonString = JsonUtility.ToJson(DataCollector.Instance.finalResults, true);

        RestClient.Put<TrialInfo>(databaseURL + playerUsername + "_" + playerRandomSeed + "/FinalResults.json", jsonString) ///Info indica come si chiama il gruppo di info
        .Then(res => { Debug.Log("Successo!"); })
        .Catch(err => Debug.LogError(err.Message));   
    }

}
