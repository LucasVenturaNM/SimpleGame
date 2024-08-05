using System;
using System.Collections;
using System.Collections.Generic;
using SimpleGame.Playground;
using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerController : MonoBehaviour
{
    [SerializeField] private float _lossDuration = 3f;
    private PlaygroundGenerator _playgroundGenerator;
    private LevelSO _levelData;
    private int _currentBlock;
    private bool _canLose = true;

    private Vector3 _initialPosition;

    private PlayerInputs _playerInputs;

    private LevelState _previousLevelState;

    #region Unity
    private void OnEnable()
    {
        _playerInputs = GetComponent<PlayerInputs>();
        _playerInputs.EnableGameplay();
        _playerInputs.GameplayActions[0].performed += Move;
        _playerInputs.GameplayActions[1].performed += Pause;

        GameManager.onGameStateChanged += HandleGameStates;
        LevelManager.onLevelStateChanged += HandleLevelStates;
    }
    private void Start()
    {
        if(LevelManager.Instance != null)
        {
            _playgroundGenerator = LevelManager.Instance.PlaygroundGenerator;
            _levelData = LevelManager.Instance.CurrentLevelData;
        }

        RepositionAndSetSpawnPoint();   
    }

    private void Update() {
        Debug.DrawRay(transform.position,Vector3.down * 5);
    }

    private void OnDisable() 
    {
        GameManager.onGameStateChanged -= HandleGameStates;
        _playerInputs.GameplayActions[0].performed -= Move;
        _playerInputs.GameplayActions[1].performed -= Pause;
        _playerInputs.DisableGameplay();

    }
    #endregion

    #region HandleStates
    private void HandleGameStates(GameState state)
    {
        switch(state)
        {
            case GameState.Paused:
            StopReadingGameInput();
            break;

            case GameState.Playing:
            ReadGameInput();
            break;
        }
    }

    private void HandleLevelStates(LevelState state)
    {
        switch(state)
        {
            case LevelState.WinCondition:

            StopReadingGameInput();

            CheckWinCondition();

            break;


            case LevelState.Reset:

            ReadGameInput();

            if(_previousLevelState == LevelState.VictoryScreen)
            {
                RepositionAndSetSpawnPoint();
            }
            else if (_previousLevelState == LevelState.DefeatScreen)
            {
                RespawnPlayer();
            }

            break;


            case LevelState.VictoryScreen:

            StopReadingGameInput();

            _levelData = LevelManager.Instance.CurrentLevelData;

            break;


            case LevelState.DefeatScreen:

            StopReadingGameInput();

            break;


        }

        _previousLevelState = state;
    }
    #endregion

    #region Input
    public void ReadGameInput()
    {
        _playerInputs.GameplayActions[0].performed -= Move;        
        _playerInputs.GameplayActions[0].performed += Move;
    }
    private void StopReadingGameInput()
    {
        _playerInputs.GameplayActions[0].performed -= Move;
    }

    private void Move(InputAction.CallbackContext ctx)
    {
        Vector2 inputVector = ctx.ReadValue<Vector2>();

        int nextBlock = GetComponent<PlayerMovement>().CheckForMovement(inputVector, _playgroundGenerator, _levelData);
        if(nextBlock >= 0 && nextBlock < _playgroundGenerator.GroundBlocks.Count)
        {
            GameObject nextGroundBlock = _playgroundGenerator.GroundBlocks[nextBlock];
            transform.position = new Vector3(nextGroundBlock.transform.position.x, 0, nextGroundBlock.transform.position.z);
        }
        
    }

    private void Pause(InputAction.CallbackContext callbackContext)
    {
        GameManager.Instance.ChangePauseState();        
    }

    #endregion

    private void CheckWinCondition()
    {
        RaycastHit hit;
        Physics.Raycast(transform.position,Vector3.down * 5, out hit, 3f, LayerMask.GetMask("Ground"));
        if(hit.collider == null || !_canLose) return;
        if(hit.collider.GetComponent<MeshRenderer>().material.color == Color.red)
        {
            LevelManager.Instance.UpdateLevelState(LevelState.DefeatScreen);
        }
        else
        {
            LevelManager.Instance.UpdateLevelState(LevelState.SuccessfulRound);
        }

        StopReadingGameInput();

    }

    private void RespawnPlayer()
    {
        transform.position = _initialPosition;
    }

    private void RepositionAndSetSpawnPoint()
    {
        if(_levelData.InitialNumberOfRows % 2 == 0 || _levelData.InitialNumberOfColumns % 2 == 0)
        {
            _initialPosition = new Vector3(_playgroundGenerator.GroundBlocks[0].transform.position.x,
                                            transform.position.y,
                                            _playgroundGenerator.GroundBlocks[0].transform.position.z);

            transform.position = _initialPosition;
        }
    }
}
