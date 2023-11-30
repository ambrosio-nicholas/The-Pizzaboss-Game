using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parrallax : MonoBehaviour
{
    [SerializeField] private Vector2 parrallaxEffectMultiplier;

    private Transform cameraTransform;
    private Vector3 lastCameraPosition;
    private float textureUnitSizeX;
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
    }

    private void LateUpdate()
    {
        deltaMovement = cameraTransform.position - lastCameraPosition;
        transform.position += new Vector3(deltaMovement.x * parrallaxEffectMultiplier.x, deltaMovement.y * parrallaxEffectMultiplier.y);
        lastCameraPosition = cameraTransform.position;

        if (Mathf.Abs(cameraTransform.position.x - transform.position.x) >= textureUnitSizeX)
        {
            float offsetPosX = (cameraTransform.position.x - transform.position.x);
            transform.position = new Vector3(cameraTransform.position.x + offsetPosX, transform.position.y);
        }

    }
}
