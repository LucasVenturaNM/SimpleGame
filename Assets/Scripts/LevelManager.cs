using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using SimpleGame.Playground;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance { get; private set;}
    private Coroutine reactionCoroutine;
    private GameState _currentGameState;
    private PlaygroundGenerator _playgroundGenerator;
    [SerializeField] private LevelsList _levelsList;
    private int _currentLevelIndex;
    private LevelSO _currentLevelData;

    private float _breathingTime;
    private float _anticipationTime;
    private float _winConditionTime;

    private List<int> _safeBlocksList = new List<int>();

    private int _currentRound;
    public static event Action<LevelState> onLevelStateChanged;

    private event Action<LevelState> onInternalLevelStateChanged;


    //Getter
    public LevelSO CurrentLevelData => _currentLevelData;

    #region Unity
    private void OnEnable()
    {
       GameManager.onGameStateChanged += CurrentGameState;
       onInternalLevelStateChanged += UpdateLevelState;
    }

    private void Awake() 
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else if(Instance != this)
        {
            Destroy(gameObject);
        }    
    }

    private void Start()
    {
        LevelStart();
    }

    private void OnDisable()
    {
        GameManager.onGameStateChanged -= CurrentGameState;
        onInternalLevelStateChanged -= UpdateLevelState;

        StopAllCoroutines();
    }
    #endregion
    
    private void LevelStart()
    {
        _currentLevelIndex = 0;

        _currentLevelData = _levelsList.LevelList[_currentLevelIndex];

        _currentRound = 0;

        _playgroundGenerator = GetComponent<PlaygroundGenerator>();

        UpdateLevelState(LevelState.Start);
    }

    private void CurrentGameState(GameState state)
    {
        Debug.Log("Current Game State got updated to: " + state);
        _currentGameState = state;
    }

    public void UpdateLevelState(LevelState state)
    {   
        Debug.Log("GameState is: " + _currentGameState);

        StopAllCoroutines();

        if(state == LevelState.NextLevel)
        {
            _currentLevelIndex ++;
            _currentRound = 0;

            if (_currentLevelIndex < _levelsList.LevelList.Count)
                {
                    _currentLevelData = _levelsList.LevelList[_currentLevelIndex];
                }
                else
                {
                    GameManager.Instance.LoadLevel(0);
                }

            StartCoroutine(TimerUtil.LevelStateCallDelayer(1f, onInternalLevelStateChanged, LevelState.Start));
        }


        //if(_currentGameState == GameState.Paused) return;


        switch (state)
        {
            case LevelState.Start:

                GetRoundInfo();

                StartCoroutine(TimerUtil.LevelStateCallDelayer(_breathingTime, onInternalLevelStateChanged, LevelState.Anticipating));

                break;


            case LevelState.Anticipating:

                StartCoroutine(TimerUtil.LevelStateCallDelayer(_anticipationTime,onInternalLevelStateChanged, LevelState.WinCondition));
                
                GetSafeBlock();

                reactionCoroutine = StartCoroutine(Anticipation());

                break;


            case LevelState.WinCondition:

                StopCoroutine(reactionCoroutine);

                NewRound();

                break;


            case LevelState.SuccessfulRound:
               
                StartCoroutine(TimerUtil.LevelStateCallDelayer(3f,onInternalLevelStateChanged, LevelState.Reset));

                break;


             case LevelState.DefeatScreen:

                _currentLevelIndex = 0;
                _currentRound = 0;
                _currentLevelData = _levelsList.LevelList[_currentLevelIndex];
                
                break;


            case LevelState.Reset:

                ResetPlaygroundAesthetic();

                if (_currentLevelData.RoundInformation.Any())
                {
                    if (_currentRound + 1 < _currentLevelData.RoundInformation.Count)
                    {
                        _currentRound += 1;
                        StartCoroutine(TimerUtil.LevelStateCallDelayer(_breathingTime, onInternalLevelStateChanged, LevelState.Anticipating));

                    }
                    else
                    {
                        UpdateLevelState(LevelState.VictoryScreen);
                        return;
                    }
                }

                break;


            case LevelState.VictoryScreen:

                break;

        }


        onLevelStateChanged?.Invoke(state);
    }

    private void GetSafeBlock()
    {
        if(_playgroundGenerator == null) return; 
        int safeBlockIndex = UnityEngine.Random.Range(0, _playgroundGenerator.GroundBlocks.Count - 1);

        if (_safeBlocksList.Contains(safeBlockIndex))
        {
            GetSafeBlock();
            return;
        }
        else
        {
            _safeBlocksList.Add(safeBlockIndex);
        }

        if (_currentLevelData.RoundInformation.Count != 0)
        {
            int totalSafeBlockAmount;
            int totalNumberOfBlocks = _currentLevelData.InitialNumberOfRows* _currentLevelData.InitialNumberOfColumns;
            
            if(_currentLevelData.RoundInformation[_currentRound].SafeBlockAmount >= totalNumberOfBlocks)
            {
                totalSafeBlockAmount = totalNumberOfBlocks -1;
            }
            else
            {
                totalSafeBlockAmount = _currentLevelData.RoundInformation[_currentRound].SafeBlockAmount;
            }

            if (_safeBlocksList.Count < totalSafeBlockAmount)
            {
                GetSafeBlock();
            }

        }

    }

    private void NewRound()
    {
            for (int i = 0; i < _playgroundGenerator.GroundBlocks.Count; i++)
            {
                if (_safeBlocksList.Contains(i))
                {
                    _playgroundGenerator.GroundBlocks[i].GetComponent<MeshRenderer>().material.color = Color.green;
                    continue;
                }
                else
                {
                    _playgroundGenerator.GroundBlocks[i].GetComponent<MeshRenderer>().material.color = Color.red;
                }
            }

    }

    IEnumerator Anticipation()
    {
        while (true)
        {

            for (int i = 0; i < _playgroundGenerator.GroundBlocks.Count; i++)
            {
                MeshRenderer blockMesh = _playgroundGenerator.GroundBlocks[i].GetComponent<MeshRenderer>();
                
                if (_safeBlocksList.Contains(i))
                {
                    blockMesh.material.color = Color.blue;
                    continue;
                }
                else
                {
                    if (blockMesh.material.color != Color.black)
                    {
                        blockMesh.material.color = Color.black;
                    }
                    else
                    {
                        blockMesh.material.color = Color.gray;
                    }
                }
            }

            yield return new WaitForSecondsRealtime(0.25f);

        }


    }

    private void ResetPlaygroundAesthetic()
    {
        foreach (GameObject groundBlock in _playgroundGenerator.GroundBlocks)
        {
            groundBlock.GetComponent<MeshRenderer>().material.color = Color.gray;
        }
        _safeBlocksList.Clear();
    }

    private void GetRoundInfo()
    {
        if (_currentLevelData.RoundInformation.Count == 0)
        {
            _breathingTime = 2f;
            _anticipationTime = 2f;
            _winConditionTime = 3f;
        }
        else
        {
            _breathingTime = _currentLevelData.RoundInformation[_currentRound].BreathingDuration;
            _anticipationTime = _currentLevelData.RoundInformation[_currentRound].AnticipationDuration;
            _winConditionTime = _currentLevelData.RoundInformation[_currentRound].WinConditionDuration;
        }

    }
}

public enum LevelState
{
    Start,
    Anticipating,
    WinCondition,
    Reset,
    SuccessfulRound,
    NextLevel,
    VictoryScreen,
    DefeatScreen,
}
