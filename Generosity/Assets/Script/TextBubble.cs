using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TextBubble : MonoBehaviour
{
    public TMP_Text text;
    public SpriteRenderer bubble;
    public Vector2 textEdge;

    public void SetText(string txt) {
        text.text = txt;
        text.ForceMeshUpdate(true, true);
        if (txt != "") {
            bubble.size = (Vector2) text.textBounds.size / bubble.transform.localScale.x + textEdge;
        } else {
            bubble.size = Vector2.zero;
        }
        text.transform.localPosition = text.textBounds.max;
    }

    public void ShowText(string txt) {
        gameObject.SetActive(true);
        SetText(txt);
        StartCoroutine(ShowCoroutine(0.1f));
    }

    public void HideText() {
        gameObject.SetActive(false);
        //StartCoroutine(HideCoroutine(0.1f));
    }

    public void ShowTextDuration(float duration, string txt) {
        StartCoroutine(ShowTextDurationCoroutine(duration, txt));
    }

    public IEnumerator ShowTextDurationCoroutine(float duration, string txt) {
        ShowText(txt);
        yield return new WaitForSeconds(duration);
        HideText();
    }

    private IEnumerator ShowCoroutine(float duration) {
        for (float t = 0; t < duration; t += Time.deltaTime) {
            float r = t / duration;
            transform.localScale = Vector3.one * r;
            yield return null;
        }
        transform.localScale = Vector3.one;
    }
    private IEnumerator HideCoroutine(float duration) {
        for (float t = 0; t < duration; t += Time.deltaTime) {
            float r = t / duration;
            transform.localScale = Vector3.one * (1 - r);
            yield return null;
        }
        transform.localScale = Vector3.one;
    }
}

