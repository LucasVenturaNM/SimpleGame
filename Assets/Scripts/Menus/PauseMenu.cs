using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
   [SerializeField] private GameObject _pauseOverlay;

   public void ChangePauseState()
   {
      GameManager.Instance.ChangePauseState();
      if(_pauseOverlay.activeInHierarchy)
      {
         _pauseOverlay.SetActive(false);
      }
      else if(!_pauseOverlay.activeInHierarchy)
      {
         _pauseOverlay.SetActive(true);
      }
   }
}
