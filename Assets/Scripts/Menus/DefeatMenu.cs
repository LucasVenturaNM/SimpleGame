using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefeatMenu : MonoBehaviour
{
     [SerializeField] private GameObject _defeatOverlay;
    [SerializeField] private GameObject _transitionScreen;
    [SerializeField] private Vector3 _outOfScreen, _onScreen;
    [SerializeField] private float _transitionSpeed;
    private Coroutine _transitionCoroutine = null;


    private void OnEnable() {
        LevelManager.onLevelStateChanged += ActivateDefeatOverlayMenu;
    }

    private void OnDisable() {
        LevelManager.onLevelStateChanged -= ActivateDefeatOverlayMenu;
    }

    private void ActivateDefeatOverlayMenu(LevelState state)
    {
        if(state == LevelState.DefeatScreen)
        {
            StopAllCoroutines();

            _transitionCoroutine = StartCoroutine(HorizontalTransitionIn(_onScreen));

            _defeatOverlay.SetActive(true);
        }
    }

    private IEnumerator HorizontalTransitionIn(Vector3 newPosition)
    {
        _transitionScreen.SetActive(true);
        RectTransform transitionScreenRect = _transitionScreen.GetComponent<RectTransform>();

        while(Vector3.Distance(transitionScreenRect.position, newPosition) > 50f)
        {
            transitionScreenRect.position += Vector3.left * _transitionSpeed * Time.deltaTime;


            yield return null;
        }

        transitionScreenRect.position = newPosition;
    }

    private IEnumerator HorizontalTransitionOut(Vector3 newPosition)
    {
        RectTransform transitionScreenRect = _transitionScreen.GetComponent<RectTransform>();

        while(Vector3.Distance(transitionScreenRect.position, newPosition) > 50f)
        {   
            transitionScreenRect.position += Vector3.right * _transitionSpeed * Time.deltaTime;


            yield return null;
        }

        transitionScreenRect.position = newPosition;
        _transitionScreen.SetActive(false);


    }
}
