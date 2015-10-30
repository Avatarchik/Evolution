using UnityEngine;
using System.Collections;
using MyUtils;
using Sfs2X.Util;
using UnityEngine.UI;

public class Test : MonoBehaviour {
    SecondsTimer timer;

    void OnEnable()
    {
        timer = (SecondsTimer) Timers.Instance.AddTimer<SecondsTimer>();
        timer.OnTick += OnTick;
        timer.OnStop += (ITimer obj) => { Log.Info("Timer destroy"); obj = null; Debug.Log(timer.Passed); };
    }

    void OnTick(int value)
    {
        Log.Info(value);
        Log.Info("Размер таймеров: " + Timers.Instance.Data.Count.ToString());
    }

    void OnDisable()
    {
        timer.Stop();
    }
}
