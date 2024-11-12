using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class MemoryGameManager : MonoBehaviour
{
    [SerializeField] BoardGameBuilderScript boardGameBuilderScript;
    [SerializeField] ScoreManager scoreManager;

    [SerializeField] GameObject gameMenuObject;
    [SerializeField] TMP_InputField rowInputField;
    [SerializeField] TMP_InputField columnInputField;
    [SerializeField] GameObject errorMessageObject;

    [SerializeField] GameObject congratulationsObject;
    [SerializeField] TMP_Text congratulationsText;

    private List<CardScript> allCards;
    private List<CardScript> selectedCards = new List<CardScript>();
    private int cardsMatched = 0;

    [SerializeField] float firstPeekTime = 1.5f;

    // Start is called before the first frame update
    void Start()
    {
        errorMessageObject.SetActive(false);
        gameMenuObject.SetActive(true);
        //Call card grid creator here
    }

    public void OnGameStart()
    {
        int row, column;
        if(!int.TryParse(rowInputField.text, out row) || !int.TryParse(columnInputField.text, out column))
        {
            errorMessageObject.SetActive(true);
            return;
        }
        allCards = boardGameBuilderScript.CreateBoard(row, column, OnCardSelected);
        if(allCards == null)
        {
            errorMessageObject.SetActive(true);
            return;
        }

        gameMenuObject.SetActive(false);
        errorMessageObject.SetActive(false);

        StartCoroutine(FirstPeekCoroutine());

    }

    public IEnumerator FirstPeekCoroutine()
    {
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
            cardsMatched += 2;
            if(cardsMatched >= allCards.Count) 
            {
                FinishGame();
            }
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

    public void FinishGame()
    {
        congratulationsObject.SetActive(true);
        congratulationsText.SetText($"Your score was: {scoreManager.GetScore()} points, want to go for another game?");
    }

    public void Restart()
    {
        cardsMatched = 0;
        congratulationsObject.SetActive(false);
        scoreManager.Reset();
        selectedCards.Clear();
        foreach(CardScript card in allCards)
        {
            Destroy(card.gameObject);
        }
        allCards.Clear();
        gameMenuObject.SetActive(true);
    }

}
