using UnityEngine;
using System.Collections;

public class MovingAI : MonoBehaviour {
    /// <summary>
    /// Дозволенная дистанция (зона действия) (круг)
    /// </summary>
    public float maxMoveDistance = 1f;

    /// <summary>
    /// Скорость перемещения
    /// </summary>
    public float moveSpeed = 0.5f;

    /// <summary>
    /// Скорость возврата если вышли из дозволенной дистанции
    /// </summary>
    public float returnSpeed = 2f;

    /// <summary>
    /// Угол в который движемся
    /// </summary>
    public float moveAngle;

    /// <summary>
    /// Координаты объекта на старте
    /// </summary>
    public Vector3 StartPosition { get; private set; }
    
    /// <summary>
    /// Анимационный float для шума
    /// </summary>
    public AnimatedFloat waveNoise;

    /// <summary>
    /// Флаг автопилот по возвращению в центр зоны
    /// </summary>
    public bool Returning { get; private set; }

    /// <summary>
    /// Локальная позиция объекта
    /// </summary>
    private Vector3 LocalPosition
    {
        get
        {
            return transform.localPosition;
        }
        set
        {
            transform.localPosition = value;
        }
    }

    /// <summary>
    /// На какое расстояние двигаться
    /// </summary>
    private float MoveDistance
    {
        get
        {
            return moveSpeed * Time.deltaTime;
        }
    }

    /// <summary>
    /// Старт
    /// </summary>
    public virtual void Start()
    {
        StartPosition = LocalPosition;
    }

    public virtual void Update()
    {
        //Если уехали далеко, то хотим вернуться
        if (Vector3.Distance(LocalPosition, StartPosition) > maxMoveDistance)
            Returning = true;

        //Если автопилот => Возвращаемся в центр зоны действия
        if(Returning)
            moveAngle = Mathf.LerpAngle(
                moveAngle,
                Mathf.Atan2(StartPosition.y - LocalPosition.y, StartPosition.x - LocalPosition.x) * Mathf.Rad2Deg,
                returnSpeed * Time.deltaTime);

        //Делаем шум волной. Изменяем угол
        if (waveNoise.enabled)
            moveAngle += waveNoise.Value;

        //Обновим позицию себя
        LocalPosition += new Vector3(
                Mathf.Cos(moveAngle * Mathf.Deg2Rad) * MoveDistance,
                Mathf.Sin(moveAngle * Mathf.Deg2Rad) * MoveDistance,
                0);

        //Если Автопилот => Если вернулись в центр зоны => Выключим автопилот
        if (Returning)
            if (Vector3.Distance(LocalPosition, StartPosition) <= maxMoveDistance / 10f)
                Returning = false;
    }
}
