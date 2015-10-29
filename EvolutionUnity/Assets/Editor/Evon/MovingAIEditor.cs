using UnityEngine;
using UnityEditor;
using System.Collections;

[CanEditMultipleObjects]
[CustomEditor(typeof(MovingAI), true)]
public class MovingAIEditor : Editor
{

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
    }

    void OnSceneGUI()
    {
        MovingAI particle = (MovingAI)target;
        Vector3 startPosition = particle.transform.position;
        if (Application.isPlaying)
            startPosition = particle.StartPosition;

        Handles.CircleCap(0, startPosition, particle.transform.rotation, particle.maxMoveDistance);

        Handles.DrawLine(
            particle.transform.position,
            particle.transform.position + new Vector3(Mathf.Cos(particle.moveAngle * Mathf.Deg2Rad) * particle.maxMoveDistance, Mathf.Sin(particle.moveAngle * Mathf.Deg2Rad) * particle.maxMoveDistance, 0));

        Handles.DrawLine(particle.transform.position, startPosition);
    }
}
