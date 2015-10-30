using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using System.Collections.Generic;

[CanEditMultipleObjects]
[CustomEditor(typeof(GraphicsBroadcaster), true)]
public class GraphicsBroadcasterEditor : Editor {
    public static bool broadcastColor;

    void OnEnable()
    {;
    if (EditorPrefs.HasKey(GetSaveKey(0)))
        broadcastColor = EditorPrefs.GetBool(GetSaveKey(0));
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        GraphicsBroadcaster broadcaster = (GraphicsBroadcaster)target;

        broadcastColor = EditorGUILayout.Toggle("Изменять цвет", broadcastColor);
        if (broadcastColor)
            broadcaster.ApplyColorChanges();

        EditorPrefs.SetBool(GetSaveKey(0), broadcastColor);
    }

    string GetSaveKey(int type)
    {
        switch (type)
        {
            case 0:
                return typeof(GraphicsBroadcasterEditor).Name + "broadcastColor";
        }

        return "";
    }
}
