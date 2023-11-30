using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerLife : MonoBehaviour
{
    private Animator anim;
    private Rigidbody2D rb;
    private Animator healthBarAnim;
    private Animator transitionAnim;

    public int Health = 3;
    private bool Invincibility = false;
    private bool Dead = false;

    private GameObject backgroundMusic;

    [SerializeField] private AudioSource hurtSoundEffect;
    [SerializeField] private AudioSource deathSoundEffect;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D > ();
        anim = GetComponent<Animator>();
        healthBarAnim = GameObject.FindWithTag("HealthBar").GetComponent<Animator>();
        transitionAnim = GameObject.FindWithTag("Transition").GetComponent<Animator>();
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Trap") && Invincibility == false && Health <= 1 && Dead == false && PauseMenu.GameIsPaused == false)
        {
            Die();
        }
        else if (collision.gameObject.CompareTag("Trap") && Invincibility == false && Dead == false && PauseMenu.GameIsPaused == false)
        {
            Hurt();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("InstaDeath") && Invincibility == false && Dead == false && PauseMenu.GameIsPaused == false)
        {
            Die();
        }
    }

    private void Hurt()
    {
        Health = Health - 1;
        hurtSoundEffect.Play();
        anim.SetTrigger("hurt");
        Invincibility = true;
        healthBarAnim.SetInteger("state", (int)Health);
    }

    private void EndInvincibility()  // Being called by Hurt animation
    {
        Invincibility = false;
    }

    private void Die()
    {
        backgroundMusic = GameObject.FindWithTag("BackgroundMusic");
        rb.bodyType = RigidbodyType2D.Static;
        deathSoundEffect.Play();
        transitionAnim.SetTrigger("Start");
        healthBarAnim.SetInteger("state", 0);
        anim.SetTrigger("death");
        Dead = true;
        backgroundMusic.SetActive(false);
    }

    private void RestartLevel() //Being called by Death animation
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

}
