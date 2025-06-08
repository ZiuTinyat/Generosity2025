using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour
{
    public Transform start;

    private GameController gc => GameController.Instance;

    public void StartLevel() {
        gameObject.SetActive(true);
        gc.master.transform.position = start.position;
        gc.dog.transform.position = start.position;
    }

    public void UnloadLevel() {
        Destroy(gameObject);
    }
}
