using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardScript : MonoBehaviour
{

    private int id;

    [SerializeField] private Image cardImage;
    [SerializeField] private GameObject cardBack;    
    
    private Action<CardScript> cardSelectionCallback;
    private Action<SoundManager.AudioType> cardFlipAudioCallback;

    private bool isCardFlipped = false;
    private bool canBeSelected = false;
    private bool isMatched = false;

    [SerializeField] private Animator animator;

    //Initialization function for the card
    public void Init(int _id, Sprite cardSprite, Action<CardScript> _cardSelectionCallback, Action<SoundManager.AudioType> _cardFlipAudioCallback)
    { 
        id = _id;
        cardImage.sprite = cardSprite;
        cardSelectionCallback = _cardSelectionCallback;
        cardFlipAudioCallback = _cardFlipAudioCallback;
    }

    //OnClick function for the card
    public void SelectCard()
    {
        if (!canBeSelected)
            return;

        canBeSelected = false;
        //Play card flip sound
        cardFlipAudioCallback.Invoke(SoundManager.AudioType.CardFlip);
        //Start flip up animation
        animator.SetTrigger("FlipUp");
    }

    //Function for turning the card back down
    public void DeselectCard()
    {
        //Start flip down animation
        animator.SetTrigger("FlipDown");
    }

    //When the card is done turning up, called on the animator
    public void OnCardFinishSelectingAnimation()
    {
        cardSelectionCallback.Invoke(this);
    }

    //When the card is turned back down, called on the animator
    public void OnCardFinishDeselectingAnimation()
    {
        Debug.Log($"Finished deselection for card with id {id}");
        canBeSelected = true;
    }

    //Flip the faces of the card by activating and deactivating the card image, called on the animator
    public void FlipCard()
    {
        isCardFlipped = !isCardFlipped;
        cardBack.SetActive(isCardFlipped);
    }

    //Function for when the card has been matched
    public void ScoreCard()
    {
        //Play small animation
        animator.SetTrigger("Match");
        isMatched = true;
    }

    public int GetID()
    {
        return id;
    }

    public bool IsMatched()
    {
        return isMatched;
    }

    //Function for setting card stated after loading
    public void SetCardFlipped()
    {
        FlipCard();
        canBeSelected = true;
    }

    //Function for setting matched card stated after loading
    public void SetCardAsMatched()
    {
        canBeSelected = false;
        isMatched = true;
        gameObject.transform.localScale = Vector3.zero;
    }

}
