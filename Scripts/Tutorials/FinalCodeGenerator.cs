using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FinalCodeGenerator : MonoBehaviour
{
  // Start is called before the first frame update
    public TMP_Text scoreText;
    public TMP_Text codeText;

    string code;
    int totalScore;

    void Start()
    {
      totalScore = ExperimentManager.Instance.completed_trials;

        // Genero il codice
        code = GenerateCode();

        codeText.text += code;
        scoreText.text = "You completed " + totalScore + "/" + ExperimentManager.Instance.mapsNumber  + " levels";

        DataCollector.Instance.CreateFinalResults(totalScore, code);
        ExperimentManager.Instance.SaveFinalInfo();

       // GameManager.Instance.SaveFinalData(results);
       // Bisogna mandare al server l'informazione sul codice
    }

    string GenerateCode()
    {
        //totalScore = ExperimentManager.Instance.starsCollected; 
        string hexValue = totalScore.ToString("X");
        return hexValue + "_42";

    }

}
