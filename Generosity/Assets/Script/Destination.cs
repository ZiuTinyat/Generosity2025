using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destination : MonoBehaviour
{
    private GameController gc => GameController.Instance;

    private void OnTriggerStay2D(Collider2D collision) {
        Dog dog = collision.GetComponent<Dog>();
        if (dog && dog.isLeashed) {
            gc.PassLevel();
            Destroy(this);
        }
    }
}
