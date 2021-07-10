using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class EndScreen : MonoBehaviour
{
    [TextArea(2,3)]
    public string endText;
    [SerializeField]
    private Sign sign;

    private void Start()
    {
        if (GameManager.tutorialFinished) {
            sign.signText = endText;
        }
    }
}
