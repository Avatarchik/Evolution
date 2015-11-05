using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class LoadAnimNoProgress : MonoBehaviour {
    /// <summary>
    /// Анимационный float как пресет
    /// </summary>
    public AnimatedFloat fadeFloatPreset = new AnimatedFloat() { enabled = true, smooth = true, changeSpeed = 10f, minValue = 0, maxValue = 1f };
    /// <summary>
    /// Анимационный float 
    /// </summary>
    private AnimatedFloat fadeFloat;

    /// <summary>
    /// Графика для анимации
    /// </summary>
    public List<GraphicsColorTransformer> graphics = new List<GraphicsColorTransformer>();

    /// <summary>
    /// Текущий итерационный индекс
    /// </summary>
    private int currentIndex;

    /// <summary>
    /// Анимировать?
    /// </summary>
    private bool animate = false;

    /// <summary>
    /// Анимирует?
    /// </summary>
    public bool IsAnimate
    {
        get
        {
            return animate;
        }
    }

    /// <summary>
    /// Старт
    /// </summary>
    void Start()
    {
        fadeFloat = fadeFloatPreset.CloneProperties();
        HideGraphics();
    }
    
    /// <summary>
    /// Каждый кадр. Анимация здесь
    /// </summary>
    void Update()
    {
        if (!animate)
            return;

        float value = fadeFloat.Value;
        float iValue = 1f - value;

        graphics[currentIndex].offset = value;
        if (currentIndex > 0)
            graphics[currentIndex - 1].offset = iValue;
        else
            graphics[graphics.Count - 1].offset = iValue;

        if (value >= fadeFloatPreset.maxValue)
        {
            if (currentIndex + 1 <= graphics.Count - 1)
                currentIndex++;
            else
                currentIndex = 0;

            fadeFloat = fadeFloatPreset.CloneProperties();
        }
    }
    
    /// <summary>
    /// Начать анимацию
    /// </summary>
    public void Animate()
    {
        if (IsAnimate)
            return;
        animate = true;
        currentIndex = 1;
    }

    /// <summary>
    /// Закончить анимацию
    /// </summary>
    public void Stop()
    {
        if (!IsAnimate)
            return;
        animate = false;
        HideGraphics();
    }

    private void HideGraphics()
    {
        foreach (GraphicsColorTransformer graphic in graphics)
            graphic.offset = 0;
    }
}
