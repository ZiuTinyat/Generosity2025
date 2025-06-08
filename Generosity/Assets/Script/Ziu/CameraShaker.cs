using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace Ziu
{
    public class CameraShaker : MonoBehaviourSingleton<CameraShaker>
    {
        public static void Shake(float duration, float freq, float amp) {
            Instance.StartCoroutine(ShakeCoroutine(duration, freq, amp));
        }

        public static IEnumerator ShakeCoroutine(float duration, float freq, float amp) {
            yield return Instance.ShakeCoroutineInternal(duration, freq, amp);
        }

        private IEnumerator ShakeCoroutineInternal(float duration, float freq, float amp) {
            Vector3 orig = transform.localPosition;
            for (float t = 0; t < duration; t += Time.deltaTime) {
                float delta = amp * Mathf.Sin(2 * Mathf.PI * freq * t);
                transform.localPosition = orig + Vector3.up * delta;
                yield return null;
            }
            transform.localPosition = orig;
        }
    }
}
