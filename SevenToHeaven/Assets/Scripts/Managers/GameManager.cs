using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using System.Linq;
using System.Collections;

public class GameManager : MonoBehaviour {
    public GameObject transitionPrefab;
    public float transitionTime;

    public GameObject checkpointPrefab;
    public GameObject seven;
    public bool respawnFinished = false;
    public static bool tutorialFinished = false;

    private static GameManager _instance;
    public static GameManager Instance {
        get {
            if (_instance == null) {
                Debug.LogError("GameManager is null");
            }
            return _instance;
        }
        set { }
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

    public List<Sign> signs = new List<Sign>();

    [SerializeField]
    private GameObject loseCanvas, winCanvas, statTemplateHolder, sevenDeathPrefab;

    [SerializeField]
    private Text statTemplate; 
    public Text inGameText;

    public static int checkpointNumber = 0;
    public List<Checkpoint> Checkpoints = new List<Checkpoint>();
    public bool gameWon = false, gameLost = false;
    private GameObject sevenDeath; // Holds the instantialized prefabs

    public static Dictionary<string, int> statList = new Dictionary<string, int>() {
        {"Points", 0},
        {"Kills", 0},
        {"Deaths",0}
    };

    public void ResetVars() {
        checkpointNumber = 0;
        statList["Points"] = 0;
        statList["Kills"] = 0;
        statList["Deaths"] = 0;
    }
    
    private void Start() {
        RespawnSeven();
    }

    public void Win() {
        winCanvas.SetActive(true); // Change to a smoother transition later if needed.
        inGameText.transform.gameObject.SetActive(false);
        gameWon = true;
        foreach (var stat in statList) { // Display the list of stats in UI from the statList dictionatry
            Text newStat = Instantiate(statTemplate, Vector3.zero, Quaternion.identity, statTemplateHolder.transform);
            newStat.text = stat.Key + ": " + stat.Value;
        }

        //Temporary for demo
        tutorialFinished = true;
    }

    public void RespawnSeven() {
        if (seven != null) {
            seven.SetActive(true);
            Destroy(sevenDeath);
        }
        Checkpoint currentCheckpoint = null;
        foreach (Checkpoint checkpoint in Checkpoints) {
            if (checkpoint.checkpointNumber == checkpointNumber) {
                currentCheckpoint = checkpoint;
                break;
            }
        }
        if (currentCheckpoint != null) {
            CameraMovement.Instance.target = currentCheckpoint.transform;
            currentCheckpoint.RespawnSeven();
        } else {
            GameObject checkpointGameObject = Instantiate(checkpointPrefab, Vector2.zero, Quaternion.identity);
            currentCheckpoint = checkpointGameObject.GetComponent<Checkpoint>();
            checkpointNumber = 0;
            currentCheckpoint.checkpointNumber = 0;
            currentCheckpoint.RespawnSeven();
        }
    }

    public void Lose() {
        PlayerAttack.Instance.StopAllCoroutines();
        gameLost = true;
        inGameText.text = "Press [SPACE] to continue...";

        sevenDeath = Instantiate(sevenDeathPrefab); // Instantiate GameObject to play Seven's death animation
        sevenDeath.transform.position = seven.transform.position;

        seven.SetActive(false); // Inactivate seven to prevent reference errors with Destory()
    }

    private void Update() {
        if (Input.GetKeyDown("space")) {
            if (gameWon) {
                // Show end of demo UI
                ResetVars();
                SceneManager.LoadScene("LevelSelect");
            }
            if (gameLost) {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }
    }

    private void Awake() {
        _enemyList.Clear();
        _instance = this;
    }
}