using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {
    public static GameManager instance;
    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this); 
    }

    public int roundNumber, roundsToWin; 

    public Dictionary<string, int> publishedTags;
    public int maxTagPublished; 
    
    public void endRound()
    {
    
        foreach (KeyValuePair<string, int> entry in publishedTags)
        {
            if(entry.Value >= maxTagPublished)
            {
                //placeholder
                Debug.Log(entry.Key + " GAME OVER");
                return;
            }
        }
        if(roundNumber >= roundsToWin)
        {
            Debug.Log("U WIN"); 
        }

    }
}
