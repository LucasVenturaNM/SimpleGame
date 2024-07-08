using System.Collections;
using System.Collections.Generic;
using SimpleGame.Playground;
using Unity.VisualScripting;
using UnityEngine;

enum RoundState
{
    Waiting,
    Anticipating,
    Ready
}

public class RoundManager : MonoBehaviour
{
    private Coroutine reactionCoroutine;
    private RoundState _currentRoundState;
    private float _time;
    private int _previousHazardBlockIndex;
    private int _safeBlockIndex;
    private PlaygroundGenerator _playgroundGenerator;
    private LevelSO _levelData;

    private float _waitingDuration;
    private float _anticipationDuration;
    private float _roundDuration;

    private void OnEnable()
    {
       PlayerController.PlayerLoss += RestartLevel; 
    }

    private void Awake() 
    {
        _time = 0;
        _playgroundGenerator = GetComponent<PlaygroundGenerator>();
        _levelData = _playgroundGenerator.GetLevelData;
    }

    private void Start() 
    {
        ResetProgressModifiers();

        _currentRoundState = RoundState.Waiting;
    }

    private void Update()
    {
        _time += Time.deltaTime;

        RoundManagement();
    }

    private void OnDisable()
    {
       PlayerController.PlayerLoss -= RestartLevel; 
    }

    private void RoundManagement()
    {   
        float totalAnticipationDuration = _waitingDuration + _anticipationDuration;
        float totalRoundDuration = totalAnticipationDuration + _roundDuration;
        
        bool canAnticipate = _time >= _waitingDuration;
        bool canHappen = _time >= totalAnticipationDuration;
        bool canStartNewRound = _time >= totalRoundDuration;

        if(canAnticipate && _currentRoundState == RoundState.Waiting)
        {
            GetSafeBlock();
            reactionCoroutine = StartCoroutine(Reaction());

            _currentRoundState = RoundState.Anticipating;
        }
        else if(canHappen && _currentRoundState == RoundState.Anticipating)
        {
            StopCoroutine(reactionCoroutine);
            NewRound();

            _currentRoundState = RoundState.Ready;
        }
        else if(canStartNewRound && _currentRoundState== RoundState.Ready)
        {   
            ResetPlayground();
            _time = 0;

            AddProgressModifiers();

            _currentRoundState = RoundState.Waiting;
        }

        
    }

    private void GetSafeBlock()
    {
        int safeBlockIndex = Random.Range(0, _playgroundGenerator.GroundBlocks.Count - 1);

        if (safeBlockIndex == _previousHazardBlockIndex)
        {
            GetSafeBlock();
            return;
        }

        _previousHazardBlockIndex = safeBlockIndex;

        _safeBlockIndex = safeBlockIndex;
    }

    private void NewRound()
    {
        foreach (GameObject groundBlock in _playgroundGenerator.GroundBlocks)
        {
            if (groundBlock == _playgroundGenerator.GroundBlocks[_safeBlockIndex])
            {
                groundBlock.GetComponent<MeshRenderer>().material.color = Color.green;
                continue;
            }

            groundBlock.GetComponent<MeshRenderer>().material.color = Color.red;
        }
    }

    IEnumerator Reaction()
    {
        while (true)
        {
            foreach(GameObject groundBlock in _playgroundGenerator.GroundBlocks)
            {
                MeshRenderer blockMesh = groundBlock.GetComponent<MeshRenderer>();
                if(groundBlock == _playgroundGenerator.GroundBlocks[_safeBlockIndex])
                {
                    blockMesh.material.color = Color.blue;
                    continue;
                }

                if(blockMesh.material.color != Color.black)
                {
                    blockMesh.material.color = Color.black;
                }
                else
                {
                    blockMesh.material.color = Color.gray;
                }
            }

            yield return new WaitForSecondsRealtime(0.25f);

        }

        
    }

    private void ResetPlayground()
    {
        foreach (GameObject groundBlock in _playgroundGenerator.GroundBlocks)
        {
            groundBlock.GetComponent<MeshRenderer>().material.color = Color.gray;
        }
    }

    private void AddProgressModifiers()
    {   
        float roundSpeedIncrease = _levelData.RoundSpeedIncrease;
        bool reachedMaxSpeed = roundSpeedIncrease >= _levelData.RoundMaxSpeedMultiplier;
        float minStateDuration = _levelData.MinRoundStateDuration;

        float newWaitDuration = _waitingDuration - _waitingDuration * roundSpeedIncrease;
        float newAnticipationDuration = _anticipationDuration - _anticipationDuration * roundSpeedIncrease;
        float newRoundDuration = _roundDuration - _roundDuration * roundSpeedIncrease;


        if(reachedMaxSpeed) return;

        if(newWaitDuration > minStateDuration) _waitingDuration = newWaitDuration;
        
        if(newAnticipationDuration > minStateDuration) _anticipationDuration = newAnticipationDuration;
        
        if(newRoundDuration > minStateDuration) _roundDuration = newRoundDuration;

    }

    private void ResetProgressModifiers()
    {
        _waitingDuration = _levelData.InitialWaitingTime;
        _anticipationDuration = _levelData.InitialAnticipationTime;
        _roundDuration = _levelData.InitialRoundTime;
    }

    private void RestartLevel()
    {
        ResetProgressModifiers();
        ResetPlayground();
        _time = 0;
        _currentRoundState = RoundState.Waiting;
    }
}
