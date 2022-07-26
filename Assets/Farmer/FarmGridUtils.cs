using UnityEngine;

public class FarmGridUtils
{
    public static Vector3 GridAlign(Vector3 position)
    {
        return new Vector3(
            Mathf.Round(position.x),
            position.y,
            Mathf.Round(position.z)
        );
    }

    public static Vector2 OrientToGrid(Vector3 position)
    {
        return new Vector2(
            position.x,
            position.z
        );
    }
}