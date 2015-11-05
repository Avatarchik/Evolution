using UnityEngine.UI;

public class PingText : GentleMonoBeh
{
    public override void NormalUpdate()
    {

    }

    public override void GentleUpdate()
    {
        if (Socket.Instance.IsLoged)
            GetComponent<Text>().text = "PING: " + PingPong.Instance.Delay;
        else
            GetComponent<Text>().text = "PING: 0";
    }
}
