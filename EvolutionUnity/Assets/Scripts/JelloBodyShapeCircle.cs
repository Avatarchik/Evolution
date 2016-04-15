using UnityEngine;
using System.Collections;

[RequireComponent(typeof(PolygonCollider2D))]
[RequireComponent(typeof(JelloBody))]
[RequireComponent(typeof(MeshLink))]
public class JelloBodyShapeCircle : MonoBehaviour {
    
    public PolygonCollider2D PolyCollider {get; private set;}
    public JelloBody JelloBody { get; private set; }
    public MeshLink MeshLink { get; private set; }

    public int segments = 5;
    public float radius = 1f;

    void Start()
    {
        UpdateGeometry();
    }

    public void UpdateGeometry()
    {
        PolyCollider = GetComponent<PolygonCollider2D>();
        MeshLink = GetComponent<SpriteMeshLink>();
        JelloBody = GetComponent<JelloBody>();

        CalculateCollider();
        JelloClosedShape shape = new JelloClosedShape(PolyCollider.points, null, false);
        JelloBody.setShape(shape, JelloBody.ShapeSettingOptions.MovePointMasses);
        MeshLink.Initialize(true);
    }

    void CalculateCollider()
    {
        Vector2[] vectors = new Vector2[segments];

        float angle = 0;
        for (int i = 0; i < segments; i++)
        {
            vectors[i].x = radius * Mathf.Cos(angle * Mathf.Deg2Rad);
            vectors[i].y = radius * Mathf.Sin(angle * Mathf.Deg2Rad);

            angle += 360f / segments;
        }
        PolyCollider.SetPath(0, vectors);
    }
}
