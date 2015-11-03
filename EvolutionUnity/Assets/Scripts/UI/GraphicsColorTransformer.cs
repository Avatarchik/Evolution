using UnityEngine;
using UnityEngine.UI;

[DisallowMultipleComponent]
[ExecuteInEditMode]
public class GraphicsColorTransformer : GentleMonoBeh {    
    public Graphic Graphic {get; private set;}

    public Color from;
    public Color to;
    [Range(0, 1f)]
    public float offset;

    void Awake()
    {
        Graphic = GetComponent<Graphic>();
        SetCPURateOf(2);
    }

    public override void NormalUpdate()
    {
        
    }

    public override void GentleUpdate()
    {
        Color c = new Color(
            Mathf.Lerp(from.r, to.r, offset),            
            Mathf.Lerp(from.g, to.g, offset),
            Mathf.Lerp(from.b, to.b, offset),
            Mathf.Lerp(from.a, to.a, offset)
            );
        Graphic.color = c;
    }
}
