using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{

    [SerializeField] int cardMatchingScore = 20;
    [SerializeField] int cardMatchingMultiplier = 1;
    [SerializeField] float scoreUpdatePerSecond = 100;

    [SerializeField] TMP_Text scoreDisplayText;
    [SerializeField] TMP_Text multiplierDisplayText;

    private int currentScore = 0;
    private int currentScoreMultiplier = 1;

    private bool shouldUpdateScoreDisplay = false;
    private int currentDisplayScore = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
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

    public void OnScore()
    {
        currentScore += cardMatchingScore * currentScoreMultiplier;
        currentScoreMultiplier += cardMatchingMultiplier;
        shouldUpdateScoreDisplay = true;

        //Update multiplier UI here
        UpdateMultiplierText();
    }

    public void OnMistake()
    {
        currentScoreMultiplier = 1;
        //Update UI here
        UpdateMultiplierText();
    }

    private void UpdateScoreText()
    {
        scoreDisplayText.SetText(currentDisplayScore.ToString());
    }

    private void UpdateMultiplierText()
    {
        multiplierDisplayText.SetText(currentScoreMultiplier.ToString());
    }

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

    public void LoadScore(int score, int multiplier)
    {
        currentScore = score;
        currentDisplayScore = score;
        currentScoreMultiplier = multiplier;
        UpdateScoreText();
        UpdateMultiplierText();
    }
}
