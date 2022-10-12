using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class CatBehavior : MonoBehaviour
{
    public Sprite restSprite;
    public Sprite awakeSprite;
    public Sprite actionSprite;

    public bool catActive = true; //Whether the cat can still act this brew.
    [Tooltip("Should be a number from 0 to 100")] public float catChance = 15; //The chance of the cat activating


    private bool inActPosition = false;
    public float phaseDelay = 0.5f; //The amount of time between animation frames

    public List<GameObject> actPositions;
    //public GameObject[] actPositions;
    public String[] actIngredients;
    public Vector3 originalPosition;
    
    private float timer = 0;
    private bool kickoff = false; //Whether the cat is acting.
    private bool drop = false;

    public Brew manager;


    private void Start()
    {
        originalPosition = transform.position;
    }

    private void Update()
    {
        if (kickoff)
        {
            timer += Time.deltaTime;
            if (timer >= phaseDelay && !inActPosition) //When the timer is up and you aren't in acting position...
            {
                inActPosition = true;
                timer = 0;
            }
            else if (timer >= phaseDelay && inActPosition)
            {
                inActPosition = false;
                kickoff = false;
                timer = 0;
            }

        }

        if (inActPosition && drop)
        {
            int selection = Random.Range(0, actPositions.Count);
            transform.position = actPositions[selection].transform.position;
            manager.ButtonInput(actIngredients[selection]);
            drop = false;
        }

        if (!inActPosition && !drop)
        {
            drop = true;
            transform.position = originalPosition;
            manager.ritualUI.SetActive(true);
            gameObject.GetComponent<SpriteRenderer>().sprite = restSprite;
        }
        
    }


    public bool CatTest()
    {
        if (!catActive) //If the cat has already activated this brew...
        {
            return false; //Don't do anything.
        }

        float test = Random.Range(0, 100); //Generate a random number between 0 and 100

        if (test <= catChance) //If it's less than the cat activation chance...
        {
            gameObject.GetComponent<SpriteRenderer>().sprite = awakeSprite;
            return true;
        }

        return false; //If it doesn't activate then return false
    }

    public void Cat()
    {
        manager.ritualUI.SetActive(false); //Deactivate input
        kickoff = true;
        catActive = false; //Set the cat to inactive.
    }
}
