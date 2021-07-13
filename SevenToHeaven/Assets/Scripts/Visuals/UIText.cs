using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UIText : MonoBehaviour
{
    public Text text;
    private static UIText _instance;
    public static UIText Instance {
        get {
            if (_instance == null) {
                Debug.LogError("UIText is null");
            }
            return _instance;
        }
        set { }
    }
    private void Awake() {
        _instance = this;
        text = GetComponent<Text>();
    }
}
