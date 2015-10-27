using UnityEngine;
using System.Collections;
using MyUtils;

public class Test : MonoBehaviour {

    void Awake()
    {
        Log.Warning(Utils.GetMacAddress());
    }
}
