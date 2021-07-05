using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : HomingObject
{
    [System.NonSerialized]
    public Fade fader;

    private Color originalColor;
    void Start() { //Initialize variables
        rb2d = gameObject.GetComponent<Rigidbody2D>();
        fader = gameObject.GetComponent<Fade>();
    }

    // Update is called once per frame
    void Update() {
        Fade();
    }
    private void FixedUpdate() {
        MoveTowardsTarget();
    }
    private void Fade() {
        if (fader != null) {
            fader.FadeObject();
            if (fader.fadeEnded) {
                PlayerAttack.Projectiles.Remove(this);
                GameObject.Destroy(this.gameObject);
            }
        }
    }
}
