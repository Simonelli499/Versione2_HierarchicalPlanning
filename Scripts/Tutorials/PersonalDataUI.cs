using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement; //I have to add it to make SceneManager work!!

public class PersonalDataUI : MonoBehaviour
{
    public TMP_Text usernameField;
    public TMP_InputField ageField;
    public TMP_Text nationalityField;
    public TMP_Dropdown genderField;
    public TMP_Dropdown gamerField;
 
    [SerializeField] GameObject CantBeEmptyUsername;
    [SerializeField] GameObject AgeNotCorrectFormat;
    [SerializeField] GameObject CantBeEmptyNationality;
    [SerializeField] GameObject SelectAnOptionGender;
    [SerializeField] GameObject SelectAnOptionGamer;

    

    public void PressButton()
    {
        //questa funzione controlla se i campi del personal data sono tutti compilati, oltre a controllare se ha inserito cose giuste!
        //genderField.value > 0 perchè non voglio che il soggetto selezioni il primo campo/la prima opzione nel drop down, che è l'opzione vuota...

        // Controllo che vadano bene tutti i campi
        bool genderGood = false;
        if (genderField.value > 0)
        {
            genderGood = true;
        }


         bool gamergood = false;
        if (gamerField.value > 0)
        {
            gamergood = true;
        }


    
        int age = 0;
        bool ageCorrectFormat = int.TryParse(ageField.text, out age);
        

        bool prolificIDGood = false;
        bool nationalityGood = false;

        Debug.Log(usernameField.text.Length );

        if (usernameField.text.Length > 1)
        {
            prolificIDGood = true;
        }

        if (nationalityField.text.Length > 1)
        {
            nationalityGood = true;
        }



         if (!prolificIDGood)
        {
            // MOSTRO IL PROBLEMA CON name 
            Debug.LogWarning("Username non good");
            CantBeEmptyUsername.SetActive(true);
        }
        else
        {
            CantBeEmptyUsername.SetActive(false);
        }


    

        if (!ageCorrectFormat)
        {
            // MOSTRO IL PROBLEMA CON età - formato età non corretto
            Debug.LogWarning("Age non good");
            AgeNotCorrectFormat.SetActive(true);
            
        }
        else
        {
            AgeNotCorrectFormat.SetActive(false);

        }

        if (!nationalityGood)
        {
            CantBeEmptyNationality.SetActive(true);
        }
        else
        {
            CantBeEmptyNationality.SetActive(false);
        }

          // SE NON VANNO BENE, GLIELO SCRIVO IN QUALCHE MODO
        if (!genderGood)
        {
            // MOSTRO IL PROBLEMA CON gender - se premono il bottone prima di riempire il genere cambio il testo da can't be empty a devi selezionare un'opzione
            Debug.LogWarning("Gender non good");
            SelectAnOptionGender.SetActive(true);
        }
        else
        {
            SelectAnOptionGender.SetActive(false);
        }



            // SE NON VANNO BENE, GLIELO SCRIVO IN QUALCHE MODO
        if (!gamergood)
        {
            // MOSTRO IL PROBLEMA CON gender - se premono il bottone prima di riempire il genere cambio il testo da can't be empty a devi selezionare un'opzione
            Debug.LogWarning("Gamer non good");
            SelectAnOptionGamer.SetActive(true);
        }
        else
        {
            SelectAnOptionGamer.SetActive(false);
        }

        



        if (genderGood && gamergood && ageCorrectFormat && prolificIDGood)
            SaveDataAndAdvance();

    }

    public void SaveDataAndAdvance()
    {
        string gender = "";

        switch (genderField.value) // switch è come una sequenza di if
        {
            case 1: 
                gender = "Male";
                break;
            case 2: 
                gender = "Female";
                break;
            case 3: 
                gender = "Other";
                break;
            case 4:
                gender = "Prefer not to say";
                break;
        }

        string gamer = "";

         switch (gamerField.value)
        {
            case 1: 
                gamer = "< 1 h>";
                break;
            case 2: 
                gamer = "1-2 h";
                break;
            case 3: 
                gamer = "2-3 h";
                break;
            case 4:
                gamer = "3-4 h";
                break;
            case 5:
                gamer = "< 4 h";
                break;
        }


        string username = usernameField.text;
        string nationality = nationalityField.text;

        int age = 0;
        bool ageCorrectFormat = int.TryParse(ageField.text, out age);

        // if (!ageCorrectFormat)
        // {
        //     Debug.LogWarning("Età formato non corretto");
        // }
       // int age = int.Parse(ageField.text); //Parse mi permette di trasformare una stringa in intero
        
        ExperimentManager.Instance.SetPlayerUsername(username);
        DataCollector.Instance.AddPlayerInfo(username, age, nationality, gender, gamer);
        ExperimentManager.Instance.SavePlayerInfo();

        SceneManager.LoadScene("HowToPlay"); 

    }
}
