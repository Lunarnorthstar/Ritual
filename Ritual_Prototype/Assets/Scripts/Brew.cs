using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Brew : MonoBehaviour
{
    
    [Tooltip("The player's current input")] public string[] currentRecipe;
    public Text cauldronDisplay;
    private int step = 0; //The current step of the potion

    public PotionRecipeStorage[] allRecipes; //All the potion recipes. These are Scriptable Objects
    private int longestRecipe = 0; //The amount of steps in the longest recipe

    [SerializeField] private int viewedPage = 0; //The potion you are viewing info for
    

    [Header("UI")] 
    public GameObject winUI;
    public GameObject loseUI;
    public GameObject instructionsUIHints; //The hints text
    public GameObject instructionsUIName; //The title text
    public GameObject ritualUI;

    public CatBehavior cat;
    private bool doesCat = false; //If the cat will act next step
    
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < allRecipes.Length; i++) //Run through all the recipes...
        {
            if (allRecipes[i].steps.Length > longestRecipe) //For each recipe, if it is longer than the previous longest...
            {
                longestRecipe = allRecipes[i].steps.Length; //Set that recipe's length as the longest.
            }
        }
        
        currentRecipe = new string[longestRecipe]; //Set the empty fields in the current recipe to the longest recipe's length
    }

    // Update is called once per frame
    void Update()
    {
        if (step >= longestRecipe) //If you've done a number of steps equal to the longest recipe...
        {
            recipeCompare(); //Automatically test for valid potions.
        }

        cauldronDisplay.text = "";
        for (int i = 0; i < currentRecipe.Length; i++)
        {
            cauldronDisplay.text += currentRecipe[i] + " ";
        }
    }

    public void ButtonInput(string input) //Called by the ritual buttons. Adds to the cauldron.
    {
        currentRecipe[step] = input; //Take the input from the button and add it to the array at the current step...
        step++; //Increment the step count by one.

        if (step < longestRecipe-1) //If you aren't done with the potion...
        {
            if (!doesCat) //If the cat isn't about to activate...
            {
                doesCat = cat.CatTest(); //Check to see if the cat will activate.
            }
            else if (doesCat) //If the cat is about to activate...
            {
                cat.Cat(); //Activate the cat.
                doesCat = false;
            }
        }
        
    }

    public void LoadInstructions() //Updates the hint UI text to display proper hints for each potion.
    {
        instructionsUIName.GetComponent<Text>().text = allRecipes[viewedPage].potionName; //Update the name UI to display the name of the potion you are viewing
        instructionsUIHints.GetComponent<Text>().text = ""; //Clear the hint section
        for (int i = 0; i < allRecipes[viewedPage].hintCount && i < allRecipes[viewedPage].hints.Length; i++) //For each available hint, if you've unlocked it and it exists...
        {
            instructionsUIHints.GetComponent<Text>().text += allRecipes[viewedPage].hints[i] + "\n"; //Add it to the hint section and start a new line.
        }
    }

    public void recipeCompare() //The logic for checking for valid potions.
    {
        ritualUI.SetActive(false); //Close the addition UI to reduce clutter in subsequent user input steps
        
        int recipeHit = -1; //This variable stores the potion that has been completed. Starts at a dummy "none" value.
        
        for (int i = 0; i < allRecipes.Length; i++) //For each recipe...
        {
            bool failed = false; //This bool detects if the recipe is accurate to what's in the cauldron
            for (int j = 0; j < allRecipes[i].steps.Length; j++) //For each step in that recipe...
            {
                if (currentRecipe[j] != allRecipes[i].steps[j]) //Compare each entry (comparing the arrays as a whole doesn't work). If it's wrong...
                {
                    Debug.Log("Failed " + i + " at " + j); //Log what failed and where.
                    failed = true; //Set the bool
                    break; //Stop checking this potion.
                }
            }

            if (!failed) //If all steps in the recipe matched the cauldron...
            {
                recipeHit = i; //Set the successful recipe tracker to the given recipe.
            }
        }
        
        if (recipeHit != -1) //If any potion succeeded...
        {
            winUI.SetActive(true); //Activate the win UI...
            winUI.GetComponentInChildren<Text>().text = "You brewed the " + allRecipes[recipeHit].potionName; //Display the brewed potion in the UI...
            allRecipes[recipeHit].complete = true; //Mark the given potion as complete (carries over between plays, currently no use)
            ResetCauldron(); //Clear the player's cauldron.
        }
        else //If no potion recipes match the player's cauldron...
        {
            loseUI.SetActive(true); //Activate the failure UI...
            ResetCauldron(); //And clear the player's cauldron.
            //hints++; //Get one more hint
        }
    }

    void ResetCauldron() //Clear's the player's inputs and resets the potion making process.
    {
        currentRecipe = new string[longestRecipe]; //re-initialize the current recipe as a blank one
        step = 0; //Reset back to the first step
        cat.catActive = true;
        doesCat = false;
    }

    public void switchHintView(int i) //Switches the potion hints you are viewing. Input should be either 1 or -1.
    {
        if (viewedPage + i < allRecipes.Length && viewedPage + i >= 0) //If the current index plus the input is within the bounds of available potions...
        {
            viewedPage += i; //Apply the change.
        }
        //Otherwise, don't.
    }
}
