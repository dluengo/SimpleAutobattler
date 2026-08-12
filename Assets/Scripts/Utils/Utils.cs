using System;
using System.Collections.Generic;
using UnityEngine;
using System.Text;

public static class Utils
{
    // --- Methods ---
    public static Vector3 GetRandomPositionCircle(Vector3 position, float radius)
    {
        float angle = UnityEngine.Random.Range(0f, 360f);
        float rad = angle * Mathf.Deg2Rad;
        float x = Mathf.Cos(rad) * radius;
        float y = Mathf.Sin(rad) * radius;
        return new Vector3(x, y, 0f) + position;
    }
}
