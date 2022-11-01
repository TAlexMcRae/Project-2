using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    #region Variables
    public static GameManager instance;

    [Header("----- Player -----")]
    public GameObject player;
    public PlayerController playerScript;

    [Header("----- UI -----")]
    public GameObject pauseMenu;
    public bool isPaused;

    public GameObject deathMenu;
    public GameObject winMenu;
    public GameObject spawnPos;
    #endregion

    void Awake()
    {

        instance = this;
        player = GameObject.FindGameObjectWithTag("Player");
        playerScript = player.GetComponent<PlayerController>();
    }

    void Update()
    {
        
        if (Input.GetButtonDown("Cancel"))
        {

            if (!deathMenu.activeSelf && !winMenu.activeSelf)
            {

                isPaused = !isPaused;
                pauseMenu.SetActive(isPaused);

                if (isPaused) { StartPause(); }
                else { StopPause(); }
            }
        }
    }

    public void StartPause()
    {

        Time.timeScale = 0;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
    }

    public void StopPause()
    {

        Time.timeScale = 1;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void WinCondition()
    {

        winMenu.SetActive(true);
        StartPause();
    }
}