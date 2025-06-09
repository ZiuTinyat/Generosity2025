using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    public void LoadMain() {
        SceneManager.LoadSceneAsync("Main");
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.Space)) {
            LoadMain();
        }
    }
}
