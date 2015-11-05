﻿using UnityEngine;
using System;
using Sfs2X;
using Sfs2X.Logging;
using Sfs2X.Util;
using Sfs2X.Core;
using Sfs2X.Entities;
using Sfs2X.Entities.Data;
using Sfs2X.Requests;
using MyUtils;
using Server;

[DisallowMultipleComponent]
public sealed class Socket : UnitySingleton<Socket>
{
    /// <summary>
    /// Событие инициализации
    /// </summary>
    public Action OnInited;

    /// <summary>
    /// Успешно соеденились
    /// </summary>
    public Action OnConnectSucceed;

    /// <summary>
    /// Не получилось соедениться
    /// </summary>
    public Action OnConnectFailed;

    /// <summary>
    /// Соединение закрылось
    /// </summary>
    public Action OnConnectClosed;

    /// <summary>
    /// Залогинились успешно
    /// </summary>
    public Action<User> OnLoginSucceed;

    /// <summary>
    /// Залогиниться не удалось
    /// </summary>
    public Action<int, string> OnLoginError;

    /// <summary>
    /// Успешно вошли в комнату
    /// </summary>
    public Action<Room> OnRoomJoinSucceed;

    /// <summary>
    /// Не получилось войти в комнату
    /// </summary>
    public Action OnRoomJoinError;

    /// <summary>
    /// Пришел ответ от расширения сервера
    /// </summary>
    public Action<string, ISFSObject> OnExtensionResponse;

    /// <summary>
    /// Логирование
    /// </summary>
    public Action<LogLevel, object> OnServerLog;

    /// <summary>
    /// Класс настроек
    /// </summary>
    [System.Serializable]
    public class ConnectionSettings
    {
        public string Host = "*.*.*.*";
        [Range(0, 65535)]
        public int PortTCP = 9933;
        public string ZoneName = "Evon";
        public string RoomName = "Default";
        public bool Debug = false;
        public float maxConnectingTime = 5f;
    }

    /// <summary>
    /// Настройки
    /// </summary>
    public ConnectionSettings Settings;
    
    /// <summary>
    /// Ссылка на смартфокс
    /// </summary>
    public static SmartFox Server
    {
        get
        {
            return _server;
        }
    }
    /// <summary>
    /// Ссылка на Смартфокс
    /// </summary>
    private static SmartFox _server;

    /// <summary>
    /// Подключен ли к серверу?
    /// </summary>
    public bool IsConnected
    {
        get
        {
            if (_server == null)
                return false;
            return _server.IsConnected;
        }
    }

    /// <summary>
    /// Идет процесс подключения к серверу?
    /// </summary>
    public bool IsConnecting
    {
        get
        {
            if (_server == null)
                return false;
            return _server.IsConnecting;
        }
    }

    /// <summary>
    /// Залогинены?
    /// </summary>
    public bool IsLoged
    {
        get
        {
            if (_server == null)
                return false;
            return _isLoged;
        }
    }
    /// <summary>
    /// Залогинены?
    /// </summary>
    private bool _isLoged = false;

    /// <summary>
    /// Идет процесс логина?
    /// </summary>
    public bool IsLoging
    {
        get
        {
            if (_server == null)
                return false;
            return _isLoging;
        }
    }
    /// <summary>
    /// Идет процесс логина?
    /// </summary>
    private bool _isLoging = false;

    /// <summary>
    /// Ссылка на объект который работает с запросами к серверу
    /// </summary>
    private Requests Requests = new Requests();

    /// <summary>
    /// Секундомер который считает время затраченное на соединение
    /// </summary>
    private Stopwatch connectingStopwatch;

    /// <summary>
    /// Пробуждение
    /// </summary>
    public override void Awake()
    {
        base.Awake();
        SetCustomErrorCodes();
    }

    /// <summary>
    /// Устанавливаю кастомные коды ошибок
    /// </summary>
    void SetCustomErrorCodes()
    {
        SFSErrorCodes.SetErrorMessage(1000, "Client version: {0} - invalid");
        SFSErrorCodes.SetErrorMessage(1001, "Database problem");
        SFSErrorCodes.SetErrorMessage(1002, "Incorrect username lenght. Username: {0}");
    }

    /// <summary>
    /// Update физики
    /// </summary>
    void FixedUpdate()
    {
        if (_server != null)
            _server.ProcessEvents();
    }

