using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public GameObject target;
    private Rigidbody2D rb2d;

    [SerializeField]
    private float speed = 1f;
    [SerializeField]
    private float rotationSpeed = 200f;
    public float despawnSeconds = 100f;
    public float timePassed = 0f;

    private Color originalColor;
    void Start() { //Initialize variables
        rb2d = gameObject.GetComponent<Rigidbody2D>();
        originalColor = gameObject.GetComponent<SpriteRenderer>().color;
    }

    // Update is called once per frame
    void Update() {
        Fade();
    }
    private void Fade() {
        Color spriteColor = gameObject.GetComponent<SpriteRenderer>().color;
        float fade = spriteColor.a - (originalColor.a * 0.8f * (Time.deltaTime/despawnSeconds));
        spriteColor = new Color(spriteColor.r, spriteColor.g, spriteColor.b, fade);
        gameObject.GetComponent<SpriteRenderer>().color = spriteColor;

        timePassed += Time.deltaTime;

        if (timePassed >= despawnSeconds) {
            PlayerAttack.Projectiles.Remove(this);
            GameObject.Destroy(this.gameObject);
        }
    }

    private void FixedUpdate() {
        if (target != null) {
            Vector2 direction = (Vector2) target.transform.position - rb2d.position;
            direction.Normalize();
            float rotation = Vector3.Cross(direction, transform.up).z;
            rb2d.angularVelocity = -rotation * rotationSpeed;
            rb2d.velocity = transform.up * speed;
        }
    }
}
