using UnityEngine;
using System;

public class Timer : UnitySingleton<Timer> {

    /// <summary>
    /// Unix Time Stamp (время от 1970) в секундах
    /// </summary>
    public int UStampInSecs
    {
        get
        {
            return (int) UnixTimeSpan.TotalSeconds;
        }
    }

    /// <summary>
    /// Unix Time Stamp (время от 1970) в милисекундах
    /// </summary>
    public long UStampInMSecs
    {
        get
        {
            return (long) UnixTimeSpan.TotalMilliseconds;
        }
    }

    /// <summary>
    /// Смещение для Unix времени
    /// </summary>
    private TimeSpan UnixTimeSpan
    {
        get
        {
            return DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1).ToUniversalTime());
        }
    }
}
