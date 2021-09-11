using System.Collections;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    [SerializeField]
    private Color completedColor;

    public int checkpointNumber;
    [SerializeField]
    private GameObject sevenPrefab;
    [SerializeField]
    private GameObject respawnAnimationPrefab;

    // Initialize checkpoint
    private void Awake() {
        GameManager.Instance.Checkpoints.Add(this);
        // Change color if already activated
        if (GameManager.checkpointNumber >= checkpointNumber) {
            GetComponent<SpriteRenderer>().color = completedColor;
        }
    }
    // Detect player entering collider
    private void OnTriggerEnter2D(Collider2D collision) {
        PlayerAttack playerAttack = collision.gameObject.GetComponent<PlayerAttack>();
        if (playerAttack != null && GameManager.checkpointNumber < checkpointNumber) {
            AudioManager.Instance.Play("checkpoint");
            GameManager.checkpointNumber = checkpointNumber;
            GetComponent<SpriteRenderer>().color = completedColor;
        }
    }
    public void RespawnSeven() {
        AudioManager.Instance.Play("respawn");
        StartCoroutine(RespawnSevenCoroutine());
    }

    private IEnumerator RespawnSevenCoroutine() {
        // Instantiate transition prefab at center of screen
        CameraMovement.Instance.target = transform;
        if (GameManager.Instance.transitionPrefab != null && GameManager.statList["Deaths"] == 0) {
            GameObject transition = Instantiate(GameManager.Instance.transitionPrefab, Vector2.zero, Quaternion.identity);
            yield return new WaitForSeconds(GameManager.Instance.transitionTime);
            Destroy(transition);
        }
        // Instantiate respawn animation and seven, then destroy animation
        GameObject respawnAnimation = Instantiate(respawnAnimationPrefab, gameObject.transform.position, Quaternion.identity);
        yield return new WaitForSeconds(1);
        GameManager.Instance.seven = Instantiate(sevenPrefab, gameObject.transform.position, Quaternion.identity);
        yield return new WaitForSeconds(0.5f);
        GameManager.Instance.respawnFinished = true;
        CameraMovement.Instance.target = GameManager.Instance.seven.transform;
        GameManager.Instance.respawnFinished = true;
        Destroy(respawnAnimation);
    }


}
