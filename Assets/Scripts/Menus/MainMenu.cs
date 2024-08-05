using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    private void OnEnable()
    {
        GameManager.onGameStateChanged += Pause;
    }

    private void OnDisable()
    {
        GameManager.onGameStateChanged -= Pause;
    }

    public void LoadSceneByIndex(int buildIndex)
    {
        if(SceneManager.GetSceneByBuildIndex(buildIndex) == null)
        {   
            Debug.LogError("Build index " + buildIndex + " is NOT valid!");
            return;
        }

        SceneManager.LoadScene(buildIndex);
    }

    public void Pause(GameState state)
    {
        if(state == GameState.Paused)
        {
            Time.timeScale = 0;
        }
        else if (state == GameState.Playing)
        {
            Time.timeScale = 1;
        }
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
