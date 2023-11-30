using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpringyRotiniScriptoni : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator anim;
    private bool IsJumping = false;
    [SerializeField] private AudioSource jumpSound;

    [SerializeField] private float jumpStrength = 7f;
    [SerializeField] private float timeBetweenJumps = 2f;
    [SerializeField] private float Stagger = 0f;

    private void Start()
    {
        enabled = false;
        Invoke("Enable", Stagger);
        rb = gameObject.GetComponent<Rigidbody2D>();
        anim = gameObject.GetComponent<Animator>();
    }

    private void Update()
    {
        if (IsJumping == false)
        {
            IsJumping = true;
            Invoke("Jump", timeBetweenJumps);
        }
    }

    private void Jump()
    {
        anim.SetInteger("State", 1);
        Invoke("AddVelocity", 0.5f);
    }

    private void AddVelocity()
    {
        rb.velocity = new Vector2(0, jumpStrength);
        jumpSound.Play();
    }

    public void BackOnTheGround() 
    {
        anim.SetInteger("State", 0);
        IsJumping = false;
    }

    public void Enable()
    {
        enabled = true;
    }
}
