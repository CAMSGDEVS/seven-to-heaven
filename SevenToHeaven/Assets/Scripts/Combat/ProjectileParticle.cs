using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileParticle : MonoBehaviour
{
    // Start is called before the first frame update
    private void Start() {
        StartCoroutine(destroyAfterTimer());
    }

    private IEnumerator destroyAfterTimer() {
        yield return new WaitForSeconds(0.5f);
        Destroy(this.gameObject);
    }
}
