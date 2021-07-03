using UnityEngine;

public class CursorSprite : MonoBehaviour {

    [SerializeField]
    private GameObject player, cursorSprite;

    private float angle;
    private Sprite sprite;
    private int spriteIndex;
    private Vector3 mousePos;
    private Object[] cursorSprites;
    private SpriteRenderer sRenderer;

    private void Awake() { // Initialize variables
        sRenderer = cursorSprite.GetComponent<SpriteRenderer>();
        cursorSprites = Resources.LoadAll("Cursor", typeof(Sprite));
        Cursor.visible = false;
    }

    private void Update() {
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        cursorSprite.transform.position = mousePos + new Vector3(0, 0, 10);
        angle = Mathf.Atan2(player.transform.position.y - mousePos.y, player.transform.position.x - mousePos.x) * Mathf.Rad2Deg;
        spriteIndex = (int)Mathf.Abs(angle / 22.5f) % 4;

        if (angle > -105f && angle < 105f) {
            sprite = (Sprite)cursorSprites[spriteIndex];
            sRenderer.flipX = false;
        } else {
            sprite = (Sprite)cursorSprites[3 - spriteIndex]; // Subtract 3 to reverse the sprite index when on the opposite side of the player y-axis.
            sRenderer.flipX = true;
        }
        
        if (angle > 0)
            sRenderer.flipY = false;
        else
            sRenderer.flipY = true;

        cursorSprite.transform.rotation = Quaternion.Euler(0, 0, 0);
        if (Mathf.Abs(angle) < 105 && Mathf.Abs(angle) > 90) { // Rotate the 0 degree sprite to 90 or -90 degrees when needed.
            if (angle < 0) {
                cursorSprite.transform.rotation = Quaternion.Euler(0, 0, -90);
            } else {
                cursorSprite.transform.rotation = Quaternion.Euler(0, 0, 90);
            }
        }

        sRenderer.sprite = sprite;
    }

}
