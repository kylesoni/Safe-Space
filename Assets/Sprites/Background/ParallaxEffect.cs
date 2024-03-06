using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxEffect : MonoBehaviour
{
    [SerializeField] private float parallaxSpeedX;
    [SerializeField] private float parallaxSpeedY;

    private Transform cameraTransform;
    private float startPositionX, startPositionY;
    private float spriteSizeX, spriteSizeY;
    // Start is called before the first frame update
    void Start()
    {
        cameraTransform = Camera.main.transform;
        startPositionX = transform.position.x;
        startPositionY = transform.position.y;
        spriteSizeX = GetComponent<SpriteRenderer>().bounds.size.x;
    }

    // Update is called once per frame
    void Update()
    {
        float relativeDist = cameraTransform.position.x * parallaxSpeedX;
        float relativeDistY = cameraTransform.position.y * parallaxSpeedY;
        transform.position = new Vector3(startPositionX + relativeDist, startPositionY + relativeDistY, transform.position.z);

        float relativeCameraDist = cameraTransform.position.x * (1 - parallaxSpeedX);
        if (relativeCameraDist > startPositionX + spriteSizeX)
        {
            startPositionX += spriteSizeX;
        }
        else if (relativeCameraDist < startPositionX - spriteSizeX)
        {
            startPositionX -= spriteSizeX;
        }
    }
}
