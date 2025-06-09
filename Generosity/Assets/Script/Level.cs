using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ziu;

public class Level : MonoBehaviour
{
    public Transform masterStart, dogStart;

    public AudioSource earthquake, collapse;

    private GameController gc => GameController.Instance;

    public void StartLevel(int level) {
        gameObject.SetActive(true);
        gc.master.transform.position = masterStart.position;
        gc.dog.transform.position = dogStart.position;

        switch (level) {
            case 0:
                StartCoroutine(Level0Coroutine());
                break;
            case 1:
                StartCoroutine(Level1Coroutine());
                break;
            default:
                break;
        }
    }

    public void UnloadLevel() {
        Destroy(gameObject);
    }

    private IEnumerator Dialog(float duration, string txt) {
        yield return gc.master.voice.ShowTextDurationCoroutine(duration, txt);
        yield return new WaitForSeconds(0.5f);
    }

    private IEnumerator DialogUntil(Func<bool> condition, string txt) {
        gc.master.voice.ShowText(txt);
        yield return new WaitUntil(condition);
        yield return new WaitForSeconds(0.5f);
        gc.master.voice.HideText();
        yield return new WaitForSeconds(0.5f);
    }

    private IEnumerator Level0Coroutine() {
        gc.dog.HoldMove();
        earthquake.Play();
        CameraFader.Fadeout(0);
        yield return new WaitForSeconds(3f);
        collapse.Play();
        yield return new WaitForSeconds(1f);
        yield return Dialog(2.5f, $"Oh no! What happened!?");
        yield return Dialog(2.5f, $"I believe it was an earthquake");

        // Bark
        gc.master.voice.ShowText($"{gc.dogName}, are you OK?");
        yield return new WaitForSeconds(2f);
        gc.barkHint.SetActive(true);
        yield return new WaitUntil(() => gc.dog.GetBarked());
        yield return new WaitForSeconds(0.5f);
        gc.master.voice.HideText();
        yield return new WaitForSeconds(0.5f);
        yield return Dialog(3f, $"Good! We should run before the building collapses");
        CameraFader.Fadein(2);
        gc.dog.UnholdMove();

        // Harness
        yield return DialogUntil(() => gc.dog.wearingHarness, $"{gc.dogName}, get your Harness and let's get out");
        yield return Dialog(4f, $"Good boy {gc.dogName}! Help me see the way");
    }

    private IEnumerator Level1Coroutine() {
        var evt = GetComponentInChildren<Event_lv2>();
        Debug.Assert(evt);

        yield return DialogUntil(() => evt.triggered, $"We should go up to find a way out...");
        
        // Shake
        gc.dog.HoldMove();
        earthquake.Play();
        yield return CameraShaker.ShakeCoroutine(1.5f, 25f, 2);

        // Crash
        CameraFader.Fadeout(0);
        if (gc.dog.isLeashed) gc.dog.SwitchLeashState();
        evt.scene1.SetActive(false);
        evt.scene2.SetActive(true);
        gc.dog.transform.position = evt.newDogMid.position;
        yield return Dialog(1f, $"{gc.dogName}! {gc.dogName}!");
        collapse.Play();
        yield return Dialog(2f, $"{gc.dogName}! Where are you!");

        // Finding dog
        gc.master.transform.position = evt.newMasterMid.position;
        yield return DialogUntil(() => gc.dog.GetBarked(), $"Speak to me!");
        yield return new WaitUntil(() => gc.dog.GetBarked());
        yield return new WaitForSeconds(3f);

        // Found
        gc.dog.transform.position = evt.newDogStart.position;
        gc.master.transform.position = evt.newMasterStart.position;
        if (!gc.dog.isLeashed) gc.dog.SwitchLeashState();
        if (gc.dog.isLeashed) gc.dog.SwitchLeashState();
        CameraFader.Fadein(1f);
        gc.dog.UnholdMove();
        yield return Dialog(2f, $"Thank god {gc.dogName}! Let's find the other stairs");

    }
}
