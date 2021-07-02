using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {
    private PlayerMovement _instance;
    public PlayerMovement Instance {
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

    private Rigidbody2D rb2d;
    private bool windIsBlowing;

    public bool onGround;
    private void Awake() {
        _instance = this;
    }

    private void Start() { // Initialize variable
        rb2d = gameObject.GetComponent<Rigidbody2D>();
        rb2d.constraints = RigidbodyConstraints2D.FreezeRotation;
        
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