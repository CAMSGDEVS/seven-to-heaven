using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class OpenDoor : MonoBehaviour {

    public Text text;
    private bool isCurrentDoorOpen;
    private LevelDoor levelDoor;

    private void Update() {
        if (Input.GetKeyDown("space"))
            if (isCurrentDoorOpen)
                SceneManager.LoadScene(levelDoor.level);
    }

    private void FixedUpdate() {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.forward, out hit)) {
            text.transform.gameObject.SetActive(true);
            levelDoor = hit.transform.GetComponent<LevelDoor>();
            if (levelDoor.isUnlocked == true) {
                text.text = "Press [SPACE] to enter";
                isCurrentDoorOpen = true;
            } else {
                text.text = "This door is locked";
            }
        } else {
            text.transform.gameObject.SetActive(false);
        }
    }

}