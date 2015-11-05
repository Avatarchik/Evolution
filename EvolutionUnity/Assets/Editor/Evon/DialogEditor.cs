using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Dialog), true)]
public class DialogEditor : Editor {
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        if (!Application.isPlaying)
            return;
        Dialog dialog = (Dialog)target;
        if (GUILayout.Button("Показать"))
            dialog.DoShow();
        if (GUILayout.Button("Спрятать"))
            dialog.DoHide();
        if (GUILayout.Button("Уничтожить"))
            dialog.DoDestroy();
    }
}
