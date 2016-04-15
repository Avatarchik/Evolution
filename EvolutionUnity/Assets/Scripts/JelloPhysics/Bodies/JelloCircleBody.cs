using UnityEngine;
using System.Collections;

public class JelloCircleBody : JelloBody, IJelloCyrcle {
    [SerializeField]
    public float radius = 1f;
    [SerializeField]
    public int segments = 9;

    public override void Awake()
    {
        UpdateShape();
    }

    public void UpdateShape()
    {
        if (polyCollider == null)
            polyCollider = (PolygonCollider2D)GetComponent<Collider2D>();
        if (meshLink == null)
            meshLink = GetComponent<MeshLink>();
        if (radius <= 0)
            radius = 0.05f;
        if (segments < 3)
            segments = 3;
        CalculateCollider();
        base.Awake();
        if (meshLink != null)
            meshLink.Initialize(true);
    }

    public void CalculateCollider()
    {
        Vector2[] vectors = new Vector2[segments];

        float angle = 0;
        for (int i = 0; i < segments; i++)
        {
            vectors[i].x = radius * Mathf.Cos(angle * Mathf.Deg2Rad);
            vectors[i].y = radius * Mathf.Sin(angle * Mathf.Deg2Rad);

            angle += 360f / segments;
        }
        polyCollider.SetPath(0, vectors);
    }
}
