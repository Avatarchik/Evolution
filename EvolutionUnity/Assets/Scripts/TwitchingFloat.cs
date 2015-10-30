using UnityEngine;

/// <summary>
/// Случайное значение Float с течением времени
/// </summary>
[System.Serializable]
public class TwitchingFloat
{
    /// <summary>
    /// Включено ли
    /// </summary>
    public bool enabled = false;

    /// <summary>
    /// Сила дрожания
    /// </summary>
    public float impact = 1f;

    /// <summary>
    /// Скорость изменения
    /// </summary>
    public float speed = 1f;

    public float Value
    {
        get
        {
            if (!enabled)
                return 0;

            return impact * Time.deltaTime * (int)Random.Range((int)-1, (int)2);
        }
    }
}