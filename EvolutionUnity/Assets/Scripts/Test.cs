using UnityEngine;
using System.Collections;
using MyUtils;
using Sfs2X.Util;
using UnityEngine.UI;

public class Test : MonoBehaviour {
    Stopwatch stopwatch;

    void OnEnable()
    {
        //stopwatch = Timers.Instance.AddStopwatch();
        //stopwatch.OnTick += OnTick;
        //stopwatch.OnSecondTick += (long value) => Log.Info("Second: " + value.ToString());
    }

    void Start()
    {
        Dialogs.Instance.Show(DialogTypes.Classic);
    }

    void OnTick(long value)
    {
        Log.Info(value);
    }

    void OnDisable()
    {
        //stopwatch.Stop();
    }
}
