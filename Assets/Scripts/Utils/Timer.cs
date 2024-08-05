using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Timing
{
    public static IEnumerator LevelStateCallDelayer(float waitTime, Action<LevelState> action, LevelState state)
    {
        yield return new WaitForSeconds(waitTime);

        Debug.Log("action was invoked, new state is " + state);
        action?.Invoke(state);
    }
}
