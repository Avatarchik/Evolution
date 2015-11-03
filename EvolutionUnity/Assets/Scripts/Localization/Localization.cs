using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Linq;
using System.IO;
using System;

[DisallowMultipleComponent]
public sealed class Localization : UnitySingleton<Localization>, ISavable
{
    /// <summary>
    /// Обновился язык локализации
    /// </summary>
    public Action OnRefresh;

    /// <summary>
    /// Ключ для PlayerPrefs
    /// </summary>
    public const string SAVE_KEY = "language";

    /// <summary>
    /// Список слов
    /// </summary>
    public static Dictionary<string, LocalizationData> words = new Dictionary<string,LocalizationData>();

    /// <summary>
    /// Текущий язык
    /// </summary>
    public SystemLanguage CurrentLanguage
    {
        get
        {
            return _currentLanguage;
        }
        set
        {
            if (value == _currentLanguage)
                return;

            _currentLanguage = value;
            ReadFiles();
            if (OnRefresh != null)
            {
                Log.Info("Localization Refresh с учетом региона " + _currentLanguage.ToStr());
                OnRefresh();
            }
            Save();
        }
    }

    /// <summary>
    /// Язык по умолчанию
    /// </summary>
    public static SystemLanguage defaultLanguage = SystemLanguage.English;

    /// <summary>
    /// Определять язык через язык системы
    /// </summary>
    public bool viaSystemLanguage = true;

    /// <summary>
    /// Текущий язык, приватно
    /// </summary>
    private static SystemLanguage _currentLanguage = SystemLanguage.English;

    /// <summary>
    /// Пробуждение
    /// </summary>
    public override void Awake()
    {
        base.Awake();
        Init();
    }

    /// <summary>
    /// Инициализация
    /// </summary>
    public void Init()
    {
        if (viaSystemLanguage)
            defaultLanguage = Application.systemLanguage;

        LoadSaves();
    }

    /// <summary>
    /// Возвращает перевод строки через ключ
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    public static string Get(string key)
    {
        LocalizationData word;
        if (words.TryGetValue(key, out word))
        {
            return word.represent.Replace("\\n","\n");
        }

        return key;
    }

    /// <summary>
    /// Читает папку с файлами переводов
    /// </summary>
    public static void ReadFiles()
    {
        try
        {
            //Читаем файл локализации
            TextAsset textAsset = (TextAsset)Resources.Load("Localization/" + _currentLanguage.ToStr(), typeof(TextAsset));

            //Если не найден
            if (textAsset == null)
            {
                //ставим по умолчанию
                Log.Warning("Файл локализации: " + _currentLanguage.ToStr() + " не найден ставлю по умолчанию: " + defaultLanguage.ToString("g"));
                _currentLanguage = defaultLanguage;

                //Читаем файл локализации еще раз
                textAsset = (TextAsset)Resources.Load("Localization/" + _currentLanguage.ToStr(), typeof(TextAsset));
            }

            XElement xElement = XElement.Parse(textAsset.text);
            textAsset = null;
            words = new Dictionary<string, LocalizationData>();
            foreach (XElement xWords in xElement.Elements())
            {
                LocalizationData word = new LocalizationData();
                foreach (XElement xWord in xWords.Elements())
                {
                    switch (xWord.Name.LocalName)
                    {
                        case "key":
                            word.key = xWord.Value;
                            break;
                        case "represent":
                            word.represent = xWord.Value;
                            break;
                        default:
                            break;
                    }
                }

                words.Add(word.key, word);
            }
        }
        catch (NullReferenceException e)
        {
            Log.Error("Файл локализации " + _currentLanguage.ToStr() + " не найден. Проверьте названия файлов в папке с переводами" + "\n" + e.Message);
        }
        catch (ArgumentException e)
        {
            Log.Error("В файле локализации " + _currentLanguage.ToStr() + " дублируются ключи. Удалите дубликаты ключей!" + "\n" + e.Message);
        }
        catch (Exception e)
        {
            Log.Error("При чтении файлов локализации возникли проблемы\n" + e.Message);
        }
    }

    /// <summary>
    /// Сохранить используемый язык
    /// </summary>
    public void Save()
    {
        PlayerPrefs.SetInt(SAVE_KEY, (int)_currentLanguage);
        PlayerPrefs.Save();
    }

    /// <summary>
    /// Загрузить язык и прочитать файлы
    /// </summary>
    public void LoadSaves()
    {
        if (PlayerPrefs.HasKey(SAVE_KEY))
            _currentLanguage = (SystemLanguage) PlayerPrefs.GetInt(SAVE_KEY);

        ReadFiles();
    }

    /// <summary>
    /// Сбросить сохранение языка
    /// </summary>
    public void ResetSaves()
    {
        PlayerPrefs.DeleteKey(SAVE_KEY);
    }

    public override void OnApplicationQuit()
    {
        base.OnApplicationQuit();
        Save();
    }
}
