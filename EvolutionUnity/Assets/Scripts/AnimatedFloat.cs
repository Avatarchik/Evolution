
using UnityEngine;
/// <summary>
/// Анимация значения переменной типа float
/// </summary>
[System.Serializable]
public class AnimatedFloat
{
    /// <summary>
    /// Включен?
    /// </summary>
    public bool enabled = false;

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
    public float Current
    {
        get
        {
            if (inc && _current >= maxValue)
                inc = false;
            if (!inc && _current <= minValue)
                inc = true;

            return _current += inc ? changeSpeed * Time.deltaTime : changeSpeed * Time.deltaTime * -1f;
        }
    }
    private float _current = 0;
    private bool inc = true;
}