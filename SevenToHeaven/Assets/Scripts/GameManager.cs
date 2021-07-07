using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

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

    public bool gameWon = false, gameLost = false;

    public Dictionary<string, int> statList = new Dictionary<string, int>() {
        {"Points", 0},
        {"Kills", 0},
    };

    public void Win() {
        winCanvas.SetActive(true); // Change to a smoother transition later if needed.
        inGameText.transform.gameObject.SetActive(false);
        gameWon = true;
        foreach (var stat in statList) {
            Text newStat = Instantiate(statTemplate, Vector3.zero, Quaternion.identity, statTemplateHolder.transform);
            newStat.text = stat.Key + ": " + stat.Value;
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