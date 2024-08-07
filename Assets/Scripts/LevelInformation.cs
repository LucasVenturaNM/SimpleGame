using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct LevelInformation
{
    private string _name;
    [SerializeField] [Range(0.5f,10)] private float _breathingDuration;
    [SerializeField] [Range(0.5f,10)]private float _anticipationDuration;
    [SerializeField] [Range(0.8f,10)]private float _winConditionDuration;
    [SerializeField] [Range(1,10)] private int _safeBlockAmount;

    public string Name {set { _name = value; } }
    public float BreathingDuration => _breathingDuration;
    public float AnticipationDuration => _anticipationDuration;
    public float WinConditionDuration => _winConditionDuration;
    public int SafeBlockAmount => _safeBlockAmount;
}
