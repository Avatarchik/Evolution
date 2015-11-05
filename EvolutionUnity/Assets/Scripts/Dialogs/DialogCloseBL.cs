using UnityEngine;
using System.Collections;

public class DialogCloseBL : ButtonListener {

    public override void OnClick()
    {
        GetComponentInParent<IDialog>().DoDestroy();
    }
}
