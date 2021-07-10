using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SignText : MonoBehaviour {
    private static SignText _instance;
    public static SignText Instance {
        get {
            if (_instance == null) {
                Debug.LogError("signtext is null");
            }
            return _instance;
        } private set { }
    }

    private void Awake() {
        _instance = this;
        panel.SetActive(false);
    }

    public GameObject panel;
    public Text text;
}
