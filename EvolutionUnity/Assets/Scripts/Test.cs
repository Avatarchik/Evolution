using UnityEngine;
using System.Collections;
using MyUtils;
using Sfs2X.Util;
using UnityEngine.UI;

public class Test : MonoBehaviour {
    Stopwatch stopwatch;
    Dialog dialog;

    void OnEnable()
    {
        stopwatch = Timers.Instance.AddStopwatch();
        stopwatch.OnSecondTick += OnSecondTick;
        dialog = Dialogs.Instance.Add(DialogTypes.Classic);
        Dialogs.Instance.ShowNext();
    }

    void OnSecondTick(long value)
    {
        
    }

    void OnDisable()
    {
        stopwatch.Stop();
    }
}
