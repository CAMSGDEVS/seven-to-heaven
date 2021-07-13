using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class MainMenu : MonoBehaviour
{
    public GameObject transitionPrefab;

    public Animator animator;
    public void OpenCredits() {
        animator.SetBool("CreditsOpen", true);
    }
    public void CloseCredits() {
        animator.SetBool("CreditsOpen", false);
    }
    public void LoadScene() {
        StartCoroutine(waitForTransition());
    }

    private IEnumerator waitForTransition() {
        Instantiate(transitionPrefab, Vector2.zero, Quaternion.identity);
        yield return new WaitForSeconds(0.5f);
        SceneManager.LoadScene("LevelSelect");
    }
    public void Exit() {
        Application.Quit();
    }
}
