using UnityEngine;
using UnityEngine.InputSystem;

public class PauseControl : MonoBehaviour {

    GameObject pauseText;

    // im tired lol
    public GameObject startMenu;
    public GameObject winMenu;
    public GameObject gameTip;

    private void Start() {
        pauseText = this.gameObject.transform.GetChild(0).gameObject;
    }

    private void Update() {

        // self explanatory
        if (startMenu == null
         || winMenu == null
         || gameTip == null) {
            return;
        }

        // avoid being able to pause game with start or win menu open
        // could work off timescale, but im tired and i think that could be error prone
        if (startMenu.activeSelf
         || winMenu.activeSelf) {
            return;
        }

        if (Keyboard.current.pKey.wasPressedThisFrame) {
            if (!pauseText.activeSelf) {
                Time.timeScale = 0;
                gameTip.SetActive(false);
                pauseText.SetActive(true);
            }
            else {
                Time.timeScale = 1;
                gameTip.SetActive(true);
                pauseText.SetActive(false);
            }
        }
    }
}
