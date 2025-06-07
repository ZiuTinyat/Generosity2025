using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ziu
{
    public static class Curve
    {
        public static float AnchoredPositionToEulerAngle(Vector2 pos)
        {
            return Mathf.Atan2(-pos.x, pos.y) * Mathf.Rad2Deg;
        }

        public static float CurveInvSqr(float ratio) // [0, 1] to [0, 1]
        {
            float r = ratio - 1;
            return 1 - r * r;
        }

        public static float CurveBidirInvSqr(float ratio) // [-1, 1] to [0, 1]
        {
            float t = (ratio >= 0) ? CurveInvSqr(ratio) : -CurveInvSqr(-ratio);
            return (t + 1) / 2;
        }
    }
}
