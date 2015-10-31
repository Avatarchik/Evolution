using UnityEngine;
using UnityEditor;
using UnityEngine.UI;

[CustomEditor(typeof(ButtonExtended), true)]
public class ButtonExtendedEditor : Editor {

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();        
    }

    public override void DrawPreview(Rect previewArea)
    {
        base.DrawPreview(previewArea);
    }

    public override bool HasPreviewGUI()
    {
        return base.HasPreviewGUI();
    }

    public override void OnInteractivePreviewGUI(Rect r, GUIStyle background)
    {
        base.OnInteractivePreviewGUI(r, background);
    }

    public override void OnPreviewGUI(Rect r, GUIStyle background)
    {
        base.OnPreviewGUI(r, background);
    }

    protected override void OnHeaderGUI()
    {
        base.OnHeaderGUI();
    }

    public override void OnPreviewSettings()
    {
        base.OnPreviewSettings();
    }

    public override void ReloadPreviewInstances()
    {
        base.ReloadPreviewInstances();
    }

    public override Texture2D RenderStaticPreview(string assetPath, Object[] subAssets, int width, int height)
    {
        return base.RenderStaticPreview(assetPath, subAssets, width, height);
    }

    public override bool RequiresConstantRepaint()
    {
        return base.RequiresConstantRepaint();
    }

    public override bool UseDefaultMargins()
    {
        return base.UseDefaultMargins();
    }

    public override GUIContent GetPreviewTitle()
    {
        return base.GetPreviewTitle();
    }

    public override string GetInfoString()
    {
        return base.GetInfoString();
    }
}
