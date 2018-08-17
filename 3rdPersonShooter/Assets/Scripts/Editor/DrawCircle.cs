using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(BaseSpawner))]
public class DrawCircle : Editor
{

    private BaseSpawner spawner;

    public void OnSceneGUI()
    {
        spawner = this.target as BaseSpawner;
        Handles.color = Color.red;
        Handles.DrawWireDisc(spawner.transform.position + (spawner.transform.forward * spawner.zOffset) + (spawner.transform.right * spawner.xOffset) // position + offset
                                      , spawner.transform.up                       // normal
                                      , spawner.radius);                              // radius
    }
}
