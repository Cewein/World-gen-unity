using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor (typeof(MapGenerator))]
public class MapGenEditor : Editor {

    public override void OnInspectorGUI()
    {
        MapGenerator mapGen = (MapGenerator)target;

        if (DrawDefaultInspector())
        {
            if(mapGen.AutoUpdate)
            {
                mapGen.UpdateMapInEditor();
            }
        }

        if(GUILayout.Button ("Generate"))
        {
            mapGen.UpdateMapInEditor();
        }

        if (GUILayout.Button("Order Biomes"))
        {
            mapGen.CheckBiomes();
        }
    }


}
