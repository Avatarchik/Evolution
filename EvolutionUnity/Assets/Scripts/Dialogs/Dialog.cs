using UnityEngine;
using UnityEngine.UI;
using System;
/// <summary>
/// Базовый класс диалога
/// </summary>
[RequireComponent(typeof(RectTransform))]
[DisallowMultipleComponent]
public class Dialog : MonoBehaviour, IDialog 
{
    /// <summary>
    /// Показался
    /// </summary>
    public Action<Dialog> OnShowed;
    /// <summary>
    /// Спрятался
    /// </summary>
    public Action<Dialog> OnHided;
    /// <summary>
    /// Был уничтожен
    /// </summary>
    public Action<Dialog> OnDestroyed;
    /// <summary>
    /// Начало уничтожения
    /// </summary>
    public Action<Dialog> OnDestroyStarted;
    /// <summary>
    /// Состояния
    /// </summary>
    public enum States
    {
        Hided,
        Showing,
        Showed,
        Hiding
    }
    /// <summary>
    /// Ссылка на состояния
    /// </summary>
    public States state = States.Showed;

    /// <summary>
    /// Тип диалога
    /// </summary>
    public DialogTypes type;

    /// <summary>
    /// Фон диалога
    /// </summary>
    public GraphicsColorTransformer Background;

    /// <summary>
    /// Тело диалога
    /// </summary>
    public RectTransform Body;

    /// <summary>
    /// Траснформ себя
    /// </summary>
    [HideInInspector]
    public RectTransform RectTransform;

    /// <summary>
    /// Анимационный float как пресет
    /// </summary>
    public AnimatedFloat fadeFloatPreset = new AnimatedFloat() { enabled = true, smooth = true, changeSpeed = 10f, minValue = 0, maxValue = 1f };

    /// <summary>
    /// Анимационный float 
    /// </summary>
    private AnimatedFloat fadeFloat;

    /// <summary>
    /// Начальная позиция диалога
    /// </summary>
    public Vector2 StartBodyPosition
    {
        get
        {
            return new Vector2(Screen.width * -1f, Body.anchoredPosition.y);
        }
    }

    /// <summary>
    /// Конечная позиция диалога
    /// </summary>
    public Vector2 EndBodyPosition
    {
        get
        {
            return new Vector2(Screen.width, Body.anchoredPosition.y);
        }
    }

    /// <summary>
    /// TextMesh заголовка
    /// </summary>
    public Text headerLabel;

    /// <summary>
    /// TextMesh тела
    /// </summary>
    public Text bodyLabel;

    /// <summary>
    /// Старт
    /// </summary>
    public virtual void Start()
    {
        RectTransform = GetComponent<RectTransform>();
        name = name.Replace("(Clone)", "");
        RectTransform.anchoredPosition = Vector2.zero;
        RectTransform.sizeDelta = Vector2.zero;
    }

    /// <summary>
    /// Каждый кадр
    /// </summary>
    public virtual void Update()
    {
        float offset = 0;
        if (state == States.Showing || state == States.Hiding)
            offset = fadeFloat.Value;

        if (state == States.Showed || state == States.Hided)
            return;

        float backgroundOffset = offset;
        Vector2 targetPosition = new Vector2(
                    StartBodyPosition.x * (1f - offset),
                    StartBodyPosition.y);

        if (state == States.Hiding)
        {
            backgroundOffset = 1f - offset;
            targetPosition = new Vector2(EndBodyPosition.x * offset, EndBodyPosition.y);
        }

        Background.offset = backgroundOffset;
        MoveBodyTo(targetPosition);
    }

    /// <summary>
    /// Двигать тело диалога к позиции
    /// </summary>
    /// <param name="pos"></param>
    void MoveBodyTo(Vector2 pos)
    {
        Body.anchoredPosition = new Vector2(
            pos.x,
            pos.y
            );
    }

    /// <summary>
    /// Начать анимацию показа
    /// </summary>
    public virtual void DoShow()
    {
        fadeFloat = fadeFloatPreset.CloneProperties();
        fadeFloat.OnUpperCap = (float value) => Show();
        MoveBodyTo(StartBodyPosition);
        Background.offset = 0;
        state = States.Showing;
    }

    /// <summary>
    /// Показать
    /// </summary>
    public virtual void Show()
    {
        state = States.Showed;
        MoveBodyTo(new Vector2(0, StartBodyPosition.y));
        Background.offset = 1f;

        if (OnShowed != null)
            OnShowed(this);
    }

    /// <summary>
    /// Начать анимацию сокрытия
    /// </summary>
    public virtual void DoHide()
    {
        fadeFloat = fadeFloatPreset.CloneProperties();
        fadeFloat.OnUpperCap = (float value) => Hide();
        MoveBodyTo(new Vector2(0, StartBodyPosition.y));
        Background.offset = 1;
        state = States.Hiding;
    }

    /// <summary>
    /// Скрыть
    /// </summary>
    public virtual void Hide()
    {
        state = States.Hided;
        MoveBodyTo(new Vector2(EndBodyPosition.x, EndBodyPosition.y));
        Background.offset = 0;

        if (OnHided != null)
            OnHided(this);
    }

    /// <summary>
    /// Начать уничтожение
    /// </summary>
    public virtual void DoDestroy()
    {
        OnHided += (Dialog d) =>
        {
            Destroy();
        };
        DoHide();

        if (OnDestroyStarted != null)
            OnDestroyStarted(this);
    }

    /// <summary>
    /// Уничтожить
    /// </summary>
    public void Destroy()
    {
        if (OnDestroyed != null)
            OnDestroyed(this);

        Destroy(gameObject);
    }

    /// <summary>
    /// Установить текст для тела
    /// </summary>
    /// <param name="text"></param>
    public void SetBodyText(string text)
    {
        LocalizedText locText = bodyLabel.GetComponent<LocalizedText>();
        if (locText != null)
            Destroy(locText);
        bodyLabel.text = text;
    }

    /// <summary>
    /// Установить текст для заголовка
    /// </summary>
    /// <param name="text"></param>
    public void SetHeaderText(string text)
    {
        LocalizedText locText = headerLabel.GetComponent<LocalizedText>();
        if (locText != null)
            Destroy(locText);
        headerLabel.text = text;
    }

    /// <summary>
    /// Диалог показан или показывается
    /// </summary>
    public bool IsShowedOrShowing
    {
        get
        {
            if (state == States.Showed)
                return true;
            if (state == States.Showing)
                return true;
            return false;
        }
    }
}
