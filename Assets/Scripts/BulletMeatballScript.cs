using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletMeatballScript : MonoBehaviour
{
    [SerializeField] float speed;
    private Rigidbody2D rb;
    private Animator anim;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = gameObject.GetComponent<Animator>();
        rb.velocity = transform.right * speed;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.gameObject.CompareTag("cannon"))
        {
            anim.SetTrigger("Hit");
            rb.velocity = new Vector2(rb.velocity.x * 0, rb.velocity.y * 0);
            Invoke("Hit", .3f);
        }
    }

    private void Hit()
    {
        Destroy(gameObject);
    }
}
