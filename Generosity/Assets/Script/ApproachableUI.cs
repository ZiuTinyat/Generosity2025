using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ApproachableUI : MonoBehaviour
{
    public Transform indicator;
    public Transform instruction;

    public TMP_Text text;

    public void SetText(string txt) {
        text.text = txt;
    }

    public void ShowInstruction(float duration = 0.2f) {
        StartCoroutine(ShowInstructionCoroutine(duration));
    }

    public void HideInstruction(float duration = 0.2f) {
        StartCoroutine(HideInstructionCoroutine(duration));
    }

    IEnumerator ShowInstructionCoroutine(float duration) {
        instruction.gameObject.SetActive(true);
        for (float t = 0; t < duration; t += Time.deltaTime) {
            float r = t / duration;
            instruction.localScale = r * Vector3.one;
            indicator.localScale = (1 - r) * Vector3.one;
            yield return null;
        }
        instruction.localScale = Vector3.one;
        indicator.gameObject.SetActive(false);
    }

    IEnumerator HideInstructionCoroutine(float duration) {
        indicator.gameObject.SetActive(true);
        for (float t = 0; t < duration; t += Time.deltaTime) {
            float r = t / duration;
            instruction.localScale = (1 - r) * Vector3.one;
            indicator.localScale = r * Vector3.one;
            yield return null;
        }
        indicator.localScale = Vector3.one;
        instruction.gameObject.SetActive(false);
    }
}
