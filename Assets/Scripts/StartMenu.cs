using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Cinemachine;

public class StartMenu : MonoBehaviour
{
    private Animator transitionAnim;
    private CinemachineVirtualCamera startCam;
    private CinemachineVirtualCamera menuCam;
    private Animator menuAnim;
    private Animator levelSelectAnim;

    private void Start()
    {
        transitionAnim = GameObject.FindWithTag("Transition").GetComponent<Animator>();
        menuCam = GameObject.FindWithTag("PlayerCam").GetComponent<CinemachineVirtualCamera>();
        startCam = GameObject.FindWithTag("StartCam").GetComponent<CinemachineVirtualCamera>();
        menuAnim = GameObject.FindWithTag("PauseScreen").GetComponent<Animator>();
        levelSelectAnim = GameObject.FindWithTag("LevelSelect").GetComponent<Animator>();

        startCam.Priority = 1;
        menuCam.Priority = 10;

        Invoke("FadeInMenu", 10);
    }

    private void StartGame() //Activated by Button
    {
        menuAnim.SetTrigger("levelSelect");
        levelSelectAnim.SetTrigger("levelSelect");
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    private void NextLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    private void FadeInMenu()
    {
        menuAnim.SetBool("visibleMenu", true);
    }

    public void LevelsButton()
    {
        transitionAnim.SetTrigger("Start");
        Invoke("NextLevel", 1);
    }
}
