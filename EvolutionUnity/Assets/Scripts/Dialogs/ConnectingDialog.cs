using UnityEngine;
using System.Collections;
using Sfs2X.Entities;

public class ConnectingDialog : Dialog {
    public LoadAnimNoProgress loading;

    public override void Start()
    {
        base.Start();
        InitListeners();
    }

    /// <summary>
    /// Потерялось соединение с сервером или не получилось соедениться
    /// </summary>
    void OnServerConnectClosedOrFailed()
    {
        DoDestroy();
    }

    void InitListeners()
    {
        Socket.Instance.OnConnectClosed += OnServerConnectClosedOrFailed;
        Socket.Instance.OnConnectFailed += OnServerConnectClosedOrFailed;
    }

    public override void Update()
    {
        base.Update();
        if (!Socket.Instance.IsLoged)
            loading.Animate();
        else if (state != States.Hiding)
            DoDestroy();
    }

    void OnDestroy()
    {
        Socket.Instance.OnConnectClosed -= OnServerConnectClosedOrFailed;
        Socket.Instance.OnConnectFailed -= OnServerConnectClosedOrFailed;
    }
}
