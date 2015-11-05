using UnityEngine;

/// <summary>
/// Джойстик для мыши
/// </summary>
public class JoystickMouse : Joystick {

    /// <summary>
    /// Обрезать значения Axis от -1 до 1?
    /// </summary>
    public bool clamp;

    /// <summary>
    /// Каждый кадр
    /// </summary>
    public override void Update()
    {
        Vector2 mouseWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        if (clamp)
            UpdateVariables(
                Mathf.Clamp(transform.position.x - mouseWorldPosition.x, -1f, 1f) * -1f,
                Mathf.Clamp(transform.position.y - mouseWorldPosition.y, -1f, 1f) * -1f
                );
        else
            UpdateVariables(
                (transform.position.x - mouseWorldPosition.x) * -1f,
                (transform.position.y - mouseWorldPosition.y) * -1f
            );
    }
}
