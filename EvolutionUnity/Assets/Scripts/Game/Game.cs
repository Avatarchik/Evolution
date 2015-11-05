using UnityEngine;
using System.Collections;
using Sfs2X.Entities;
using Sfs2X.Entities.Data;
using Server;
using System;
using Sfs2X.Util;
using Sfs2X.Logging;

[DisallowMultipleComponent]
public sealed class Game : UnitySingleton<Game> {

#if UNITY_ANDROID
    /// <summary>
    /// Ссылка на магазин приложения
    /// </summary>
    public string UpdateURL
    {
        get
        {
            return "http://localhost";
        }
    }
#else
    /// <summary>
    /// Ссылка на магазин приложения
    /// </summary>
    public string UpdateURL
    {
        get
        {
            return "http://localhost";
        }
    }
#endif

    void Start()
    {
        FacebookMy.Instance.OnUserInfoLoaded += OnFacebookUserInfoLoaded;
        Socket.Instance.OnInited += OnServerInited;
        Socket.Instance.OnConnectClosed += OnServerConnectClosedOrFailed;
        Socket.Instance.OnConnectFailed += OnServerConnectClosedOrFailed;
        Socket.Instance.OnLoginSucceed += OnServerLoginSucceed;
        Socket.Instance.OnLoginError += OnServerLoginError;

        Socket.Instance.Init();
    }

    /// <summary>
    /// Загрузить главное меню
    /// </summary>
    public void LoadMenu()
    {
        Application.LoadLevel(1);
    }

    /// <summary>
    /// Сервер был инициализирован
    /// </summary>
    void OnServerInited()
    {
        Dialogs.Instance.Add(DialogTypes.Connecting);
        Dialogs.Instance.ShowNext();
    }

    /// <summary>
    /// Успешно авторизовались на сервере
    /// </summary>
    /// <param name="user"></param>
    void OnServerLoginSucceed(User user)
    {
        FacebookMy.Instance.Init();
    }

    /// <summary>
    /// Потерялось соединение с сервером или не получилось соедениться
    /// </summary>
    void OnServerConnectClosedOrFailed() {
        if (Dialogs.Instance.Data.Find((Dialog d) => d.type == DialogTypes.ConnectionLost) == null)
        {
            Dialogs.Instance.Add(DialogTypes.ConnectionLost);
            Dialogs.Instance.ShowNext();
        }
    }

    /// <summary>
    /// Не получилось авторизоваться на сервере
    /// </summary>
    /// <param name="code"></param>
    void OnServerLoginError(int code, string errorMessage)
    {
        Dialog dialog = null;
        switch (code)
        {
            case 1000:
                dialog = Dialogs.Instance.Add(DialogTypes.ClientUpdate);
                break;
            default:
                dialog = Dialogs.Instance.Add(DialogTypes.Classic);
                dialog.SetHeaderText(Localization.Get("dialog_coonection_lost_header").ToUpper());
                dialog.SetBodyText(errorMessage);
                break;
        }
        Dialogs.Instance.ShowNext();
    }

    /// <summary>
    /// Пришла информация о моем фейсбук пользователе
    /// </summary>
    void OnFacebookUserInfoLoaded()
    {
        ISFSObject data = new SFSObject();
        data.PutClass("FacebookUserInfoMy", FacebookMy.Instance.LocalUserInfo);
        Socket.Instance.Request(Requests.Types.FacebookUserData, data);
    }

    /// <summary>
    /// Элегантный Update
    /// </summary>
    public override void GentleUpdate()
    {
        base.GentleUpdate();
    }
}
