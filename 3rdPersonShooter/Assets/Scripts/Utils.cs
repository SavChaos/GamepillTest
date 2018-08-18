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

    public static string ConvertMilliToTimeString(long totalMillis)
    {
        float seconds = 0;
        float minutes = 0;

        seconds = Mathf.Floor((totalMillis / 1000f));

        if (seconds >= 60)
        {
            minutes = Mathf.Floor((seconds / 60f));
            seconds -= (minutes * 60);
        }


        //Debug.Log(string.Format("{0}:{1}:{2}", minutes, seconds, (int)miliseconds));
        string timeString = string.Format("{0}:{1}", minutes, seconds.ToString("00"));

        return timeString;
    }
}
