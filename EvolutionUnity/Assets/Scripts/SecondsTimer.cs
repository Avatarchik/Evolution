using System;
using UnityEngine;

/// <summary>
/// Таймер
/// </summary>
public class SecondsTimer : ITimer
{
    private int oldPassedValue = -1;
    private int _startTime;
    public Action<int> OnTick;
    public Action<ITimer> OnStop;

    /// <summary>
    /// Начало отсчета
    /// </summary>
    /// <param name="startTime"></param>
    public SecondsTimer(int startTime)
    {
        _startTime = startTime;
    }

    /// <summary>
    /// Сколько прошли секунд
    /// </summary>
    public int Passed
    {
        get
        {
            return Mathf.Abs(_startTime - Timers.UStampInSecs);
        }
    }

    /// <summary>
    /// Делаем логику таймера
    /// </summary>
    public void ProccessEvents()
    {
        //Если прошла секунда, то сообщаем
        if (Passed != oldPassedValue)
        {
            if (OnTick != null)
                OnTick(Passed);
            oldPassedValue = Passed;
        }
    }

    /// <summary>
    /// Уничтожить таймер
    /// </summary>
    public void Stop()
    {
        if (OnStop != null)
            OnStop(this);
    }
}