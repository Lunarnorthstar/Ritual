using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Brew : MonoBehaviour
{
    
    [SerializeField][Tooltip("The correct recipe for the potion")] private string[] solutionRecipe = new []{"White", "Green", "Stir", "Red", "White"};
    [Tooltip("The player's current input")] public string[] currentRecipe;
    private int step = 0; //The current step of the potion

    public string[] hintText = new[]
    {
        "White before Green \n", "Stir before Red \n", "White after Red \n", "Green second of all \n", 
        "Two whites? \n", "White sandwich! \n", "Stir in the middle... \n"
    }; //All the hint text. Make a new line after every one.
    public int hints = 4; //The number of displayed hints

    [Header("UI")] 
    public GameObject winUI;
    public GameObject loseUI;
    public GameObject instructionsUI;
    public GameObject ritualUI;
    
    // Start is called before the first frame update
    void Start()
    {
        currentRecipe = new string[solutionRecipe.Length]; //Set the empty fields in the current recipe to the solution recipe's length
    }

    // Update is called once per frame
    void Update()
    {
        if (step >= solutionRecipe.Length)
        {
            ritualUI.SetActive(false);
            if(recipeCompare())
            {
                winUI.SetActive(true);
            }
            else
            {
                loseUI.SetActive(true);
                currentRecipe = new string[solutionRecipe.Length]; //re-initialize the current recipe as a blank one
                step = 0; //Reset back to the first step
                hints++; //Get one more hint
            }
        }
    }

    public void ButtonInput(string input)
    {
        currentRecipe[step] = input;
        step++;
    }

    public void LoadInstructions()
    {
        instructionsUI.GetComponentInChildren<Text>().text = "";
        for (int i = 0; i < hints && i < hintText.Length; i++)
        {
            instructionsUI.GetComponentInChildren<Text>().text += hintText[i];
        }
    }

    bool recipeCompare()
    {
        for (int i = 0; i < solutionRecipe.Length; i++)
        {
            if (currentRecipe[i] != solutionRecipe[i]) //Compare each entry (comparing the arrays as a whole doesn't work)
            {
                return false; //If any are different, return false
            }
        }

        return true; //If they are all the same, return true.
    }
}
