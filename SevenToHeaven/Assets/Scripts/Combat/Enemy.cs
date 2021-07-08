using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : HomingObject
{
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
    public float DistanceFromPlayer { get; private set; }
    public bool TargettingPlayer = true;

    [SerializeField]
    private float visionLength = 5f;
    [SerializeField]
    private float projectileVisionLength = 4f;

    [SerializeField]
    GameManager gameManager;

    private void Awake() { // Initialize values 
        rb2d = gameObject.GetComponent<Rigidbody2D>();
    }
    private void Start() {
        PlayerAttack.EnemyList.Add(this);
    }
    private void Update() {
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

    public void CalculateDistanceFromPlayer() {
        DistanceFromPlayer = Vector3.Distance(PlayerMovement.Instance.rb2d.position, rb2d.position);
    }

    private void TargetPlayer() {
        CalculateDistanceFromPlayer();
        if (TargettingPlayer) {
            ShootProjectile();
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
        if (timeSinceLastFire >= fireCooldown) {
            if (shootsProjectiles && DistanceFromPlayer <= projectileVisionLength) {
                GameObject projectile = Instantiate(enemyProjectilePrefab, gameObject.transform.position, Quaternion.identity);
                Projectile projectileComponent = projectile.GetComponent<Projectile>();
                projectiles.Add(projectileComponent);
                projectileComponent.source = gameObject;
                projectileComponent.playerProjectile = false;
                projectileComponent.target = PlayerAttack.Instance.gameObject;
                timeSinceLastFire = 0f;
                projectile.transform.rotation = gameObject.transform.rotation;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        Projectile projectile = collision.gameObject.GetComponent<Projectile>();
        if (projectile != null && projectile.playerProjectile) {
            health -= projectile.damage;
            PlayerAttack.PlayerProjectiles.Remove(projectile);
            GameObject.Destroy(projectile.gameObject);
            if (health <= 0) {
                foreach (Projectile proj in PlayerAttack.PlayerProjectiles) {
                    if (proj.target = gameObject) {
                        proj.fader.timePassed = proj.fader.despawnSeconds - 0.125f*(proj.fader.despawnSeconds - proj.fader.timePassed);
                    }
                }
                foreach (Projectile proj in projectiles) {
                    if (proj != null) {
                        GameObject.Destroy(proj.gameObject);
                    }
                }
                projectiles.Clear();
                PlayerAttack.EnemyList.Remove(this);
                gameManager.statList["Kills"] += 1;
                gameManager.statList["Points"] += 10;
                GameObject.Destroy(gameObject);
            }
        }
    }


}

