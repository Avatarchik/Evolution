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
        Socket.Instance.Request(Requests.Types.Ping, new SFSObject());
    }

    /// <summary>
    /// Пришел ответ от сервера
    /// </summary>
    /// <param name="type"></param>
    /// <param name="data"></param>
    void OnServerResponse(Responses.Types type, ISFSObject data)
    {
        if (type == Responses.Types.Pong || type == Responses.Types.Ping)
        {
            if (type == Responses.Types.Pong)
            {
                ISFSObject request = new SFSObject();
                request.PutLong("time", data.GetLong("time"));
                Socket.Instance.Request(Requests.Types.Pong, request);
            }

            if (type == Responses.Types.Ping)
                Delay = data.GetLong("ping");
        }

        Log.Info(type.ToStr());
    }
}
