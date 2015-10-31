using UnityEngine;
using System.Collections;

public class FacebookLogoutBL : ButtonListener
{

    public override void OnClick()
    {
        FacebookMy.Instance.Logout();
    }

    public override void GentleUpdate()
    {
        Button.interactable = FacebookMy.Instance.IsInited && FacebookMy.Instance.IsAuthenticated;
    }
}
