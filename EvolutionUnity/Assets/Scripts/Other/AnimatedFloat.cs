using System;
using UnityEngine;
/// <summary>
/// Анимация значения переменной типа float
/// </summary>
[System.Serializable]
public class AnimatedFloat
{
    public Action<float> OnUpperCap;

    /// <summary>
    /// Включен?
    /// </summary>
    public bool enabled = false;

    /// <summary>
    /// Плавно менять значение
    /// </summary>
    public bool smooth = false;

    /// <summary>
    /// Скорость изменения
    /// </summary>
    public float changeSpeed = 4f;

    /// <summary>
    /// Минимальное значение
    /// </summary>
    public float minValue = -2.25f;

    /// <summary>
    /// Максимальное значение
    /// </summary>
    public float maxValue = 2.25f;

    /// <summary>
    /// Получить текущее значение аниматора. После обращения счетчик меняется со скоростью changeSpeed
    /// </summary>
    public float Value
    {
        get
        {
            if (!enabled)
                return 0;
            if (inc && _value >= maxValue)
            {
                if (OnUpperCap != null)
                    OnUpperCap(_value);
                inc = false;
            }
            if (!inc && _value <= minValue)
                inc = true;

            if (!smooth)
                return _value += inc ? changeSpeed * Time.deltaTime : changeSpeed * Time.deltaTime * -1f;
            else
                return _value = inc ? 
                    Mathf.SmoothDamp(_value, maxValue, ref smoothVelo, changeSpeed * Time.deltaTime) 
                    : 
                    Mathf.SmoothDamp(_value, minValue, ref smoothVelo, changeSpeed * Time.deltaTime);
        }
    }
    private float _value = 0;
    private bool inc = true;
    private float smoothVelo;
}