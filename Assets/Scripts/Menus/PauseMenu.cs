using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    
   [SerializeField] private GameObject _pauseOverlay;

   private void OnEnable() 
   {
      GameManager.onGameStateChanged += ControlPauseMenu;
   }

   private void OnDisable() 
   {
      GameManager.onGameStateChanged -= ControlPauseMenu;
   }

   public void ControlPauseMenu(GameState state)
   {
     if(state == GameState.Playing)
     {
        _pauseOverlay.SetActive(false);
     }
     else if(state == GameState.Paused)
     {
        _pauseOverlay.SetActive(true);
     }
   }

   public void ChangePauseState()
   {
      GameManager.Instance.ChangePauseState();
   }
}
