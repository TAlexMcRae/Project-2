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
    public GameObject[] spawnPos;

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
    public GameObject playHealScreen;
    public GameObject playBoostScreen;
    public Image hpBar;

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
        waveCount = 1;
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

        if (enemiesToKill <= 0)
        {

            waveCount++;

            if (waveCount < 7)
            {
                WinCondition();
            }
        }

        UpdateUI();
    }

    #region User Interface
    public IEnumerator PlayDMGFlash()
    {

        playDMGScreen.SetActive(true);
        yield return new WaitForSeconds(0.1f);
        playDMGScreen.SetActive(false);
    }

    public IEnumerator PlayHealFlash()
    {

        playHealScreen.SetActive(true);
        yield return new WaitForSeconds(0.1f);
        playHealScreen.SetActive(false);
    }

    public void UpdateUI()
    {

        eText.text = enemiesToKill.ToString("F0");
        wText.text = waveCount.ToString("F0");
        dText.text = deathCount.ToString("F0");
        aText.text = playerScript.ammoCount.ToString("F0");
        hpBar.fillAmount = (float)playerScript.currentHP / (float)playerScript.startHP;
    }
    #endregion

    // returns a random spawn position (game object) when called
    public GameObject SpawnPoint()
    {

        return spawnPos[Random.Range(0, spawnPos.Length - 1)];
    }
}