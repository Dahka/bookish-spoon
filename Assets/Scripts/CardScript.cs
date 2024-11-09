using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardScript : MonoBehaviour
{

    private int id;
    [SerializeField] private Image cardImage;
    private Action<CardScript> cardSelectionCallback;

    private bool canBeSelected;

    public void Init(int _id, Sprite cardSprite, Action<CardScript> _cardSelectionCallback)
    { 
        id = _id;
        cardImage.sprite = cardSprite;
        cardSelectionCallback = _cardSelectionCallback;
    }

    public void SelectCard()
    {
        if (!canBeSelected)
            return;

        canBeSelected = false;
        //Play card flip sound
        //Start flip up animation
    }

    public void DeselectCard()
    {
        //Start flip down animation
    }

    //When the card is done turning up
    public void OnCardFinishSelectingAnimation()
    {
        cardSelectionCallback.Invoke(this);
    }

    //When the card is turned back down
    public void OnCardFinishDeselectingAnimation()
    {
        canBeSelected = true;
    }

    public void ScoreCard()
    {
        //Play small animation
    }

    public int GetID()
    {
        return id;
    }
}
