using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/PotionData", order = 1)]

public class PotionRecipeStorage : ScriptableObject
{
    public string potionName;
    public string potionDesc;
    
    [Header("All steps should be in proper noun case. THESE ARE CASE SENSITIVE!")]
    public string[] steps;
    
    public string[] hints;
    [Tooltip("The starting amount of hints")]   public int baseHintCount = 3;
    [Tooltip("The active amount of hints - no touching! Modify baseHintCount instead.")]  public int hintCount;
    public bool complete = false;

    [Tooltip("What this potion unlocks when brewed")] public string special;
}
