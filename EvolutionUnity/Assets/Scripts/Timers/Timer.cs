using System;

/// <summary>
/// Таймер
/// </summary>
public abstract class Timer : ITimer {
    /// <summary>
    /// Тикает
    /// </summary>
    public Action<long> OnTick;

    /// <summary>
    /// Остановлен
    /// </summary>
    public Action<ITimer> OnStop;

    /// <summary>
    /// Делает логику
    /// </summary>
    public abstract void ProccessEvents();

    /// <summary>
    /// Остановливаем
    /// </summary>
    public abstract void Stop();
}
