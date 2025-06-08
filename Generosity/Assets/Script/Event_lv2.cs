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
    private bool triggered = false;

    private void OnTriggerEnter2D(Collider2D collision) {
        if (triggered) return;
        Dog dog = collision.GetComponent<Dog>();
        if (dog) {
            // Trigger event
            triggered = true;
            StartCoroutine(EventCoroutine());
        }
    }

    private IEnumerator EventCoroutine() {
        // Shake
        gc.dog.HoldMove();
        yield return CameraShaker.ShakeCoroutine(1.5f, 25f, 2);

        // Crash
        CameraFader.Fadeout(0); 
        if (gc.dog.isLeashed) gc.dog.SwitchLeashState();
        scene1.SetActive(false);
        scene2.SetActive(true);
        gc.dog.transform.position = newDogMid.position;
        yield return new WaitForSeconds(1f);

        // Finding dog
        gc.master.transform.position = newMasterMid.position;
        yield return new WaitUntil(() => gc.dog.GetBarked());
        yield return new WaitForSeconds(1f);

        // Found
        gc.dog.transform.position = newDogStart.position;
        gc.master.transform.position = newMasterStart.position;
        if (!gc.dog.isLeashed) gc.dog.SwitchLeashState();
        yield return new WaitForSeconds(0.5f);
        if (gc.dog.isLeashed) gc.dog.SwitchLeashState();
        CameraFader.Fadein(0);
        gc.dog.UnholdMove();

    }
}
