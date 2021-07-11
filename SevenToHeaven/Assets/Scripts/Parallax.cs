using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    private float length;
    private float width;
    [SerializeField]
    private float startPositionX;
    [SerializeField]
    private float startPositionY;
    [SerializeField]
    private GameObject cameraObj;
    [SerializeField]
    private float parallaxScale;

    private float offsetX;
    private float offsetY;

    void Start() {
        startPositionX = cameraObj.transform.position.x + transform.position.x;
        startPositionY = cameraObj.transform.position.y + transform.position.y;
        length = GetComponent<SpriteRenderer>().bounds.size.x;
        width = GetComponent<SpriteRenderer>().bounds.size.y;
        offsetX = cameraObj.transform.position.x - transform.position.x;
        offsetY = cameraObj.transform.position.y - transform.position.y;
    }

    void Update() {
        float temp = cameraObj.transform.position.x * (1 - parallaxScale);
        float temp2 = cameraObj.transform.position.y * (1 - parallaxScale);
        float distance = cameraObj.transform.position.x * parallaxScale;
        float distance2 = cameraObj.transform.position.y * parallaxScale;
        transform.position = new Vector3(startPositionX + distance + offsetX, startPositionY + distance2 + offsetY, transform.position.z);
        
        if (temp > startPositionX + length) {
            startPositionX += length;
        } else if (temp < startPositionX - length) {
            startPositionX -= length;
        }
        if (temp2 > startPositionY + width) {
            startPositionY += width;
        } else if (temp2 < startPositionY - width) {
            startPositionY -= width;
        }
    }
}
