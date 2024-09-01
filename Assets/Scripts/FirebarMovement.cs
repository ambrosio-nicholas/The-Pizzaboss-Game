using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirebarMovement : MonoBehaviour
{
    [SerializeField] private float speed = 2f;
    [SerializeField] private bool spinClockwise = true;
    private Transform fireBarTransform;
    private float direction = 1;

    private void Start()
    {
        fireBarTransform = gameObject.transform.GetChild(0).transform;
    }
    private void Update()
    {
        if (spinClockwise)
        {
            direction = -1f;
        }
        else
        {
            direction = 1f;
        }

        fireBarTransform.Rotate(new Vector3(0, 0, Time.deltaTime * speed * 100 * direction));
    }
}
