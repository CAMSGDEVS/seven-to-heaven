using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dart : MonoBehaviour
{
    private void Start() {
        StartCoroutine(selfDestruct());
    }
    private IEnumerator selfDestruct() {
        yield return new WaitForSeconds(0.5f);
        Destroy(gameObject);
    }
}

