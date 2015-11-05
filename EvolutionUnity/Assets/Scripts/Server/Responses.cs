namespace Server
{
    using UnityEngine;
    using System.Collections;
    using Sfs2X.Entities.Data;
    using System;

    /// <summary>
    /// Клас по работе с ответами от расширения сервера
    /// </summary>
    public class Responses : UnitySingleton<Responses>
    {
        public Action<Types, ISFSObject> OnResponse;
        /// <summary>
        /// Типы ответа
        /// </summary>
        public enum Types
        {
            Pong
        }

        void Start()
        {
            Socket.Instance.OnExtensionResponse += OnExtensionResponse;
        }

        /// <summary>
        /// Пришел ответ
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="data"></param>
        void OnExtensionResponse(string cmd, ISFSObject data)
        {
            Types type = (Types) Enum.Parse(typeof(Types), cmd);
            Response(type, data);
        }

        /// <summary>
        /// Ответ
        /// </summary>
        /// <param name="type"></param>
        /// <param name="?"></param>
        void Response(Types type, ISFSObject data) {            
            if (OnResponse != null)
                OnResponse(type, data);
        }
    }
}