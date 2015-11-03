namespace com.littleteam.evon.serialization
{
    using Sfs2X.Protocol.Serialization;
    /// <summary>
    /// Инфо о пользователе фейсбука в моем представлении
    /// </summary>
    public class FacebookUserInfoMy : SerializableSFSType
    {
        public string Id = "";
        public string Email = "";
        public string FirstName = "";
        public string LastName = "";
        public int Gender = 0; //0 Unknow, 1 Male, 2 Female
        public string Locale = "";

        public string getDump()
        {
            return "Dump:\n" +
                Id + "\n" +
                Email + "\n" +
                FirstName + "\n" +
                LastName + "\n" +
                Gender.ToString() + "\n" +
                Locale;
        }
    }
}