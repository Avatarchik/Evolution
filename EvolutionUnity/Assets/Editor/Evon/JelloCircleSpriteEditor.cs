using UnityEngine;
using UnityEditor;
[CustomEditor(typeof(JelloBodyShapeCircle))]
public class JelloCircleSpriteEditor : Editor {
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        JelloBodyShapeCircle jcSprite = (JelloBodyShapeCircle) target;
        if (GUILayout.Button("Обновить гейометрию"))
        {
            jcSprite.UpdateGeometry();
            EditorUtility.SetDirty(jcSprite.JelloBody);
        }
    }
}
