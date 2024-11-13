using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using UnityEngine.SceneManagement;

public class MemoryGameManager : MonoBehaviour
{
    [Header("Scripts References")]
    [SerializeField] BoardGameBuilderScript boardGameBuilderScript;
    [SerializeField] ScoreManager scoreManager;
    [SerializeField] SoundManager soundManager;

    [Header("Game Menu References")]
    [SerializeField] GameObject gameMenuObject;
    [SerializeField] TMP_InputField rowInputField;
    [SerializeField] TMP_InputField columnInputField;
    [SerializeField] GameObject errorMessageObject;

    [Header("Congratulations Menu References")]
    [SerializeField] GameObject congratulationsObject;
    [SerializeField] TMP_Text congratulationsText;

    [Header("Exit Menu References")]
    [SerializeField] GameObject exitMenu;

    [Header("Settings Variables")]
    [SerializeField] float firstPeekTime = 1.5f;

    private List<CardScript> allCards = new List<CardScript>();
    private List<CardScript> selectedCards = new List<CardScript>();
    private int cardsMatched = 0;
    private bool gameIsRunning = false;

    private int rows, columns;

    public const string SAVE_DATA_FILENAME = "save.data";

    // Start is called before the first frame update
    void Start()
    {
        errorMessageObject.SetActive(false);
        exitMenu.SetActive(false);
        gameMenuObject.SetActive(true);

        if(PlayerPrefs.GetInt("LoadGame", 0) == 1)
        {
            PlayerPrefs.SetInt("LoadGame", 0);
            LoadGame();
        }
    }

    public void OnGameStart()
    {
        if(!int.TryParse(rowInputField.text, out rows) || !int.TryParse(columnInputField.text, out columns))
        {
            errorMessageObject.SetActive(true);
            return;
        }
        allCards = boardGameBuilderScript.CreateBoard(rows, columns, OnCardSelected, soundManager.PlayAudio);
        if(allCards == null)
        {
            errorMessageObject.SetActive(true);
            return;
        }

        gameIsRunning = true;
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
            else
            {
                soundManager.PlayAudio(SoundManager.AudioType.CardMatch);
            }
        }
        else
        {
            //Play no match sound
            card1.DeselectCard();
            card2.DeselectCard();
            scoreManager.OnMistake();
            soundManager.PlayAudio(SoundManager.AudioType.CardMiss);
        }
        selectedCards.RemoveRange(0, 2);
    }

    public void FinishGame()
    {
        congratulationsObject.SetActive(true);
        congratulationsText.SetText($"Your score was: {scoreManager.GetScore()} points, want to go for another game?");
        soundManager.PlayAudio(SoundManager.AudioType.Victory);
        gameIsRunning = false;
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
        gameIsRunning = false;
    }

    public void SaveGame()
    {
        //Info to be saved: card list with image index
        //list of cards that were matched
        //score and multiplier

        SaveData save = new SaveData();
        save.cardsIds = GetCardsIds();
        save.cardsMatchedIndex = GetMatchedCardsIndexes();
        save.score = scoreManager.GetScore();
        save.multiplier = scoreManager.GetMultiplier();
        save.rows = rows;
        save.columns = columns;

        string savePath = Application.persistentDataPath + Path.DirectorySeparatorChar + SAVE_DATA_FILENAME;
        File.WriteAllText(savePath, JsonUtility.ToJson(save));
        Debug.Log($"Game sucessfully save at {savePath}");
    }

    public void LoadGame()
    {
        string filePath = Application.persistentDataPath + Path.DirectorySeparatorChar + SAVE_DATA_FILENAME;
        if (!File.Exists(filePath))
        {
            Debug.LogError($"Could not find save file at path: {filePath}");
            return;
        }

        Restart();

        SaveData saveData = JsonUtility.FromJson<SaveData>(File.ReadAllText(filePath));

        allCards = boardGameBuilderScript.RestoreBoard(saveData, OnCardSelected, soundManager.PlayAudio);

        if (allCards == null)
        {
            Debug.LogError($"Error when loading file from path: {filePath}");
            return;
        }

        gameIsRunning = true;

        scoreManager.LoadScore(saveData.score, saveData.multiplier);

        gameMenuObject.SetActive(false);
        errorMessageObject.SetActive(false);

        rows = saveData.rows;
        columns = saveData.columns;

        cardsMatched = saveData.cardsMatchedIndex.Count;

        if(cardsMatched >= allCards.Count)
            FinishGame();
    }

    public void DeleteSave()
    {
        string filePath = Application.persistentDataPath + Path.DirectorySeparatorChar + SAVE_DATA_FILENAME;
        if (!File.Exists(filePath))
        {
            Debug.LogError($"Could not find save file at path: {filePath}");
            return;
        }
        File.Delete(filePath);
    }

    private List<int> GetCardsIds()
    {
        List<int> retVal = new List<int>();
        foreach(CardScript card in allCards)
        {
            retVal.Add(card.GetID());
        }
        return retVal;
    }

    private List<int> GetMatchedCardsIndexes()
    {
        List<int> retVal = new List<int>();
        for(int i = 0; i < allCards.Count; i++)
        {
            if (allCards[i].IsMatched())
            {
                retVal.Add(i);
            }
        }
        return retVal;
    }

    public void OnOpenExitMenu()
    {
        if (gameIsRunning)
            exitMenu.SetActive(true);
        else
            OnExitToTitle(false);
    }

    public void OnExitToTitle(bool saveBeforeExiting)
    {
        if (saveBeforeExiting)
            SaveGame();
        SceneManager.LoadScene("StartMenuScene");
    }

}

[System.Serializable]
public class SaveData
{
    public List<int> cardsIds;
    public List<int> cardsMatchedIndex;
    public int score;
    public int multiplier;
    public int rows;
    public int columns;
}
