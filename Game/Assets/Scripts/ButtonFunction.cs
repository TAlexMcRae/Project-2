using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonFunction : MonoBehaviour
{

    public void Resume()
    {

        GameManager.instance.StopPause();
        GameManager.instance.pauseMenu.SetActive(false);
        GameManager.instance.isPaused = false;
    }

    public void Restart()
    {

        GameManager.instance.StopPause();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void Respawn()
    {

        GameManager.instance.StopPause();
        GameManager.instance.playerScript.Respawn();
    }

    public void GameQuit()
    {

        Application.Quit();
    }
}