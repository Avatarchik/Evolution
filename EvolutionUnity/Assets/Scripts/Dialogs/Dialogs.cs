using UnityEngine.UI;
using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Диалоги (поп-ап)
/// </summary>
[DisallowMultipleComponent]
[RequireComponent(typeof(RectTransform))]
[RequireComponent(typeof(Canvas))]
[RequireComponent(typeof(CanvasScaler))]
[RequireComponent(typeof(GraphicRaycaster))]
public class Dialogs : UnitySingleton<Dialogs> {
    /// <summary>
    /// Ссылка на канвас
    /// </summary>
    public Canvas Canvas {get; private set;}

    /// <summary>
    /// Список диалогов
    /// </summary>
    public List<IDialog> Data
    {
        get
        {
            return _dialogs;
        }
    }
    private List<IDialog> _dialogs = new List<IDialog>();

    public GraphicsColorTransformer backgroundColorTransformer;

    /// <summary>
    /// Пробуждение
    /// </summary>
    public override void Awake()
    {
        base.Awake();
        Canvas = GetComponent<Canvas>();
    }

    public void Show(DialogTypes type)
    {
        Dialog dialog = Instantiate<Dialog>(Resources.Load<Dialog>("Dialogs/" + type.ToStr()));
        dialog.transform.SetParent(transform);
        dialog.transform.localPosition = Vector3.zero;
    }
}