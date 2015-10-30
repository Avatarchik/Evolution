using UnityEngine;
using System;

/// <summary>
/// Класс реализует экномный Update
/// </summary>
public abstract class GentleMonoBeh : MonoBehaviour, IGentleMonoBeh {

    /// <summary>
    /// Как часто обновляется GentleUpdate()
    /// </summary>
    public int Rate { get; private set; }

    /// <summary>
    /// Сколько кадров прошло
    /// </summary>
    public int FrameCounter { get; private set; }

    private int _resetFramesCount = 2147483647;

    /// <summary>
    /// Конструктор GentleMonoBeh
    /// </summary>
    public GentleMonoBeh() {
        SetCPURateOf(6);
    }

    /// <summary>
    /// Каждый кадр
    /// </summary>
    public virtual void Update()
    {
        NormalUpdate();

        if (FrameCounter % Rate == 0)
            GentleUpdate();

        if (FrameCounter == _resetFramesCount)
            FrameCounter = 0;

        FrameCounter++;
    }

    /// <summary>
    /// Обычный Update
    /// </summary>
    public abstract void NormalUpdate();

    /// <summary>
    /// Эконмный Update
    /// </summary>
    public abstract void GentleUpdate();

    /// <summary>
    /// Установить значение экономии ресурсов на N. Чем больше тем экономней.
    /// </summary>
    /// <param name="value"></param>
    public void SetCPURateOf(int value)
    {
        if (value <= 1)
            throw new ArgumentException("Значение не может быть <= 1", "Rate");
        Rate = value;
    }
}
