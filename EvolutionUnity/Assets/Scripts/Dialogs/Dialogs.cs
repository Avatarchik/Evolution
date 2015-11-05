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
    public List<Dialog> Data
    {
        get
        {
            return _data;
        }
    }
    /// <summary>
    /// Спсиок диалогов
    /// </summary>
    private List<Dialog> _data = new List<Dialog>();

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
        dialog.transform.localScale = Vector3.one;
        dialog.gameObject.SetActive(true);
        dialog.OnDestroyed += OnDialogDestroyed;
        dialog.OnDestroyStarted += OnDestroyStarted;
        _data.Add(dialog);
        return dialog;
    }

    /// <summary>
    /// Диалог был унчитожен
    /// </summary>
    /// <param name="dialog"></param>
    void OnDialogDestroyed(Dialog dialog)
    {
        _data.Remove(dialog);
    }

    /// <summary>
    /// Диалог начал уничтожение
    /// </summary>
    /// <param name="dialog"></param>
    void OnDestroyStarted(Dialog dialog)
    {
        int index = _data.IndexOf(dialog);
        if (index >= 1)
            _data[index - 1].DoShow();
    }

    /// <summary>
    /// Показать диалог
    /// </summary>
    /// <param name="dialog">Диалог</param>
    public void Show(Dialog dialog)
    {
        if (_data.Count >= 2)
            foreach (Dialog d in _data)
                if (d.state == Dialog.States.Showed)
                {
                    d.DoHide();
                    break;
                }
        dialog.DoShow();
        MakeLast(dialog);
    }

    /// <summary>
    /// Показать диалог
    /// </summary>
    /// <param name="dialog">Диалог</param>
    /// <param name="delay">Задержка в секундах</param>
    public void Show(Dialog dialog, float delay)
    {
        StartCoroutine(ShowNumerator(dialog, delay));
    }

    /// <summary>
    /// Показать диалог с задержкой
    /// </summary>
    /// <param name="dialog"></param>
    /// <param name="delay"></param>
    /// <returns></returns>
    System.Collections.IEnumerator ShowNumerator(Dialog dialog, float delay)
    {
        yield return new WaitForSeconds(delay);
        Show(dialog);
    }

    /// <summary>
    /// Показать следующий диалог
    /// </summary>
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

    /// <summary>
    /// Показать следующий диалог
    /// </summary>
    /// <param name="yield">задержка в секундах</param>
    public void ShowNext(float delay)
    {
        StartCoroutine(ShowNextNumerator(delay));
    }

    /// <summary>
    /// Показать следющий диалог с задержкой
    /// </summary>
    /// <param name="delay"></param>
    /// <returns></returns>
    System.Collections.IEnumerator ShowNextNumerator(float delay)
    {
        yield return new WaitForSeconds(delay);
        ShowNext();
    }

    /// <summary>
    /// Делает диалог последним в списке диалогов
    /// </summary>
    void MakeLast(Dialog dialog)
    {
        List<Dialog> dialogs = new List<Dialog>();
        _data.Remove(dialog);
        foreach (Dialog d in _data)
        {
            dialogs.Add(d);
        }
        dialogs.Add(dialog);
        _data = dialogs;
        dialog.transform.SetAsLastSibling();
    }
}