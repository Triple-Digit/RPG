using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    #region Singleton
    public static GameManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
        }
    }
    #endregion

    [Header("Player info")]
    public CombatStats[] m_playerStats;
       
    [Header("UI components")]
    [SerializeField] private GameObject startScreen;
    [SerializeField] private GameObject pauseMenuUI;
    [SerializeField] private GameObject loseScreen;
    [SerializeField] private GameObject winScreen;
    [SerializeField] private GameObject m_combatScene;

    public bool m_paused, m_combatActive;
        
    private void FixedUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!m_paused)
            {
                Pause();
                m_paused = true;
            }
            else
            {
                Resume();
                m_paused = false;
            }
        }

        if(m_combatActive)
        {
            PlayerController.Instance.m_movementSpeed = 0f;
            PlayerController.Instance.m_canMove = false;
            m_combatScene.SetActive(true);
        }
        else
        {
            PlayerController.Instance.m_movementSpeed = 5f;
            PlayerController.Instance.m_canMove = true;
            m_combatScene.SetActive(false);
        }
    }

    private void Start()
    {
        UIFade.Instance.FadeToBlack();
    }

    public void Begin()
    {
        startScreen.SetActive(false);
        UIFade.Instance.FadeFromBlack();
        m_paused = false;
    }

    public void Win()
    {
        winScreen.SetActive(true);
        Time.timeScale = 0f;
    }

    public void Lose()
    {
        if (!m_paused)
        {
            loseScreen.SetActive(true);
            m_paused = true;
        }
        else return;
    }

    #region Pause Menu Methods

    public void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;

    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void Quit()
    {
        Application.Quit();
    }

    #endregion       
}
