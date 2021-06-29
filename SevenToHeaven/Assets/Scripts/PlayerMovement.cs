using UnityEngine;

public class PlayerMovement : MonoBehaviour {

    public float windStrength = 5f;

    private Vector3 mousePos;
    private Vector2 direction;

    private Rigidbody2D rb2d;
    private bool windIsBlowing;


    private void Start() { // Initialize variable
        rb2d = gameObject.GetComponent<Rigidbody2D>();
    }

    private void Update() {
        if (Input.GetMouseButton(0)) {
            windIsBlowing = true;
            mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            direction = (mousePos - transform.position).normalized;
        } else
            windIsBlowing = false;
    }

    private void FixedUpdate() {
        if (windIsBlowing) {
            rb2d.velocity = new Vector2(direction.x * -1 * windStrength, direction.y * -1 * windStrength);
        }
    }

}