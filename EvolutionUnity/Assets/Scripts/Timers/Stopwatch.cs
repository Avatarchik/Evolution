using System;
using UnityEngine;

/// <summary>
/// Таймер
/// </summary>
public class Stopwatch : Timer
{
    /// <summary>
    /// Тикнула секунда
    /// </summary>
    public Action<long> OnSecondTick;

    /// <summary>
    /// Начало отсчета
    /// </summary>
    private long _startTime;

    /// <summary>
    /// Предыдущее значение в секундах
    /// </summary>
    private int oldSeconds = -1;

    /// <summary>
    /// Начало отсчета
    /// </summary>
    /// <param name="startTime">В милисекундах Unix UTC</param>
    public Stopwatch(long startTime)
    {
        _startTime = startTime;
    }

    public bool IsStoped { get; private set; }

    /// <summary>
    /// Сколько прошли милисекунд
    /// </summary>
    public long Passed
    {
        get
        {
            return Timers.UnixStampMillisecond - _startTime;
        }
    }

    /// <summary>
    /// Делаем логику таймера
    /// </summary>
    public override void ProccessEvents()
    {
        if (IsStoped)
            return;

        if (OnTick != null)
            OnTick(Passed);

        int seconds = Mathf.FloorToInt((float) Passed / 1000);
        if (seconds != oldSeconds)
        {
            if (OnSecondTick != null)
                OnSecondTick(Passed);

            oldSeconds = seconds;
        }
    }

    /// <summary>
    /// Остановить таймер
    /// </summary>
    public override void Stop()
    {
        IsStoped = true;
        if (OnStop != null)
            OnStop(this);
    }
}