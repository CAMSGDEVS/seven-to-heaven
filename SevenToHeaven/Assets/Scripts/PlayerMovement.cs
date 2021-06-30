using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {

    public float windStrength = 5f;

    private Vector3 mousePos;
    private Vector2 direction;

    private Rigidbody2D rb2d;
    private bool windIsBlowing;

    private Animator movementAnimator;
    [SerializeField]
    private SpriteRenderer sevenSprite;
    private List<int> animatorParameterIds = new List<int>();

    private void Start() { // Initialize variable
        rb2d = gameObject.GetComponent<Rigidbody2D>();
        movementAnimator = gameObject.GetComponent<Animator>();
        animatorParameterIds.Add(Animator.StringToHash("IsInAir"));
        animatorParameterIds.Add(Animator.StringToHash("MovingHorizontally"));
    }

    private void Update() {
        if (Input.GetMouseButton(0)) {
            windIsBlowing = true;
            mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            direction = (mousePos - transform.position).normalized;
        } else
            windIsBlowing = false;
        if (rb2d.velocity.x > -0.5 && rb2d.velocity.x < 0.5) {
            movementAnimator.SetBool(animatorParameterIds[1], false);
        }
        else {
            movementAnimator.SetBool(animatorParameterIds[1], true);
            if (rb2d.velocity.x > 0) {
                sevenSprite.flipX = false;
            }
            else {
                sevenSprite.flipX = true;
            }
        }
    }

    private void FixedUpdate() {
        if (windIsBlowing) {
            rb2d.velocity = new Vector2(direction.x * -1 * windStrength, direction.y * -1 * windStrength);
        }
    }

}