using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class SwitchSceneButton : MonoBehaviour {

    GameObject buttonText;

    private void Start() {
        buttonText = this.gameObject.transform.GetChild(0).gameObject;

        if (SceneManager.GetActiveScene().name == "Test Level 1 - Navmesh") {
            buttonText.GetComponent<TMP_Text>().text = "Switch To A*Star";
        }
        else if (SceneManager.GetActiveScene().name == "Test Level 1 - AStar") {
            buttonText.GetComponent<TMP_Text>().text = "Switch To Navmesh";
        }
    }

    public void SwitchScene() {
        if (SceneManager.GetActiveScene().name == "Test Level 1 - Navmesh") {
            SceneManager.LoadScene("Test Level 1 - AStar");
        }
        else if (SceneManager.GetActiveScene().name == "Test Level 1 - AStar") {
            SceneManager.LoadScene("Test Level 1 - Navmesh");
        }
    }

    //private void Update() {
    //    // this is dumb, but alas, i am tired
    //    buttonText = this.gameObject.transform.GetChild(0).gameObject;
    //    Debug.Log(buttonText.name);

    //    if (buttonText.GetComponent<TMP_Text>() == null) {
    //        Debug.Log("CANT FIND TEXT (SOMEHOW!)");
    //        return;
    //    }

    //    if (SceneManager.GetActiveScene().name == "Test Level 1 - Navmesh"
    //     && buttonText.GetComponent<TMP_Text>().text == "Switch To Navmesh") {
    //        return;
    //    }
    //    else if (SceneManager.GetActiveScene().name == "Test Level 1 - A*Star"
    //     && buttonText.GetComponent<TMP_Text>().text == "Switch To A*Star") {
    //        return;
    //    }

    //    if (SceneManager.GetActiveScene().name == "Test Level 1 - Navmesh") {
    //        buttonText.GetComponent<TMP_Text>().text = "Switch To A*Star";
    //    }
    //    else if (SceneManager.GetActiveScene().name == "Test Level 1 - AStar") {
    //        buttonText.GetComponent<TMP_Text>().text = "Switch To Navmesh";
    //    }
    //}
}
