using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    [Header("Score Variables")]
    [SerializeField] int cardMatchingScore = 20;
    [SerializeField] int cardMatchingMultiplier = 1;
    [SerializeField] float scoreUpdatePerSecond = 100;

    [Header("Text References")]
    [SerializeField] TMP_Text scoreDisplayText;
    [SerializeField] TMP_Text multiplierDisplayText;

    private int currentScore = 0;
    private int currentScoreMultiplier = 1;

    private bool shouldUpdateScoreDisplay = false;
    private int currentDisplayScore = 0;

    // Update is called once per frame
    //We use it to update the score in a smooth way
    void Update()
    {
        if(shouldUpdateScoreDisplay)
        {
            currentDisplayScore += Mathf.FloorToInt(Time.deltaTime * scoreUpdatePerSecond);
            if (currentDisplayScore >= currentScore)
            {
                currentDisplayScore = currentScore;
                shouldUpdateScoreDisplay = false;
            }
            //Update UI here
            UpdateScoreText();
        }
    }

    //Function called when the player matches cards to score
    public void OnScore()
    {
        currentScore += cardMatchingScore * currentScoreMultiplier;
        currentScoreMultiplier += cardMatchingMultiplier;
        shouldUpdateScoreDisplay = true;

        //Update multiplier UI here
        UpdateMultiplierText();
    }

    //Function called when the player makes a mistake matching cards
    public void OnMistake()
    {
        currentScoreMultiplier = 1;
        //Update UI here
        UpdateMultiplierText();
    }

    //Used for updating the display of the score
    private void UpdateScoreText()
    {
        scoreDisplayText.SetText(currentDisplayScore.ToString());
    }

    //Used for updating the display of the multiplier
    private void UpdateMultiplierText()
    {
        multiplierDisplayText.SetText($"x{currentScoreMultiplier.ToString()}");
    }

    //Function to reset the score of the game
    public void Reset()
    {
        currentScore = 0;
        currentDisplayScore = 0;
        currentScoreMultiplier = 1;
        UpdateScoreText();
        UpdateMultiplierText();
    }

    public int GetScore()
    {
        return currentScore;
    }

    public int GetMultiplier()
    {
        return currentScoreMultiplier;
    }

    //Function to restore a previously saved score
    public void LoadScore(int score, int multiplier)
    {
        currentScore = score;
        currentDisplayScore = score;
        currentScoreMultiplier = multiplier;
        UpdateScoreText();
        UpdateMultiplierText();
    }
}
