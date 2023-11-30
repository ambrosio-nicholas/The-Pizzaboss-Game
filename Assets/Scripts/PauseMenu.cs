using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static bool GameIsPaused = false;
    [SerializeField] private GameObject pauseMenuUI;
    private Animator transitionAnim;
    private Animator pauseAnim;

    private void Start()
    {
        transitionAnim = GameObject.FindWithTag("Transition").GetComponent<Animator>();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P) || Input.GetKeyDown(KeyCode.Escape))
        {
            if (GameIsPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    public void Resume()
    {
        GameIsPaused = false;
        Time.timeScale = 1f;
        pauseAnim.SetTrigger("unpause");
        Invoke("unPause", .2f);
    }

    private void Pause()
    {
        GameIsPaused = true;
        pauseMenuUI.SetActive(true);
        pauseAnim = GameObject.FindWithTag("PauseScreen").GetComponent<Animator>();
        Time.timeScale = 0f;

    }

    public void LoadMenu()
    {
        transitionAnim.SetTrigger("Start");
        Time.timeScale = 1f;
        Invoke("LoadMenuScreen", 1f);
    }

    public void RestartLevel()
    {
        transitionAnim.SetTrigger("Start");
        Time.timeScale = 1f;
        Invoke("Restart", 1f);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    private void Restart()
    {
        GameIsPaused = false;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    
    private void LoadMenuScreen()
    {
        GameIsPaused = false;
        SceneManager.LoadScene(1);
    }

    private void unPause()
    {
        pauseMenuUI.SetActive(false);
    }
}
