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
                _value = maxValue;
                if (OnUpperCap != null)
                    OnUpperCap(_value);
                inc = false;
                return _value;
            }
            if (!inc && _value <= minValue)
            {
                _value = minValue;
                inc = true;
                return _value;
            }

            if (!smooth)
                return _value += inc ? changeSpeed * Time.deltaTime : changeSpeed * Time.deltaTime * -1f;
            else
                return _value = inc ? 
                    Mathf.SmoothDamp(_value, maxValue + 0.01f, ref smoothVelo, changeSpeed * Time.deltaTime) 
                    : 
                    Mathf.SmoothDamp(_value, minValue - 0.01f, ref smoothVelo, changeSpeed * Time.deltaTime);
        }
    }
    private float _value = 0;
    private bool inc = true;
    private float smoothVelo;

    /// <summary>
    /// Новый инстанс с клонироваными свойствами
    /// </summary>
    /// <returns></returns>
    public AnimatedFloat CloneProperties()
    {
        AnimatedFloat animatedFloat = new AnimatedFloat();
        animatedFloat.enabled = enabled;
        animatedFloat.smooth = smooth;
        animatedFloat.changeSpeed = changeSpeed;
        animatedFloat.minValue = minValue;
        animatedFloat.maxValue = maxValue;

        return animatedFloat;
    }
}