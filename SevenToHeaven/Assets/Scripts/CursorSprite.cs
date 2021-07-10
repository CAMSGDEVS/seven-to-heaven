using UnityEngine;

public class CursorSprite : MonoBehaviour {

    [SerializeField]
    private GameObject player;

    private float angle;
    private Texture2D texture;
    private int textureIndex;
    private Vector3 mousePos;
    private Object[] cursorTextures, cursorTexturesFlipped;

    private void Awake() {
        cursorTextures = Resources.LoadAll("Cursor/Cursor", typeof(Texture2D));
        cursorTexturesFlipped = Resources.LoadAll("Cursor/CursorFlipped", typeof(Texture2D));
    }

    private void Update() {
        if (GameManager.Instance.respawnFinished) {
            player = GameManager.Instance.seven;
            mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            angle = Mathf.Atan2(player.transform.position.y - mousePos.y, player.transform.position.x - mousePos.x) * Mathf.Rad2Deg;
            textureIndex = (int) Mathf.Abs(angle / 20f) % 8;

            if (angle > -20f && angle < 160f) {
                texture = (Texture2D)cursorTextures[textureIndex];
            } else {
                texture = (Texture2D)cursorTexturesFlipped[textureIndex];
            }

            Cursor.SetCursor(texture, Vector2.zero, CursorMode.ForceSoftware);
        }
    }

}
