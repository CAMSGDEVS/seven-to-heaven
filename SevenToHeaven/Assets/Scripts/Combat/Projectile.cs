using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : HomingObject
{
    
    [System.NonSerialized]
    public EnemyBoundingBox boundingBox;

    private float length;

    public Color enemyParticleColor;
    public GameObject particlePrefab;

    public float damage = 1f;
    public bool playerProjectile = true;
    [System.NonSerialized]
    public Fade fader;
    [System.NonSerialized]
    public GameObject source;
    private Color originalColor;
    private void Awake() {
        length = GetComponent<CircleCollider2D>().radius;
    }
    void Start() { //Initialize variables
        rb2d = gameObject.GetComponent<Rigidbody2D>();
        fader = gameObject.GetComponent<Fade>();
    }

    public void SpawnParticles() {
        GameObject particle = Instantiate(particlePrefab, transform.position, transform.rotation);
        if (!playerProjectile) {
            particle.GetComponentInChildren<SpriteRenderer>().color = enemyParticleColor;
        }
    }

    // Update is called once per frame
    void Update() {
        Fade();
    }
    private void FixedUpdate() {
        MoveTowardsTarget();
        CheckIfOutsideBoundingBox();
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
    private void CheckIfOutsideBoundingBox() {
        if (boundingBox != null) {
            float x = transform.position.x;
            float y = transform.position.y;
            if (x < boundingBox.xLeft + length) {
                x = boundingBox.xLeft + length;
            } else if (x > boundingBox.xRight - length) {
                x = boundingBox.xRight - length;
            }
            if (y < boundingBox.yBottom + length) {
                y = boundingBox.yBottom + length;
            } else if (y > boundingBox.yTop - length) {
                y = boundingBox.yTop - length;
            }
            transform.position = new Vector3(x, y);
        }
    }
}
