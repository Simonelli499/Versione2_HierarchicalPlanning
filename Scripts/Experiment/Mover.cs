using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mover : MonoBehaviour
{
 
  [SerializeField] float moveSpeed = 20f; //velocità del movimento del player (NB mi riferisco al player perchè lo scipt è attaccato a lui)
  float oneStep = 1f; //spazio tra una casella e l'altra, lunghezza tra uno step e l'altro = 1
  int gridSize = 9; //la griglia ha, sia da destra a sinistra che dal basso all'alto, uno span che va da 0 a 9 (ho 9 caselle, la griglia è 9x9)
  float moveTimer = 0f;

  float interactTimer = 0f;
  [SerializeField] float coolDown = 0.1f;


  BoxCollider2D boxColl;
  AudioManager audioManager;

     
    void Start()
    {
      //Landmark landmark = new Landmark (); //diversamente da get component "prendimi" questa cosa
      //landmark.isGoal = true;
      boxColl = GetComponent<BoxCollider2D>(); //accedo al box collider del player
      boxColl.enabled = false; //disattivo il box collider  
    }

    
    void Update()
    {
      MovePlayer(); //per gestire movimento player
      InteractLandmark(); //per gestire interazione del player con landmarks
    }

    void MovePlayer() //void quando non deve restituire nulla, nessun risultato
    {
      if (moveTimer > 0) // qui dico: se il player ha movetimer più grande di zero, e dunque se si sta ancora muovendo... allora ignorami tutto il resto che viene dopo quindi movimento con keys
      //Se il soggetto ad esempio muove il player a 0.3, questo è > 0 e allora...
      {
        moveTimer = moveTimer - Time.deltaTime; //uguale a scrivere moveTimer -= Time.deltaTime; ... e allora il timer è 0.3 - il tempo che ci vuole tra l'ultimo frame e il corrente (Time.deltaTime)...
        return; // return di solito si usa per restuire un valore. In questo caso lo uso per terminare prima la funzione prima che esegua il resto
      }

      //a seguire voglio che se il soggetto preme le key inserite e se non si trova fuori dalla griglia, avvengono le translate del player
      if (ExperimentManager.Instance.isInterTrial == false)
      {
        if (transform.position.y < gridSize - 1.6f && Input.GetKeyDown (KeyCode.UpArrow) || transform.position.y < gridSize - 1.6f && Input.GetKeyDown (KeyCode.W))
        {
          DataCollector.Instance.AddEvent("Movement_N"); //prima che si muove, alla riga successiva, voglio sapere dove è il player
          transform.Translate(0,oneStep,0);
        } 
        else if (transform.position.y > 0f && Input.GetKeyDown (KeyCode.DownArrow) || transform.position.y > 0f && Input.GetKeyDown (KeyCode.S))
        {
          DataCollector.Instance.AddEvent("Movement_S"); //prima che si muove, alla riga successiva, voglio sapere dove è il player
          transform.Translate(0,-oneStep,0);
        }
        else if (transform.position.x < gridSize - 1 && Input.GetKeyDown (KeyCode.RightArrow)|| transform.position.x < gridSize - 1 && Input.GetKeyDown (KeyCode.D))
        {
          DataCollector.Instance.AddEvent("Movement_E"); //prima che si muove, alla riga successiva, voglio sapere dove è il player
          transform.Translate(oneStep,0,0);
        }
        else if (transform.position.x > 0f && Input.GetKeyDown (KeyCode.LeftArrow)||transform.position.x > 0f && Input.GetKeyDown (KeyCode.A))
        {
          DataCollector.Instance.AddEvent("Movement_W"); //prima che si muove, alla riga successiva, voglio sapere dove è il player
          transform.Translate(-oneStep,0,0);
        }
        else
        {
          return; 
        }
        moveTimer = coolDown; //deve passare tot tempo ( = coolDown) da quando preme una delle keys scritte sopra prima che possa ripremerle

      }

    }

  void InteractLandmark()
  {
    if (interactTimer > 0)
      {
        interactTimer = interactTimer - Time.deltaTime; //uguale a interactTimer -= Time.deltaTime;
        return;
      }

    if (Input.GetKeyDown (KeyCode.Space))
    {
      if (ExperimentManager.Instance.isInterTrial == false)
      {
        DataCollector.Instance.AddEvent("PressedSpacebar");

      StartCoroutine (InteractLandmarkCoroutine());
      interactTimer = coolDown; //deve passare tot tempo ( = coolDown) da quanto preme space prima che possa ripremere space
      }
    }

  }

  IEnumerator InteractLandmarkCoroutine()
  {
    boxColl.enabled = true; //si attiva il box collider così la collisione è possibile
    yield return new WaitForSeconds(coolDown); // prima di disabilitare il box collider di nuovo aspetta tot...
    boxColl.enabled = false; //disabilita il box collider
  }
}

