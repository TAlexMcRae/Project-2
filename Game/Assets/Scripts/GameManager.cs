using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{

    #region Variables
    public static GameManager instance;

    [Header("----- Player -----")]
    public GameObject player;
    public PlayerController playerScript;

    // spawn positions
    public GameObject spawnPos1;
    public GameObject spawnPos2;
    public GameObject spawnPos3;
    public GameObject spawnPos4;

    [Header("----- UI -----")]
    // menus
    public GameObject pauseMenu;
    public GameObject deathMenu;
    public GameObject winMenu;
    public bool isPaused;

    // texts
    public TextMeshProUGUI eText;
    public TextMeshProUGUI wText;
    public TextMeshProUGUI dText;
    public TextMeshProUGUI aText;

    // other UI
    public GameObject playDMGScreen;
    public int enemiesToKill;
    public int waveCount;
    public int deathCount;
    #endregion

    void Awake()
    {

        instance = this;
        player = GameObject.FindGameObjectWithTag("Player");
        playerScript = player.GetComponent<PlayerController>();
        deathCount = 0;
    }

    void Update()
    {

        if (Input.GetButtonDown("Cancel"))
        {

            if (!deathMenu.activeSelf || !winMenu.activeSelf)
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
        UpdateUI();

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

    public void UpdateUI()
    {

        eText.text = enemiesToKill.ToString("F0");
        wText.text = waveCount.ToString("F0");
        dText.text = deathCount.ToString("F0");
        aText.text = playerScript.ammoCount.ToString("F0");
    }
    
    // returns a random spawn position (game object) when called
    public GameObject SpawnPoint()
    {

        GameObject temp = null;
        int SPnum = (int)Random.Range(1, 4);

        switch (SPnum)
        {

            case 1:
                temp = spawnPos1;
                break;

            case 2:
                temp = spawnPos2;
                break;

            case 3:
                temp = spawnPos3;
                break;

            case 4:
                temp = spawnPos4;
                break;

            default:
                temp = spawnPos1;
                break;
        }

        return temp;
    }
}