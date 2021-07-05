using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomingObject : MonoBehaviour
{
    public GameObject target;
    public Rigidbody2D rb2d;
    public float speed = 1f;
    [SerializeField]
    private float rotationSpeed = 200f;
    public void MoveTowardsTarget() { //Call during FixedUpdate
        if (target != null) {
            Vector2 direction = (Vector2) target.transform.position - rb2d.position;
            direction.Normalize();
            float rotation = Vector3.Cross(direction, transform.up).z;
            rb2d.angularVelocity = -rotation * rotationSpeed;
            rb2d.velocity = transform.up * speed;
        }
    }
    public void MoveAwayFromTarget() {
        MoveTowardsTarget();
        rb2d.angularVelocity *= -1;
    }
}
