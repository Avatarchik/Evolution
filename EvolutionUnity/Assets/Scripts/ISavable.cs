/// <summary>
/// Интерфейс сохранения и загрузки
/// </summary>
public interface ISavable
{
    /// <summary>
    /// Сохранить состояние
    /// </summary>
    void Save();
    /// <summary>
    /// Загрузить состояние
    /// </summary>
    void LoadSaves();
    /// <summary>
    /// Сбросить состояние
    /// </summary>
    void ResetSaves();
}