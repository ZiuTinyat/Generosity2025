using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using Ziu;

public class Event_lv2 : MonoBehaviour
{
    public GameObject scene1, scene2;
    public Transform newMasterMid, newDogMid;
    public Transform newMasterStart, newDogStart;

    private GameController gc => GameController.Instance;
    public bool triggered = false;

    private void OnTriggerEnter2D(Collider2D collision) {
        if (triggered) return;
        Dog dog = collision.GetComponent<Dog>();
        if (dog) {
            // Trigger event
            triggered = true;
        }
    }
}
