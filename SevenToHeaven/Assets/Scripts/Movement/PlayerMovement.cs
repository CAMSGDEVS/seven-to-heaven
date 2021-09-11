using System.Collections;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {
    private static PlayerMovement _instance;
    public static PlayerMovement Instance {
        get {
            if (_instance == null) {
                Debug.LogError("PlayerMovement is null");
            }
            return _instance;
        } set {}
    }

    private float bounceEndVelocity = 2f; //The velocity when control is returned to the player after a bounce
    public float windStrength = 5f;
    public float yBound = 4f;
    public float xBound = 4f;

    private Vector3 mousePos, lastVelocity;
    public Vector2 direction, magnitude;

    private bool isBouncing = false;

    public Rigidbody2D rb2d { get; private set; }
    public bool windIsBlowing { get; private set; }

    private void Awake() { // Initialize variables
        rb2d = gameObject.GetComponent<Rigidbody2D>();
        rb2d.constraints = RigidbodyConstraints2D.FreezeRotation;
        _instance = this;
    }

    private void Update() {
        lastVelocity = rb2d.velocity;

        if (Input.GetMouseButton(0) && !GameManager.Instance.gameWon) {
            windIsBlowing = true;
            mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            direction = mousePos - transform.position;
            float x = Mathf.Abs(direction.x);
            float y = Mathf.Abs(direction.y);
            direction = direction.normalized;
            
            // Decrease speed if too fast
            if (x > xBound) {
                x = xBound;
                windIsBlowing = false;
            }
            if (y > yBound) {
                y = yBound;
                windIsBlowing = false;
            }
            x = xBound - x;
            y = yBound - y;
            magnitude = new Vector2(x,y);

        } else
            windIsBlowing = false;
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        // Bounce lightly off of tiles and greatly off of bumpers
        if (collision.collider.transform.tag == "Tile") {
            isBouncing = true;
            float bounceSpeed = lastVelocity.magnitude;
            Vector3 bounceDirection = Vector3.Reflect(lastVelocity.normalized, collision.contacts[0].normal);
            bounceDirection.y = 0;
            rb2d.velocity = bounceDirection * Mathf.Min(bounceSpeed, 5f);
        } else if (collision.collider.transform.tag == "Bumper") {
            isBouncing = true;
            float bounceSpeed = lastVelocity.magnitude*2;
            Vector3 bounceDirection = Vector3.Reflect(lastVelocity.normalized, collision.contacts[0].normal);
            rb2d.velocity = bounceDirection * Mathf.Min(bounceSpeed, 10f);
        }
    }

    private void FixedUpdate() {
        // Reverse direction if bouncing
        if (!isBouncing) {
            if (windIsBlowing)
                rb2d.velocity = new Vector2(direction.x * windStrength * magnitude.x, direction.y * windStrength * magnitude.y) * -1;
        } else {
            if (rb2d.velocity.magnitude <= bounceEndVelocity) {
                isBouncing = false;
            }
        }
    }
}