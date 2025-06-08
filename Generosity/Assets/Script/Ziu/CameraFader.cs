using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace Ziu
{
    public class CameraFader : MonoBehaviourRequire<SpriteRenderer>
    {
        public static CameraFader Instance;
        private void Awake() {
            Debug.Assert(!Instance);
            Instance = this;
        }

        private SpriteRenderer spriteRenderer => _t;

        public static void Fadeout(float duration = 0.5f) {
            Instance.StartCoroutine(FadeoutCoroutine(duration));
        }

        public static void Fadein(float duration = 0.5f) {
            Instance.StartCoroutine(FadeinCoroutine(duration));
        }

        public static IEnumerator FadeoutCoroutine(float duration = 0.5f) {
            yield return Instance.FadeoutCoroutineInternal(duration);
        }

        public static IEnumerator FadeinCoroutine(float duration = 0.5f) {
            yield return Instance.FadeinCoroutineInternal(duration);
        }

        private IEnumerator FadeoutCoroutineInternal(float duration) {
            for (float t = 0; t < duration; t += Time.deltaTime) {
                float r = t / duration;
                spriteRenderer.color = new Color(0, 0, 0, r);
                yield return null;
            }
            spriteRenderer.color = new Color(0, 0, 0, 1);
        }

        private IEnumerator FadeinCoroutineInternal(float duration) {
            for (float t = 0; t < duration; t += Time.deltaTime) {
                float r = t / duration;
                spriteRenderer.color = new Color(0, 0, 0, (1 - r));
                yield return null;
            }
            spriteRenderer.color = new Color(0, 0, 0, 0);
        }
    }
}
