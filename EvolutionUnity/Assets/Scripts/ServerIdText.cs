using UnityEngine;
using UnityEngine.UI;

public class ServerIdText : GentleMonoBeh {


    public override void NormalUpdate()
    {
        
    }

    public override void GentleUpdate()
    {
        if (Socket.Instance.IsLoged)
            GetComponent<Text>().text = "ID: " + Socket.Server.MySelf.Name;
        else
            GetComponent<Text>().text = "ID: Unknow";
    }
}
