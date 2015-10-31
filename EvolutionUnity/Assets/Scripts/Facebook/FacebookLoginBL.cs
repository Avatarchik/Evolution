public class FacebookLoginBL : ButtonListener {
    
    public override void OnClick()
    {
        FacebookMy.Instance.Login();
    }

    public override void GentleUpdate()
    {
        Button.interactable = FacebookMy.Instance.IsInited && !FacebookMy.Instance.IsAuthenticated;
    }
}
