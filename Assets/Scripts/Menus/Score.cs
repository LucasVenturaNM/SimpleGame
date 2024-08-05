using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Score : MonoBehaviour
{
    private int _score = 0;
    [SerializeField] private TextMeshProUGUI _scoreText;

    private void OnEnable() 
    {
        LevelManager.onLevelStateChanged += UpdateScore;
    }

    private void Start()
    {
        _scoreText.text = "Score: 0";
    }

    private void OnDisable() 
    {
        LevelManager.onLevelStateChanged -= UpdateScore;
    }

    

    private void UpdateScore(LevelState state)
    {
        if(state == LevelState.SuccessfulRound)
        {
            _score += 1;
        }
        else if (state == LevelState.DefeatScreen)
        {
            _score = 0;
        }
        _scoreText.text = "Score: " + _score;
    }
}
