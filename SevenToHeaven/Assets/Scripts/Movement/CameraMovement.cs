using UnityEngine;
using System.Collections;

public class CameraMovement : MonoBehaviour {
    private static CameraMovement _instance;
    public static CameraMovement Instance {
        get {
            if (_instance == null) {
                Debug.LogError("CameraMovement is null");
            }
            return _instance;
        }
        set { }
    }
    public Transform target;

    public Transform anchor;
    public Transform alternateTarget;

    public IEnumerator Shake (float duration, float magnitude) {
        float elapsed = 0.0f;
        Vector3 originalPos = transform.localPosition;

        while (elapsed < duration) {
            float x = Random.Range(-1f, 1f) * magnitude;
            float y = Random.Range(-1f, 1f) * magnitude;

            transform.localPosition = new Vector3(x, y, -10f);
            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.localPosition = originalPos;
    }

    private void Awake() {
        _instance = this;
        target = GameManager.Instance.transform;
    }
    private void LateUpdate() { // Do this by parenting in editor
        if (GameManager.Instance.respawnFinished == false) {
            if (alternateTarget != null) {
                target = alternateTarget;
            }
        } else {
            target = GameManager.Instance.seven.transform;
        }

        anchor.position = new Vector3(target.position.x, target.position.y, -10f); // Lock z-axis
    }

    private IEnumerator ClearAltTarget() {
        yield return new WaitForSeconds(2f);
        target = null;
    }
}