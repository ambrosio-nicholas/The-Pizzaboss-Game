using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPatrol : MonoBehaviour
{
    [SerializeField] private GameObject PointA;
    [SerializeField] private GameObject PointB;

    private SpriteRenderer sprite;

    [SerializeField] private float speed = 2f;

    private Rigidbody2D rb;
    private Transform currentPoint;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();

        currentPoint = PointB.transform;
    }

    void Update()
    {
        Vector2 point = currentPoint.position - transform.position;

        if (currentPoint == PointB.transform)
        {
            rb.velocity = new Vector2(speed, 0);
            sprite.flipX = false;
        }
        else
        {
            rb.velocity = new Vector2(-speed, 0);
            sprite.flipX = true;
        }

        if (Vector2.Distance(transform.position, currentPoint.position) < 0.5f && currentPoint == PointB.transform)
        {
            currentPoint = PointA.transform;
        }

        if (Vector2.Distance(transform.position, currentPoint.position) < 0.5f && currentPoint == PointA.transform)
        {
            currentPoint = PointB.transform;
        }
    }
}
