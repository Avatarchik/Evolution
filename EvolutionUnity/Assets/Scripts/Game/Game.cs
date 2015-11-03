using UnityEngine;
using System.Collections;
using Sfs2X.Entities;
using Sfs2X.Entities.Data;
using Server;
using System;

[DisallowMultipleComponent]
public sealed class Game : UnitySingleton<Game> {
    
    void Start()
    {
        FacebookMy.Instance.OnUserInfoLoaded += OnFacebookUserInfoLoaded;
        Socket.Instance.OnLoginSucceed += OnServerLoginSucceed;

        Socket.Instance.Init();
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
    /// Пришла информация о моем фейсбук пользователе
    /// </summary>
    void OnFacebookUserInfoLoaded()
    {
        ISFSObject data = new SFSObject();
        data.PutClass("FacebookUserInfoMy", FacebookMy.Instance.LocalUserInfo);
        Socket.Instance.Request(Requests.Types.FacebookUserData, data);
    }
}
