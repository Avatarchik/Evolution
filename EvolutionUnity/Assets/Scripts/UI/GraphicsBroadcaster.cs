using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

/// <summary>
/// Транслирует целям изменения в графике текущего объекта
/// </summary>
public class GraphicsBroadcaster : MonoBehaviour {
    
        
    public List<Graphic> targets = new List<Graphic>();

    /// <summary>
    /// Применить изменения в цвете целям
    /// </summary>
    public void ApplyColorChanges()
    {
        Graphic myGraphic = GetComponent<Graphic>();
        foreach (Graphic target in targets)
        {
            target.color = myGraphic.color;
        }
    }
}
