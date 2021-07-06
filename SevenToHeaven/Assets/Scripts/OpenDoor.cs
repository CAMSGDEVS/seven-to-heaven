using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class OpenDoor : MonoBehaviour {

    [SerializeField]
    private Text text;

    [SerializeField]
    private Animator animator;

    private LevelDoor levelDoor;
    private bool isCurrentDoorOpen;

    IEnumerator WaitForAnim() {
        yield return new WaitForSeconds(3f);
        // animator.GetCurrentAnimatorStateInfo(0).length is slow, and returns the length of the previous state. A hard-coded value is used instead.

        SceneManager.LoadScene(levelDoor.level);
    }

    private void Update() {
        if (Input.GetKeyDown("space"))
            if (isCurrentDoorOpen) {
                animator.SetBool("doorIsOpening", true);
                StartCoroutine(CameraMovement.Instance.Shake(3f, 0.04f));
                StartCoroutine(WaitForAnim());
            }
    }

    private void FixedUpdate() {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.forward, out hit)) {
            text.transform.gameObject.SetActive(true);
            levelDoor = hit.transform.GetComponent<LevelDoor>();
            if (levelDoor.isUnlocked == true) {
                text.text = "Press [SPACE] to enter";
                isCurrentDoorOpen = true;
            } else
                text.text = "This door is locked";
        } else
            text.transform.gameObject.SetActive(false);
    }

}