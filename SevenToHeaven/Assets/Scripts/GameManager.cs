using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

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
    private Text pointText, statTemplate;

    public void Win(int points, List<string> stats) {
        winCanvas.SetActive(true); // Change to a smoother transition later if needed.
        pointText.text = points.ToString();
        for (int i = 0; i < stats.Count; i++) {
            Text newStat = Instantiate(statTemplate, Vector3.zero, Quaternion.identity, statTemplateHolder.transform);
            newStat.text = stats[i];
        }
    }

    private void Awake() {
        _instance = this;
    }
}