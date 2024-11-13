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

    public void Init(int _id, Sprite cardSprite, Action<CardScript> _cardSelectionCallback, Action<SoundManager.AudioType> _cardFlipAudioCallback)
    { 
        id = _id;
        cardImage.sprite = cardSprite;
        cardSelectionCallback = _cardSelectionCallback;
        cardFlipAudioCallback = _cardFlipAudioCallback;
    }

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

    public void FlipCard()
    {
        isCardFlipped = !isCardFlipped;
        cardBack.SetActive(isCardFlipped);
    }

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

    public void SetCardFlipped()
    {
        FlipCard();
        canBeSelected = true;
    }

    public void SetCardAsMatched()
    {
        canBeSelected = false;
        isMatched = true;
        gameObject.transform.localScale = Vector3.zero;
    }

}
