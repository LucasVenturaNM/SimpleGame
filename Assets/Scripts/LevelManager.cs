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
    [SerializeField] private PlaygroundGenerator _playgroundGenerator;
    [SerializeField] private LevelsList _levelsList;
    private int _currentLevelIndex;
    private LevelSO _currentLevelData;

    private float _breathingTime;
    private float _anticipationTime;
    private float _winConditionTime;

    private List<int> _safeBlocksList = new List<int>();

    private int _currentRound;
    private LevelState _currentLevelState;
    public static event Action<LevelState> onLevelStateChanged;

    private event Action<LevelState> onInternalLevelStateChanged;

    //Getter
    public LevelSO CurrentLevelData => _currentLevelData;
    public PlaygroundGenerator PlaygroundGenerator => _playgroundGenerator;

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
            Destroy(this);
        }
        
        _currentLevelIndex = 0;

        _currentLevelData = _levelsList.LevelList[_currentLevelIndex];

        _currentRound = 0;
    }

    private void Start()
    {
        UpdateLevelState(LevelState.Start);
    }

    private void OnDisable()
    {
        GameManager.onGameStateChanged -= CurrentGameState;
        onInternalLevelStateChanged -= UpdateLevelState;

        StopAllCoroutines();
    }
    #endregion
    
    private void CurrentGameState(GameState state)
    {
        _currentGameState = state;
    }

    public void UpdateLevelState(LevelState state)
    {
        if(_currentGameState == GameState.Paused) return;

        _currentLevelState = state;

        StopAllCoroutines();

        switch (state)
        {
            case LevelState.VictoryScreen:

                break;


            case LevelState.DefeatScreen:

                //StartCoroutine(Timing.LevelStateCallDelayer(3f,onInternalLevelStateChanged, LevelState.Reset));

                _currentRound = 0;
                
                break;


            case LevelState.Anticipating:

                StartCoroutine(Timing.LevelStateCallDelayer(_anticipationTime,onInternalLevelStateChanged, LevelState.WinCondition));
                GetRoundInfo();

                GetSafeBlock();

                reactionCoroutine = StartCoroutine(Anticipation());

                break;


            case LevelState.WinCondition:

                StartCoroutine(Timing.LevelStateCallDelayer(_winConditionTime,onInternalLevelStateChanged, LevelState.Reset));

                StopCoroutine(reactionCoroutine);

                NewRound();

                break;


            case LevelState.Reset:

                ResetPlaygroundAesthetic();

                if (_currentLevelData.RoundInformation.Any())
                {
                    if (_currentRound + 1 < _currentLevelData.RoundInformation.Count)
                    {
                        _currentRound += 1;
                        StartCoroutine(Timing.LevelStateCallDelayer(_breathingTime, onInternalLevelStateChanged, LevelState.Anticipating));

                    }
                    else
                    {
                        Victory();
                        return;
                    }
                }

                break;


            case LevelState.SuccessfulRound:
               
                StartCoroutine(Timing.LevelStateCallDelayer(3f,onInternalLevelStateChanged, LevelState.Reset));

                break;


            case LevelState.Start:
                GetRoundInfo();

                StartCoroutine(Timing.LevelStateCallDelayer(_breathingTime, onInternalLevelStateChanged, LevelState.Anticipating));
                
                break;


            case LevelState.NextLevel:

                _currentLevelData = _levelsList.LevelList[_currentLevelIndex ++];

                _currentRound = 0;

                break;

        }

        onLevelStateChanged?.Invoke(state);
    }

    private void GetSafeBlock()
    {
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
            if (_safeBlocksList.Count < _currentLevelData.RoundInformation[_currentRound].SafeBlockAmount)
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

    private void Victory()
    {
        UpdateLevelState(LevelState.VictoryScreen);
        Debug.Log("Victory Screen Showing!");
        //Display a feedback message
        //Move to the next level data SO
        //rebuild the playground grid, it would be fun to have some cool animation
        //Start the round stage logic all over again
    }

    private void Defeat()
    {
        UpdateLevelState(LevelState.DefeatScreen);
        Debug.Log ("Defeat Screen Showing!");
        //Display a feedback message and menu
    }

    public void NextLevel()
    {

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
