using UnityEngine;
using System.Collections;
using MyUtils;
using Sfs2X.Util;
using UnityEngine.UI;

public class Test : MonoBehaviour {
    public Transform t;
    public Joystick j;
    public float spd = 1f;
    void Update()
    {
        t.localPosition += new Vector3(
                Mathf.Cos(j.Angle * Mathf.Deg2Rad) * spd * j.Power * Time.deltaTime,
                Mathf.Sin(j.Angle * Mathf.Deg2Rad) * spd * j.Power * Time.deltaTime,
                0);
    }
}