    /// <summary>
    /// Инициализирую сервер. Добавляю слушатели. Соединяемся
    /// </summary>
    /// <returns>Запустились ли методы?</returns>
    public bool Init()
    {
        if (IsConnected || IsConnecting)
            return false;
        _server = new SmartFox();
        _server.ThreadSafeMode = true;

        _server.AddEventListener(SFSEvent.CONNECTION, OnConnection);
        _server.AddEventListener(SFSEvent.CONNECTION_LOST, OnConnectionLost);

        _server.AddEventListener(SFSEvent.LOGIN, OnLoginToServer);
        _server.AddEventListener(SFSEvent.LOGIN_ERROR, OnLoginToServerError);

        _server.AddEventListener(SFSEvent.ROOM_JOIN, OnServerRoomJoin);
        _server.AddEventListener(SFSEvent.ROOM_JOIN_ERROR, OnServerRoomJoinError);
        //_server.AddEventListener(SFSEvent.USER_EXIT_ROOM, OnServerRoomLeave);

        _server.AddEventListener(SFSEvent.EXTENSION_RESPONSE, OnServerExtensionResponse);

        _server.AddLogListener(LogLevel.INFO, OnLogMessageInfo);
        _server.AddLogListener(LogLevel.WARN, OnLogMessageWarn);
        _server.AddLogListener(LogLevel.ERROR, OnLogMessageError);

        Connect();

        if (OnInited != null)
            OnInited();
        return true;
    }

    private void CoonectingStopwatchTick(long time)
    {
        if (time / 1000 >= Settings.maxConnectingTime)
            ConnectFailed();
    }

    void StopConnectingStopwatch()
    {
        if (!connectingStopwatch.IsStoped)
            connectingStopwatch.Stop();
    }

    /// <summary>
    /// Сбрасываем слушатели и переменные
    /// </summary>
    private void Reset()
    {
        StopConnectingStopwatch();        
        _server.RemoveEventListener(SFSEvent.CONNECTION, OnConnection);
        _server.RemoveEventListener(SFSEvent.CONNECTION_LOST, OnConnectionLost);

        _server.RemoveEventListener(SFSEvent.LOGIN, OnLoginToServer);
        _server.RemoveEventListener(SFSEvent.LOGIN_ERROR, OnLoginToServerError);

        _server.RemoveEventListener(SFSEvent.ROOM_JOIN, OnServerRoomJoin);
        _server.RemoveEventListener(SFSEvent.ROOM_JOIN_ERROR, OnServerRoomJoinError);
        //_server.RemoveEventListener(SFSEvent.USER_EXIT_ROOM, OnServerRoomLeave);

        _server.RemoveEventListener(SFSEvent.EXTENSION_RESPONSE, OnServerExtensionResponse);

        _server.RemoveLogListener(LogLevel.INFO, OnLogMessageInfo);
        _server.RemoveLogListener(LogLevel.WARN, OnLogMessageWarn);
        _server.RemoveLogListener(LogLevel.ERROR, OnLogMessageError);

        ResetVariables();

        _server = null;
    }

    /// <summary>
    /// Сбросить переменные
    /// </summary>
    private void ResetVariables()
    {
        _isLoged = false;
        _isLoging = false;
    }

    /// <summary>
    /// Проверим инстанс SmartFox чтобы не был null. 
    /// ***! ВАЖНО потом сделай так чтобы при дисконнекте и новом коннекте все слушатели сбрасывались и делался новый интсанс сервера со слушателями новыми
    /// </summary>
    /// <returns></returns>
    private bool HasServerInstance()
    {
        if (_server == null)
        {
            Debug.LogError("SmartFox Instance не найден!");
            return false;
        }
        return true;
    }

    #region Соединение
    /// <summary>
    /// Подключиться к серверу
    /// </summary>
    private void Connect()
    {
        connectingStopwatch = Timers.Instance.AddStopwatch();
        connectingStopwatch.OnSecondTick += CoonectingStopwatchTick;

        ConfigData cfg = new ConfigData();
        cfg.Host = Settings.Host;
        cfg.Port = Settings.PortTCP;
        cfg.Zone = Settings.ZoneName;
        cfg.Debug = Settings.Debug;

        LogMessage(LogLevel.INFO, "Пытаюсь соедениться с сервером");
        _server.Connect(cfg);
    }

