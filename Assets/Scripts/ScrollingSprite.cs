using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollingSprite : MonoBehaviour
{
    [SerializeField] private float speedX;
    [SerializeField] float speedY;

    private Transform cameraTransform;
    private Vector3 lastCameraPosition;
    private float textureUnitSizeX;
    private float textureUnitSizeY;
    private Sprite sprite;
    private Texture2D texture;
    private Vector3 deltaMovement;

    private void Start()
    {
        cameraTransform = Camera.main.transform;
        lastCameraPosition = cameraTransform.position;
        sprite = GetComponent<SpriteRenderer>().sprite;
        texture = sprite.texture;
        textureUnitSizeX = texture.width / sprite.pixelsPerUnit;
        textureUnitSizeY = texture.height / sprite.pixelsPerUnit;
    }

    private void LateUpdate()
    {
        deltaMovement = cameraTransform.position - lastCameraPosition;
        transform.position += new Vector3(deltaMovement.x - speedX * Time.deltaTime, deltaMovement.y - speedY * Time.deltaTime);
        lastCameraPosition = cameraTransform.position;

        if (Mathf.Abs(cameraTransform.position.x - transform.position.x) >= textureUnitSizeX)
        {
            float offsetPosX = (cameraTransform.position.x - transform.position.x);
            transform.position = new Vector3(cameraTransform.position.x + offsetPosX, transform.position.y); 
        }


       // if (Mathf.Abs(cameraTransform.position.y - transform.position.y) >= textureUnitSizeY)
       // {
       //     float offsetPosY = (cameraTransform.position.y - transform.position.y);
       //     transform.position = new Vector3(transform.position.x, cameraTransform.position.y + offsetPosY);
       // }
    }
}
