/// <summary>
/// Интерфейс диалога
/// </summary>
public interface IDialog {
    /// <summary>
    /// Показать
    /// </summary>
    void DoShow();
    /// <summary>
    /// Установить показанным
    /// </summary>
    void Show();
    /// <summary>
    /// Спрятать
    /// </summary>
    void DoHide();
    /// <summary>
    /// Установить спрятанным
    /// </summary>
    void Hide();
    /// <summary>
    /// Уничтожить
    /// </summary>
    void Destroy();
}
