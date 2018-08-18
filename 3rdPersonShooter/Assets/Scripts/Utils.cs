using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Utils
{
    //checks if a point is within a circle area
    public static bool WithinBounds(Vector2 point, Circle2D circle)
    {
        float dist = Vector2.Distance(point, circle.pos);
        return (dist < circle.radius);
    }
}
