using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : HomingObject
{
    public float damage = 1f;
    public bool playerProjectile = true;
    [System.NonSerialized]
    public Fade fader;
    [System.NonSerialized]
    public GameObject source;
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
                PlayerAttack.PlayerProjectiles.Remove(this);
                GameObject.Destroy(this.gameObject);
            }
        }
    }
}
