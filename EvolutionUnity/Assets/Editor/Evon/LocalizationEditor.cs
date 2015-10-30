using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Localization), true)]
public class LocalizationEditor : Editor {

    public static bool changeFoldout;
    public static SystemLanguage changeLanguage;

    void OnEnable()
    {
        if (EditorPrefs.HasKey(GetSaveKey(0)))
            changeFoldout = EditorPrefs.GetBool(GetSaveKey(0));

        if (EditorPrefs.HasKey(GetSaveKey(1)))
            changeLanguage = (SystemLanguage) EditorPrefs.GetInt(GetSaveKey(1));
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        ChangeLanguageView();
    }

    void ChangeLanguageView()
    {
        if (!Application.isPlaying)
            return;
        Localization localization = (Localization)target;
        changeFoldout = EditorGUILayout.Foldout(changeFoldout, "Изменить язык");
        EditorPrefs.SetBool(GetSaveKey(0), changeFoldout);
        if (changeFoldout)
        {
            EditorGUILayout.LabelField("Текущий язык: " + localization.CurrentLanguage.ToStr());
            changeLanguage = (SystemLanguage)EditorGUILayout.EnumPopup("Новый язык:", (System.Enum)changeLanguage);
            EditorPrefs.SetInt(GetSaveKey(1), (int)changeLanguage);
            if (GUILayout.Button("Применить"))
                localization.CurrentLanguage = changeLanguage;
        }
    }
    
    string GetSaveKey(int type)
    {
        string baseStr = typeof(LocalizationEditor).Name;
        switch (type)
        {
            case 0:
                return baseStr += "changeFoldout";
            case 1:
                return baseStr += "changeLanguage";
        }

        return "";
    }
}
