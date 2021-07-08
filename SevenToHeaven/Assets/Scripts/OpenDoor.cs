using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class OpenDoor : MonoBehaviour {

    [SerializeField]
    private Text text;
    [SerializeField]
    GameManager gameManager;


    private LevelDoor levelDoor;
    private bool isCurrentDoorOpen;

    IEnumerator WaitForAnim() {
        yield return new WaitForSeconds(3f);
        // animator.GetCurrentAnimatorStateInfo(0).length is slow, and returns the length of the previous state. A hard-coded value is used instead.
        SceneManager.LoadScene(levelDoor.level);
    }

    IEnumerator WaitForWinAnim() {
        yield return new WaitForSeconds(3f);
        gameManager.Win();
    }

    private void Update() {
        if (Input.GetKeyDown("space"))
            if (isCurrentDoorOpen) {
                if (levelDoor.level != "Win") {
                    Animator animator = levelDoor.GetComponent<Animator>();
                    animator.SetBool("doorIsOpening", true);
                    StartCoroutine(CameraMovement.Instance.Shake(3f, 0.04f));
                    StartCoroutine(WaitForAnim());
                } else {
                    // Check if winning is possible here
                    Animator animator = levelDoor.GetComponent<Animator>();
                    animator.SetBool("doorIsOpening", true);
                    StartCoroutine(CameraMovement.Instance.Shake(3f, 0.04f));
                    StartCoroutine(WaitForWinAnim());
                }
            }
    }

    private void FixedUpdate() {
        RaycastHit hit;
        isCurrentDoorOpen = false;
        if (Physics.Raycast(transform.position, Vector3.forward, out hit)) {
            levelDoor = hit.transform.GetComponent<LevelDoor>();
            if (gameManager != null) {
                if (!gameManager.gameLost) {
                    if (levelDoor.isUnlocked == true) {
                        text.text = "Press [SPACE] to enter";
                        isCurrentDoorOpen = true;
                    } else {
                        text.text = "This door is locked";
                    }
                }
            } else {
                if (levelDoor.isUnlocked == true) {
                    text.text = "Press [SPACE] to enter";
                    isCurrentDoorOpen = true;
                } else {
                    text.text = "This door is locked";
                }
            }
        }/* else
            text.text = "";
*/
    }

}