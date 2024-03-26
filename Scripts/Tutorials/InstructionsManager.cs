using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstructionsManager : MonoBehaviour
{
    // Serve a caricare per la prima volta il primo livello di gioco
    public void LoadFirstLevel()
    {
        ExperimentManager.Instance.StartCoroutine(ExperimentManager.Instance.LoadLevel());
    }
}
