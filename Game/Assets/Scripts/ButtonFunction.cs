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

        switch (GameManager.instance.playerScript.playerLives)
        {

            case 1:
                GameManager.instance.twoLife.SetActive(false);
                break;

            case 2:
                GameManager.instance.threeLife.SetActive(false);
                break;

            case 3:
                GameManager.instance.fourLife.SetActive(false);
                break;

            case 4:
                GameManager.instance.fiveLife.SetActive(false);
                break;

            default:
                break;
        }
    }

    public void GameQuit()
    {

        Application.Quit();
    }

    public void GameMenu()
    {

        SceneManager.LoadScene("Menu");
    }

    public void GamePlay()
    {

        SceneManager.LoadScene("Mode");
    }

    public void EasyMode()
    {

        GameManager.instance.StopPause();
        SceneManager.LoadScene("Level 1");
    }

    public void MedMode()
    {

        GameManager.instance.StopPause();
        SceneManager.LoadScene("Level 1");
        GameManager.instance.mediumMode = true;
    }

    public void HardMode()
    {

        GameManager.instance.StopPause();
        SceneManager.LoadScene("Level 1");
        GameManager.instance.hardMode = true;
    }
}