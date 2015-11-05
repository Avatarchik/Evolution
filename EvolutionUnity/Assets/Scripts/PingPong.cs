using UnityEngine;
using Sfs2X.Entities.Data;
using Server;

/// <summary>
/// Считает пинг каждую секунду
/// </summary>
public class PingPong : UnitySingleton<PingPong> {
    /// <summary>
    /// Секундомер
    /// </summary>
    private Stopwatch stopwatch;

    /// <summary>
    /// Задержка
    /// </summary>
    public long Delay { get; private set; }

    /// <summary>
    /// Старт
    /// </summary>
    void Start()
    {
        stopwatch = Timers.Instance.AddStopwatch();
        stopwatch.OnSecondTick += OnStopwatchTick;
        Responses.Instance.OnResponse += OnServerResponse;
    }

    /// <summary>
    /// Тик секундомера
    /// </summary>
    /// <param name="time"></param>
    void OnStopwatchTick(long time)
    {
        if (!Socket.Instance.IsLoged)
            return;
        ISFSObject data = new SFSObject();
        data.PutLong("time", Timers.UnixStampMillisecond);
        Socket.Instance.Request(Requests.Types.Ping, data);
    }

    /// <summary>
    /// Пришел ответ от сервера
    /// </summary>
    /// <param name="type"></param>
    /// <param name="data"></param>
    void OnServerResponse(Responses.Types type, ISFSObject data)
    {
        if (type != Responses.Types.Pong)
            return;
        long serverTime = data.GetLong("time");
        CalculateDelay(serverTime);
    }

    /// <summary>
    /// Расчитываем задержку
    /// </summary>
    /// <param name="serverTime"></param>
    void CalculateDelay(long serverTime)
    {
        Delay = System.Math.Abs(Timers.UnixStampMillisecond - serverTime);
    }
}
