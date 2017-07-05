using UnityEngine;
using System.Collections;

class myMath
{
    public static float parabolicScaleCalc(float time, float scale)
    {
        return scale * (-Mathf.Pow(time, 2) + 1);
    }
}