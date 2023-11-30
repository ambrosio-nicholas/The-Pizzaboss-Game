using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate : MonoBehaviour
{
    [SerializeField] private float Speed = 1f;

    private void Update()
    {
        transform.Rotate(0, 0, 360 * Speed * Time.deltaTime);
    }
}
