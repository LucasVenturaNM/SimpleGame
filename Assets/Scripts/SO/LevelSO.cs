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
    [SerializeField][Range(1,5)] private float _marginBetweenCells;

    [Header("Round Settings")]
    [SerializeField] private List<LevelInformation> _roundInformation = new List<LevelInformation>();

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




}
