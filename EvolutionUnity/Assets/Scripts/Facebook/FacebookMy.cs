using UnityEngine;
using System;
using Sfs2X.Protocol.Serialization;
using com.littleteam.evon.serialization;

/// <summary>
/// Работа с фейсбуком в моей игре
/// </summary>
[DisallowMultipleComponent]
public sealed class FacebookMy : UnitySingleton<FacebookMy>, ISavable {
    /// <summary>
    /// Событие о загрузке информации о пользователе
    /// </summary>
    public Action OnUserInfoLoaded;

    /// <summary>
    /// Информация о пользователе из сохранений. Кэш
    /// </summary>
    public FacebookUserInfoMy LocalUserInfo
    {
        get
        {
            return _userInfo;
        }
    }
    private FacebookUserInfoMy _userInfo = new FacebookUserInfoMy();

    /// <summary>
    /// Загружена ли инфа о пользователе
    /// </summary>
    public bool IsUserInfoLoaded {get; private set;}

    /// <summary>
    /// Информация о пользователе
    /// </summary>
    public FacebookUserInfo FacebookUserInfo
    {
        get
        {
            if (!IsUserInfoLoaded)
                return null;
            return SPFacebook.Instance.userInfo;
        }
    }

    /// <summary>
    /// Вошел в аккаунт?
    /// </summary>
    public bool IsAuthenticated
    {
        get
        {
            return SPFacebook.Instance.IsLoggedIn;
        }
    }

    /// <summary>
    /// Плагин инициализирован?
    /// </summary>
    public bool IsInited { get; private set; }

    public override void Awake()
    {
        base.Awake();

        SPFacebook.Instance.OnInitCompleteAction += OnInitComplete;
        SPFacebook.Instance.OnAuthCompleteAction += OnAuthenticated;
        SPFacebook.Instance.OnUserDataRequestCompleteAction += OnUserDataLoaded;
        IsInited = false;
        IsUserInfoLoaded = false;
        OnUserInfoLoaded += () => { };
        LoadSaves();
    }

    public void Init()
    {
        if(!IsInited)
            SPFacebook.Instance.Init();
    }

    /// <summary>
    /// Инициализировались
    /// </summary>
    void OnInitComplete()
    {
        IsInited = true;
        if (IsAuthenticated)
            SPFacebook.Instance.LoadUserData();
    }

    /// <summary>
    /// После попытки авторизации
    /// </summary>
    /// <param name="result"></param>
    void OnAuthenticated(FB_APIResult result)
    {
        if (result.Error.Equals(string.Empty))
            SPFacebook.Instance.LoadUserData();
        else
            Log.Warning("Facebook authenticate error:" + "\n" + result.Error);
    }

    /// <summary>
    /// Фейсбук ответил информацией о пользователе
    /// </summary>
    /// <param name="result"></param>
    void OnUserDataLoaded(FB_APIResult result)
    {
        if (result.Error.Equals(string.Empty))
        {
            IsUserInfoLoaded = true;
            LocalUserInfo.Id = FacebookUserInfo.id;
            LocalUserInfo.Email = FacebookUserInfo.email;
            LocalUserInfo.FirstName = FacebookUserInfo.first_name;
            LocalUserInfo.LastName = FacebookUserInfo.last_name;
            LocalUserInfo.Gender = (int)FacebookUserInfo.gender;
            LocalUserInfo.Locale = FacebookUserInfo.locale;
            Save();
            SPFacebook.Instance.userInfo.LoadProfileImage(FacebookProfileImageSize.square);
            if (OnUserInfoLoaded != null)
                OnUserInfoLoaded();
        }
        else
        {
            Log.Warning("Facebook user data load failed with error: " + "\n" + result.Error);
        }
    }

    /// <summary>
    /// Войти в аккаунт
    /// </summary>
    public void Login()
    {
        if (!IsAuthenticated)
            SPFacebook.Instance.Login();
        else
            Log.Info("Уже авторизованы");
    }

    /// <summary>
    /// Деавторизоваться...выйти короче
    /// </summary>
    public void Logout()
    {
        if (IsAuthenticated)
            SPFacebook.Instance.Logout();
        else
            Log.Info("Уже вышли");
    }

    /// <summary>
    /// Получить квадратную текстуру аватара
    /// </summary>
    /// <returns></returns>
    public Texture2D SquareProfileImage
    {
        get
        {
            if (!IsUserInfoLoaded)
                return null;
            return FacebookUserInfo.GetProfileImage(FacebookProfileImageSize.square);
        }
    }

    #region Сохранения
    private string ppId = "FacebookId";
    private string ppEmail = "FacebookEmail";
    private string ppFirstName = "FacebookFirstName";
    private string ppLastName = "FacebookLastName";
    private string ppGender = "FacebookGender";
    private string ppLocale = "FacebookLocale";

    /// <summary>
    /// Сохранить в PlyaerPrefs
    /// </summary>
    public void Save()
    {
        PlayerPrefs.SetString(ppId, LocalUserInfo.Id);
        PlayerPrefs.SetString(ppEmail, LocalUserInfo.Email);
        PlayerPrefs.SetString(ppFirstName, LocalUserInfo.FirstName);
        PlayerPrefs.SetString(ppLastName, LocalUserInfo.LastName);
        PlayerPrefs.SetInt(ppGender, LocalUserInfo.Gender);
        PlayerPrefs.SetString(ppLocale, LocalUserInfo.Locale);
        PlayerPrefs.Save();
    }

    /// <summary>
    /// Загрузить из PlyaerPrefs
    /// </summary>
    public void LoadSaves()
    {
        if (PlayerPrefs.HasKey(ppId))
            LocalUserInfo.Id = PlayerPrefs.GetString(ppId);
        if (PlayerPrefs.HasKey(ppEmail))
            LocalUserInfo.Email = PlayerPrefs.GetString(ppEmail);
        if (PlayerPrefs.HasKey(ppFirstName))
            LocalUserInfo.FirstName = PlayerPrefs.GetString(ppFirstName);
        if (PlayerPrefs.HasKey(ppLastName))
            LocalUserInfo.LastName = PlayerPrefs.GetString(ppLastName);
        if (PlayerPrefs.HasKey(ppGender))
            LocalUserInfo.Gender = PlayerPrefs.GetInt(ppGender);
        if (PlayerPrefs.HasKey(ppLocale))
            LocalUserInfo.Locale = PlayerPrefs.GetString(ppLocale);
    }

    /// <summary>
    /// Сбросить PlayerPrefs
    /// </summary>
    public void ResetSaves()
    {
        PlayerPrefs.DeleteKey(ppId);
        PlayerPrefs.DeleteKey(ppEmail);
        PlayerPrefs.DeleteKey(ppFirstName);
        PlayerPrefs.DeleteKey(ppLastName);
        PlayerPrefs.DeleteKey(ppGender);
        PlayerPrefs.DeleteKey(ppLocale);
    }
    #endregion
}
