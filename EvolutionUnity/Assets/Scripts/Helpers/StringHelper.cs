using System.Security.Cryptography;
using System.Text;

public static class StringHelper {

    /// <summary>
    /// Переводим строку в MD5
    /// </summary>
    /// <param name="str"></param>
    public static string MD5(this string input)
    {
        using (MD5 md5Hash = System.Security.Cryptography.MD5.Create())
        {
            return GetMd5Hash(md5Hash, input);
        }
    }

    /// <summary>
    /// Получить хеш MD5
    /// </summary>
    /// <param name="md5Hash"></param>
    /// <param name="input"></param>
    /// <returns></returns>
    static string GetMd5Hash(MD5 md5Hash, string input)
    {

        // Convert the input string to a byte array and compute the hash.
        byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));

        // Create a new Stringbuilder to collect the bytes
        // and create a string.
        StringBuilder sBuilder = new StringBuilder();

        // Loop through each byte of the hashed data 
        // and format each one as a hexadecimal string.
        for (int i = 0; i < data.Length; i++)
        {
            sBuilder.Append(data[i].ToString("x2"));
        }

        // Return the hexadecimal string.
        return sBuilder.ToString();
    }
}
