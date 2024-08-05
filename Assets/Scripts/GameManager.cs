using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set;}
    [SerializeField] private Image _imageToFade;
    [SerializeField] private float _fadeSpeed;
    private GameState _currentState;
    public static event Action<GameState> onGameStateChanged;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else if(Instance != this)
        {
            Destroy(this);
        }

        DontDestroyOnLoad(this);
    }

    public void Start()
    {
        UpdateGameState(GameState.Playing);
    }

    public void UpdateGameState(GameState newState)
    {
        switch (newState)
        {
            case GameState.Paused:
            PauseGame();
            break;

            case GameState.Playing:
            ResumeGame();
            break;

        }

        _currentState = newState;
        onGameStateChanged?.Invoke(newState);
    }

    public void ChangePauseState()
    {
        if(_currentState == GameState.Paused)
        {
            UpdateGameState(GameState.Playing);
        }
        else if(_currentState == GameState.Playing)
        {
            UpdateGameState(GameState.Paused);
        }
    }

    private void PauseGame()
    {
        Time.timeScale = 0;
    }

    private void ResumeGame()
    {
        Time.timeScale = 1;
    }
    
}

public enum GameState
{
    Paused,
    Playing,
}


   

