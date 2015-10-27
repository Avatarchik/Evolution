using Sfs2X.Core;
using Sfs2X.Entities;
using Sfs2X.Logging;

public interface ISmartFoxSocket {
    void FixedUpdate();
    void Init();
    void Reset();
    void ResetVariables();
    void HasServerInstance();
    void Connect();
    void Disconnect();
    void OnConnection(BaseEvent be);
    void OnConnectionLost(BaseEvent be);
    void ConnectSucceed();
    void ConnectFailed();
    void ConnectClosed();
    void Login();
    void OnLoginToServer(BaseEvent be);
    void OnLoginToServerError(BaseEvent be);
    void LoginSucceed(User user);
    void LoginError();
    void RoomDefaultJoin();
    void RoomJoin(string roomName);
    void OnServerRoomJoin(BaseEvent be);
    void OnServerRoomJoinError(BaseEvent be);
    void RoomJoinSucceed(Room room);
    void RoomJoinError();
    void OnLogMessageInfo(BaseEvent be);
    void OnLogMessageWarn(BaseEvent be);
    void OnLogMessageError(BaseEvent be);
    void LogMessage(LogLevel level, object message);
    void OnServerExtensionResponse(BaseEvent be);
    void OnApplicationQuit();
}
