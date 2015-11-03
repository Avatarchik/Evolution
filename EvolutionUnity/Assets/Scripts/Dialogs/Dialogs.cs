using UnityEngine.UI;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;

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
            return _data;
        }
    }
    /// <summary>
    /// Спсиок диалогов
    /// </summary>
    private List<IDialog> _data = new List<IDialog>();

    /// <summary>
    /// Название GameObject с пресетами
    /// </summary>
    public Transform presetsParent;

    /// <summary>
    /// Список пресетов
    /// </summary>
    public List<Dialog> presets = new List<Dialog>();

    /// <summary>
    /// Пробуждение
    /// </summary>
    public override void Awake()
    {
        base.Awake();
        Canvas = GetComponent<Canvas>();
    }

    /// <summary>
    /// Добавить диалог в список диалогов
    /// </summary>
    /// <param name="type"></param>
    public Dialog Add(DialogTypes type)
    {
        Dialog dialog = Instantiate<Dialog>(presets.First(d => d.type == type));
        dialog.transform.SetParent(transform);
        dialog.gameObject.SetActive(true);
        dialog.OnDestroyed += OnDialogDestroyed;
        dialog.OnDestroy += OnDialogDestroy;
        _data.Add(dialog);
        return dialog;
    }

    void OnDialogDestroyed(Dialog dialog)
    {
        _data.Remove(dialog);
    }

    void OnDialogDestroy(Dialog dialog)
    {
        int index = _data.IndexOf(dialog);
        if (index >= 1)
            _data[index - 1].DoShow();
    } 

    public void ShowNext()
    {
        int Count = _data.Count;
        if (Count == 0)
            return;

        if (Count == 1)
        {
            _data[Count - 1].DoShow();
        }
        else if (Count >= 2)
        {
            _data[Count - 2].DoHide();
            _data[Count - 1].DoShow();
            if (Count > 2)
                for (int i = Count - 3; i >= 0; i--)
                    _data[i].Hide();
        }
    }
}