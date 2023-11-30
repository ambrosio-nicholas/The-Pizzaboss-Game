using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonScript : MonoBehaviour
{
    [SerializeField] private Transform firepoint;
    [SerializeField] private GameObject bullet;
    [SerializeField] private float delay = 0.5f;
    [SerializeField] private AudioSource cannonSound;
    private Animator anim;

    private void Start()
    {
        anim = gameObject.GetComponent<Animator>();
        Invoke("Enable", delay);
    }

    private void Enable()
    {
        anim.enabled = true;
    }

    private void Shoot()
    {
        Instantiate(bullet, firepoint.position, firepoint.rotation);
        cannonSound.Play();
    }
}
