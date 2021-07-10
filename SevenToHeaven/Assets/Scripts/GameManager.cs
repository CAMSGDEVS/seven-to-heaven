using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using System.Linq;

public class GameManager : MonoBehaviour {
    public GameObject checkpointPrefab;
    public GameObject seven;
    public bool respawnFinished = false;

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

    [SerializeField]
    private GameObject loseCanvas, winCanvas, statTemplateHolder;

    [SerializeField]
    private Text statTemplate, inGameText;

    public static int checkpointNumber = 0;
    public List<Checkpoint> Checkpoints = new List<Checkpoint>();

    public bool gameWon = false, gameLost = false;

    public Dictionary<string, int> statList = new Dictionary<string, int>() {
        {"Points", 0},
        {"Kills", 0},
    };

    private void Start() {
        RespawnSeven();
    }

    public void Win() {
        winCanvas.SetActive(true); // Change to a smoother transition later if needed.
        inGameText.transform.gameObject.SetActive(false);
        gameWon = true;
        foreach (var stat in statList) {
            Text newStat = Instantiate(statTemplate, Vector3.zero, Quaternion.identity, statTemplateHolder.transform);
            newStat.text = stat.Key + ": " + stat.Value;
        }
    }

    public void RespawnSeven() {
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
        gameLost = true;
        inGameText.text = "Press [SPACE] to continue...";
    }

    private void Update() {
        if (Input.GetKeyDown("space")) {
            if (gameWon) {
                // Show end of demo UI
                SceneManager.LoadScene("LevelSelect");
            }
            if (gameLost) {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }
    }

    private void Awake() {
        _instance = this;
    }
}