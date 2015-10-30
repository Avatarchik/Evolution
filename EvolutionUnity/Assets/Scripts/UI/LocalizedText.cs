using UnityEngine;
using UnityEngine.UI;
using System;

/// <summary>
/// Переводит содержимое Text компоненты
/// </summary>
[RequireComponent(typeof(Text))]
public class LocalizedText : MonoBehaviour
{
    /// <summary>
    /// Ключ
    /// </summary>
    public string key;

    /// <summary>
    /// Переводить текст в верхний регистр?
    /// </summary>
    public bool upperCase;

    /// <summary>
    /// Сссылка на меш
    /// </summary>
    private Text textMesh;

    /// <summary>
    /// Пробуждение
    /// </summary>
    void Awake()
    {
        textMesh = GetComponent<Text>();
    }

    /// <summary>
    /// Старт
    /// </summary>
    void Start()
    {
        if (key.Equals("") || key.Contains(" "))
            throw new ArgumentException("Ключ не должен содержать пробелы или пустоты", "key");

        Localization.Instance.OnRefresh += OnRefresh;
        OnRefresh();
    }

    /// <summary>
    /// Обновился язык
    /// </summary>
    void OnRefresh()
    {
        string text = Localization.Instance.Get(key);
        if (upperCase)
            text = text.ToUpper();
        textMesh.text = text;
    }

    /// <summary>
    /// Уничтожились
    /// </summary>
    void OnDestroy()
    {
        if(!Game.Quiting)
            Localization.Instance.OnRefresh -= OnRefresh;
    }
}
