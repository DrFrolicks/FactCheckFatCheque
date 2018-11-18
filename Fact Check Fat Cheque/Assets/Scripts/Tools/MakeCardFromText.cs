using System; 
using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Collections.Generic;
 
public class MakeCardFromText : EditorWindow
{
    [MenuItem("Window/Custom Functions")]

    public static void ShowWindow()
    {
        GetWindow<MakeCardFromText>("Card Generation");
    }

    private void OnGUI()
    {
        if (GUILayout.Button("Generate Card Objects From Text"))
        {
            GenerateCardAssetsFromText();
        }
    }


    public static void GenerateCardAssetsFromText()
    {

        var rawCardsText = Resources.Load<TextAsset>("CardsText");
        string[] rawCards = rawCardsText.text.Split(new string[] { "~~" }, StringSplitOptions.None);
        rawCards = trimNewLines(rawCards); 
        
        int cardNumber = 0; 
        foreach(string cardSection in rawCards)
        {
            CardClass cardAsset = ScriptableObject.CreateInstance<CardClass>();
            if (isComment(cardSection))
                continue; 
            
            string[] cardField = cardSection.Split(new string[] { "@@" }, StringSplitOptions.None);
            cardField = trimNewLines(cardField); 

            cardAsset.headline = cardField[0];
            Debug.Log(cardSection);
            cardAsset.excerpts = cardField[1];
            cardAsset.comments = cardField[2];
            switch (cardField[3].ToUpper())
            {
                case "LOW":
                    cardAsset.profitLevel = ProfitLevel.LOW;
                    break;
                case "MED":
                    cardAsset.profitLevel = ProfitLevel.MEDIUM;
                    break;
                case "HIGH":
                    cardAsset.profitLevel = ProfitLevel.HIGH;
                    break; 
            }
            cardAsset.cheat = cardField[4] != "LEGIT"; 
            if (cardAsset.cheat)
            {
                cardAsset.tags = new List<string>();
                cardAsset.tags.Add(cardField[4]);
                
            } 
            string assetDirectory = cardAsset.cheat ? "Assets/Resources/Cards/Cheat/" + cardNumber + ".asset" :
                                                        "Assets/Resources/Cards/Legit/" + cardNumber + ".asset"; 
            AssetDatabase.CreateAsset(cardAsset, assetDirectory);
            cardNumber++; 
        }
        AssetDatabase.SaveAssets();
        EditorUtility.FocusProjectWindow();
    }

    //helper functions for raw text parsing
    static bool isComment(string section)
    {
        string[] lines = section.Split( new[] { Environment.NewLine }, StringSplitOptions.None);
        foreach (string line in lines)
        {
            if (line.StartsWith("//"))
                return true;
        }
        return false; 
    }

    static string[] trimNewLines(string[] rawStringArray)
    {
        string[] trimedArray = rawStringArray; 
        for(int i = 0; i < trimedArray.Length; i++)
        {
            trimedArray[i] = trimedArray[i].Trim('\r', '\n');
        }
        return trimedArray; 
    }
}