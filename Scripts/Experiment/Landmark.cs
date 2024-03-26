using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Landmark : MonoBehaviour
{
    [SerializeField] SpriteRenderer spriteRenderer;
    [SerializeField] Collider2D coll;
    [SerializeField] GameObject checkmarkPrefab;
    public AudioManager audioManager;
    
    public MapManager mapManager;

    public bool isGoal = false;
    public bool isFirstGoal = false;
    public int color;
    public int shape;
    public int positionInLegend;
    public int x;
    public int y;
    public bool isGoalReal = false;
    public SpriteRenderer outline;
    

    public void SetGoalStatus()
    {
        isGoal = true;
        positionInLegend = mapManager.goalIndex;
        
        if (mapManager.goalIndex == 0)
        {
            isFirstGoal = true;
            CreateFirstGoalOutline();
        }
        else
        {
            isFirstGoal = false;
            CreateOtherGoalOutline();
            outline.enabled = false;
        }
    }

    public void CreateFirstGoalOutline()
    {
        GameObject outlineObj = Instantiate(checkmarkPrefab, transform.position, Quaternion.identity);
        outline = outlineObj.GetComponent<SpriteRenderer>();
        outline.sortingOrder = -5;
        outline.color = Color.white;
        outline.transform.localScale = Vector3.one * 1f;

    }

    public void CreateOtherGoalOutline()
    {
        GameObject outlineObj = new GameObject();
        outlineObj.tag = "Outline";
        outline = outlineObj.AddComponent<SpriteRenderer>();
        outline.sprite = spriteRenderer.sprite;
        outline.sortingOrder = -5;
        outline.color = Color.white;
        outline.transform.position = spriteRenderer.transform.position;
        outline.transform.eulerAngles = spriteRenderer.transform.eulerAngles;
        outline.transform.localScale = spriteRenderer.transform.localScale * 1.2f; 

    }

    public void Darken()
    {
        Color color = Color.black;
        spriteRenderer.color = color;
        AudioManager audioManager = AudioManager.GetInstance();
        if (audioManager != null)
        {
            audioManager.PlaySound("success");
        }

        outline.gameObject.SetActive(false);
    }

    public void ChangeColor(Color newColor)
    {
        spriteRenderer.color = newColor;
    }



    void OnTriggerEnter2D(Collider2D other)
    {

        if (mapManager.firstGoalTaken == true && !isFirstGoal && isGoal)
        {
            isGoal = false;
            DataCollector.Instance.AddEvent("GoalTaken");

            Darken();
            ExperimentManager.instance.CollectGoal();

        }
        else if (isFirstGoal && isGoal)
        {
            isFirstGoal = false;
            DataCollector.Instance.AddEvent("GoalTaken");
            Darken();

            GameObject[] otherOutlines = GameObject.FindGameObjectsWithTag("Outline");
            for (int i = 0; i < otherOutlines.Length; i++) otherOutlines[i].GetComponent<SpriteRenderer>().enabled = true;

            mapManager.firstGoalTaken = true;
            ExperimentManager.instance.CollectGoal();
        }
    }

    internal void SetActive(bool v)
    {
        throw new NotImplementedException();
    }
}
