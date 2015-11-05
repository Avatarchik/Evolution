using UnityEngine;
using System.Collections;

public class ClientUpdateDialog : Dialog {
    public override void Start()
    {
        base.Start();
    }

    public override void Update()
    {
        base.Update();
    }

    public void OnDownloadClick()
    {
        DoDestroy();
        Application.OpenURL(Game.Instance.UpdateURL);
    }
}
