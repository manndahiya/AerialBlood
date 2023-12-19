using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Scoreboard : MonoBehaviour
{
    int score;
    TMP_Text scoreText;

    private void Start()
    {
        scoreText = GetComponent<TMP_Text>();
        scoreText.text = "SCORE:";
    }
    public void IncreaseScore(int amountToIncrease)
    {
        score = score + amountToIncrease;
        scoreText.text = $"SCORE:{score.ToString()}";
    }

}
