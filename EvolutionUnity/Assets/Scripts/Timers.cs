using UnityEngine;
using System;
using System.Collections.Generic;

public class Timers : UnitySingleton<Timers> {
    
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
    public static int UStampInSecs
    {
        get
        {
            return (int) UnixTimeSpan.TotalSeconds;
        }
    }

    /// <summary>
    /// Unix Time Stamp (время от 1970) в милисекундах
    /// </summary>
    public static long UStampInMSecs
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
            return DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1).ToUniversalTime());
        }
    }

    /// <summary>
    /// Мягкий Update
    /// </summary>
    public override void GentleUpdate()
    {
        base.GentleUpdate();
        foreach (SecondsTimer timer in _data)
            timer.ProccessEvents();
    }

    /// <summary>
    /// Создать новый таймер, отсчет от сейчас
    /// </summary>
    /// <returns></returns>
    public ITimer AddTimer<T>() where T : ITimer
    {
        if (typeof(SecondsTimer).Equals(typeof(T)))
            return AddSecondsTimer(UStampInSecs);

        return null;
    }

    /// <summary>
    /// Создать новый таймер
    /// </summary>
    /// <param name="startTime"></param>
    /// <returns></returns>
    public SecondsTimer AddSecondsTimer(int startTime)
    {
        SecondsTimer newTimer = new SecondsTimer(startTime);
        newTimer.OnStop += (ITimer timer) => { 
            _data.Remove(timer);
            timer = null;    
        };
        _data.Add(newTimer);
        return newTimer;
    }
}
