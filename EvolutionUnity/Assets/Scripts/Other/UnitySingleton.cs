using UnityEngine;

/// <summary>
/// Держит в памяти только одну копию себя
/// </summary>
/// <typeparam name="T"></typeparam>
public class UnitySingleton<T> : GentleMonoBeh, IUnitySingleton where T : MonoBehaviour
{    
    private static T _instance;

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
            if (_instance != this)
                Destroy(gameObject);
    }

    /// <summary>
    /// Объект появился в сцене
    /// </summary>
    public virtual void Awake()
    {
        CheckAndSetForInstance();
        DestroyMySelfIfExist();
        DontDestroyOnLoad(this);        
    }

    /// <summary>
    /// При уничтожении себя
    /// </summary>
    public virtual void OnDestroy()
    {
        Log.Warning("Unichtojena kopiya: " + name);
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
}