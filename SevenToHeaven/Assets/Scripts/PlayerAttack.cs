using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PlayerAttack : MonoBehaviour
{
    private static PlayerAttack _instance;
    public static PlayerAttack Instance {
        get { 
            if (_instance = null){
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

    [SerializeField]
    private GameObject projectilePrefab;

    [SerializeField]
    private float cooldownSeconds = 3;
    [SerializeField]
    private float attackRange = 3;
    private float secondsSinceLastAttack = 3;

    public Rigidbody2D rb2d { get; private set; }

    private List<Enemy> enemiesInRange = new List<Enemy>();
    private bool mouseAlreadyClicked = false;
    private Enemy targetedEnemy;

    private void Awake() {
        rb2d = gameObject.GetComponent<Rigidbody2D>();
    }
    private void Start() {
        
    }

    private void Update() {
        if (secondsSinceLastAttack < cooldownSeconds) {
            secondsSinceLastAttack += Time.deltaTime;
        }
        if (Input.GetMouseButtonDown(1)) {
            if (!mouseAlreadyClicked) {
                mouseAlreadyClicked = true;
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
        if (enemiesInRange.Any()) {
            targetedEnemy = enemiesInRange[0];
            Instantiate(projectilePrefab, gameObject.transform.position - new Vector3(0, 0.125f), Quaternion.identity);
            secondsSinceLastAttack = 0;
        }
    }
    private void CheckEnemiesInRange() {
        enemiesInRange = new List<Enemy>();
        foreach (Enemy enemy in EnemyList) {
            enemy.CalculateDistanceFromPlayer();
            if (enemy.DistanceFromPlayer <= attackRange) { //Enemies in the attack range are added to the list
                enemiesInRange.Add(enemy);
            }
        }
        enemiesInRange = enemiesInRange.OrderBy(enemy => enemy.DistanceFromPlayer).ToList();
    }
}
