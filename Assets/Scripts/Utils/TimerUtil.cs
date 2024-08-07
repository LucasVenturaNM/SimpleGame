using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class TimerUtil
{
    public static IEnumerator LevelStateCallDelayer(float waitTime, Action<LevelState> action, LevelState state)
    {
        yield return new WaitForSeconds(waitTime);

        Debug.Log("action was invoked, new state is " + state);
        action?.Invoke(state);
    }

    public static IEnumerator DelayAction(float delayTime, Action action)
    {
        yield return new WaitForSeconds(delayTime);
        Debug.Log("action will be invoked now");
        action?.Invoke();
    }
}
