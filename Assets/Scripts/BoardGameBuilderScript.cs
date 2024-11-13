using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class BoardGameBuilderScript : MonoBehaviour
{
    [SerializeField] GridLayoutGroup gridLayoutGroup;
    [SerializeField] CardScript cardPrefabScript;
    [SerializeField] List<Sprite> possibleCardSprites;

    [SerializeField] RectTransform targetContainer;

    public List<CardScript> CreateBoard(int rows, int columns, Action<CardScript> cardSelectedCallback, Action<SoundManager.AudioType> cardFlipAudioCallback)
    {
        if(!CheckForInputValidity(rows) || !CheckForInputValidity(columns) || !CheckForNumberOfCardsValidity(rows,columns))
        {
            Debug.LogError($"Invalid input for lines and columns: {rows} by {columns}");
            return null;
        }

        ScaleGameBoard(rows, columns);

        List<CardScript> retVal = new List<CardScript>();

        int cardsToCreate = rows * columns;
        int spritesToPick = cardsToCreate / 2;

        List<int> cardsSpriteIndexes = SelectCardSpriteIndexes(spritesToPick);
        if (cardsSpriteIndexes == null)
            return null;

        //List<int> cardNumbers = CreateCardNumberList(cardsToCreate);



        for (int i = 0; i < cardsToCreate; i++)
        {
            int index = UnityEngine.Random.Range(0, cardsSpriteIndexes.Count);
            int cardId = cardsSpriteIndexes[index];
            cardsSpriteIndexes.RemoveAt(index);
            Debug.Log($"Created with cardId:{cardId}");
            CardScript instantiatedCard = Instantiate(cardPrefabScript, gridLayoutGroup.transform);
            instantiatedCard.Init(cardId, possibleCardSprites[cardId], cardSelectedCallback, cardFlipAudioCallback);
            retVal.Add(instantiatedCard);
        }

        return retVal;
    }

    public List<CardScript> RestoreBoard(SaveData saveData, Action<CardScript> cardSelectedCallback, Action<SoundManager.AudioType> cardFlipAudioCallback)
    {
        List<CardScript> retVal = new List<CardScript>();

        if (!CheckForInputValidity(saveData.rows) || !CheckForInputValidity(saveData.columns) || !CheckForNumberOfCardsValidity(saveData.rows, saveData.columns))
        {
            Debug.LogError($"Invalid input for lines and columns: {saveData.rows} by {saveData.columns}");
            return null;
        }

        ScaleGameBoard(saveData.rows, saveData.columns);

        foreach(int cardId in saveData.cardsIds)
        {
            CardScript instantiatedCard = Instantiate(cardPrefabScript, gridLayoutGroup.transform);
            instantiatedCard.Init(cardId, possibleCardSprites[cardId], cardSelectedCallback, cardFlipAudioCallback);
            retVal.Add(instantiatedCard);
            instantiatedCard.SetCardFlipped();
        }

        foreach(int matchedCardIndex in saveData.cardsMatchedIndex)
        {
            retVal[matchedCardIndex].SetCardAsMatched();
        }

        return retVal;
    }

    private void ScaleGameBoard(int rows, int columns)
    {
        float scalingFactor = CalculateScalingFactor(rows, columns);
        Debug.Log($"Scaling factor found: {scalingFactor}");

        gridLayoutGroup.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
        gridLayoutGroup.constraintCount = columns;
        gridLayoutGroup.cellSize = gridLayoutGroup.cellSize * scalingFactor;
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

    private List<int> SelectCardSpriteIndexes(int spriteNumber)
    {
        if(spriteNumber > possibleCardSprites.Count)
        {
            Debug.LogError($"Not enough sprites to create board, resquested {spriteNumber} sprites");
            return null;
        }

        List<int> retVal = new List<int>();
        for(int i = 0; i < spriteNumber; i++)
        {
            int selectedSpriteIndex = UnityEngine.Random.Range(0, possibleCardSprites.Count);

            //Randomly selected sprite has already been selected, we try again
            if(retVal.Contains(selectedSpriteIndex))
            {
                i--;
                continue;
            }
            //We add twice here to have two matching cards
            retVal.Add(selectedSpriteIndex);
            retVal.Add(selectedSpriteIndex);
        }
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
