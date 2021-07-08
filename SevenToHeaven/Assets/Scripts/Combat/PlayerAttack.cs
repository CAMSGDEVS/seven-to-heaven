using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PlayerAttack : MonoBehaviour
{
    private static PlayerAttack _instance;
    public static PlayerAttack Instance {
        get { 
            if (_instance == null){
                Debug.LogError("Playerattack is null");
            }
            return _instance;
        } private set { }
    }
    private static List<Enemy> _enemyList = new List<Enemy>();
    public static List<Enemy> EnemyList {
        get {
            if (_enemyList == null) {
                Debug.LogError("EnemyList is null");
            }
            return _enemyList;
        }
        set { }
    }
    private static List<Projectile> _playerProjectiles = new List<Projectile>();
    public static List<Projectile> PlayerProjectiles {
        get {
            if (_playerProjectiles == null) {
                Debug.LogError("Projectiles is null");
            }
            return _playerProjectiles;
        }
        set { }
    }
    public float health = 5f;

    [SerializeField]
    private GameObject projectilePrefab;

    [SerializeField]
    private float cooldownSeconds = 3;
    [SerializeField]
    private float attackRange = 3;
    private float secondsSinceLastAttack = 3;
    [SerializeField]
    private float hitInvincibility = 0.5f;
    private float invincibilityTimePassed = 0.5f;

    public Rigidbody2D rb2d { get; private set; }

    private List<Enemy> enemiesInRange = new List<Enemy>();
    private bool mouseAlreadyClicked = false;
    private bool dead = false;

    private void Awake() { //Initialize Variables
        _playerProjectiles.Clear();
        _enemyList.Clear();
        PlayerAttack._instance = this;
        rb2d = gameObject.GetComponent<Rigidbody2D>();
        invincibilityTimePassed = hitInvincibility;
    }

    private void Update() {
        //Update attack and invincibility timers
        if (secondsSinceLastAttack < cooldownSeconds) {
            secondsSinceLastAttack += Time.deltaTime;
        }
        if (invincibilityTimePassed < hitInvincibility) {
            invincibilityTimePassed += Time.deltaTime;
        }
        //Detect right click for attacks
        if (Input.GetMouseButtonDown(1)) {
            if (!mouseAlreadyClicked) {
                mouseAlreadyClicked = true; //Prevent input from being detected twice
                if (secondsSinceLastAttack >= cooldownSeconds) {
                    Attack();
                }
            }
        } else {
            mouseAlreadyClicked = false;
        }
    }

    private void Attack() {
        CheckEnemiesInRange();
        GameObject projectile = Instantiate(projectilePrefab, gameObject.transform.position - new Vector3(0, 0.125f), Quaternion.identity);
        Projectile projectileComponent = projectile.GetComponent<Projectile>();
        PlayerProjectiles.Add(projectile.GetComponent<Projectile>());
        secondsSinceLastAttack = 0;
        if (EnemyList.Any()) {
            if (enemiesInRange.Any()) {
                projectileComponent.target = enemiesInRange[0].gameObject;
            } else {
                projectileComponent.target = gameObject;
                projectile.GetComponent<Fade>().despawnSeconds /= 3;
            }
        } else {
            projectileComponent.target = gameObject;
            projectile.GetComponent<Fade>().despawnSeconds /= 3;
        }
        projectile.transform.rotation = gameObject.transform.rotation;
        projectileComponent.source = gameObject;
    }

    private void CheckEnemiesInRange() { //Checks if any enemies are in the attack range using distance formula
        enemiesInRange = new List<Enemy>();
        foreach (Enemy enemy in EnemyList) {
            enemy.CalculateDistanceFromPlayer(); //Distance formula
            if (enemy.DistanceFromPlayer <= attackRange) { //Enemies in the attack range are added to the list
                enemiesInRange.Add(enemy);
            }
        }
        enemiesInRange = enemiesInRange.OrderBy(enemy => enemy.DistanceFromPlayer).ToList();
    }

    private void OnTriggerEnter2D(Collider2D collision) { //Detects Collisions
        Projectile projectile = collision.gameObject.GetComponent<Projectile>(); 
        Enemy collidedEnemy = collision.gameObject.GetComponent<Enemy>();
        if (invincibilityTimePassed >= hitInvincibility) { //Prevents hits during invulnerability
            if (projectile != null && !projectile.playerProjectile) { //Detect Projectiles
                health -= projectile.damage;
                projectile.source.GetComponent<Enemy>().projectiles.Remove(projectile);
                GameObject.Destroy(projectile.gameObject);
                if (health > 0) {
                    StartCoroutine(damageAnimation());
                }
            } else { //Detect Enemies
                if (collidedEnemy != null && collidedEnemy.attackMelee) {
                    health -= collidedEnemy.meleeDamage;
                    if (health > 0) {
                        StartCoroutine(damageAnimation());
                    }
                }   
            }
            invincibilityTimePassed = 0f; //Reset Invincibility Timer
        } else { //Destroys projectiles that touch the player during invincibility
            if (projectile != null && !projectile.playerProjectile) {
                projectile.source.GetComponent<Enemy>().projectiles.Remove(projectile);
                GameObject.Destroy(projectile.gameObject);
            }
        }
        if (health <= 0) { //Check for player death

            //Destroying all projectiles to prevent further damage or
            //an accidental level completion if killing enemies becomes an objective
            foreach (Projectile proj in PlayerProjectiles) {
                GameObject.Destroy(proj.gameObject);
            }
            PlayerProjectiles.Clear();
            foreach (Enemy enemy in EnemyList) {
                foreach (Projectile proj in enemy.projectiles) {
                    GameObject.Destroy(proj);
                }

                enemy.projectiles.Clear();
                enemy.target = enemy.gameObject;
            }

            if (dead == false) { // Prevent Lose from being called multiple times
                GameManager.Instance.Lose();
                dead = true;
            }
        }
    }
    
    private IEnumerator damageAnimation() { //Sprite flashes to black 2 times during hitInvincibility
        SpriteRenderer spriteR = gameObject.GetComponentInChildren<SpriteRenderer>();
        Color originalColor = spriteR.color;
        for (int j = 0; j < 2; j++) { 
            for (float i = 1; i > 0.5f; i += -0.5f / ((hitInvincibility / 4) * 60) ) {
                spriteR.color = new Color(i * originalColor.r, i * originalColor.g, i * originalColor.b, spriteR.color.a);
                yield return new WaitForSeconds(0.016f);
            }
            for (float i = 0.5f; i < 1; i += 0.5f / ((hitInvincibility / 4) * 60)) {
                spriteR.color = new Color(i * originalColor.r, i * originalColor.g, i * originalColor.b, spriteR.color.a);
                yield return new WaitForSeconds(0.016f);
            }
        }
        spriteR.color = originalColor;
        yield return null;
    }
}
