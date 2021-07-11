using System.Collections.Generic;
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

    public float windStrength = 5f;

    private Vector3 mousePos;
    private Vector2 direction;
    private Vector2 magnitude;

    public float xBound = 4f;
    public float yBound = 4f;

    public Rigidbody2D rb2d { get; private set; }
    public bool windIsBlowing { get; private set; }

    private void Awake() { // Initialize variables
        rb2d = gameObject.GetComponent<Rigidbody2D>();
        rb2d.constraints = RigidbodyConstraints2D.FreezeRotation;
        _instance = this;
    }

    private void Update() {
        if (Input.GetMouseButton(0) && !GameManager.Instance.gameWon) {
            windIsBlowing = true;
            mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            direction = mousePos - transform.position;
            float x = Mathf.Abs(direction.x);
            float y = Mathf.Abs(direction.y);
            direction = direction.normalized;
            
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

    private void FixedUpdate() {
        if (windIsBlowing) {
            rb2d.velocity = new Vector2(direction.x * windStrength * magnitude.x, direction.y * windStrength * magnitude.y) * -1;
        }
    }
}