    /// <summary>
    /// Отключиться от сервера
    /// </summary>
    public void Disconnect()
    {
        if (!IsConnected)
            return;
        
        _server.Disconnect();
    }

    /// <summary>
    /// Событие SmartFox о соединении попытке
    /// </summary>
    /// <param name="be"></param>
    private void OnConnection(BaseEvent be)
    {
        if ((bool)be.Params["success"])
        {
            LogMessage(
                LogLevel.INFO,
                "Connection established successfully" + "\n" +
                "SFS2x API Version:" + _server.Version + "\n" +
                "Connection mode is: " + _server.ConnectionMode);
            ConnectSucceed();
        }
        else
        {
            LogMessage(LogLevel.ERROR, "Connection failed; is the server running at all?");
            ConnectFailed();
        }
    }

    /// <summary>
    /// Событие SmartFox когда соединение было потеряно
    /// </summary>
    /// <param name="be"></param>
    private void OnConnectionLost(BaseEvent be)
    {
        LogMessage(LogLevel.WARN, "Connection was lost; reason is: " + (string)be.Params["reason"]);
        ConnectClosed();
    }

    /// <summary>
    /// Успешно соеденились
    /// </summary>
    private void ConnectSucceed()
    {
        StopConnectingStopwatch();
        if (OnConnectSucceed != null)
            OnConnectSucceed();
        Login();
    }

    /// <summary>
    /// Не получилось соедениться
    /// </summary>
    private void ConnectFailed()
    {
        Reset();

        if (OnConnectFailed != null)
            OnConnectFailed();
    }

    /// <summary>
    /// Потеряли соединение
    /// </summary>
    private void ConnectClosed()
    {
        Reset();

        if (OnConnectClosed != null)
            OnConnectClosed();
    }
    #endregion

    #region Логин

    /// <summary>
    /// Делаем логин
    /// </summary>
    private void Login()
    {
        if (!IsConnected)
            return;
        if (IsLoged || IsLoging)
            return;
        _isLoging = true;

        ISFSObject loginInData = new SFSObject();
        loginInData.PutUtfString("version", Version.CurrentBundle);
        loginInData.PutUtfString("platform", Application.platform.ToStr());
        loginInData.PutUtfString("facebookId", FacebookMy.Instance.LocalUserInfo.Id);

        _server.Send(
            new LoginRequest(
                    Utils.UniqueDeviceID.MD5(),
                    null,
                    Settings.ZoneName,
                    loginInData)
            );
    }

    /// <summary>
    /// SmartFox событие, успешно залогинились 
    /// </summary>
    /// <param name="be"></param>
    private void OnLoginToServer(BaseEvent be)
    {
        User user = (User)be.Params["user"];
        LogMessage(
            LogLevel.INFO,
            "Login successfully" + "\n" +
            "SFS2X API version: " + _server.Version + "\n" +
            "Connection mode is: " + _server.ConnectionMode + "\n" +
            "Logged in as " + user.Name);
        LoginSucceed(user);
    }

    /// <summary>
    /// SmartFox событие, не получилось залогиниться
    /// </summary>
    /// <param name="be"></param>
    private void OnLoginToServerError(BaseEvent be)
    {
        LogMessage(LogLevel.INFO, "Login failed: " + (string)be.Params["errorMessage"]);
        LoginError(Convert.ToInt32(be.Params["errorCode"]), (string)be.Params["errorMessage"]);
    }

    /// <summary>
    /// Успешно залогинились
    /// </summary>
    /// <param name="user"></param>
    private void LoginSucceed(User user)
    {
        _isLoging = false;
        _isLoged = true;
        if (OnLoginSucceed != null)
            OnLoginSucceed(user);
        RoomDefaultJoin();
    }

    /// <summary>
    /// Ошибка при логине
    /// </summary>
    private void LoginError(int code, string message)
    {
        _isLoging = false;
        if (OnLoginError != null)
            OnLoginError(code, message);
        Disconnect();
    }
    #endregion

    #region Базовая комната

    /// <summary>
    /// Зайти в комнату по умолчанию
    /// </summary>
    private void RoomDefaultJoin()
    {
        RoomJoin(Settings.RoomName);
    }

    /// <summary>
    /// Запрос на вход в комнату
    /// </summary>
    /// <param name="roomName"></param>
    public void RoomJoin(string roomName)
    {
        _server.Send(new JoinRoomRequest(roomName));
    }

