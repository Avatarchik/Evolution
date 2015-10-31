using UnityEngine;
using UnityEditor;
using UnityEngine.UI;

[CanEditMultipleObjects]
[CustomEditor(typeof(LocalizedText))]
public class LocalizedTextEditor : Editor {
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        if (Application.isPlaying)
            return;
        Localization.ReadFiles();
        LocalizedText localizedText = (LocalizedText) target;
        localizedText.GetComponent<Text>().text = localizedText.upperCase ? Localization.Get(localizedText.key).ToUpper() : Localization.Get(localizedText.key);
        EditorUtility.SetDirty(localizedText);
    }
}
