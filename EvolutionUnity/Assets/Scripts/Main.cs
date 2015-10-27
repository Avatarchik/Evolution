using UnityEngine;
using System;

public class Main : MonoBehaviour {

    private int _frameCounter = 0;

    void Awake()
    {
        Application.targetFrameRate = 60;

#if UNITY_IOS || UNITY_ANDROID
        if (IsBigScreen())
            ReduceResolutionOf(2);
#endif
    }

    void Update()
    {
        if (_frameCounter > 0)
        {
            if (_frameCounter > 1)
                return;

            CallFirstFrameMethods();
        }
        _frameCounter++;
    }

    /// <summary>
    /// Вызвать методы для первого кадра
    /// </summary>
    void CallFirstFrameMethods()
    {
        LoadFirstLevel();
    }

    /// <summary>
    /// Загрузим уровень под индексом 1
    /// </summary>
    void LoadFirstLevel()
    {
        Application.LoadLevel(1);
    }

    /// <summary>
    /// Экран большой?
    /// </summary>
    /// <returns></returns>
    bool IsBigScreen()
    {
        return Screen.currentResolution.width > 1280 && Screen.currentResolution.height > 800 && Screen.dpi > 240f;
    }

    /// <summary>
    /// Уменьшить разрешение экрана в N раз
    /// </summary>
    /// <param name="divider">Делитель</param>
    void ReduceResolutionOf(int divider)
    {
        Screen.SetResolution(Convert.ToInt32(Screen.currentResolution.width / divider), Convert.ToInt32(Screen.currentResolution.height / divider), true);
    }
}
