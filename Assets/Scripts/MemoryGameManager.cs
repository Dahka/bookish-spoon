using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MemoryGameManager : MonoBehaviour
{
    [SerializeField] BoardGameBuilderScript boardGameBuilderScript;
    [SerializeField] ScoreManager scoreManager;

    private List<CardScript> allCards;
    private List<CardScript> selectedCards = new List<CardScript>();

    [SerializeField] float firstPeekTime = 1.5f;

    // Start is called before the first frame update
    void Start()
    {
        //Call card grid creator here
        allCards = boardGameBuilderScript.CreateBoard(4, 12, OnCardSelected);
        StartCoroutine(FirstPeekCoroutine());
    }

    public IEnumerator FirstPeekCoroutine()
    {
        //TODO set peek time as variable
        yield return new WaitForSeconds(firstPeekTime);
        foreach (CardScript card in allCards)
        {
            card.DeselectCard();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnCardSelected(CardScript card)
    {
        if (selectedCards.Contains(card)) 
        {
            Debug.LogError("Error: card is already on selected list");
            return;
        }

        selectedCards.Add(card);

        if(selectedCards.Count >= 2) 
        {
            CompareCards(selectedCards[0], selectedCards[1]);
        }
    }

    public void CompareCards(CardScript card1,  CardScript card2)
    {
        if(card1.GetID() == card2.GetID())
        {
            //Play match sound
            card1.ScoreCard();
            card2.ScoreCard();
            scoreManager.OnScore();
        }
        else
        {
            //Play no match sound
            card1.DeselectCard();
            card2.DeselectCard();
            scoreManager.OnMistake();
        }
        selectedCards.RemoveRange(0, 2);
    }

}
