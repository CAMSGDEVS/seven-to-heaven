using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dart : MonoBehaviour
{
    // Self-destructs after 0.5 seconds
    private void Start() {
        StartCoroutine(selfDestruct());
    }
    private IEnumerator selfDestruct() {
        yield return new WaitForSeconds(0.5f);
        Destroy(gameObject);
    }
}

