using UnityEngine;
using UnityEditor;

/// <summary>
/// Редактор EditorPrefs
/// </summary>
public class EditorPrefsEditor
{
    /// <summary>
    /// Удаляет все
    /// </summary>
    [MenuItem("My Game/EditorPrefs/Clear")]
    private static void Clear()
    {
        EditorPrefs.DeleteAll();
        Log.Info("EditorPrefs удалены");
    }
}