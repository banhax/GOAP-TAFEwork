using UnityEngine;
using UnityEngine.InputSystem;

public class TestingExitControl : MonoBehaviour {
    Keyboard kb;
    
    private void Awake() {
        kb = Keyboard.current;
    }

    void Update() {
        if (kb.escapeKey.wasPressedThisFrame) {
            Application.Quit();
        }
    }
}
