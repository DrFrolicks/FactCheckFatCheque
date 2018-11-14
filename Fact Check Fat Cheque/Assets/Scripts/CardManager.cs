using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

//to remove later
using UnityEngine.SceneManagement; 

public class CardManager : MonoBehaviour {
    public static CardManager instance; 

    public List<CardClass> CardStackQueue, ApprovedCards, RejectedCards;
    public GameObject CardPrefab, CardQueue;

    public List<string> TagsPublished;

    List<CardClass> UnusedLegitCards, UnusedCheatCards; 


    private void Awake()
    {
        
        if (instance != null)
            Destroy(gameObject); 
        else
            instance = this;

        loadCards();  
    }

    private void Start()    
    {
        startNewSet();
    }

    public void spawnCards()
    {
        Transform cardQueue = transform.GetChild(0);
        foreach (CardClass card in CardStackQueue)
        {
            GameObject instantiatedCard = Instantiate(CardPrefab, cardQueue);

            instantiatedCard.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = card.headline;
            instantiatedCard.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = card.excerpts;
        }
    }
    
    public bool publishedCheatCard()
    {
        foreach(CardClass card in ApprovedCards)
        {
            if (card.cheat)
                return true;  
        }
        return false;  
    }

    //todo refactor 
    int cheatCardsPublished = 0; 
    public void judgeCard(bool approve)
    {
        //placeholder 
        if (CardStackQueue.Count < 1)
            GameManager.instance.endRound(); 
        
        if(approve)
        {
            ApprovedCards.Add(CardStackQueue[0]);
            foreach(string tagName in CardStackQueue[0].tags)
            {
                Dictionary<string, int> publishedTags = GameManager.instance.publishedTags;
                if(publishedTags.ContainsKey(tagName))
                    publishedTags[tagName]++;
                else
                {
                    publishedTags.Add(tagName, 1);
                }
            }
            if (CardStackQueue[0].cheat)
                cheatCardsPublished++;

            GameObject.Find("Score").GetComponent<TextMeshProUGUI>().text = "Cheat Cards Published: " + cheatCardsPublished;
        }
        else
        {
            RejectedCards.Add(CardStackQueue[0]);
        }

        Destroy(CardQueue.transform.GetChild(CardQueue.transform.childCount - 1).gameObject); 
        CardStackQueue.RemoveAt(0); 
    }

    public void startNewSet()
    {
        ApprovedCards = new List<CardClass>();
        RejectedCards = new List<CardClass>();

        CardStackQueue.takeRandomElements<CardClass>(UnusedLegitCards, 3);
        CardStackQueue.takeRandomElements<CardClass>(UnusedCheatCards, 2);
        CardStackQueue.Shuffle<CardClass>();

        spawnCards();

    }

    private void loadCards()
    {
        UnusedLegitCards = new List<CardClass>(Resources.LoadAll<CardClass>("Cards/Legit"));
        UnusedCheatCards = new List<CardClass>(Resources.LoadAll<CardClass>("Cards/Cheat"));  
    }
 
   
    private void Update()   
    {

        if (Input.GetKeyDown(KeyCode.LeftArrow))
            judgeCard(false);
        if (Input.GetKeyDown(KeyCode.RightArrow))
            judgeCard(true); 
    }

}
