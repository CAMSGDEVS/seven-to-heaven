using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class SignDetector : MonoBehaviour
{
    [SerializeField]
    private Text text;
    [SerializeField]
    private GameObject panel;

    private void Start() {
        panel = SignText.Instance.panel;
        text = SignText.Instance.text;
        StartCoroutine(detectSign());
    }

    // Detect if transform inside sign bounds
    private IEnumerator detectSign() {
        while (true) {
            bool inSign = false;
            foreach (Sign sign in GameManager.Instance.signs) {
                if (transform.position.x > sign.xLeft && transform.position.x < sign.xRight) {
                    if (transform.position.y > sign.yBottom && transform.position.y < sign.yTop) {
                        inSign = true;
                        panel.SetActive(true);
                        text.text = sign.signText;
                        break;
                    }
                }
            }
            if (!inSign) {
                text.text = "";
                panel.SetActive(false);
            }
            yield return new WaitForSeconds(0.125f);
        }
    }
}
