using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public abstract class ButtonListener : GentleMonoBeh, IButtonListener, IPointerClickHandler
{
    /// <summary>
    /// Ссылка на кнопку
    /// </summary>
    public Button Button {get; private set;}

    /// <summary>
    /// Пробуждение
    /// </summary>
    public virtual void Awake()
    {
        Button = GetComponent<Button>();
    }

    /// <summary>
    /// Кликнули на кнопку
    /// </summary>
    public abstract void OnClick();

    /// <summary>
    /// Кликнули на кнопку
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerClick(PointerEventData eventData)
    {
        if (!Button.interactable)
            return;

        OnClick();
    }

    public override void NormalUpdate()
    {
        
    }

    public override void GentleUpdate()
    {
        
    }
}
