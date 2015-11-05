using Sfs2X.Entities;
using UnityEngine;
public class ConnectionLostDialog : Dialog
{
    public LoadAnimNoProgress loading;
        
    public void OnReconnectButtonClick()
    {
        Socket.Instance.Init();
    }

    public override void Update()
    {
        base.Update();
        if (Socket.Instance.IsLoged)
        {
            Game.Instance.LoadMenu();
            Destroy();
        }
    }
}
