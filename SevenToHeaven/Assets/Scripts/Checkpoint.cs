using System.Collections;
using System.Collections.Generic;
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
    private void Awake() {
        GameManager.Instance.Checkpoints.Add(this);
    }
    private void OnTriggerEnter2D(Collider2D collision) {
        PlayerAttack playerAttack = collision.gameObject.GetComponent<PlayerAttack>();
        if (playerAttack != null && GameManager.checkpointNumber < checkpointNumber) {
            GameManager.checkpointNumber = checkpointNumber;
            GetComponent<SpriteRenderer>().color = completedColor;
        }
    }
    public void RespawnSeven() {
        StartCoroutine(RespawnSevenCoroutine());
    }

    private IEnumerator RespawnSevenCoroutine() {
        GetComponent<SpriteRenderer>().color = completedColor;
        GameObject respawnAnimation = Instantiate(respawnAnimationPrefab, gameObject.transform.position, Quaternion.identity);
        yield return new WaitForSeconds(1);
        GameManager.Instance.seven = Instantiate(sevenPrefab, gameObject.transform.position, Quaternion.identity);
        yield return new WaitForSeconds(0.5f);
        GameManager.Instance.respawnFinished = true;
        CameraMovement.Instance.target = GameManager.Instance.seven.transform;
        Destroy(respawnAnimation);
        
        
    }
}
