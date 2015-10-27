public static class EnumHelper {

    /// <summary>
    /// Мой хелпер который преобразует в строку с флагом "g"
    /// </summary>
    /// <param name="enumerable"></param>
    /// <returns></returns>
    public static string ToStr(this System.Enum enumerable) {
        return enumerable.ToString("g");
    }
}