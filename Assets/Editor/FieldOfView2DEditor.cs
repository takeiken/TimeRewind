using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(EnemyMovement), true)] // Use true to include subclasses
public class FieldOfView2DEditor : Editor
{
    private void OnSceneGUI()
    {
        EnemyMovement fov = (EnemyMovement)target;
        Handles.color = Color.white;

        // Draw the wire arc for the field of view
        Handles.DrawWireArc(new Vector3(fov.transform.position.x, fov.transform.position.y, 0), Vector3.forward, Vector3.up, 360, fov.radius);

        // Calculate the view angles based on the Z rotation
        Vector2 viewAngle01 = DirectionFromAngle(fov.transform.eulerAngles.z, -fov.angle / 2);
        Vector2 viewAngle02 = DirectionFromAngle(fov.transform.eulerAngles.z, fov.angle / 2);

        Handles.color = Color.yellow;
        Handles.DrawLine(new Vector3(fov.transform.position.x, fov.transform.position.y, 0), new Vector3(fov.transform.position.x, fov.transform.position.y, 0) + (Vector3)viewAngle01 * fov.radius);
        Handles.DrawLine(new Vector3(fov.transform.position.x, fov.transform.position.y, 0), new Vector3(fov.transform.position.x, fov.transform.position.y, 0) + (Vector3)viewAngle02 * fov.radius);

        if (fov.canSeePlayer)
        {
            Handles.color = Color.green;
            Handles.DrawLine(new Vector3(fov.transform.position.x, fov.transform.position.y, 0), fov.playerRef.transform.position);
        }
    }

    private Vector2 DirectionFromAngle(float eulerZ, float angleInDegrees)
    {
        // Correctly compute the direction based on the Z rotation
        angleInDegrees += eulerZ;

        return new Vector2(Mathf.Cos(angleInDegrees * Mathf.Deg2Rad), Mathf.Sin(angleInDegrees * Mathf.Deg2Rad));
    }
}