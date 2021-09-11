using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatingSpikes : MonoBehaviour
{
    [SerializeField]
    private Rigidbody2D center;

    [SerializeField]
    private float radius = 2f;
    [SerializeField]
    private float degreesPerFrame = 10f;
    [SerializeField]
    private float rotationSpeed = 200f;
    [SerializeField]
    private bool clockwise = false;

    [SerializeField]
    private Rigidbody2D rb2d;

    [SerializeField]
    private float angle = 0;

    private void Awake() {
        rb2d = GetComponent<Rigidbody2D>();
    }

    // Rotate every frame
    private void Update() {
        transform.position = PolarToCartesian(radius, (clockwise ? 360 - angle : angle)) + center.position;
        rb2d.angularVelocity = rotationSpeed;
        angle = (angle + degreesPerFrame) % 360;
    }

    private Vector2 PolarToCartesian(float radius, float angle) {
        float x = radius * Mathf.Cos(Mathf.Deg2Rad * angle);
        float y = radius * Mathf.Sin(Mathf.Deg2Rad * angle);
        return new Vector2(x, y);
    }
}
