using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

[CustomEditor(typeof(Dialogs))]
public class DialogsEditor : Editor {
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        if (Application.isPlaying)
            return;
        Dialogs dialogs = (Dialogs)target;
        dialogs.presets = new List<Dialog>();
        foreach (Transform t in dialogs.presetsParent)
            dialogs.presets.Add(t.GetComponent<Dialog>());

        if (GUILayout.Button("Выключить пресеты"))
            foreach (Dialog dialog in dialogs.presets)
                dialog.gameObject.SetActive(false);

        EditorUtility.SetDirty(dialogs);
    }
}
