using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Levels List", menuName ="LevelData/Levels List", order = 0)]
public class LevelsList : ScriptableObject
{
    [SerializeField] private List<LevelSO> _levelList = new List<LevelSO>();
    private int _currentLevelIndex = 0;
    public List<LevelSO> LevelList => _levelList;

    //Accessor
    public int GetCurrentLevel => _currentLevelIndex;
    
    //Mutator
    public int SetCurrentLevel {set { _currentLevelIndex = value; }}
}
