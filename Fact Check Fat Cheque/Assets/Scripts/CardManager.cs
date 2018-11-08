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
        TagsPublished = new List<string>();

    }

    private void Start()    
    {
        //placeholder card selection
        CardStackQueue.takeRandomElements<CardClass>(UnusedLegitCards, 3);
        CardStackQueue.takeRandomElements<CardClass>(UnusedCheatCards, 2);
        CardStackQueue.Shuffle<CardClass>();  

        spawnCards();
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

    //todo refactor 
    int cheatCardsPublished = 0; 
    public void judgeCard(bool approve)
    {
        if (CardStackQueue.Count < 1)
            SceneManager.LoadScene(0); 
        if(approve)
        {
            ApprovedCards.Add(CardStackQueue[0]);
            foreach(string str in CardStackQueue[0].tags)
            {
                TagsPublished.Add(str);
                GameObject.Find("Tags").GetComponent<TextMeshProUGUI>().text += str + "\n";  
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
