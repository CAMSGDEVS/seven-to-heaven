using UnityEngine;

public class Audio : MonoBehaviour
{
    private static Audio _instance;
    public static Audio Instance {
        get { 
            if (_instance == null) {
                Debug.LogError("Audio is null");
            }
            return _instance;
        } private set { }

    }
    public AudioSource audioSource;
    public bool playing;
    public bool toggleChange;

    private void Awake()
    {
        _instance = this;
    }
    private void Start() {
        audioSource = GetComponent<AudioSource>();
        playing = true;
    }

    private void Update() {
        if (playing == true && toggleChange == true) {
            audioSource.Play();
            toggleChange = false;
        }
        if (playing == false && toggleChange == true) {
            audioSource.Stop();
            toggleChange = false;
        }
    }
    public void StopAudio() {
        audioSource.Stop();
        toggleChange = false;
    }

    public void PlayAudio() {
        audioSource.Play();
        toggleChange = false;
    }

    public void ToggleAudio() {
        toggleChange = true;
        playing = !playing;
    }
}
