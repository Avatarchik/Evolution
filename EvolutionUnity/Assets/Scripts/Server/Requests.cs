namespace Server
{
    using Sfs2X.Entities.Data;
    using Sfs2X.Requests;
    using System;

    /// <summary>
    /// Запросы на сервер
    /// </summary>
    public class Requests
    {
        /// <summary>
        /// Типы запроса
        /// </summary>
        public enum Types
        {
            FacebookUserData
        }

        /// <summary>
        /// Отправить с отсутсвующей информацией
        /// </summary>
        /// <param name="type">Тип запроса</param>
        public void Send(Types type)
        {
            Send(type, new SFSObject());
        }

        /// <summary>
        /// Отправить с информацией
        /// </summary>
        /// <param name="type">Тип запроса</param>
        /// <param name="data">Информация</param>
        public void Send(Types type, ISFSObject data)
        {
            if (Socket.Server == null)
                throw new NullReferenceException("Инстанс SmartFoxServer не установлен");

            Socket.Server.Send(new ExtensionRequest(type.ToStr(), data));
        }
    }
}
