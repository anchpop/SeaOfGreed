using UnityEngine;
using System.Collections;

public static class Extensions
{
    public static void AddForce(this Rigidbody2D rigidbody2D, Vector2 force, ForceMode mode = ForceMode.Force)
    {
        switch (mode)
        {
            case ForceMode.Force:
                rigidbody2D.AddForce(force);
                break;
            case ForceMode.Impulse:
                rigidbody2D.AddForce(force / Time.fixedDeltaTime);
                break;
            case ForceMode.Acceleration:
                rigidbody2D.AddForce(force * rigidbody2D.mass);
                break;
            case ForceMode.VelocityChange:
                rigidbody2D.AddForce(force * rigidbody2D.mass / Time.fixedDeltaTime);
                break;
        }
    }

    public static void AddForce(this Rigidbody2D rigidbody2D, float x, float y, ForceMode mode = ForceMode.Force)
    {
        rigidbody2D.AddForce(new Vector2(x, y), mode);
    }



    public static Vector2 Rotate(this Vector2 v, float degrees)
    {
        float radians = degrees * Mathf.Deg2Rad;
        float sin = Mathf.Sin(radians);
        float cos = Mathf.Cos(radians);

        float tx = v.x;
        float ty = v.y;

        return new Vector2(cos * tx - sin * ty, sin * tx + cos * ty);
    }

    public static bool Colinear(this Vector3 a, Vector3 b, Vector3 c)
    {
        return Mathf.Abs((b.y - a.y) * (c.x - b.x) - (c.y - b.y) * (b.x - a.x)) < .00001;
    }


}