using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    private float length;
    private float width;
    private float startPosition;
    private GameObject cameraObj;
    [SerializeField]
    private float parallaxScale;

    void Start() {
        cameraObj = CameraMovement.Instance.anchor.gameObject;
        startPosition = transform.position.x;
        length = GetComponent<SpriteRenderer>().bounds.size.x;
        width = GetComponent<SpriteRenderer>().bounds.size.y;
    }

    // Update is called once per frame
    void Update() {
        float temp = cameraObj.transform.position.x * (1 - parallaxScale);
        float distance = (cameraObj.transform.position.x * parallaxScale);
        transform.position = new Vector3(startPosition + distance, transform.position.y, transform.position.z);
        
        if (temp > startPosition + length) {
            startPosition += length;
        } else if (temp < startPosition - length) {
            startPosition -= length;
        }
    }
}
