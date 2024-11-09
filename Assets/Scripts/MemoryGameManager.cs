using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MemoryGameManager : MonoBehaviour
{
    [SerializeField] BoardGameBuilderScript boardGameBuilderScript;

    private List<CardScript> selectedCards = new List<CardScript>();

    // Start is called before the first frame update
    void Start()
    {
        //Call card grid creator here
        boardGameBuilderScript.CreateBoard(4, 12, OnCardSelected);
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
            Score();
        }
        else
        {
            //Play no match sound
            card1.DeselectCard();
            card2.DeselectCard();
        }
        selectedCards.RemoveRange(0, 2);
    }

    private void Score()
    {
        //score and combo logic here
    }

}
