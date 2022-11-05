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
    // menus
    public GameObject pauseMenu;
    public GameObject deathMenu;
    public GameObject winMenu;
    public bool isPaused;

    public GameObject spawnPos;
    public GameObject playDMGScreen;
    public GameObject reticle;
    public int enemiesToKill;
    #endregion

    void Awake()
    {

        instance = this;
        player = GameObject.FindGameObjectWithTag("Player");
        playerScript = player.GetComponent<PlayerController>();
        spawnPos = GameObject.FindGameObjectWithTag("Spawn Pos");
    }

    void Update()
    {

        if (Input.GetButtonDown("Cancel"))
        {

            if (deathMenu.activeSelf == winMenu.activeSelf == false)
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

    public void UpdateEnemies()
    {

        enemiesToKill--;

        if (enemiesToKill <= 0)
        {

            WinCondition();
        }
    }

    public IEnumerator PlayDMGFlash()
    {

        playDMGScreen.SetActive(true);
        yield return new WaitForSeconds(0.1f);
        playDMGScreen.SetActive(false);
    }
}