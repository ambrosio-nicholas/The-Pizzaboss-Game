using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemCollector : MonoBehaviour
{
    private int coins = 0;
    private Animator yogurtUIAnim;

    [SerializeField] private AudioSource collectionSoundEffect;
    [SerializeField] private AudioSource specialItemSoundEffect;

    [SerializeField] private Text pineappleText;

    private void Start()
    {
        yogurtUIAnim = GameObject.FindWithTag("YogurtUI").GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Coin"))
        {
            collectionSoundEffect.Play();
            Destroy(collision.gameObject);
            coins++;
            pineappleText.text = "Pineapples: " + coins;
        }
        
        if (collision.gameObject.CompareTag("Yogurt"))
        {
            specialItemSoundEffect.Play();
            Destroy(collision.gameObject);
            yogurtUIAnim.SetBool("HasYogurt", true);
        }
    }
}