    /// <summary>
    /// Запрос на выход из комнаты
    /// </summary>
    /// <param name="room"></param>
    public void RoomLeave(Room room)
    {
        _server.Send(new LeaveRoomRequest(room));
    }

    /// <summary>
    /// Событие SmartFox, вошли в комнату
    /// </summary>
    /// <param name="be"></param>
    private void OnServerRoomJoin(BaseEvent be)
    {
        Room room = (Room)be.Params["room"];
        LogMessage(LogLevel.INFO, "You joined room " + room.Name);
        RoomJoinSucceed(room);
    }

    /// <summary>
    /// Событие SmartFox, не получилось войти в комнату
    /// </summary>
    /// <param name="be"></param>
    private void OnServerRoomJoinError(BaseEvent be)
    {
        LogMessage(LogLevel.ERROR, "Room join failed: " + (string)be.Params["errorMessage"]);
        RoomJoinError();
    }

    /*/// <summary>
    /// Какойто пользователь покинул комнату
    /// </summary>
    /// <param name="be"></param>
    private void OnServerRoomLeave(BaseEvent be)
    {
        Room room = (Room)be.Params["room"];
        User user = (User)be.Params["user"];
        LogMessage(LogLevel.INFO, "Leave room: " + room.Name + ", user: " + user.Name);
    }*/

    /// <summary>
    /// Успешно вошли в комнату
    /// </summary>
    /// <param name="room"></param>
    private void RoomJoinSucceed(Room room)
    {
        if (OnRoomJoinSucceed != null)
            OnRoomJoinSucceed(room);
    }

    /// <summary>
    /// Не получилось войти в комнату
    /// </summary>
    private void RoomJoinError()
    {
        if (OnRoomJoinError != null)
            OnRoomJoinError();
        Disconnect();
    }
    #endregion

    #region Логирование
    /// <summary>
    /// Лог смартфокса уровень INFO
    /// </summary>
    /// <param name="be"></param>
    private void OnLogMessageInfo(BaseEvent be)
    {
        LogMessage(LogLevel.INFO, (string)be.Params["message"]);
    }

    /// <summary>
    /// Лог смартфокса уровень WARN
    /// </summary>
    /// <param name="be"></param>
    private void OnLogMessageWarn(BaseEvent be)
    {
        LogMessage(LogLevel.WARN, (string)be.Params["message"]);
    }

    /// <summary>
    /// Лог смартфокса уровень ERROR
    /// </summary>
    /// <param name="be"></param>
    private void OnLogMessageError(BaseEvent be)
    {
        LogMessage(LogLevel.ERROR, (string)be.Params["message"]);
    }

    /// <summary>
    /// Логирование
    /// </summary>
    /// <param name="level"></param>
    /// <param name="message"></param>
    private void LogMessage(LogLevel level, object message)
    {
        switch (level)
        {
            case LogLevel.INFO:
                Log.Info(message);
                break;
            case LogLevel.WARN:
                Log.Warning(message);
                break;
            case LogLevel.ERROR:
                Log.Error(message);
                break;
            default:
                Log.Info(message);
                break;
        }

        if (OnServerLog != null)
            OnServerLog(level, message);
    }
    #endregion

    /// <summary>
    /// Пришел ответ от расширения сервера
    /// </summary>
    /// <param name="be"></param>
    private void OnServerExtensionResponse(BaseEvent be)
    {
        ISFSObject data = (ISFSObject)be.Params["params"];
        string cmd = (string)be.Params["cmd"];
        if (OnExtensionResponse != null)
            OnExtensionResponse(cmd, data);
    }

    /// <summary>
    /// Отправить запрос с пустой информацией
    /// </summary>
    /// <param name="type">Тип запроса</param>
    public void Request(Requests.Types type)
    {
        Request(type, new SFSObject());
    }

    /// <summary>
    /// Отправить запрос с информацией
    /// </summary>
    /// <param name="type">Тип запроса</param>
    /// <param name="data">Информация</param>
    public void Request(Requests.Types type, ISFSObject data)
    {
        if (IsLoged)
            Requests.Send(type, data);
        else
            Log.Warning("Вы пытаетесь отправить запрос к расширению сервера, но вы не авторизованы!");
    }

    /// <summary>
    /// Приложение закрылось
    /// </summary>
    public override void OnApplicationQuit()
    {
        base.OnApplicationQuit();
        Disconnect();
    }
}
