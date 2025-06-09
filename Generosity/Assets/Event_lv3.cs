using System;
using System.Collections;
using System.Collections.Generic;
using System.Timers;
using UnityEngine;

public class Event_lv3 : MonoBehaviour
{
    public Transform wall, pile;
    public GameObject hiddenBlocker;

    public void HammerHit() {
        StartCoroutine(HitCoroutine());
    }

    private IEnumerator HitCoroutine() {
        for (float t = 0; t < 1f; t += Time.deltaTime) {
            wall.Translate(Vector3.down * Time.deltaTime * 5f);
            pile.Translate(Vector3.up * Time.deltaTime * 2f);
            yield return null;
        }
        wall.gameObject.SetActive(false);
        Destroy(hiddenBlocker.gameObject);
    }

    private void Update() {
        if (hiddenBlocker) {
            hiddenBlocker.SetActive(GameController.Instance.dog.isLeashed);
        }
    }
}
