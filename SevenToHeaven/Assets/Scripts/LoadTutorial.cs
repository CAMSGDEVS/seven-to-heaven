using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class LoadTutorial : MonoBehaviour
{
    public GameObject transitionPrefab;
    public float transitionTime;

    private bool tutorialAlreadyLoaded = false;
    public int xLeft, yBottom, yTop;

    private void Update()
    {
        if (!tutorialAlreadyLoaded) {
            if (GameManager.Instance.respawnFinished) {
                float x = GameManager.Instance.seven.transform.position.x;
                float y = GameManager.Instance.seven.transform.position.y;
                if (x < xLeft || y < yBottom || y > yTop) {
                    LoadTutorialLevel();
                    tutorialAlreadyLoaded = true;
                }
            }
        }
    }

    private void LoadTutorialLevel() {
        GameManager.Instance.ResetVars();
        StartCoroutine(waitForTransition());
    }

    private IEnumerator waitForTransition() {
        Instantiate(transitionPrefab, Vector2.zero, Quaternion.identity);
        yield return new WaitForSeconds(transitionTime);
        SceneManager.LoadScene("Tutorial");
    }
}
