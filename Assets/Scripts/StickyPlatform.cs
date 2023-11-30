using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StickyPlatform : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name == "Player" && collision.contacts.Length > 0)
        {
            var rb = collision.gameObject.GetComponent<Rigidbody2D>();
            foreach (var contact in collision.contacts)
            {
                if (contact.normal.y < -0.5)
                {
                    collision.gameObject.transform.SetParent(transform);
                    rb.interpolation = RigidbodyInterpolation2D.None;
                    break;
                }
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.name == "Player")
        {
            var rb = collision.gameObject.GetComponent<Rigidbody2D>();
            collision.gameObject.transform.SetParent(null);
            rb.interpolation = RigidbodyInterpolation2D.Interpolate;
        }
    }
}
