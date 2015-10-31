using UnityEngine;

/// <summary>
/// Работа с фейсбуком в моей игре
/// </summary>
public class FacebookMy : UnitySingleton<FacebookMy> {

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
        IsInited = false;
    }

    void Start()
    {
        SPFacebook.Instance.Init();
    }

    /// <summary>
    /// Инициализировались
    /// </summary>
    void OnInitComplete()
    {
        IsInited = true;
    }

    /// <summary>
    /// После попытки авторизации
    /// </summary>
    /// <param name="result"></param>
    void OnAuthenticated(FB_APIResult result)
    {
        Log.Info(result.IsSucceeded);
        Log.Info(result.Error);
        Log.Info(result.Responce);
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

}
