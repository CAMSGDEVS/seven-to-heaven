using UnityEngine;

public class CursorSprite : MonoBehaviour {

    public GameObject player;
    public GameObject cursorSprite;
    public SpriteRenderer sRenderer;

    private float angle;
    private Vector3 mousePos;
    private Object[] cursorSprites;
    private Sprite sprite;

    private void Start() {
        sRenderer = cursorSprite.GetComponent<SpriteRenderer>();
        cursorSprites = Resources.LoadAll("Cursor", typeof(Sprite));
        Cursor.visible = false;
    }

    private void Update() {
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        cursorSprite.transform.position = mousePos + new Vector3(0, 0, 10);
        angle = Mathf.Atan2(player.transform.position.y - mousePos.y, player.transform.position.x - mousePos.x) * Mathf.Rad2Deg;

        if (angle > -90f && angle < 90f) {
            sprite = (Sprite)cursorSprites[(int)Mathf.Abs(angle / 22.5f) % 4];
            sRenderer.flipX = false;
        } else {
            sprite = (Sprite)cursorSprites[3 - (int)Mathf.Abs(angle / 22.5f) % 4];
            sRenderer.flipX = true;
        }
        if (angle > 0)
            sRenderer.flipY = false;
        else
            sRenderer.flipY = true;

        sRenderer.sprite = sprite;
    }

}
