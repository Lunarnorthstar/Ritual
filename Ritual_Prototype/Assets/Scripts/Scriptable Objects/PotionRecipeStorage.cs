using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/PotionData", order = 1)]

public class PotionRecipeStorage : ScriptableObject
{
    public string potionName;
    
    [Header("All steps should be in proper noun case. THESE ARE CASE SENSITIVE!")]
    public string[] steps;
    
    public string[] hints;
    public int hintCount = 3;
    public bool complete = false;
}
