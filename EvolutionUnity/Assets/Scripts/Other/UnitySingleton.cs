using UnityEngine;

/// <summary>
/// Держит в памяти только одну копию себя
/// </summary>
/// <typeparam name="T"></typeparam>
public class UnitySingleton<T> : GentleMonoBeh, IUnitySingleton where T : MonoBehaviour
{
    /// <summary>
    /// Инстанс локально
    /// </summary>
    private static T _instance;

    /// <summary>
    /// Выходим из приложения?
    /// </summary>
    public static bool Quiting { get; private set; }

    /// <summary>
    /// Ссылка на объект
    /// </summary>
    public static T Instance
    {
        get
        {
            CheckAndSetForInstance();
            return _instance;
        }
    }

    /// <summary>
    /// Проверяет есть ли ссылка на объект и устанавливает её если нет
    /// </summary>
    private static void CheckAndSetForInstance()
    {
        if (_instance == null)
        {
            _instance = GameObject.FindObjectOfType<T>();
            if (_instance == null)
                _instance = new GameObject(typeof(T).Name).AddComponent<T>();
        }
    }

    /// <summary>
    /// Уничтожить самого себя если уже есть копия
    /// </summary>
    private void DestroyMySelfIfExist()
    {
        if (_instance != null)
            if (_instance.gameObject != gameObject)
                Destroy(gameObject);
    }

    /// <summary>
    /// Объект появился в сцене
    /// </summary>
    public virtual void Awake()
    {
        CheckAndSetForInstance();
        DestroyMySelfIfExist();
        DontDestroyOnLoad(gameObject);        
    }

    /// <summary>
    /// При уничтожении себя
    /// </summary>
    public virtual void OnDestroy()
    {
        if (!Quiting)
            Log.Warning("Уничтожена копия: " + name);
    }

    /// <summary>
    /// Каждый кадр
    /// </summary>
    public override void NormalUpdate()
    {
        if (_instance != null)
            if (_instance != this)
                Destroy(gameObject);
    }

    /// <summary>
    /// Экономный Update
    /// </summary>
    public override void GentleUpdate()
    {
        
    }

    /// <summary>
    /// Выходим из приложения
    /// </summary>
    public virtual void OnApplicationQuit()
    {
        Quiting = true;
    }
}