using UnityEngine;

public class CameraFollow : MonoBehaviour {

    [SerializeField]
    private Transform target;

    private void LateUpdate() {
        transform.position = new Vector3(target.position.x, target.position.y, -10f); // Lock z-axis
    }
}