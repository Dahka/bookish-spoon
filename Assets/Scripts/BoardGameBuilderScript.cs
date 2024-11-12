using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BoardGameBuilderScript : MonoBehaviour
{
    [SerializeField] GridLayoutGroup gridLayoutGroup;
    [SerializeField] CardScript cardPrefabScript;
    [SerializeField] List<Sprite> possibleCardSprites;

    [SerializeField] RectTransform targetContainer;

    [SerializeField] List<Sprite> selectedCardSprites;

    public List<CardScript> CreateBoard(int rows, int columns, Action<CardScript> cardSelectedCallback, Action<SoundManager.AudioType> cardFlipAudioCallback)
    {
        if(!CheckForInputValidity(rows) || !CheckForInputValidity(columns) || !CheckForNumberOfCardsValidity(rows,columns))
        {
            Debug.LogError($"Invalid input for lines and columns: {rows} by {columns}");
            return null;
        }

        float scalingFactor = CalculateScalingFactor(rows, columns);
        Debug.Log($"Scaling factor found: {scalingFactor}");

        gridLayoutGroup.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
        gridLayoutGroup.constraintCount = columns;
        gridLayoutGroup.cellSize = gridLayoutGroup.cellSize * scalingFactor;

        List<CardScript> retVal = new List<CardScript>();

        int cardsToCreate = rows * columns;
        int spritesToPick = cardsToCreate / 2;

        List<Sprite> cardsSprites = SelectCardSprites(spritesToPick);
        //Debug, remove later
        selectedCardSprites = cardsSprites;

        List<int> cardNumbers = CreateCardNumberList(cardsToCreate);



        for (int i = 0; i < cardsToCreate; i++)
        {
            int cardNumber = cardNumbers[UnityEngine.Random.Range(0, cardNumbers.Count)];
            cardNumbers.Remove(cardNumber);
            int cardId = cardNumber / 2;
            Debug.Log($"Created with cardNumber:{cardNumber}, cardId:{cardId}");
            CardScript instantiatedCard = Instantiate(cardPrefabScript, gridLayoutGroup.transform);
            instantiatedCard.Init(cardId, cardsSprites[cardId], cardSelectedCallback, cardFlipAudioCallback);
            retVal.Add(instantiatedCard);
        }

        return retVal;
    }

    private float CalculateScalingFactor(int rows, int columns)
    {
        float targetSizeX = targetContainer.rect.width;
        float predictedSizeX = gridLayoutGroup.cellSize.x * columns;
        float scaleX = targetSizeX / predictedSizeX;
        Debug.Log($"X info targetSizeX:{targetSizeX} predictedSizeX:{predictedSizeX} scaleX:{scaleX}");

        float targetSizeY = targetContainer.rect.height;
        float predictedSizeY = gridLayoutGroup.cellSize.y * rows;
        float scaleY = targetSizeY / predictedSizeY;
        Debug.Log($"Y info targetSizeY:{targetSizeY} predictedSizeY:{predictedSizeY} scaleY:{scaleY}");

        return MathF.Min(scaleX, scaleY);

    }

    private List<Sprite> SelectCardSprites(int spriteNumber)
    {
        if(spriteNumber > possibleCardSprites.Count)
        {
            Debug.LogError($"Not enough sprites to create board, resquested {spriteNumber} sprites");
        }

        List<Sprite> retVal = new List<Sprite>();
        for(int i = 0; i < spriteNumber; i++)
        {
            Sprite selectedSprite = possibleCardSprites[UnityEngine.Random.Range(0, possibleCardSprites.Count)];

            //Randomly selected sprite has already been selected, we try again
            if(retVal.Contains(selectedSprite))
            {
                i--;
                continue;
            }
            retVal.Add(selectedSprite);
        }
        return retVal;
    }

    private List<int> CreateCardNumberList(int cardNumber)
    {
        List<int> retVal = new List<int>();
        for(int i = 0; i < cardNumber; i++)
            retVal.Add(i);
        return retVal;
    }

    public bool CheckForInputValidity(int value)
    {
        if (value < 1 || value > 20)
            return false;
        return true;
    }

    private bool CheckForNumberOfCardsValidity(int lines, int columns)
    {
        if ((lines * columns) % 2 != 0)
            return false;
        return true;
    }
}
