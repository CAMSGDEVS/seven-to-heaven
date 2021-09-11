using UnityEngine;

public class AudioManager : MonoBehaviour {

    // Singleton
    private static AudioManager _instance;
    public static AudioManager Instance {
        get {
            if (_instance == null) {
                Debug.LogError("AudioManager is null");
            }
            return _instance;
        }
        set { }
    }

    [SerializeField]
	private AudioSource source;

	[SerializeField]
    private AudioClip[] clips; 

    // Play audio clip
	public void Play(string clip) {
        foreach (AudioClip c in clips) {
            if (c.name == clip)
		        source.clip = c;
        }
		source.Play();
	}

    private void Awake() {
        _instance = this;
    }

}