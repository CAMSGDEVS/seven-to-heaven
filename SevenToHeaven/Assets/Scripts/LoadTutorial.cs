using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class LoadTutorial : MonoBehaviour
{
    public int xLeft, yBottom, yTop;

    private void Update()
    {
        if (GameManager.Instance.respawnFinished) {
            float x = GameManager.Instance.seven.transform.position.x;
            float y = GameManager.Instance.seven.transform.position.y;
            if (x < xLeft || y < yBottom || y > yTop) {
                LoadTutorialLevel();
            }
        }
    }

    private void LoadTutorialLevel() {
        GameManager.Instance.ResetVars();
        SceneManager.LoadScene("Tutorial");
    }
}
