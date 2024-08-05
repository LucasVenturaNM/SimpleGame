using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Level SO", menuName ="LevelData/Level SO")]
public class LevelSO : ScriptableObject
{
    [Header("Level Identification")]
    [SerializeField] private string _levelName = "SampleLevel";
    [SerializeField] private int _levelIndex = -1;

    [Header("Level Size")]
    [SerializeField][Range(2,7)] private int _initialNumberOfRows;
    [SerializeField][Range(2,7)] private int _initialNumberOfColumns;
    [SerializeField] private float _marginBetweenCells;

    [Header("Round Settings")]
    [SerializeField] private List<LevelInformation> _roundInformation = new List<LevelInformation>();
    // [SerializeField] private float _initialWaitingTime = 1;
    // [SerializeField] private float _initialAnticipationTime = 2;
    // [SerializeField] private float _initialRoundTime = 1;

    // [Header("Progress Settings")]
    // [SerializeField] private float _roundSpeedIncrease = 0.25f;
    // [SerializeField] private float _roundMaxSpeedMultiplier = 1;
    // [SerializeField] private int _numberOfRounds = 5;
    // [SerializeField] private float _minRoundStateDuration = 0.25f;

    //Getters
    //Level Identification
    public string LevelName => _levelName;
    public int LevelIndex => _levelIndex;

    //Level Size
    public int InitialNumberOfRows => _initialNumberOfRows;
    public int InitialNumberOfColumns => _initialNumberOfColumns;
    public float MarginBetweenCells => _marginBetweenCells;

    //Round Settings
    public List<LevelInformation> RoundInformation => _roundInformation;

    // public float InitialWaitingTime => _initialWaitingTime;
    // public float InitialAnticipationTime => _initialAnticipationTime;
    // public float InitialRoundTime => _initialRoundTime;

    // //Progress Settings
    // public float RoundSpeedIncrease => _roundSpeedIncrease;
    // public float RoundMaxSpeedMultiplier => _roundMaxSpeedMultiplier;
    // public int NumberOfRounds => _numberOfRounds;
    // public float MinRoundStateDuration => _minRoundStateDuration;



}
