using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardManager : MonoBehaviour {
    public static CardManager instance; 

    public List<CardClass> CardStackQueue, ApprovedCards, RejectedCards;
    public GameObject CardPrefab, CardQueue;

    public List<string> TagsPublished; 
   
    private void Awake()
    {
        if (instance != null)
            Destroy(gameObject); 
        else
            instance = this;

        TagsPublished = new List<string>();
    }

    private void Start()    
    {
        spawnCards();
    }

    public void spawnCards()
    {
        Transform cardQueue = transform.GetChild(0);
        foreach (CardClass card in CardStackQueue)
        {
            GameObject instantiatedCard = Instantiate(CardPrefab, cardQueue);

            instantiatedCard.transform.GetChild(0).GetComponent<Text>().text = card.headline;
            instantiatedCard.transform.GetChild(1).GetComponent<Text>().text = card.excerpts;
        }
    }

    int cheatCardsPublished = 0; 
    public void judgeCard(bool approve)
    {
        if(approve)
        {
            ApprovedCards.Add(CardStackQueue[0]);
            foreach(string str in CardStackQueue[0].tags)
            {
                TagsPublished.Add(str);
                GameObject.Find("Tags").GetComponent<Text>().text += str;  
            }
            if (CardStackQueue[0].cheat)
                cheatCardsPublished++;

            GameObject.Find("Score").GetComponent<Text>().text = "" + cheatCardsPublished);
        }
        else
        {
            RejectedCards.Add(CardStackQueue[0]);
        }

        Destroy(CardQueue.transform.GetChild(CardQueue.transform.childCount - 1).gameObject); 
        CardStackQueue.RemoveAt(0); 
    }

 
   
    private void Update()   
    {
         
        if (Input.GetKeyDown(KeyCode.LeftArrow))
            judgeCard(true);
        if (Input.GetKeyDown(KeyCode.RightArrow))
            judgeCard(false); 
    }

}
