using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fade : MonoBehaviour
{
    public float despawnSeconds = 5f;
    public float timePassed = 0f;
    public bool fadeActive = true;
    public bool fadeEnded = false;

    private Color originalColor;

    private void Start() {
        originalColor = gameObject.GetComponentInChildren<SpriteRenderer>().color;
    }

    public void FadeObject() { // Call on update to fade
        timePassed += Time.deltaTime;
        Color spriteColor = gameObject.GetComponentInChildren<SpriteRenderer>().color;

        float fade = originalColor.a * 0.125f + 0.875f * originalColor.a * ((despawnSeconds - timePassed) / despawnSeconds);
        spriteColor = new Color(spriteColor.r, spriteColor.g, spriteColor.b, fade);
        gameObject.GetComponentInChildren<SpriteRenderer>().color = spriteColor;
        if (timePassed >= despawnSeconds) {
            fadeEnded = true;
        }
    }
}
