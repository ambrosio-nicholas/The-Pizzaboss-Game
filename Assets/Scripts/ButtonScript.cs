using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonScript : MonoBehaviour
{
    [SerializeField] private Sprite buttonUp;
    [SerializeField] private Sprite buttonDown;
    [SerializeField] private AudioSource buttonSound;
    [SerializeField] private GameObject door;
    [SerializeField] private GameObject endPos;
    [SerializeField] private GameObject startPos;
    [SerializeField] private int speed;

    private bool isClicked = false;

    private void Update()
    {
        if (isClicked)
        {
            door.transform.position = Vector2.MoveTowards(door.transform.position, endPos.transform.position, Time.deltaTime * speed);
        }
        else if (!isClicked)
        {
            door.transform.position = Vector2.MoveTowards(door.transform.position, startPos.transform.position, Time.deltaTime * speed);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isClicked == false)
        {
            gameObject.GetComponent<SpriteRenderer>().sprite = buttonDown;
            buttonSound.Play();
            isClicked = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (isClicked)
        {
            gameObject.GetComponent<SpriteRenderer>().sprite = buttonUp;
            isClicked = false;
        }
    }
}
