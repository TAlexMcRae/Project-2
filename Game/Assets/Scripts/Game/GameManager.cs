using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    #region Variables
    public static GameManager instance;

    [Header("----- Player -----")]
    public GameObject player;
    public PlayerController playerScript;

    public GameObject oneLife;
    public GameObject twoLife;
    public GameObject threeLife;
    public GameObject fourLife;
    public GameObject fiveLife;

    // spawn positions
    public GameObject[] spawnPos;

    // level advance reminder
    public GameObject advancement;

    [Header("----- UI -----")]
    // menus
    public GameObject pauseMenu;
    public GameObject deathMenu;
    public GameObject winMenu;
    public GameObject gameOverMenu;
    public bool isPaused;

    // texts
    public TextMeshProUGUI cText;
    public TextMeshProUGUI eText;
    public TextMeshProUGUI wText;
    public TextMeshProUGUI aText;
    public TextMeshProUGUI wTotalText;

    // timer
    public GameObject timerText;
    public float secondsLeft = 00.00f;

    // other UI
    public GameObject playDMGScreen;
    public GameObject playHealScreen;
    public GameObject playBoostScreen;
    public Image hpBar;

    public int enemiesToKill;
    public int waveCount;
    public int captureCount;

    //capture zone
    public bool capturedAll;
    public GameObject[] captureZones;
    public GameObject capturing;
    #endregion

    void Awake()
    {

        instance = this;
        player = GameObject.FindGameObjectWithTag("Player");

        if (player != null)
        {

            playerScript = player.GetComponent<PlayerController>();
        }


        waveCount = 1;
        capturedAll = false;
        captureCount = captureZones.Length;
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

        if (timerText.activeSelf)
        {

            UpdateUI();
        }

        if (advancement.activeSelf && Input.GetButtonDown("Advance"))
        {

            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            advancement.SetActive(false);
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

        if (enemiesToKill <= 0 && capturedAll)
        {

            waveCount++;

            if (waveCount > waveManager.instance.waves.Length)
            {

                if (SceneManager.GetActiveScene().buildIndex < 4)
                {

                    AdvancePrompt();
                }

                else
                {

                    WinCondition();
                }
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

        if (waveManager.instance != null)
        {

            wTotalText.text = waveManager.instance.waves.Length.ToString("F0");
        }

        aText.text = playerScript.ammoCount.ToString("F0");
        hpBar.fillAmount = (float)playerScript.currentHP / (float)playerScript.startHP;
        cText.text = captureCount.ToString("F0");

        if (timerText.activeSelf)
        {

            timerText.GetComponent<TextMeshProUGUI>().text = secondsLeft.ToString("F2");
            secondsLeft -= Time.deltaTime;
        }


        if (playerScript.playerLives != 0)
        {

            if (playerScript.playerLives >= 1)
            {

                oneLife.SetActive(true);
            }

            if (playerScript.playerLives >= 2)
            {

                twoLife.SetActive(true);
            }

            if (playerScript.playerLives >= 3)
            {

                threeLife.SetActive(true);
            }

            if (playerScript.playerLives >= 4)
            {

                fourLife.SetActive(true);
            }

            if (playerScript.playerLives == 5)
            {

                fiveLife.SetActive(true);
            }
        }
    }
    #endregion

    // returns a random spawn position (game object) when called
    public GameObject SpawnPoint()
    {

        return spawnPos[Random.Range(0, spawnPos.Length - 1)];
    }

    // advance prompt
    public void AdvancePrompt()
    {

        advancement.SetActive(true);
    }
}