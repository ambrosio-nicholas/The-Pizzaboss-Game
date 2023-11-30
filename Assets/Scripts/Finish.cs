using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Cinemachine;

public class Finish : MonoBehaviour
{
    private bool levelCompleted = false;

    private CinemachineVirtualCamera playerCam;
    private CinemachineVirtualCamera finishCam;

    private AudioSource finishSound;
    private Rigidbody2D rb;
    private Animator transitionAnim;
    private Animator playerAnimation;

    private void Start()
    {
        finishSound = GetComponent<AudioSource>();
        rb = GameObject.FindWithTag("Player").GetComponent<Rigidbody2D>();
        transitionAnim = GameObject.FindWithTag("Transition").GetComponent<Animator>();
        playerAnimation = GameObject.FindWithTag("Player").GetComponent<Animator>();
        rb = GameObject.FindWithTag("Player").GetComponent<Rigidbody2D>();
        playerCam = GameObject.FindWithTag("PlayerCam").GetComponent<CinemachineVirtualCamera>();
        finishCam = GameObject.FindWithTag("FinishCam").GetComponent<CinemachineVirtualCamera>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Player" && !levelCompleted)
        {
            FinishLevel();
            Invoke("Fly", 1f);
            Invoke("FadeOut", 1.5f);
            Invoke("CompleteLevel", 2.5f); //The invoke allows us to wait 2 seconds
        }
    }

    public void CompleteLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        PlayerPrefs.SetInt("levelAt", SceneManager.GetActiveScene().buildIndex + 1);
    }

    private void FadeOut()
    {
        transitionAnim.SetTrigger("Start");
    }

    private void FinishLevel()
    {
        finishSound.Play();
        playerCam.Priority = 1;
        finishCam.Priority = 10;
        levelCompleted = true;
        playerAnimation.SetTrigger("finnish");
        rb.velocity = new Vector2(0, 0);        
    }

    private void Fly()
    {
        rb.bodyType = RigidbodyType2D.Kinematic;
        rb.velocity = new Vector2(0, 5);
    }
}
