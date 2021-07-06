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

    [SerializeField]
    private Transform target;

    [SerializeField]
    private Transform anchor;

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
    }

    private void LateUpdate() {
        anchor.position = new Vector3(target.position.x, target.position.y, -10f); // Lock z-axis
    }
}