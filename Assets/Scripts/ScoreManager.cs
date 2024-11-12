using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{

    [SerializeField] int cardMatchingScore = 20;
    [SerializeField] int cardMatchingMultiplier = 1;
    [SerializeField] float scoreUpdatePerSecond = 100;

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
        }
    }

    public void OnScore()
    {
        currentScore += cardMatchingScore * currentScoreMultiplier;
        cardMatchingMultiplier += cardMatchingMultiplier;
        shouldUpdateScoreDisplay = true;

        //Update multiplier UI here
    }

    public void OnMistake()
    {
        currentScoreMultiplier = 1;
        //Update UI here
    }

}
