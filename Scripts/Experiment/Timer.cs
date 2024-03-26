using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Timer : MonoBehaviour
{
    private float availableTime = 30f;
    public TMP_Text timerText;

    private bool isTimeFinished = false;

    private void Update()
    {
        availableTime -= Time.deltaTime;
        //nella riga a seguire mi assicuro che il assicura che availableTime non scenda sotto allo zero e non superi l'infinito. 
        availableTime = Mathf.Clamp(availableTime, 0f, Mathf.Infinity); //tra parentesi ci sono in ordine: il valore da limitare, il valore minimo consentito e il valore massimo consentito. 
        TimeShow();


        // Qui mettere un controllo, se il tempo è finito, dice all'experiment manager di chiamare TimeEnded
        if (!isTimeFinished && availableTime == 0 && ExperimentManager.Instance.isInterTrial == false)
        {
            ExperimentManager.Instance.TimeEnded();
            isTimeFinished = true;
        }
        
    }

    private void TimeShow()
    {
        int minutes = Mathf.FloorToInt(availableTime / 60);
        int seconds = Mathf.FloorToInt(availableTime % 60);
        timerText.text = Mathf.FloorToInt(availableTime).ToString();

        //timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }
}
