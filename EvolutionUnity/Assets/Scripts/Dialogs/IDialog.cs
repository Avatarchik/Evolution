/// <summary>
/// Интерфейс диалога
/// </summary>
public interface IDialog {
    /// <summary>
    /// Начать показывание
    /// </summary>
    void DoShow();
    /// <summary>
    /// Установить показанным
    /// </summary>
    void Show();
    /// <summary>
    /// Начать прятание
    /// </summary>
    void DoHide();
    /// <summary>
    /// Установить спрятанным
    /// </summary>
    void Hide();
    /// <summary>
    /// Начать уничтожение
    /// </summary>
    void DoDestroy();
    /// <summary>
    /// Уничтожить
    /// </summary>
    void Destroy();
}
