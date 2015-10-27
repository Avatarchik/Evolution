namespace MyUtils
{
    using System.Net.NetworkInformation;
    using UnityEngine;

    public static class Utils
    {
#if !UNITY_IOS && ! UNITY_ANDROID
        /// <summary>
        /// Получить MAC адрес устройства
        /// </summary>
        /// <returns></returns>
        public static string GetMacAddress()
        {
            const int MIN_MAC_ADDR_LENGTH = 12;
            string macAddress = string.Empty;
            long maxSpeed = -1;

            foreach (NetworkInterface nic in NetworkInterface.GetAllNetworkInterfaces())
            {
                Log.Info("Found MAC Address: " + nic.GetPhysicalAddress() + " Type: " + nic.NetworkInterfaceType);

                string tempMac = nic.GetPhysicalAddress().ToString();
                if (nic.Speed > maxSpeed &&
                    !string.IsNullOrEmpty(tempMac) &&
                    tempMac.Length >= MIN_MAC_ADDR_LENGTH)
                {
                    Log.Info("New Max Speed = " + nic.Speed + ", MAC: " + tempMac);
                    maxSpeed = nic.Speed;
                    macAddress = tempMac;
                }
            }

            return macAddress;
        }
#endif
        /// <summary>
        /// Уникальный ID для каждой инсталяции
        /// </summary>
        /// <returns></returns>
        public static string UniqueDeviceID
        {
            get
            {
                return SystemInfo.deviceUniqueIdentifier;
            }
        }
        
        /// <summary>
        /// Монитор хороший?
        /// </summary>
        /// <returns></returns>
        public static bool IsNiceScreen()
        {
            return Screen.currentResolution.width > 1280 && Screen.currentResolution.height > 800 && Screen.dpi > 240f;
        }

    }
}