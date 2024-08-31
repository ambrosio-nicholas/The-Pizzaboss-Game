using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spring : MonoBehaviour
{
    private Animator anim;
    private AudioSource sound;

    [SerializeField] private float SpringStrength = 25f;

    void Start()
    {
        anim = gameObject.GetComponent<Animator>();
        sound = gameObject.GetComponent<AudioSource>();
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        print("Collide");
        if (collision.gameObject.name == "Player" && anim.GetInteger("State") == 0)
        {
            collision.gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(collision.gameObject.GetComponent<Rigidbody2D>().velocity.x, SpringStrength);
            sound.Play();
            anim.SetInteger("State", 1);
        }
    }

    public void ReActivateSpring()
    {
        anim.SetInteger("State", 0);
    }
}
