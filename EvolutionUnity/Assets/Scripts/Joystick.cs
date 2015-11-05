using UnityEngine;
using System.Collections;
using System;

/// <summary>
/// Обычный джойстик
/// </summary>
public class Joystick : MonoBehaviour {

    /// <summary>
    /// Оси
    /// </summary>
    public Vector2 Axis;

    /// <summary>
    /// Угол между Vector(1,0) и осями
    /// </summary>
    public float Angle;

    /// <summary>
    /// Сила натяжения
    /// </summary>
    public float Power;

    /// <summary>
    /// Каждый кадр
    /// </summary>
    public virtual void Update()
    {
        UpdateVariables(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
    }

    /// <summary>
    /// Установить оси, посчитать угол и силу натяжения
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    public void UpdateVariables(float xAxis, float yAxis)
    {
        Axis.x = xAxis;
        Axis.y = yAxis;
        Angle = Vector2.Angle(Axis, Vector2.right) * (Axis.y < 0 ? -1f : 1f);
        Power = Mathf.Clamp(Vector2.Distance(Vector2.zero, Axis), 0, 1f);
    }
}
