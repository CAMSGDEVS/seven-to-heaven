using System.Collections.Generic;
using UnityEngine;

public class Enemy : HomingObject
{
    [SerializeField]
    private EnemyBoundingBox boundingBox;
    private float length, width;

    public List<Projectile> projectiles;
    [SerializeField]
    private GameObject enemyProjectilePrefab;

    public bool attackMelee = true;
    public float meleeDamage = 1f;
    [SerializeField]
    private bool shootsProjectiles = false;
    [SerializeField]
    private float projectileSpacingFromPlayer = 1f;
    [SerializeField]
    private float health = 3f;
    [SerializeField]
    private float fireCooldown = 3f;
    private float timeSinceLastFire = 0f;
    [SerializeField]
    private bool invulnerable = false;
    public float DistanceFromPlayer { get; private set; }
    public bool TargettingPlayer = true;

    [SerializeField]
    private float visionLength = 5f;
    [SerializeField]
    private float projectileVisionLength = 4f;

    private void Awake() { // Initialize values 
        rb2d = gameObject.GetComponent<Rigidbody2D>();
        length = transform.lossyScale.x/2;
        width = transform.lossyScale.y/2;
    }
    private void Start() {
        if (!invulnerable) {
            GameManager.EnemyList.Add(this);
        }
    }
    private void Update() {
        if (GameManager.Instance.respawnFinished) {
            if (PlayerAttack.Instance != null) {
                TargetPlayer();
            } else {
                rb2d.velocity = Vector2.zero;
                rb2d.angularVelocity = 0f;
            }
            if (timeSinceLastFire < fireCooldown) {
                timeSinceLastFire += Time.deltaTime;
            }
        }
    }
    private void FixedUpdate() {
        CheckIfOutsideBoundingBox();
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
            if (y < boundingBox.yBottom + width) {
                y = boundingBox.yBottom + width;
            } else if (y > boundingBox.yTop - width) {
                y = boundingBox.yTop - width;
            }
            transform.position = new Vector3(x, y);
        }
    }

    public float CalculateDistanceFromPlayer() {
        return Vector3.Distance(PlayerMovement.Instance.rb2d.position, rb2d.position);
    }

    private void TargetPlayer() {
        DistanceFromPlayer = CalculateDistanceFromPlayer();
        if (TargettingPlayer) {
            if (timeSinceLastFire >= fireCooldown) {
                if (shootsProjectiles && DistanceFromPlayer <= projectileVisionLength) {
                    ShootProjectile();
                }
            }
            if (DistanceFromPlayer <= visionLength) {
                target = PlayerAttack.Instance.gameObject;
                MoveTowardsTarget();
            } else {
                target = null;
                rb2d.velocity = Vector2.zero;
            }
        } else {
            target = null;
            rb2d.velocity = Vector2.zero;
        }
        if (!attackMelee && DistanceFromPlayer <= projectileSpacingFromPlayer) {
            MoveAwayFromTarget();
        }
    }
    private void ShootProjectile() {
        AudioManager.Instance.Play("enemy");
        GameObject projectile = Instantiate(enemyProjectilePrefab, gameObject.transform.position, Quaternion.identity);
        Projectile projectileComponent = projectile.GetComponent<Projectile>();
        projectiles.Add(projectileComponent);
        projectileComponent.source = gameObject;
        projectileComponent.playerProjectile = false;
        projectileComponent.target = PlayerAttack.Instance.gameObject;
        projectileComponent.boundingBox = boundingBox;
        timeSinceLastFire = 0f;
        projectile.transform.rotation = gameObject.transform.rotation;
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (!invulnerable) {
            Projectile projectile = collision.gameObject.GetComponent<Projectile>();
            if (projectile != null && projectile.playerProjectile) { // Take damage
                AudioManager.Instance.Play("enemyDamage");
                health -= projectile.damage/2;
                PlayerAttack.PlayerProjectiles.Remove(projectile);
                projectile.SpawnParticles();
                Destroy(projectile.gameObject);
                if (health <= 0) { // Death
                    AudioManager.Instance.Play("enemyDamage");
                    foreach (Projectile proj in PlayerAttack.PlayerProjectiles) {
                        if (proj.target = gameObject) {
                            proj.fader.timePassed = proj.fader.despawnSeconds - 0.125f*(proj.fader.despawnSeconds - proj.fader.timePassed);
                        }
                    }
                    foreach (Projectile proj in projectiles) {
                        if (proj != null) {
                            Destroy(proj.gameObject);
                        }
                    }
                    projectiles.Clear();
                    GameManager.EnemyList.Remove(this);
                    GameManager.statList["Kills"] += 1;
                    GameManager.statList["Points"] += 10;
                    Destroy(gameObject);
                }
            }
        }
    }


}

