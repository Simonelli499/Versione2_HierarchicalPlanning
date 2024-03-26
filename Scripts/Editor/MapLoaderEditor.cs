using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


[CustomEditor(typeof(MapLoader))]
public class MapLoaderEditor : Editor
{
    int mapIndex = 0;
    
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        MapLoader mapLoader = (MapLoader)target;


        //open window to load map from file
        if (GUILayout.Button("Load Map"))
        {
            string filePath = EditorUtility.OpenFilePanel("Load map file", "", "csv");
            if (filePath.Length != 0)
            {
                mapLoader.LoadMapFromCSV(filePath);
                
            }
        }
        
        GUILayout.Space(10);

        if (mapLoader.maps != null)
            EditorGUILayout.LabelField("Maps Count: " + mapLoader.maps.Count);

        EditorGUILayout.BeginHorizontal();
        
        mapIndex = EditorGUILayout.IntField("Map Index to print", mapIndex);

        //print map to console
        if (GUILayout.Button("Print Map"))
        {
            mapLoader.PrintMapMatrix(mapIndex);
        }

        EditorGUILayout.EndHorizontal();
        EditorUtility.SetDirty(this);
        EditorUtility.SetDirty(target);
    }
}

