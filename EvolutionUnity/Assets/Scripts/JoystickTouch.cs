using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

/// <summary>
/// Джойстик для пальцев
/// </summary>
public class JoystickTouch : Joystick {

    /// <summary>
    /// Максимальная дистанция от начала координат при которой Axis будет еденица
    /// </summary>
    public float distanceMax = 27.5f;

    /// <summary>
    /// Обрезать значения Axis от -1 до 1?
    /// </summary>
    public bool clamp;

    /// <summary>
    /// Фоновое изображение, служит как начало координат
    /// </summary>
    public Image backgroundImage;

    /// <summary>
    /// Изображение указателя
    /// </summary>
    public Image pointerImage;

    /// <summary>
    /// Начальная позиция указателя
    /// </summary>
    private Vector2 startDragPosition;

    /// <summary>
    /// текущая позиция указателя
    /// </summary>
    private Vector2 pointerPosition;

    /// <summary>
    /// Палец
    /// </summary>
    private PointerEventData pointerData;

    /// <summary>
    /// Старт
    /// </summary>
    void Start()
    {
        ToggleGraphics(false);
    }

    /// <summary>
    /// Каждый кадр
    /// </summary>
    public override void Update()
    {
        if (pointerData == null)
            return;
        backgroundImage.transform.position = startDragPosition;
        pointerImage.transform.position = pointerPosition;

        if(clamp)
            UpdateVariables(
                Mathf.Clamp((backgroundImage.transform.localPosition.x - pointerImage.transform.localPosition.x) / distanceMax, -1f, 1f) * -1f,
                Mathf.Clamp((backgroundImage.transform.localPosition.y - pointerImage.transform.localPosition.y) / distanceMax, -1f, 1f) * -1f
            );
        else
            UpdateVariables(
                (backgroundImage.transform.localPosition.x - pointerImage.transform.localPosition.x) / distanceMax * -1f,
                (backgroundImage.transform.localPosition.y - pointerImage.transform.localPosition.y) / distanceMax * -1f
            );
    }

    /// <summary>
    /// Палец на джойстик положили
    /// </summary>
    /// <param name="data"></param>
    public void OnPointerDown(BaseEventData data)
    {
        PointerEventData pData = (PointerEventData)data;
        if (pointerData == null)
            pointerData = pData;
        if (pointerData != pData)
            return;

        ToggleGraphics(true);
        startDragPosition = pointerData.position;
        pointerPosition = startDragPosition;
    }

    /// <summary>
    /// Палецем елозим по джойстику
    /// </summary>
    /// <param name="data"></param>
    public void OnPointerDrag(BaseEventData data)
    {
        PointerEventData pData = (PointerEventData)data;
        if (pointerData == null)
            pointerData = pData;
        if (pointerData != pData)
            return;

        pointerPosition = pointerData.position;
    }

    /// <summary>
    /// Палец с джойстика убрали
    /// </summary>
    /// <param name="data"></param>
    public void OnPointerUp(BaseEventData data)
    {
        PointerEventData pData = (PointerEventData)data;
        if (pointerData != pData)
            return;
        pointerData = null;
        ToggleGraphics(false);
    }

    /// <summary>
    /// Включить/выключить графические элементы управления
    /// </summary>
    void ToggleGraphics(bool toggle)
    {
        backgroundImage.gameObject.SetActive(toggle);
        pointerImage.gameObject.SetActive(toggle);
    }
}
