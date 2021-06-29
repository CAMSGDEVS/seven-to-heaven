using UnityEngine;

public class CursorSprite : MonoBehaviour {

    public GameObject player;
    public GameObject cursorSprite;

    private float angle;
    private Vector3 mousePos;

    private void Start() {
        Cursor.visible = false;
    }

    private void Update() {
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        cursorSprite.transform.position = mousePos + new Vector3(0, 0, 10);

        angle = Mathf.Atan2(player.transform.position.y - mousePos.y, player.transform.position.x - mousePos.x) * Mathf.Rad2Deg;
        cursorSprite.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
    }

}
