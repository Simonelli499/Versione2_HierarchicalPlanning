using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; //I have to add it to make SceneManager work!!

public class Button : MonoBehaviour
{
    [SerializeField] int levelNumber;
    public void UseButton()
    {
        string nextSceneName = GetSceneNameFromBuildIndex(levelNumber);

        if (nextSceneName == "Experiment")
        {
            Debug.Log("FATTO");
            ExperimentManager.Instance.StartCoroutine(ExperimentManager.Instance.LoadLevel());
        }
        else
        {
            SceneManager.LoadScene(levelNumber); //SceneManager.LoadScene is used to lead the current scene, in this case I want that when it reloads it goes to the newGameLevel
            // l'1 tra parentesi indica la scena 1 nel build settings, in questo caso la successiva alla scena del livello1
        }
    }
    public string GetSceneNameFromBuildIndex(int index)
     {
        string scenePath = SceneUtility.GetScenePathByBuildIndex(index);
        string sceneName = System.IO.Path.GetFileNameWithoutExtension(scenePath);

        return sceneName;
     }
}

