using UnityEngine;

public class PausedPlayButton : MonoBehaviour {
    public GameObject menuObject;

    void Awake() {
        Time.timeScale = 0;
    }

    public void Play() {
        Time.timeScale = 1;
        menuObject.SetActive(false);
    }
}
