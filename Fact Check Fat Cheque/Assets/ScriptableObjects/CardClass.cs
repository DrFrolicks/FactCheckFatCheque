using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum ProfitLevel {LOW, MEDIUM, HIGH}

[CreateAssetMenu(fileName = "Card")]
public class CardClass : ScriptableObject
{
    public string headline, tagline; 

    [TextArea(15, 20)]
    public string excerpts, comments;

    public List<string> tags;
    public bool cheat; 
    public ProfitLevel profitLevel;

}