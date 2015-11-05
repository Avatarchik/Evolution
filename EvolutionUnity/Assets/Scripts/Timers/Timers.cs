using UnityEngine;
using System;
using System.Collections.Generic;

[DisallowMultipleComponent]
public sealed class Timers : UnitySingleton<Timers> {
    
    /// <summary>
    /// Список таймеров
    /// </summary>
    public List<ITimer> Data
    {
        get
        {
            return _data;
        }
    }
    private List<ITimer> _data = new List<ITimer>();

    /// <summary>
    /// Unix Time Stamp (время от 1970) в секундах
    /// </summary>
    public static int UnixStampSeconds
    {
        get
        {
            return (int) UnixTimeSpan.TotalSeconds;
        }
    }

    /// <summary>
    /// Unix Time Stamp (время от 1970) в милисекундах
    /// </summary>
    public static long UnixStampMillisecond
    {
        get
        {
            return (long) UnixTimeSpan.TotalMilliseconds;
        }
    }

    /// <summary>
    /// Смещение для Unix времени
    /// </summary>
    private static TimeSpan UnixTimeSpan
    {
        get
        {
            return DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc));
        }
    }

    /// <summary>
    /// Мягкий Update
    /// </summary>
    public override void GentleUpdate()
    {
        base.GentleUpdate();
        foreach (ITimer timer in _data)
            timer.ProccessEvents();
    }

    /// <summary>
    /// Создать секундомер с временем старта от "UnixStampMillisecond"
    /// </summary>
    /// <returns></returns>
    public Stopwatch AddStopwatch()
    {
        return AddStopwatch(UnixStampMillisecond);
    }

    /// <summary>
    /// Создать секундомер
    /// </summary>
    /// <param name="startTime">В милисекундах Unix UTC</param>
    /// <returns></returns>
    public Stopwatch AddStopwatch(long startTime)
    {
        return Add<Stopwatch>(new Stopwatch(startTime));
    }

    /// <summary>
    /// Добавить таймер в список
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="timer"></param>
    /// <returns></returns>
    public T Add<T>(T timer) where T : Timer
    {
        timer.OnStop += OnTimerStop;
        _data.Add(timer);

        return timer;
    }

    /// <summary>
    /// Остановился таймер
    /// </summary>
    /// <param name="timer"></param>
    private void OnTimerStop(ITimer timer)
    {
        _data.Remove(timer);
    }
}
