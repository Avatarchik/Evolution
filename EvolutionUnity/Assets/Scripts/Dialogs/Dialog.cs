using UnityEngine;
using UnityEngine.UI;
using System;
/// <summary>
/// Базовый класс диалога
/// </summary>
[RequireComponent(typeof(RectTransform))]
[DisallowMultipleComponent]
public class Dialog : MonoBehaviour, IDialog {

    public Action<Dialog> OnShowed;
    public Action<Dialog> OnHided;
    public Action<Dialog> OnDestroyed;
    public Action<Dialog> OnDestroy;

    public enum States
    {
        Hided,
        Showing,
        Showed,
        Hiding
    }

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
    /// Анимационный float
    /// </summary>
    public AnimatedFloat fadeFloatPreset;

    private AnimatedFloat fadeFloat;

    private Vector2 StartBodyPosition
    {
        get
        {
            return new Vector2(Camera.main.pixelWidth * -1f, Body.anchoredPosition.y);
        }
    }

    private Vector2 EndBodyPosition
    {
        get
        {
            return new Vector2(Camera.main.pixelWidth, Body.anchoredPosition.y);
        }
    }

    public virtual void Start()
    {
        RectTransform = GetComponent<RectTransform>();
        name = name.Replace("(Clone)", "");
        RectTransform.anchoredPosition = Vector2.zero;
        RectTransform.sizeDelta = Vector2.zero;
    }

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

    void MoveBodyTo(Vector2 pos)
    {
        Body.anchoredPosition = new Vector2(
            pos.x,
            pos.y
            );
    }

    public virtual void DoShow()
    {
        fadeFloat = fadeFloatPreset.CloneProperties();
        fadeFloat.OnUpperCap = (float value) => Show();
        MoveBodyTo(StartBodyPosition);
        Background.offset = 0;
        state = States.Showing;
    }

    public virtual void Show()
    {
        state = States.Showed;
        MoveBodyTo(new Vector2(0, StartBodyPosition.y));
        Background.offset = 1f;

        if (OnShowed != null)
            OnShowed(this);
    }

    public virtual void DoHide()
    {
        fadeFloat = fadeFloatPreset.CloneProperties();
        fadeFloat.OnUpperCap = (float value) => Hide();
        MoveBodyTo(new Vector2(0, StartBodyPosition.y));
        Background.offset = 1;
        state = States.Hiding;
    }

    public virtual void Hide()
    {
        state = States.Hided;
        MoveBodyTo(new Vector2(EndBodyPosition.x, EndBodyPosition.y));
        Background.offset = 0;

        if (OnHided != null)
            OnHided(this);
    }

    public virtual void Destroy()
    {
        OnHided += (Dialog d) =>
        {
            if (d.OnDestroyed != null)
                d.OnDestroyed(this);

            Destroy(d.gameObject);
        };
        DoHide();
        if (OnDestroy != null)
            OnDestroy(this);
    }
}
