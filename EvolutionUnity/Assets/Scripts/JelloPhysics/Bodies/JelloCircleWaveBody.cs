using UnityEngine;
using System.Collections;

public class JelloCircleWaveBody : JelloCircleBody {
    [SerializeField]
    public float waveHeight = 1f;
    [SerializeField]
    public float waveLenght = 1f;
    [SerializeField]
    public float waveSpeed = 1f;

    private Vector2[] lowerPoints;
    private Vector2[] upperPoints;

    [System.Serializable]
    public class FloatOffset
    {
        public bool inc = true;
        public float value;
    }
    private FloatOffset[] offsets;
    private Vector2[] velo;

    public override void Awake()
    {
        base.Awake();

        if (waveHeight >= radius)
            waveHeight = radius - 0.05f;

        lowerPoints = new Vector2[segments];
        upperPoints = new Vector2[segments];
        offsets = new FloatOffset[segments];
        velo = new Vector2[segments];
        for (int i = 0; i < segments; i++)
            offsets[i] = new FloatOffset();
        offsets[0].value = 0.5f;

        float angle = 0;
        for (int i = 0; i < segments; i++)
        {
            lowerPoints[i].x = (radius - waveHeight) * Mathf.Cos(angle * Mathf.Deg2Rad);
            lowerPoints[i].y = (radius - waveHeight) * Mathf.Sin(angle * Mathf.Deg2Rad);
            upperPoints[i].x = (radius + waveHeight) * Mathf.Cos(angle * Mathf.Deg2Rad);
            upperPoints[i].y = (radius + waveHeight) * Mathf.Sin(angle * Mathf.Deg2Rad);

            angle += 360f / segments;
        }
    }

    public override void Update()
    {
        float speed = waveSpeed * Time.deltaTime;
        for (int i = 0; i < segments; i++)
        {
            JelloPointMass point = mEdgePointMasses[i];
            point.LocalPosition = offsets[i].inc ? Vector2.SmoothDamp(
                point.LocalPosition, upperPoints[i], ref velo[i], waveLenght * Time.deltaTime
                ):
                Vector2.SmoothDamp(
                point.LocalPosition, lowerPoints[i], ref velo[i], waveLenght * Time.deltaTime
                );
            FloatOffset nextOffset = offsets[i < segments - 1 ? i + 1 : 0];
            if (offsets[i].inc)
            {
                if (offsets[i].value + speed <= 1f)
                {
                    nextOffset.value = offsets[i].value + speed;
                    nextOffset.inc = true;
                }
                else
                {
                    nextOffset.value = 1f;
                    nextOffset.inc = false;
                }
            }
            else
            {
                if (offsets[i].value - speed >= 0f)
                {
                    nextOffset.value = offsets[i].value - speed;
                    nextOffset.inc = false;
                }
                else
                {
                    nextOffset.value = 0f;
                    nextOffset.inc = true;
                }
            }
        }
        base.Update();
    }
}
